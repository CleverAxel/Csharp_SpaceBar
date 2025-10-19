using System;
using System.Collections.Specialized;
using Clengine;
using Clengine.Colliders;
using Clengine.Effects;
using Clengine.Input.KeyboardInput;
using Clengine.Pools;
using Clengine.Texture;
using Clengine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceBar.Particles;

namespace SpaceBar.Entities.Player {
    public class PlayerTopDown : Entity {
        public const float SCALE = 3f;
        private const float COLLIDER_FRACTION_WIDTH = 0.65f;
        private const float COLLIDER_FRACTION_HEIGHT = 0.7f;
        private const float COLLIDER_OFFSET_X = 0.175f;
        private const float COLLIDER_OFFSET_Y = 0.15f;
        private Random _random = new Random();
        private AABB _collider = new AABB();
        private Texture2D _texture;
        private Texture2D _laserTexture;
        private Texture2D _flameTexture;
        private Rectangle _srcRectangle;
        // private Vector2 _scale = new Vector2(SCALE, SCALE);
        private Pulse _pulse = new Pulse(SCALE - 0.1f, SCALE, SCALE + 0.3f, 2.0f);
        private bool startYDeccelerating = false;
        private bool startXDeccelerating = false;
        private Vector2 velocity = Vector2.Zero;
        private Vector2 direction = Vector2.Zero;
        private Vector2 prevDirection = Vector2.Zero;
        private Vector2 directionDecc = Vector2.Zero;

        const float ACCELERATION_FACTOR = 1800.0f;
        const float DECCELERATION_FACTOR = 750.0f;
        const float MAX_VELOCITY = 500.0f;

        Texture2D color;

        private Timer _shootCoolDown = new Timer(delayMs: 100);
        private Animation _shootAnimation = new Animation(frameCount: 4, frameDimension: 32, frameDurationMs: 33);
        private bool _canShoot = true;

        private Timer _particleSpawnCoolDown = new Timer(500);

        private PoolEntities<PlayerShipParticle> _shipParticlesPool = new PoolEntities<PlayerShipParticle>(30);
        private PoolEntities<PlayerLaser> _lasersPool = new PoolEntities<PlayerLaser>(20);

        public PlayerTopDown() {
            const int withPlayerTexture = 32;
            _shootCoolDown.OnFinish += ShootCoolDownFinish;
            _shootAnimation.OnFinish += ShootAnimationFinish;
            _particleSpawnCoolDown.OnFinish += SpawnShipParticleOnFinish;

            _position = new Vector2(0, 0);
            UpdateDestRectDimension(withPlayerTexture, withPlayerTexture);
            Scale(new Vector2(3f, 3f));

            _collider
            .SetOffsetHost(new Vector2(COLLIDER_OFFSET_X, COLLIDER_OFFSET_Y))
            .SetDestRectHost(_destRect);
            _collider.WidthFractionHost = COLLIDER_FRACTION_WIDTH;
            _collider.HeightFractionHost = COLLIDER_FRACTION_HEIGHT;

            _collider.Set(new Rectangle(_destRect.X + (int)(_destRect.Width * COLLIDER_OFFSET_X), _destRect.Y + (int)(_destRect.Height * COLLIDER_OFFSET_Y), (int)(_destRect.Width * COLLIDER_FRACTION_WIDTH), (int)(_destRect.Height * COLLIDER_FRACTION_HEIGHT)));

            _particleSpawnCoolDown.Start(0);


        }

        private void SpawnShipParticleOnFinish() {
            ref PlayerShipParticle particle = ref _shipParticlesPool.Create(out bool success);
            if (success) {
                int randAngle = _random.Next(225, 315);
                // int randAngle = _random.Next(270, 270);
                Vector2 randVelocity = new Vector2(randAngle, randAngle);
                // randVelocity.X = (float)Math.Cos(MathHelper.ToRadians(randVelocity.X) + 1 * Math.PI);
                // randVelocity.Y = (float)Math.Sin(MathHelper.ToRadians(randVelocity.Y) + 1 * Math.PI);

                randVelocity.X = (float)-Math.Cos(MathHelper.ToRadians(randVelocity.X));
                randVelocity.Y = (float)-Math.Sin(MathHelper.ToRadians(randVelocity.Y));

                // System.Console.WriteLine((MathHelper.ToDegrees((float)Math.Atan2(randVelocity.Y, randVelocity.X)) + 90) % 360);


                //here
                // int particleSpeed = _random.Next(350, 400);
                // randVelocity.Normalize();
                // randVelocity *= particleSpeed;
                // particle.Velocity = randVelocity;
                particle.Position = new Vector2(_position.X + _destRect.Width * 0.5f - 7.5f, _position.Y + _destRect.Height * 0.82f);
            }

            _particleSpawnCoolDown.Start(ClengineCore.LogicGameTime.TotalGameTime.TotalMilliseconds);
        }

        private void ShootAnimationFinish() {
            this._shootCoolDown.Start(ClengineCore.LogicGameTime.TotalGameTime.TotalMilliseconds);
        }

        private void ShootCoolDownFinish() {
            this._canShoot = true;
        }

        public override void Draw() {
            _lasersPool.Draw();
            ClengineCore.SpriteBatch.Draw(_texture, _destRect, _srcRectangle, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 1f);
            _shipParticlesPool.Draw();
            // _collider.Draw();
            // ClengineCore.SpriteBatch.Draw(_texture, _position, _srcRectangle, Color.White, 0.0f, _origin, _scale, SpriteEffects.None, 1);
        }

        public void LoadContent() {
            _texture = ClengineCore.Content.Load<Texture2D>("images/player");
            _srcRectangle = new Rectangle(32, 0, 32, 32);
            _laserTexture = ClengineCore.Content.Load<Texture2D>("images/player_laser");
            _flameTexture = ClengineCore.Content.Load<Texture2D>("images/flame");
            // _origin = new Vector2(_srcRectangle.Width / 2f, _srcRectangle.Height / 2f);

            color = new Texture2D(ClengineCore.GraphicsDevice, 1, 1);
            color.SetData([Color.White]);


            _lasersPool.InitEachItems((ref PlayerLaser laser) => {
                const int widthLaserTexture = 8;
                laser.Position = new Vector2(69, 69);
                laser.Texture = _laserTexture;
                laser.SrcRect = new Rectangle(0, 0, 8, 8);
                laser.DestRect = new Rectangle(0, 0, (int)(widthLaserTexture * SCALE), (int)(widthLaserTexture * SCALE));
            });
        }

        public override void Update() {
            float dT = ClengineCore.LogicDeltaTime;
            direction = GetDirection();

            if (direction != Vector2.Zero || velocity != Vector2.Zero) {
                Displace(dT);
            }
            ManageTiltState();

            ManageIdleState(dT);
            ManageShootingEvent();

            _collider
            .UpdatePosition(ref _position)
            .UpdateDimension(_destRect.Width, _destRect.Height);

            ClampToWindowsBound();



            _particleSpawnCoolDown.Update(ClengineCore.LogicGameTime.TotalGameTime.TotalMilliseconds);

            _shipParticlesPool.Update();
            _lasersPool.Update();
        }

        private void ClampToWindowsBound() {

            int gameWidth = ClengineCore.VirtualWidth;
            int gameHeight = ClengineCore.VirtualHeight;

            Vector2 oldPos = new Vector2(_collider.Left, _collider.Top);
            Vector2 newPos = Vector2.Zero;
            newPos.X = Math.Clamp(oldPos.X, 0, gameWidth - _collider.Width);
            newPos.Y = Math.Clamp(oldPos.Y, 0, gameHeight - _collider.Height);

            bool mustCalculateNewPos = oldPos.X != newPos.X || oldPos.Y != newPos.Y;

            if (mustCalculateNewPos) {
                _collider.SetPositon(ref newPos);
                _position = _collider.CalculateNewPositionForHost();

                if (newPos.X != oldPos.X) {
                    velocity.X = 0;
                }
                if (newPos.Y != oldPos.Y) {
                    velocity.Y = 0;
                }
            }
        }

        private void ManageShootingEvent() {
            double totalMilliSeconds = ClengineCore.LogicGameTime.TotalGameTime.TotalMilliseconds;
            bool isShooting = ClengineCore.Input.Keyboard.IsKeyDownSpace();
            if (isShooting && _canShoot && !_lasersPool.IsFull) {
                //skip one frame
                _shootAnimation.Play(totalMilliSeconds - _shootAnimation.FrameDurationMs);
                _canShoot = false;

                ref PlayerLaser laser = ref _lasersPool.Create(out bool success);
                const float offsetLaser =0.28125f;
                const float offsetLaserY = 0.25f;
                if (success) {
                    laser.Velocity = new Vector2(0, -MAX_VELOCITY); 
                    laser.Position = new Vector2(_position.X + (_destRect.Width * offsetLaser - (24 * 0.625f)), _position.Y + _destRect.Height * offsetLaserY);
                }
                
                ref PlayerLaser laser_ = ref _lasersPool.Create(out bool success_);
                if (success_) {
                    laser_.Velocity = new Vector2(0, -MAX_VELOCITY);
                    laser_.Position = new Vector2(_position.X + (_destRect.Width * 0.75f - (24 * 0.625f)), _position.Y + _destRect.Height * offsetLaserY); 
                }
            }

            if (_shootAnimation.IsPlaying) {
                _srcRectangle.Y = _shootAnimation.FrameDimension * _shootAnimation.Play(totalMilliSeconds);
            }

            if (_shootCoolDown.IsActive) {
                _shootCoolDown.Update(totalMilliSeconds);
            }
        }

        private void ManageTiltState() {
            if (direction.X == 0) {
                _srcRectangle.X = 32;
            } else if (direction.X == -1) {
                _srcRectangle.X = 0;
            } else {
                _srcRectangle.X = 64;
            }
        }

        private void ManageIdleState(float dT) {
            if (direction == Vector2.Zero) {
                Scale(_pulse.Update(dT));
                _particleSpawnCoolDown.DelayMs = 200;
            } else {
                Scale(new Vector2(SCALE, SCALE));
                _pulse.ResetTimer();
                _particleSpawnCoolDown.DelayMs = 16; //75
            }

        }

        private void Displace(float dT) {
            bool movingHorizontally = direction.X == -1 || direction.X == 1;
            bool movingVertically = direction.Y == -1 || direction.Y == 1;

            if (movingVertically) {
                startYDeccelerating = false;

                // if (prevDirection.Y == direction.Y && Math.Abs(velocity.Y) < MAX_VELOCITY) {
                velocity.Y += direction.Y * ACCELERATION_FACTOR * dT;
                velocity.Y = MathHelper.Clamp(velocity.Y, -MAX_VELOCITY, MAX_VELOCITY);
                // }

                prevDirection.Y = direction.Y;
            } else if (velocity.Y != 0) {
                if (!startYDeccelerating) {
                    directionDecc.Y = prevDirection.Y;
                    startYDeccelerating = true;
                }
                velocity.Y -= directionDecc.Y * DECCELERATION_FACTOR * dT;
                if (prevDirection.Y == 1 && velocity.Y < 0 || prevDirection.Y == -1 && velocity.Y > 0) {
                    velocity.Y = 0;
                }
            }

            if (movingHorizontally) {
                startXDeccelerating = false;

                // if (prevDirection.X == direction.X && Math.Abs(velocity.X) < MAX_VELOCITY) {
                velocity.X += direction.X * ACCELERATION_FACTOR * dT;
                velocity.X = MathHelper.Clamp(velocity.X, -MAX_VELOCITY, MAX_VELOCITY);
                // }

                prevDirection.X = direction.X;
            } else if (velocity.X != 0) {
                if (!startXDeccelerating) {
                    directionDecc.X = prevDirection.X;
                    startXDeccelerating = true;
                }
                velocity.X -= directionDecc.X * DECCELERATION_FACTOR * dT;
                if (prevDirection.X == 1 && velocity.X < 0 || prevDirection.X == -1 && velocity.X > 0) {
                    velocity.X = 0;
                }
            }


            if (velocity.LengthSquared() > MAX_VELOCITY * MAX_VELOCITY) {
                velocity.Normalize();
                velocity *= MAX_VELOCITY;
            }

            _position += velocity * dT;
            _destRect.X = (int)Math.Round(_position.X);
            _destRect.Y = (int)Math.Round(_position.Y);
        }

        private Vector2 GetDirection() {
            Vector2 dir = Vector2.Zero;

            IKeyboardListener keyboardListener = ClengineCore.Input.Keyboard;
            if (keyboardListener.IsKeyDownLeft()) {
                dir.X -= 1;
            }

            if (keyboardListener.IsKeyDownRight()) {
                dir.X += 1;
            }

            if (keyboardListener.IsKeyDownDown()) {
                dir.Y += 1;
            }

            if (keyboardListener.IsKeyDownUp()) {
                dir.Y -= 1;
            }

            return dir;
        }
    }
}
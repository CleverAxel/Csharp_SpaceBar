using System;
using Clengine;
using Clengine.Colliders;
using Clengine.Effects;
using Clengine.Input.KeyboardInput;
using Clengine.Texture;
using Clengine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceBar.Entities {
    public class Player : Entity {
        public const float SCALE = 3f;
        private const float COLLIDER_FRACTION_WIDTH = 0.65f;
        private const float COLLIDER_FRACTION_HEIGHT = 0.7f;
        private const float COLLIDER_OFFSET_X = 0.175f;
        private const float COLLIDER_OFFSET_Y = 0.15f;

        private AABB _collider = new AABB();
        private Texture2D _texture;
        private Rectangle _srcRectangle;
        // private Vector2 _scale = new Vector2(SCALE, SCALE);
        private Pulse _pulse = new Pulse(SCALE - 0.1f, SCALE, SCALE + 0.3f, 2.0f);
        private Vector2 _origin = Vector2.Zero;

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

        public Player() {
            _shootCoolDown.OnFinish += ShootCoolDownFinish;
            _shootAnimation.OnFinish += ShootAnimationFinish;

            _position = new Vector2(0, 0);
            UpdateDestRectDimension(32, 32);
            Scale(new Vector2(3f, 3f));

            _collider.Set(new Rectangle(_destRect.X + (int)(_destRect.Width * COLLIDER_OFFSET_X), _destRect.Y + (int)(_destRect.Height * COLLIDER_OFFSET_Y), (int)(_destRect.Width * COLLIDER_FRACTION_WIDTH), (int)(_destRect.Height * COLLIDER_FRACTION_HEIGHT)));


        }

        private void ShootAnimationFinish() {
            this._shootCoolDown.Start(ClengineCore.LogicGameTime.TotalGameTime.TotalMilliseconds);
        }

        private void ShootCoolDownFinish() {
            this._canShoot = true;
        }

        public override void Draw() {
            ClengineCore.SpriteBatch.Draw(_texture, _destRect, _srcRectangle, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 1f);
            _collider.Draw();
            // ClengineCore.SpriteBatch.Draw(_texture, _position, _srcRectangle, Color.White, 0.0f, _origin, _scale, SpriteEffects.None, 1);
        }

        public void LoadContent() {
            _texture = ClengineCore.Content.Load<Texture2D>("images/player");
            _srcRectangle = new Rectangle(32, 0, 32, 32);
            _origin = new Vector2(_srcRectangle.Width / 2f, _srcRectangle.Height / 2f);

            color = new Texture2D(ClengineCore.GraphicsDevice, 1, 1);
            color.SetData([Color.White]);
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
            _collider.Set(new Rectangle(_destRect.X + (int)(_destRect.Width * COLLIDER_OFFSET_X), _destRect.Y + (int)(_destRect.Height * COLLIDER_OFFSET_Y), (int)(_destRect.Width * COLLIDER_FRACTION_WIDTH), (int)(_destRect.Height * COLLIDER_FRACTION_HEIGHT)));


            ClampToWindowsBound();
        }

        private void ClampToWindowsBound() {
            GameBound.PrintStatus(GameBound.GetStatus(_collider));
            // int width = ClengineCore.VirtualWidth;
            // int height = ClengineCore.VirtualHeight;
            // int prevY = _destRect.Y;
            // int prevX = _destRect.X;
            // _destRect.Y = Math.Clamp(_destRect.Y, 0, height - _destRect.Height);
            // _destRect.X = Math.Clamp(_destRect.X, 0, width - _destRect.Width);

            // if (prevY != _destRect.Y) {
            //     velocity.Y = 0;
            //     _collider.SetY(_destRect.Y);
            //     _position.Y = _destRect.Y;
            // }

            // if (prevX != _destRect.X) {
            //     velocity.X = 0;
            //     _collider.SetX(_destRect.X + + (int)(_destRect.Width * COLLIDER_OFFSET));
            //     _position.X = _destRect.X;
            // }
        }

        private void ManageShootingEvent() {
            double totalMilliSeconds = ClengineCore.LogicGameTime.TotalGameTime.TotalMilliseconds;
            bool isShooting = ClengineCore.Input.Keyboard.IsKeyDownSpace();
            if (isShooting && _canShoot) {
                //skip one frame
                _shootAnimation.Play(totalMilliSeconds - _shootAnimation.FrameDurationMs);
                _canShoot = false;
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
            } else {
                Scale(new Vector2(SCALE, SCALE));
                _pulse.ResetTimer();
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
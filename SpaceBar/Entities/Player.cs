using System;
using Clengine;
using Clengine.Effects;
using Clengine.Input.KeyboardInput;
using Clengine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceBar.Entities {
    public class Player : Entity {
        public const float SCALE = 3f;
        private Texture2D _texture;
        private Rectangle _srcRectangle;
        private Vector2 _position = Vector2.Zero;
        private Vector2 _scale = new Vector2(SCALE, SCALE);
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

        private Timer _timerShoot = new Timer(delayMs: 500);
        private bool _canShoot = true;
        private short _shootingFrameCount = 4;
        private double _startTimeShooting = 0;

        public Player() {
            // _timerShoot.OnTimeOut += Test;
        }

        private void Test() {
            System.Console.WriteLine("bang");
        }

        public override void Draw() {
            ClengineCore.SpriteBatch.Draw(_texture, _position, _srcRectangle, Color.White, 0.0f, _origin, _scale, SpriteEffects.None, 1);
            // ClengineCore.SpriteBatch.Draw(color, rectangle, Color.White);
        }

        public void LoadContent() {
            _texture = ClengineCore.Content.Load<Texture2D>("images/player");
            _srcRectangle = new Rectangle(32, 0, 32, 32);
            _origin = new Vector2(_srcRectangle.Width / 2f, _srcRectangle.Height / 2f);

            color = new Texture2D(ClengineCore.GraphicsDevice, 1, 1);
            color.SetData([Color.White]);
        }

        public override void Update() {
            const int frameDuration = 28;
            // System.Console.WriteLine(ClengineCore.LogicGameTime.TotalGameTime.TotalMilliseconds  );
            float dT = ClengineCore.LogicDeltaTime;
            direction = GetDirection();
            bool isShooting = ClengineCore.Input.Keyboard.IsKeyDownSpace();

            if (direction != Vector2.Zero || velocity != Vector2.Zero) {
                Displace(dT);

                if (direction.X == 0) {
                    _srcRectangle.X = 32;
                } else if (direction.X == -1) {
                    _srcRectangle.X = 0;
                } else {
                    _srcRectangle.X = 64;
                }
            }
            ManageIdleState(dT);

            if(isShooting && _canShoot) {
                _startTimeShooting = ClengineCore.LogicGameTime.TotalGameTime.TotalMilliseconds - frameDuration;
                _canShoot = false;
            }

            // if (isShooting && _timerShoot.HasFinised) {
            //     _srcRectangle.Y = 0;
            //     _startTimeShooting = _timerShoot.Start() - frameDuration;
            // } else {
            //     _timerShoot.Update();
            // }
            
            if(_startTimeShooting != 0) {
                double elapsed = ClengineCore.LogicGameTime.TotalGameTime.TotalMilliseconds - _startTimeShooting;
                int frame = (int)(elapsed / frameDuration);

                if(frame > _shootingFrameCount) {
                    _startTimeShooting = 0;
                    frame = 0;
                    _canShoot = true;
                }

                _srcRectangle.Y = 32 * frame;
            }


        }

        private void ManageIdleState(float dT) {
            if (direction == Vector2.Zero) {
                _scale = _pulse.Update(dT);
            } else {
                _scale.X = SCALE;
                _scale.Y = SCALE;
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


            // Vector2 position = _position;

            // position += velocity * dT;

            _position += velocity * dT;

            // rectangle.X = (int)Math.Round(position.X);
            // rectangle.Y = (int)Math.Round(position.Y);
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
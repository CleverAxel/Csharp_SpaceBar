using System;
using Clengine;
using Clengine.Input.KeyboardInput;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceBar.Entities {
    public class Player : Entity {

        private bool startYDeccelerating = false;
        private bool startXDeccelerating = false;
        private Vector2 velocity = Vector2.Zero;
        private Vector2 direction = Vector2.Zero;
        private Vector2 prevDirection = Vector2.Zero;
        private Vector2 directionDecc = Vector2.Zero;

        const float ACCELERATION = 1800.0f;
        const float DECCELERATION = 750.0f;
        const float MAX_VELOCITY = 500.0f;

        Texture2D color;
        Rectangle rectangle = new Rectangle(0, 0, 50, 50);
        public override void Draw() {
            ClengineCore.SpriteBatch.Draw(color, rectangle, Color.White);
        }

        public void LoadContent() {
            color = new Texture2D(ClengineCore.GraphicsDevice, 1, 1);
            color.SetData([Color.White]);
        }

        public override void Update() {
            float dT = ClengineCore.LogicDeltaTime;
            direction = GetDirection();
            if (direction != Vector2.Zero || velocity != Vector2.Zero) {
                Displace(dT);

            }
        }

        private void Displace(float dT) {
            bool movingHorizontally = direction.X == -1 || direction.X == 1;
            bool movingVertically = direction.Y == -1 || direction.Y == 1;
            bool movingDiagonally = movingHorizontally && movingVertically;

            if (movingVertically) {
                prevDirection.Y = direction.Y;
                startYDeccelerating = false;

                velocity.Y += direction.Y * ACCELERATION * dT;
                velocity.Y = MathHelper.Clamp(velocity.Y, -MAX_VELOCITY, MAX_VELOCITY);
            } else if (velocity.Y != 0) {
                if (!startYDeccelerating) {
                    directionDecc.Y = prevDirection.Y;
                    startYDeccelerating = true;
                }
                velocity.Y -= directionDecc.Y * DECCELERATION * dT;
                if (prevDirection.Y == 1 && velocity.Y < 0 || prevDirection.Y == -1 && velocity.Y > 0) {
                    velocity.Y = 0;
                }
            }

            if (movingHorizontally) {
                prevDirection.X = direction.X;
                startXDeccelerating = false;

                velocity.X += direction.X * ACCELERATION * dT;
                velocity.X = MathHelper.Clamp(velocity.X, -MAX_VELOCITY, MAX_VELOCITY);
            } else if (velocity.X != 0) {
                if (!startXDeccelerating) {
                    directionDecc.X = prevDirection.X;
                    startXDeccelerating = true;
                }
                velocity.X -= directionDecc.X * DECCELERATION * dT;
                if (prevDirection.X == 1 && velocity.X < 0 || prevDirection.X == -1 && velocity.X > 0) {
                    velocity.X = 0;
                }
            }





            // direction.Normalize();

            Vector2 position = new Vector2(rectangle.X, rectangle.Y);

            position += velocity * dT;

            rectangle.X = (int)Math.Round(position.X);
            rectangle.Y = (int)Math.Round(position.Y);
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
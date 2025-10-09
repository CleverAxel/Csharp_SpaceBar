using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clengine;
using Clengine.Input.KeyboardInput;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceBar.Entities {
    public class Player : Entity {

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
            Vector2 direction = GetDirection();
            if (direction != Vector2.Zero) {
                bool movingHorizontally = direction.X == -1 || direction.X == 1;
                bool movingVertically = direction.Y == -1 || direction.Y == 1;
                bool movingDiagonally = movingHorizontally && movingVertically;

                direction.Normalize();

                Vector2 position = new Vector2(rectangle.X, rectangle.Y);
                position += direction * (movingDiagonally ? 135.0f : 100.0f) * dT;

                rectangle.X = (int)Math.Round(position.X);
                rectangle.Y = (int)Math.Round(position.Y);

            }
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
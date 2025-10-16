using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clengine.Texture;
using Microsoft.Xna.Framework;

namespace Clengine.Colliders {
    public class AABB {

        private Rectangle _rectangle;

        public int Left => _rectangle.X;
        public int Top => _rectangle.Y;
        public int Width => _rectangle.Width;
        public int Height => _rectangle.Height;
        public int Right => _rectangle.Right;
        public int Bottom => _rectangle.Bottom;

        public void Set(Rectangle rectangle) {
            _rectangle = rectangle;
        }

        public void SetX(int x) {
            _rectangle.X = x;
        }

        public void SetY(int y) {
            _rectangle.Y = y;
        }

        public void Draw() {
            ClengineCore.SpriteBatch.Draw(TextureColor.Red, _rectangle, Color.White * 0.25f);
        }
    }
}
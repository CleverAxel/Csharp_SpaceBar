using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clengine.Texture;
using Microsoft.Xna.Framework;

namespace Clengine.Colliders {
    public class AABB {

        private Rectangle _rectangle;
        public Rectangle Rectangle => _rectangle;

        public AABB SetX(int x) {
            _rectangle.X = x;

            return this;
        }
        public AABB SetY(int y) {
            _rectangle.Y = y;

            return this;
        }
        public AABB SetWidth(int width) {
            _rectangle.Width = width;

            return this;
        }

        public AABB SetHeight(int height) {
            _rectangle.Height = height;
            return this;
        }

        public void Draw() {
            ClengineCore.SpriteBatch.Draw(TextureColor.Red, _rectangle, Color.White);
        }
    }
}
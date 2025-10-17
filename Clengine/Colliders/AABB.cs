using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clengine.Texture;
using Microsoft.Xna.Framework;

namespace Clengine.Colliders {
    public class AABB {

        private Rectangle _rectangle;
        public float WidthFractionHost { get; set; }
        public float HeightFractionHost { get; set; }
        private Vector2 _offsetHost;
        private Rectangle _destRectHost;

        public int Left => _rectangle.X;
        public int Top => _rectangle.Y;
        public int Width => _rectangle.Width;
        public int Height => _rectangle.Height;
        public int Right => _rectangle.Right;
        public int Bottom => _rectangle.Bottom;

        public void Set(Rectangle rectangle) {
            _rectangle = rectangle;
        }

        public AABB SetPositon(ref readonly Vector2 position) {
            _rectangle.X = (int)position.X;
            _rectangle.Y = (int)position.Y;
            return this;
        }

        public AABB UpdatePosition(ref readonly Vector2 position) {
            _rectangle.X = (int)(position.X + _destRectHost.Width * _offsetHost.X);
            _rectangle.Y = (int)(position.Y + _destRectHost.Height * _offsetHost.Y);
            return this;
        }

        public AABB UpdateDimension(int width, int height) {
            _destRectHost.Width = width;
            _destRectHost.Height = height;
            _rectangle.Width = (int)(width * WidthFractionHost);
            _rectangle.Height = (int)(height * HeightFractionHost);
            return this;
        }

        public AABB SetDestRectHost(Rectangle rectangle) {
            _destRectHost = rectangle;
            return this;
        }

        public AABB SetOffsetHost(Vector2 offset) {
            _offsetHost = offset;
            return this;
        }
        
        public Vector2 CalculateNewPositionForHost() {
            Vector2 position = new Vector2(_rectangle.X, _rectangle.Y);
            position.X -= _destRectHost.Width * _offsetHost.X;
            position.Y -= _destRectHost.Height * _offsetHost.Y;

            return position;
        }

        public AABB SetX(int x) {
            _rectangle.X = x;
            return this;
        }

        public AABB SetY(int y) {
            _rectangle.Y = y;
            return this;
        }

        public void Draw() {
            ClengineCore.SpriteBatch.Draw(TextureColor.Green, _rectangle, Color.White * 0.25f);
        }
    }
}
using System;
using Microsoft.Xna.Framework;

namespace SpaceBar.Entities {
    public abstract class Entity {
        protected Vector2 _position = Vector2.Zero;
        protected Rectangle _destRect;
        private Rectangle _baseDestRect;
        protected Vector2 _originScale = new Vector2(0.5f, 0.5f);
        protected Vector2 _scale = new Vector2(1f, 1f);


        public void Scale(Vector2 scale) {
            if (_scale == scale) {
                return;
            }

            Vector2 anchor = new Vector2(
                _position.X + _destRect.Width * _originScale.X,
                _position.Y + _destRect.Height * _originScale.Y
            );

            _scale = scale;
            int newWidth = (int)(_baseDestRect.Width * _scale.X);
            int newHeight = (int)(_baseDestRect.Height * _scale.Y);

            _destRect.Width = newWidth;
            _destRect.Height = newHeight;

            _position.X = anchor.X - newWidth * _originScale.X;
            _position.Y = anchor.Y - newHeight * _originScale.Y;

            _destRect.X = (int)Math.Round(_position.X);
            _destRect.Y = (int)Math.Round(_position.Y);
        }

        protected void UpdateDestRectDimension(int width, int height) {
            _destRect = new Rectangle((int)Math.Round(_position.X), (int)Math.Round(_position.Y), width, height);
            _baseDestRect = _destRect;
        }

        public abstract void Draw();
        public abstract void Update();
    }
}
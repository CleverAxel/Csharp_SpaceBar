using Microsoft.Xna.Framework;

namespace SpaceBar.Entities {
    public abstract class Entity {
        protected Rectangle _destRect;
        protected Rectangle _baseDestRect;
        protected Vector2 _originScale = new Vector2(0.5f, 0.5f);
        protected Vector2 _scale;


        public void Scale(Vector2 scale) {
            if (_scale == scale) {
                return;
            }

            Vector2 anchor = new Vector2(
                _baseDestRect.X + _baseDestRect.Width * _originScale.X,
                _baseDestRect.Y + _baseDestRect.Height * _originScale.Y
            );

            _scale = scale;
            int newWidth = (int)(_baseDestRect.Width * _scale.X);
            int newHeight = (int)(_baseDestRect.Height * _scale.Y);

            _destRect.Width = newWidth;
            _destRect.Height = newHeight;
            _destRect.X = (int)(anchor.X - newWidth * _originScale.X);
            _destRect.Y = (int)(anchor.Y - newHeight * -_originScale.Y);
        }

        public abstract void Draw();
        public abstract void Update();
    }
}
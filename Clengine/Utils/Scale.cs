using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Clengine.Utils {
    public class Scale {
        private int _baseWidth = 0;
        private int _baseHeight = 0;
        private Vector2 _origin = new Vector2(0.5f, 0.5f);
        private Vector2 _scale = new Vector2(1f, 1f);

        public Scale SetBaseDimension(int width, int height) {
            _baseWidth = width;
            _baseHeight = height;
            return this;
        }

        public Scale SetOrigin(Vector2 origin) {
            _origin = origin;
            return this;
        }

        public void Update(ref Vector2 position, ref Rectangle destRect, Vector2 scale) {
            if (scale == _scale)
                return;
                
            _scale = scale;
            Vector2 anchor = new Vector2(
                position.X + destRect.Width * _origin.X,
                position.Y + destRect.Height * _origin.Y
            );

            int newWidth = (int)Math.Round(_baseWidth * scale.X);
            int newHeight = (int)Math.Round(_baseHeight * scale.Y);

            position.X = anchor.X - newWidth * _origin.X;
            position.Y = anchor.Y - newHeight * _origin.Y;

            destRect.X = (int)Math.Round(position.X);
            destRect.Y = (int)Math.Round(position.Y);
            destRect.Width = newWidth;
            destRect.Height = newHeight;
        }
    }
}
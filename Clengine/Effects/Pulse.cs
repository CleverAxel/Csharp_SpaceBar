using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Clengine.Effects {
    public struct Pulse {

        private float _minScale = 0.0f;
        private float _baseScale = 0.0f;
        private float _maxScale = 0.0f;
        private float _frequency = 0.0f;
        private float _t = 0.0f;

        public Pulse(float minScale, float baseScale, float maxScale, float frequency) {
            _minScale = minScale;
            _baseScale = baseScale;
            _maxScale = maxScale;
            _frequency = frequency;
        }
        public Vector2 Update(float dT) {
            _t += dT;

            // don't lose precision if the float is becoming too big
            if (_t > MathHelper.TwoPi) {
                _t %= MathHelper.TwoPi;
            }

            // SCALE + (float)((Math.Sin(scaleTime * frequency) + 1) / 2f) * amplitude;

            float normalized = (float)((Math.Sin(_t * _frequency) + 1) / 2f); // [-1, 1];
            float scale = _minScale + normalized * (_maxScale - _minScale);

            return new Vector2(scale, scale);
        }

        public void ResetTimer() {
            _t = 0;
        }
    }
}
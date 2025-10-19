using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clengine;
using Clengine.Pools;
using Clengine.Texture;
using Microsoft.Xna.Framework;
using SpaceBar.Entities;

namespace SpaceBar.Particles {
    public struct PlayerShipParticle : IPoolableEntity {


        public PlayerShipParticle() {
        }

        private Color _currentColor = Color.Red;
        private Vector2 _position;
        public Vector2 Position { get => _position; set => _position = value; }

        private Vector2 _velocity;
        public Vector2 Velocity { get => _velocity; set => _velocity = value; }

        private Rectangle _destRect = new Rectangle(0, 0, 15, 15);

        private float _opacity = 1.0f;
        private float _tOpacity = 0.0f;

        public int NextIndexInPool { get; set; }
        public int IndexInPool { get; set; }
        public bool IsInUse { get; set; }

        public void Reset() {
            _tOpacity = 0f;
            _opacity = 1f;
            _currentColor = Color.Red;
        }

        public void Draw() {
            ClengineCore.SpriteBatch.Draw(TextureColor.White, _destRect, _currentColor * _opacity);
        }

        public bool MustBeReturnedToPool() {
            if (_opacity <= 0f) {
                Reset();
                return true;
            }
            return false;
        }

        public void Update() {
            const float decaySpeed = 6.0f;
            float dT = ClengineCore.LogicDeltaTime;


            _opacity = MathHelper.Lerp(1.0f, 0.0f, _tOpacity);
            _currentColor = Color.Lerp(Color.Red, Color.Yellow, _tOpacity);
            _tOpacity += dT * decaySpeed;
            if (_tOpacity > 1f) {
                _tOpacity = 1f;
            }
            _position += _velocity * dT;
            _destRect.X = (int)Math.Round(_position.X);
            _destRect.Y = (int)Math.Round(_position.Y);


        }
    }
}
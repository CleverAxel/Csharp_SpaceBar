using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clengine;
using Clengine.Colliders;
using Clengine.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceBar.Entities.Player {
    public struct PlayerLaser : IPoolableEntity {
        public int NextIndexInPool { get; set; }
        public int IndexInPool { get; set; }
        public bool IsInUse { get; set; }

        public Texture2D Texture { get; set; }
        private Vector2 _position = Vector2.One;
        public Vector2 Position { get => _position; set => _position = value; }

        private Vector2 _velocity = Vector2.Zero;
        public Vector2 Velocity { get => _velocity; set => _velocity = value; }

        private Rectangle _srcRect = Rectangle.Empty;
        public Rectangle SrcRect { get => _srcRect; set => _srcRect = value; }

        private Rectangle _destRect = Rectangle.Empty;
        public Rectangle DestRect { get => _destRect; set => _destRect = value; }

        public Animation Animation { get; set; } = new Animation(2-1, 8, 32);
        public AABB Collider { get; set; }

        public PlayerLaser() {
        }
        
   

        public void Draw() {
            ClengineCore.SpriteBatch.Draw(Texture, _destRect, _srcRect, Color.White);
        }

        public bool MustBeReturnedToPool() {
            if (_position.Y <= 0) {
                _position = Vector2.One;
                return true;
            }

            return false;
        }

        public void Update() {
            _position += _velocity * ClengineCore.LogicDeltaTime;
            _srcRect.X = _srcRect.Width * Animation.Play(ClengineCore.LogicGameTime.TotalGameTime.TotalMilliseconds);
            _destRect.X = (int)_position.X;
            _destRect.Y = (int)_position.Y;

        }

    }
}
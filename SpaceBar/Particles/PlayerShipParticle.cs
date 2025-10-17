using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clengine.Pools;
using SpaceBar.Entities;

namespace SpaceBar.Particles {
    public struct PlayerShipParticle : IPoolableEntity {
        public int NextIndexInPool { get; set; }
        public int IndexInPool { get; set; }
        public bool IsInUse { get; set; }

        public void Draw() {
            throw new NotImplementedException();
        }

        public bool MustBeReturnedToPool() {
            throw new NotImplementedException();
        }

        public void Update() {
            throw new NotImplementedException();
        }
    }
}
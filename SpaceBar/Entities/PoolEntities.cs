using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Clengine.Pools;

namespace SpaceBar.Entities {
    public class PoolEntities<T> : Pool<T> where T : struct, IPoolableEntity {
        public PoolEntities(int capacity) : base(capacity) {
        }

        public void Update() {
            for (int i = 0; i < _pool.Count; i++) {
                ref T entity = ref CollectionsMarshal.AsSpan(_pool)[i];
                if (entity.IsInUse) {
                    entity.Update();
                }
                if (entity.MustBeReturnedToPool()) {
                    ReturnToPool(i);
                }
            }
        }

        public void Draw() {
            for (int i = 0; i < _pool.Count; i++) {
                ref T entity = ref CollectionsMarshal.AsSpan(_pool)[i];
                if (entity.IsInUse) {
                    entity.Draw();
                }
            }
        }
    }
}
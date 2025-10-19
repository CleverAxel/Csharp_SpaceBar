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
            if (_itemInUseCount == 0)
                return;

            int localCount = 0;
            for (int i = 0; i < _pool.Count; i++) {
                ref T entity = ref CollectionsMarshal.AsSpan(_pool)[i];
                if (entity.IsInUse) {
                    entity.Update();
                    localCount++;
                    
                    if (entity.MustBeReturnedToPool()) {
                        ReturnToPool(i);
                        localCount--;
                    }
                }

                if (localCount == _itemInUseCount)
                    return;
            }
        }

        public void Draw() {
            if (_itemInUseCount == 0)
                return;

            int localCount = 0;

            for (int i = 0; i < _pool.Count; i++) {
                ref T entity = ref CollectionsMarshal.AsSpan(_pool)[i];
                if (entity.IsInUse) {
                    localCount++;
                    entity.Draw();
                }

                if (localCount == _itemInUseCount)
                    return;
            }
        }
    }
}
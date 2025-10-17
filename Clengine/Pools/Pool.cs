using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Clengine.Pools {
    public class Pool<T> where T : struct, IPoolableItem {
        private T _dummy = new T();
        protected List<T> _pool;
        private int _indexFirstAvailable = -1;
        protected int _itemInUseCount = 0;


        public Pool(int capacity) {
            _pool = new List<T>(capacity);

            for (int i = 0; i < capacity; i++) {
                _pool.Add(new T());
                ref T item = ref CollectionsMarshal.AsSpan(_pool)[i];
                item.IndexInPool = i;
                item.NextIndexInPool = i + 1;
            }

            _indexFirstAvailable = 0;

            ref T lastItem = ref CollectionsMarshal.AsSpan(_pool)[_pool.Count - 1];
            lastItem.NextIndexInPool = -1;
        }

        public ref T Create(out bool success) {
            if (_indexFirstAvailable == -1) {
                success = false;
                return ref _dummy;
            }

            _itemInUseCount++;
            success = true;
            ref T firstAvailable = ref CollectionsMarshal.AsSpan(_pool)[_indexFirstAvailable];
            _indexFirstAvailable = firstAvailable.NextIndexInPool;
            firstAvailable.IsInUse = true;
            return ref firstAvailable;
        }

        protected void ReturnToPool(int index) {
            _itemInUseCount--;
            ref T item = ref CollectionsMarshal.AsSpan(_pool)[index];
            item.IsInUse = false;
            item.NextIndexInPool = _indexFirstAvailable;
            _indexFirstAvailable = index;
        }
    }
}
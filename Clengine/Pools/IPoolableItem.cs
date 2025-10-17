using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clengine.Pools {
    public interface IPoolableItem {
        
        /// <summary>
        /// Do no fricking touch it - Managed by the pool
        /// </summary>
        public int NextIndexInPool { get; set; }

        /// <summary>
        /// Do not fricking touch it - Managed by the pool
        /// </summary>
        public int IndexInPool { get; set; }

        /// <summary>
        /// Do not fricking touch it - Managed by the pool
        /// </summary>
        public bool IsInUse { get; set; }
        public bool MustBeReturnedToPool();

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clengine.Pools;

namespace SpaceBar.Entities
{
    public  interface IPoolableEntity : IPoolableItem {
        public void Update();
        public void Draw();
    }
}
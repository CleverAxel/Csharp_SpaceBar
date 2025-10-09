using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clengine.Input.KeyboardInput
{
    public interface IKeyboardListener {
        internal void Update();

        public bool IsKeyDownLeft();
        public bool IsKeyDownRight();
        public bool IsKeyDownUp();
        public bool IsKeyDownDown();
        public bool IsKeyDownSpace();

        public bool HasReleasedLeft();
        public bool HasReleasedRight();
        public bool HasReleasedUp();
        public bool HasReleasedDown();
        public bool HasReleasedSpace();
        


    }
}
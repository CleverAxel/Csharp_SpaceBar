using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clengine.Input.KeyboardInput
{
    public interface IKeyboardListener {
        internal void Update();

        public bool IsPressingLeft();
        public bool IsPressingRight();
        public bool IsPressingUp();
        public bool IsPressingDown();
        public bool IsPressingSpace();

        public bool HasReleasedLeft();
        public bool HasReleasedRight();
        public bool HasReleasedUp();
        public bool HasReleasedDown();
        public bool HasReleasedSpace();
        


    }
}
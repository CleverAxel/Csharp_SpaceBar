using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clengine.Input.KeyboardInput;

namespace Clengine.Input {
    public class InputController {

        public IKeyboardListener Keyboard { get; protected set; }
        public InputController(IKeyboardListener keyboardListener) {
            Keyboard = keyboardListener;
        }

        public void Update() {
            Keyboard.Update();
        }



    }
}
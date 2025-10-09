using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clengine.Input.KeyboardInput;
using Clengine.Input.MouseInput;

namespace Clengine.Input {
    public class InputController {

        public IKeyboardListener Keyboard { get; protected set; }
        public IMouseInputListener Mouse { get; protected set; }
        public InputController(IKeyboardListener keyboardListener, IMouseInputListener mouseListener) {
            Keyboard = keyboardListener;
            Mouse = mouseListener;
        }

        public void Update() {
            Keyboard.Update();
            Mouse.Update();
        }
    }
}
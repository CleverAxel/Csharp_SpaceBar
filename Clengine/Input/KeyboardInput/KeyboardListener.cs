using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace Clengine.Input.KeyboardInput {
    public class KeyboardListener : IKeyboardListener {
        private KeyboardState _currentKeyboardState;
        private KeyboardState _prevKeyboardState;

        public bool HasReleasedDown() {
            return _prevKeyboardState.IsKeyDown(Keys.S) && _currentKeyboardState.IsKeyUp(Keys.S);
        }

        public bool HasReleasedLeft() {
            return _prevKeyboardState.IsKeyDown(Keys.D) && _currentKeyboardState.IsKeyUp(Keys.D);
        }

        public bool HasReleasedRight() {
            return _prevKeyboardState.IsKeyDown(Keys.Q) && _currentKeyboardState.IsKeyUp(Keys.Q);
        }

        public bool HasReleasedUp() {
            return _prevKeyboardState.IsKeyDown(Keys.Z) && _currentKeyboardState.IsKeyUp(Keys.Z);
        }

        public bool HasReleasedSpace() {
            return _prevKeyboardState.IsKeyDown(Keys.Space) && _currentKeyboardState.IsKeyUp(Keys.Space);
        }

        public bool IsPressingDown() {
            return _currentKeyboardState.IsKeyDown(Keys.S);
        }

        public bool IsPressingLeft() {
            return _currentKeyboardState.IsKeyDown(Keys.Q);
        }

        public bool IsPressingRight() {
            return _currentKeyboardState.IsKeyDown(Keys.D);
        }

        public bool IsPressingUp() {
            return _currentKeyboardState.IsKeyDown(Keys.Z);
        }

        public bool IsPressingSpace() {
            return _currentKeyboardState.IsKeyDown(Keys.Space);
        }

        public void Update() {
            _prevKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();
        }
    }
}
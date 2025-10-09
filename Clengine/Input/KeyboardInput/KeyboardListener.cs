using Microsoft.Xna.Framework.Input;

namespace Clengine.Input.KeyboardInput {
    public class KeyboardListener : IKeyboardListener {
        private KeyboardState _currentKeyboardState;
        private KeyboardState _prevKeyboardState;

        public bool HasReleasedDown() {
            return ClengineCore.WindowHasFocus && _prevKeyboardState.IsKeyDown(Keys.S) && _currentKeyboardState.IsKeyUp(Keys.S);
        }

        public bool HasReleasedLeft() {
            return ClengineCore.WindowHasFocus && _prevKeyboardState.IsKeyDown(Keys.D) && _currentKeyboardState.IsKeyUp(Keys.D);
        }

        public bool HasReleasedRight() {
            return ClengineCore.WindowHasFocus && _prevKeyboardState.IsKeyDown(Keys.Q) && _currentKeyboardState.IsKeyUp(Keys.Q);
        }

        public bool HasReleasedUp() {
            return ClengineCore.WindowHasFocus && _prevKeyboardState.IsKeyDown(Keys.Z) && _currentKeyboardState.IsKeyUp(Keys.Z);
        }

        public bool HasReleasedSpace() {
            return ClengineCore.WindowHasFocus && _prevKeyboardState.IsKeyDown(Keys.Space) && _currentKeyboardState.IsKeyUp(Keys.Space);
        }

        public bool IsKeyDownDown() {
            return ClengineCore.WindowHasFocus && _currentKeyboardState.IsKeyDown(Keys.S);
        }

        public bool IsKeyDownLeft() {
            return ClengineCore.WindowHasFocus && _currentKeyboardState.IsKeyDown(Keys.Q);
        }

        public bool IsKeyDownRight() {
            return ClengineCore.WindowHasFocus && _currentKeyboardState.IsKeyDown(Keys.D);
        }

        public bool IsKeyDownUp() {
            return ClengineCore.WindowHasFocus && _currentKeyboardState.IsKeyDown(Keys.Z);
        }

        public bool IsKeyDownSpace() {
            return ClengineCore.WindowHasFocus && _currentKeyboardState.IsKeyDown(Keys.Space);
        }

        public void Update() {
            _prevKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();
        }
    }
}
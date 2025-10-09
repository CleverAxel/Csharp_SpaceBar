using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Clengine.Input.MouseInput {
    public class MouseInputListener : IMouseInputListener {
        private MouseState _currentState;
        private MouseState _prevState;
        private Point _currentPosition = Point.Zero;
        private Point _prevPosition = Point.Zero;

        public event Action<Point> OnMouseMove;


        public ref readonly Point GetMousePosition() {
            return ref _currentPosition;
        }

        private void TriggerOnMouseMove() {
            if (ClengineCore.WindowHasFocus && OnMouseMove != null && !_currentPosition.Equals(_prevPosition))
                OnMouseMove(_currentPosition);
        }

        public bool IsLeftClickDown() {
            return ClengineCore.WindowHasFocus && _currentState.LeftButton == ButtonState.Pressed;
        }

        public bool HasReleasedLeftClick() {
            return ClengineCore.WindowHasFocus && _prevState.LeftButton == ButtonState.Pressed && _currentState.LeftButton == ButtonState.Released;
        }

        private void CalculateMousePositionBasedOnRenderDestRect() {
            Rectangle playground = ClengineCore.RenderDestRect;

            if (_currentPosition.X < playground.Left) {
                _currentPosition.X = 0;
            } else if (_currentPosition.X > playground.Right) {
                _currentPosition.X = playground.Width;
            } else {
                _currentPosition.X -= playground.Left;
            }

            if (_currentPosition.Y < playground.Top) {
                _currentPosition.Y = 0;
            } else if (_currentPosition.Y > playground.Bottom) {
                _currentPosition.Y = playground.Height;
            } else {
                _currentPosition.Y -= playground.Top;
            }
        }

        public void Update() {
            _prevState = _currentState;
            _prevPosition = _currentPosition;

            _currentState = Mouse.GetState();

            _currentPosition.X = _currentState.X;
            _currentPosition.Y = _currentState.Y;

            CalculateMousePositionBasedOnRenderDestRect();
            TriggerOnMouseMove();
        }
    }
}
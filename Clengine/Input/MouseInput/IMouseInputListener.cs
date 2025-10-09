using System;
using Microsoft.Xna.Framework;

namespace Clengine.Input.MouseInput {
    public interface IMouseInputListener {
        public ref readonly Point GetMousePosition();
        public bool IsLeftClickDown();
        public bool HasReleasedLeftClick();
        public void Update();
        event Action<Point> OnMouseMove;
    }
}
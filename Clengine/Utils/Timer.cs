using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clengine.Utils {
    public class Timer {
        private double _start;
        public int DelayMs { get; set; }
        public bool HasFinised { get; set; } = true;
        public bool IsActive { get; set; } = false;
        public event Action OnTimeOut;

        public Timer(int delayMs)
        {
            DelayMs = delayMs;
        }

        public double Start() {
            HasFinised = false;
            _start = ClengineCore.LogicGameTime.TotalGameTime.TotalMilliseconds;
            return _start;
        }

        public void Update() {
            if (HasFinised)
                return;

            double elapsed = ClengineCore.LogicGameTime.TotalGameTime.TotalMilliseconds - _start;
            if (elapsed > DelayMs) {
                HasFinised = true;

                OnTimeOut?.Invoke();
            }
        }
    }
}
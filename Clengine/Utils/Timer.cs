using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clengine.Utils {
    public class Timer {
        private double _start;
        public int DelayMs { get; set; }
        public bool HasFinished { get; set; } = true;
        public bool IsActive { get; set; } = false;
        public event Action OnFinish;

        public Timer(int delayMs)
        {
            DelayMs = delayMs;
        }

        public void Start(double time) {
            HasFinished = false;
            IsActive = true;
            _start = time;
        }

        public void Update(double time) {
            if (HasFinished)
                return;

            double elapsed = time - _start;
            if (elapsed > DelayMs) {
                HasFinished = true;
                IsActive = false;
                OnFinish?.Invoke();
            }
        }
    }
}
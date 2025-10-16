using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clengine.Texture
{
    public class Animation
    {
        public int FrameCount { get; set; }
        public int FrameDimension { get; set; }
        public int FrameDurationMs { get; set; }

        public bool IsPlaying { get; private set; } = false;
        public bool HasFinished { get; private set; } = true;
        private double _start;

        public Animation(int frameCount, int frameDimension, int frameDurationMs) {
            FrameCount = frameCount;
            FrameDimension = frameDimension;
            FrameDurationMs = frameDurationMs;
        }
        
        public int Play(double time) {
            if (!IsPlaying && HasFinished) {
                IsPlaying = true;
                HasFinished = false;
                _start = time;
            }

            double elapsed = time - _start;
            int frame = (int)(elapsed / FrameDurationMs);

            if (frame > FrameCount) {
                frame = 0;
                HasFinished = true;
                IsPlaying = false;
            }

            return frame;

        }
    }
}
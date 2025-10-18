using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Clengine.Colliders {
    [Flags]
    public enum GameBoundStatus {
        None = 0,
        FullyTop = 1 << 0,   // 1
        FullyRight = 1 << 1,   // 2
        FullyDown = 1 << 2,   // 4
        FullyLeft = 1 << 3,   // 8
        PartiallyTop = 1 << 4,   // 16
        PartiallyRight = 1 << 5,   // 32
        PartiallyDown = 1 << 6,   // 64
        PartiallyLeft = 1 << 7    // 128
    }

    public class GameBound {
        public static GameBoundStatus GetStatus(AABB collider) {
            GameBoundStatus status = GameBoundStatus.None;
            int width = ClengineCore.VirtualWidth;
            int height = ClengineCore.VirtualHeight;

            int right = collider.Right;
            float left = collider.Left;
            float top = collider.Top;
            int bottom = collider.Bottom;

            if (right < 0) {
                status |= GameBoundStatus.FullyLeft;
            } else if (left < 0) {
                status |= GameBoundStatus.PartiallyLeft;
            }

            if (left > width) {
                status |= GameBoundStatus.FullyRight;
            } else if (right > width) {
                status |= GameBoundStatus.PartiallyRight;
            }

            if (bottom < 0) {
                status |= GameBoundStatus.FullyTop;
            } else if (top < 0) {
                status |= GameBoundStatus.PartiallyTop;
            }

            if (top > height) {
                status |= GameBoundStatus.FullyDown;
            } else if (bottom > height) {
                status |= GameBoundStatus.PartiallyDown;
            }

            return status;
        }

        public static void PrintStatus(GameBoundStatus status) {
            if (status == GameBoundStatus.None) {
                Console.WriteLine("Status: Inside bounds");
                return;
            }


            if (status.HasFlag(GameBoundStatus.PartiallyLeft))
                Console.WriteLine(" - Partially Left");
            if (status.HasFlag(GameBoundStatus.FullyLeft))
                Console.WriteLine(" - Fully Left");
            if (status.HasFlag(GameBoundStatus.PartiallyRight))
                Console.WriteLine(" - Partially Right");
            if (status.HasFlag(GameBoundStatus.FullyRight))
                Console.WriteLine(" - Fully Right");
            if (status.HasFlag(GameBoundStatus.PartiallyTop))
                Console.WriteLine(" - Partially Top");
            if (status.HasFlag(GameBoundStatus.FullyTop))
                Console.WriteLine(" - Fully Top");
            if (status.HasFlag(GameBoundStatus.PartiallyDown))
                Console.WriteLine(" - Partially Down");
            if (status.HasFlag(GameBoundStatus.FullyDown))
                Console.WriteLine(" - Fully Down");
        }
    }
}
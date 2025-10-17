using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Clengine.Texture {
    public static class TextureColor {
        public static Texture2D Red;
        public static Texture2D Green;
        public static Texture2D Blue;
        public static Texture2D White;
        public static void LoadContent() {
            Red = new Texture2D(ClengineCore.GraphicsDevice, 1, 1);
            Red.SetData([Color.Red]);

            Green = new Texture2D(ClengineCore.GraphicsDevice, 1, 1);
            Green.SetData([Color.Green]);

            Blue = new Texture2D(ClengineCore.GraphicsDevice, 1, 1);
            Blue.SetData([Color.Blue]);
            
            White = new Texture2D(ClengineCore.GraphicsDevice, 1, 1);
            White.SetData([Color.White]);

        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Clengine.Texture
{
    public struct TextureRegion
    {
        public Texture2D Texture { get; set; }
        public Rectangle SourceRectangle { get; set; }
        public readonly int Width => SourceRectangle.Width;
        public readonly int Height => SourceRectangle.Height;

        public TextureRegion(Texture2D texture, Rectangle src) {
            SourceRectangle = src;
            Texture = texture;
        }        
    }
}
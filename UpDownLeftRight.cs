using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public class LeftButton : Button
    {
        public static Texture2D texture;
        public LeftButton()
        {
            properties.spriteEffects = SpriteEffects.FlipHorizontally;
        }
    }
    public class RightButton : Button
    {
        public static Texture2D texture;
    }
    public class UpButton : Button
    {
        public static Texture2D texture;
    }
    public class DownButton : Button
    {
        public static Texture2D texture;
        public DownButton()
        {
            properties.spriteEffects = SpriteEffects.FlipVertically;
        }
    }
}

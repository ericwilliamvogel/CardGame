using Microsoft.Xna.Framework;
using System;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace SiegewithCleanCode
{
    public class Properties
    {
        public Sprite sprite { get; set; }
        public Vector2 ACTUALPOSITION { get; set; }
        //public static Vector2 WEIGHTEDPOSITION { get; set; } //maybe move this to a child later
        public float scale { get; set; }
        public static float globalScale { get; set; }
        public SpriteEffects spriteEffects = SpriteEffects.None;
        public int width { get; set; }
        public int height { get; set; }
        public Color color { get; set; }

        public Vector2 getDrawingPosition()
        {
            return new Vector2(ACTUALPOSITION.X + WEIGHTEDPOSITION.X, Game1.windowH - ACTUALPOSITION.Y + WEIGHTEDPOSITION.Y - height);
        }
        public Rectangle getParams()
        {
            return new Rectangle((int)ACTUALPOSITION.X, (int)ACTUALPOSITION.Y, width, height);
        }
    }
}
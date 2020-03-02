using Microsoft.Xna.Framework;
using System;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace CardGame
{
    public class Properties
    {
        private Vector2 position;
        private int width;
        private int height;




        public Sprite sprite { get; set; }
        public Vector2 POS { get; set; }
        public static Vector2 WEIGHTEDPOSITION { get; set; } //maybe move this to a child later
        public static Vector2 globalScale { get; set; }
        public SpriteEffects spriteEffects = SpriteEffects.None;
        public int Width { get { return Resolution.toResolution(width); } set { width = value; } }
        public int Height { get { return Resolution.toResolution(height); } set { height = value; } }
        public Color color { get; set; }
        public Vector2 scale { get; set; }
        public float rotation { get; set; }

        public Properties()
        {
            scale = new Vector2(1f, 1f);
        }

    }
}
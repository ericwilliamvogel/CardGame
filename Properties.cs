using Microsoft.Xna.Framework;
using System;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace CardGame
{
    public class Properties
    {
        private Vector2 position;
        public int width;
        public int height;

        //GraphicsSettings.toResolution(position)


        public Sprite sprite;
        public Vector2 POS { get { return position; } set { position = value; } }
        public static Vector2 WEIGHTEDPOSITION { get; set; } //maybe move this to a child later
        public static Vector2 globalScale { get; set; }
        public SpriteEffects spriteEffects = SpriteEffects.None;
        public int Width { get { return toScale(width); } set { width = value; } }
        public int Height { get { return toScale(height); } set { height = value; } }
        public Color color { get; set; }
        public Vector2 scale { get; set; }
        public float rotation { get; set; }
        public float transparency = 1f;

        private int toScale(int input)
        {
            float multiplier = globalScale.X + scale.X;
            return (int)(input * multiplier);
        }
        public Properties()
        {
            sprite = new Sprite();
            //scale = new Vector2(0f, 0f);
        }

    }
}
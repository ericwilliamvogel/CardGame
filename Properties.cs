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


        public Sprite sprite { get; set; }
        public Vector2 POS { get { return position; } set { position = value; } }
        public static Vector2 WEIGHTEDPOSITION { get; set; } //maybe move this to a child later
        public static Vector2 globalScale { get; set; }
        public SpriteEffects spriteEffects = SpriteEffects.None;
        public int Width { get { return GraphicsSettings.toResolution(width); } set { width = value; } }
        public int Height { get { return GraphicsSettings.toResolution(height); } set { height = value; } }
        public Color color { get; set; }
        public Vector2 scale { get; set; }
        public float rotation { get; set; }

        public Properties()
        {
            scale = new Vector2(1f, 1f);
        }

    }
}
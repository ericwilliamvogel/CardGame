using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public static class MouseTransformer
    {
        private static Texture2D regularTexture;
        private static Texture2D attackTexture;
        private static Texture2D targetTexture;

        private static Texture2D SelectedTexture;
        public static State state;
        public enum State
        {
            Reg,
            Atk,
            Tgt
        }
        public static void initTextures(ContentManager content)
        {
            regularTexture = content.Load<Texture2D>("regularMouse");
            attackTexture = content.Load<Texture2D>("attackMouse");
            targetTexture = content.Load<Texture2D>("targetMouse");
            Set(State.Reg);
        }
        private static int x;
        private static int y;

        public static void drawMouseTransformer(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(SelectedTexture, new Vector2(x,y), Color.White);
        }
        public static void updateMouseTransformerPosition(MouseState mouseState)
        {
            x = mouseState.X;
            y = mouseState.Y;
        }
        public static void Set(State newState)
        {
            state = newState;
            ReselectTexture();
        }
        private static void ReselectTexture()
        {
            SelectedTexture = correctTexture();
        }
        private static Texture2D correctTexture()
        {
            switch (state)
            {
                case State.Reg:
                    return regularTexture;
                case State.Atk:
                    return attackTexture;
                case State.Tgt:
                    return targetTexture;

                default:
                    return regularTexture;
            }
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{

    public class Token : GameComponent
    {
        public int resources;
        public Card.Race race;
        public Token(Card.Race race)
        {
            this.race = race;
        }
        public override void drawSprite(SpriteBatch spriteBatch)
        {

            base.drawSprite(spriteBatch);
            spriteBatch.DrawString(Game1.spritefont, resources.ToString(), new Vector2(getPosition().X + getWidth() / 2 - 10 * getScale().X, getPosition().Y + getHeight() / 2 - 20 * getScale().X), Color.Black, 0, new Vector2(0, 0), getScale(), SpriteEffects.None, 0);
        }
        public void adjustResourceValue(Side side)
        {
            int x = 0;
            foreach (Card.Race resource in side.Resources)
            {
                if (resource == race)
                {
                    x++;
                }
            }

            resources = x;
        }
    }
}

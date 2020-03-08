using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public class CardSupplement : GameComponent
    {
        public Vector2 relativePosition;
        public Vector2 offSet;

        public void setOffset(float x, float y)
        {
            offSet.X = x;
            offSet.Y = y;
        }
        public void setRelativePosition(GameComponent component)
        {
            relativePosition = component.getPosition();
        }
        public override void drawSprite(SpriteBatch spriteBatch)
        {
            properties.POS = relativePosition + offSet;
            base.drawSprite(spriteBatch);
        }
    }
}

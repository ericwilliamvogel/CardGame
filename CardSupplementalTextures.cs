
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public class CardSupplementalTextures
    {
        public CardSupplement cardBack;
        public CardSupplement cardBorder;
        public CardSupplement cardImageBorder;
        public CardSupplement cardFilling;
        public CardSupplement portrait;

        public CardSupplementalTextures()
        {
            cardBack = new CardSupplement();
            cardBorder = new CardSupplement();
            cardImageBorder = new CardSupplement();
            cardFilling = new CardSupplement();
            portrait = new CardSupplement();
            cardBack.setContentName("cardBack");
            cardBorder.setContentName("cardBorder");
            cardImageBorder.setContentName("cardImageBorder");
            cardFilling.setContentName("cardFilling");
        }

        public void setAllPositionsRelativeTo(GameComponent component)
        {
            cardBack.setRelativePosition(component);
            cardImageBorder.setRelativePosition(component);
            cardFilling.setRelativePosition(component);
            cardBorder.setRelativePosition(component);
            portrait.setRelativePosition(component);
        }

        public void scaleAllTo(float x)
        {
            cardBack.setScale(x);
            cardImageBorder.setScale(x);
            cardFilling.setScale(x);
            cardBorder.setScale(x);
            portrait.setScale(x);
        }
        public void moveAllTo(Vector2 newPosition)
        {
            cardBack.setPos(newPosition);
            cardImageBorder.setPos(newPosition);
            cardFilling.setPos(newPosition);
            cardBorder.setPos(newPosition);
            portrait.setPos(newPosition);
        }
    }
}

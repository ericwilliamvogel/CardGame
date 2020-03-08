using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace CardGame
{
    public class Hand : HorizontalContainer
    {
        public Hand()
        {
            containerScale = CardScale.Hand;
        }
        int extension = GraphicsSettings.toResolution(150);

        protected override void additionalAction(Card card)
        {
            card.properties.POS = new Vector2(card.properties.POS.X, getPosition().Y - extension);
        }
        protected override void resetAction(Card card)
        {
            card.properties.POS = new Vector2(card.properties.POS.X, getPosition().Y);
        }
    }
}

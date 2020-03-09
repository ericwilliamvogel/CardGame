
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
        public int cardBack = 0;
        public int cardBorder = 1;
        public int cardImageBorder = 2;
        public int cardFilling = 3;
        public int portrait = 4;
        public int selectionIndicator = 5;
        public int abilityDisplay = 6;


        public int TOTAL = 7;
        public List<CardSupplement> supplements;
        public CardSupplementalTextures()
        {
            supplements = new List<CardSupplement>();
            for(int i = 0; i < TOTAL; i++)
            {
                supplements.Add(new CardSupplement());
            }


            supplements[abilityDisplay].setContentName("abilityTexture");
            supplements[selectionIndicator].setContentName("selectedSymbol");
            supplements[cardBack].setContentName("cardBack");
            supplements[cardBorder].setContentName("cardBorder");
            supplements[cardImageBorder].setContentName("cardImageBorder");
            supplements[cardFilling].setContentName("cardFilling");
        }

        public void setAllPositionsRelativeTo(GameComponent component)
        {
            foreach(CardSupplement supp in supplements)
            {
                supp.setRelativePosition(component);
            }
        }

        public void scaleAllTo(float x)
        {
            foreach (CardSupplement supp in supplements)
            {
                supp.setScale(x);
            }
        }
        public void moveAllTo(Vector2 newPosition)
        {
            foreach (CardSupplement supp in supplements)
            {
                supp.setPos(newPosition);
            }
        }
    }
}

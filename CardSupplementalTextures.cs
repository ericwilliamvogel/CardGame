
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
        public readonly int cardBack = 0;
        public readonly int cardBorder = 1;
        public readonly int cardBody = 2;
        public readonly int cardFilling = 3;
        public readonly int portrait = 4;
        public readonly int selectionIndicator = 5;
        public readonly int abilityDisplay = 6;
        public readonly int orcToken = 7;
        public readonly int humanToken = 8;
        public readonly int elfToken = 9;
        public readonly int unanimousToken = 10;
        public readonly int attackIcon = 11;
        public readonly int defenseIcon = 12;
        public readonly int generalSymbol = 13;
        public readonly int armySymbol = 14;
        public readonly int fieldUnitSymbol = 15;
        public readonly int cardShading = 16;

        public int TOTAL = 17;
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
            supplements[cardBody].setContentName("cardBody");
            supplements[cardFilling].setContentName("cardFilling");
            supplements[orcToken].setContentName("orcToken");
            supplements[elfToken].setContentName("elfToken");
            supplements[humanToken].setContentName("humanToken");
            supplements[unanimousToken].setContentName("unanimousToken");
            supplements[attackIcon].setContentName("attackIcon");
            supplements[defenseIcon].setContentName("defenseIcon");
            supplements[generalSymbol].setContentName("generalSymbol");
            supplements[armySymbol].setContentName("armySymbol");
            supplements[fieldUnitSymbol].setContentName("fieldUnitSymbol");
            supplements[cardShading].setContentName("cardShading");
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

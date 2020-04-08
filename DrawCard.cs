using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public class BothSidesDrawCard : NonTargetAbility
    {
        private void setDesc()
        {
            if (power <= 1)
            {
                description = "Both draw card.";
            }
            else
            {
                description = "Both draw " + power.ToString() + " cards.";
            }
        }
        public BothSidesDrawCard(int amountOfCards)
        {
            this.power = amountOfCards;
            setDesc();

        }
        public BothSidesDrawCard(int amountOfCards, int exchangeValue)
        {
            this.power = amountOfCards;
            displayGeneralIncrements(exchangeValue);
            setDesc();
        }

        public override void abilityImplementation(MouseState mouseState, BoardFunctionality boardFunc)
        {

            boardFunc.AbilityBothDrawCard(INITIALCARD, this, boardFunc.friendlySide);
        }

    }
    public class DrawCard : NonTargetAbility
    {
        private void setDesc()
        {
            if (power <= 1)
            {
                description = "Draw a card.";
            }
            else
            {
                description = "Draw " + power.ToString() + " cards.";
            }
        }
        public DrawCard(int amountOfCards)
        {
            this.power = amountOfCards;
            setDesc();

        }
        public DrawCard(int amountOfCards, int exchangeValue)
        {
            this.power = amountOfCards;
            displayGeneralIncrements(exchangeValue);
            setDesc();
        }

        public override void abilityImplementation(MouseState mouseState, BoardFunctionality boardFunc)
        {
            boardFunc.AbilityDrawCard(INITIALCARD, this, boardFunc.friendlySide);
        }
    }
}

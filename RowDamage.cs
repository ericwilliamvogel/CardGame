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
    public class RowDamage : TargetDamage
    {
        public RowDamage(int damage, int exchangeValue) : base(damage, exchangeValue)
        {
            description = "Deal " + damage + " to row.";
        }

        public override void abilityImplementation(MouseState mouseState, BoardFunctionality boardFunc)
        {
            if (returnSelectedCard(mouseState, boardFunc) != null)
                boardFunc.BoardDamage(INITIALCARD, this, returnSelectedCard(mouseState, boardFunc));
        }

        public override void useAIAbility(AIPlayer player, BoardFunctionality boardFunc, Card targetCard)
        {
            if (player != null)
            {
                foreach (Card card in targetCard.correctRow(boardFunc.enemySide).cardsInContainer)
                {
                    card.cardProps.aiCalcDefense -= this.power;
                }

            }

        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CardGame
{
    public class TargetDamage : Target
    {

        public TargetDamage(int damage)
        {
            power = damage;
            description = "Deal " + damage + " to unit.";

        }
        public TargetDamage(int damage, int exchangeValue) : this(damage)
        {
            displayGeneralIncrements(exchangeValue);

        }
        public override void abilityImplementation(MouseState mouseState, BoardFunctionality boardFunc)
        {

            boardFunc.DirectDamage(INITIALCARD, this, returnSelectedCard(mouseState, boardFunc));
        }

        public override void useAIAbility(AIPlayer player, BoardFunctionality boardFunc, Card targetCard)
        {
            if (player != null)
            {
                targetCard.cardProps.aiCalcDefense -= this.power;
            }

        }
    }
}

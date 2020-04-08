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
    public class LifeDamage : NonTargetAbility
    {
        public LifeDamage(int damage)
        {
            power = damage;
            description = "Deal " + damage + " to enemy player.";

        }
        public LifeDamage(int damage, int exchangeValue) : this(damage)
        {
            displayGeneralIncrements(exchangeValue);
        }
        public override void abilityImplementation(MouseState mouseState, BoardFunctionality boardFunc)
        {
            boardFunc.LifeDamage(INITIALCARD, this);
        }
    }
}

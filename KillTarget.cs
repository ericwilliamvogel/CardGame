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
    public class KillTarget : Target
    {
        public KillTarget()
        {
            description = "Kill unit.";
        }
        public KillTarget(int exchangeValue) : this()
        {
            displayGeneralIncrements(exchangeValue);

        }
        public override void abilityImplementation(MouseState mouseState, BoardFunctionality boardFunc)
        {
            boardFunc.KillTarget(INITIALCARD, this, returnSelectedCard(mouseState, boardFunc)); //Kill(boardFunc.enemySide, returnSelectedCard(mouseState, boardFunc).correctRow(boardFunc.enemySide), returnSelectedCard(mouseState, boardFunc));
        }
    }
}

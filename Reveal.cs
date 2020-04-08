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
    public class Reveal : NonTargetAbility
    {
        public Reveal()
        {
            description = "Reveal board.";
        }
        public Reveal(int exchangeValue) : this()
        {
            displayGeneralIncrements(exchangeValue);

        }
        public override void abilityImplementation(MouseState mouseState, BoardFunctionality boardFunc)
        {
            boardFunc.RevealBoard(INITIALCARD, this);
        }

        public override void useAIAbility(AIPlayer player, BoardFunctionality boardFunc, Card targetCard)
        {
            if (player != null)
            {
                boardFunc.boardDef.revealBoardForRemainderOfTurn(targetCard, this, boardFunc);
            }

        }
    }
}

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
    public class Exhaust : Target
    {
        public Exhaust()
        {
            description = "Exhaust enemy unit.";
        }
        public Exhaust(int exchangeValue)
        {
            displayGeneralIncrements(exchangeValue);
        }
        public override Card returnSelectedCard(MouseState mouseState, BoardFunctionality boardFunc)
        {

            if (selectAction.TargetEnemyCard(mouseState, boardFunc, true) != null)
            {
                return selectAction.TargetEnemyCard(mouseState, boardFunc, true);
            }
            else
                return null;

            //throw new Exception();
        }
        /*public override void activateAbilityOnSelection(MouseState mouseState, BoardFunctionality boardFunc)
        {
            if (returnSelectedCard(mouseState, boardFunc) != null && INITIALCARD.cardProps.exhausted == false && clickedInAbilityBox == false)
            {
                if (selectAction.TargetEnemyCard(mouseState, boardFunc, true).correctRow(boardFunc.enemySide).revealed)
                {
                    boardFunc.cardViewer.resetCardSelectionOnRightClick(mouseState, boardFunc);
                    useAbility(mouseState, boardFunc);
                    clickedInAbilityBox = true;
                    resetAllCards(boardFunc);
                }
            }
            selectAction.resetIfNoSelection(mouseState, INITIALCARD.getCurrentContainer(boardFunc.friendlySide), INITIALCARD, boardFunc);
        }*/
        public override void abilityImplementation(MouseState mouseState, BoardFunctionality boardFunc)
        {
            boardFunc.Exhaust(INITIALCARD, this, returnSelectedCard(mouseState, boardFunc));
        }

        public override void useAIAbility(AIPlayer player, BoardFunctionality boardFunc, Card targetCard)
        {

        }

    }
}

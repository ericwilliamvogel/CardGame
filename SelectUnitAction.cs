using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public class SelectUnitAction
    {

        private Action action;
        public void setAction(Action newAction)
        {
            action = newAction;
        }
        /*private Card targetedCard = null;
        public Card targetCard()
        {
            return targetedCard;
        }*/

        //needs testing, may break if selecting a friendly source
        public void SetTargetCard(MouseState mouseState, HorizontalContainer container, Card card, Side side)
        {
            if (mouseState.LeftButton == ButtonState.Pressed && !container.isWithinModifiedPosition(mouseState, card))
            {
                foreach (FunctionalRow row in side.Rows)
                {
                    if (row.playState == PlayState.Revealed)
                    {
                        foreach (Card newCard in row.cardsInContainer)
                        {
                            if (row.isWithinModifiedPosition(mouseState, newCard))
                            {
                                newCard.setSelected();
                            }
                            else
                                newCard.setRegular();
                        }
                    }
                }
            }
            /*if(mouseState.LeftButton == ButtonState.Released)
            {
                foreach(FunctionalRow row in side.Rows)
                {
                    if (row.playState == Card.PlayState.Revealed)
                    {
                        foreach (Card newCard in row.cardsInContainer)
                        {
                                newCard.setRegular();
                        }
                    }
                }
            }*/
        }
        public Card TargetAnyCard(MouseState mouseState, BoardFunctionality boardFunc, bool pressToTarget)
        {
            if (TargetCard(mouseState,boardFunc.friendlySide,pressToTarget) != null)
            {
                return TargetCard(mouseState, boardFunc.friendlySide,pressToTarget);
            }
            if (TargetCard(mouseState,  boardFunc.enemySide,pressToTarget) != null)
            {
                return TargetCard(mouseState,boardFunc.enemySide,pressToTarget);
            }
            return null;
        }
        public Card TargetEnemyCard(MouseState mouseState, BoardFunctionality boardFunc, bool pressToTarget)
        {
            return TargetCard(mouseState,  boardFunc.enemySide, pressToTarget);
        }
        public Card TargetFriendlyCard(MouseState mouseState, BoardFunctionality boardFunc, bool pressToTarget)
        {
            return TargetCard(mouseState, boardFunc.friendlySide, pressToTarget);
        }
        public Card TargetCard(MouseState mouseState, Side side, bool pressToTarget)
        {
            ButtonState state;
            if(pressToTarget)
            {
                state = ButtonState.Pressed;
            }
            else
            {
                state = ButtonState.Released;
            }
            if (mouseState.LeftButton == state)
            {
                foreach (FunctionalRow row in side.Rows)
                {
                    if (row.playState == PlayState.Revealed)
                    {
                        foreach (Card newCard in row.cardsInContainer)
                        {
                            if (row.isWithinModifiedPosition(mouseState, newCard))
                            {
                                return newCard;
                            }
                        }
                    }
                }
            }

            return null;
        }
        public virtual void resetIfNoSelection(MouseState mouseState, HorizontalContainer container, Card card, BoardFunctionality boardFunc)
        {
            if (card.cardProps.type != CardType.Manuever)
            {
                if (!boardFunc.boardActions.isActive() && mouseState.LeftButton == ButtonState.Released && !container.isWithinModifiedPosition(mouseState, card))
                {
                    card.setRegular();
                    card.resetCardSelector();
                    boardFunc.SELECTEDCARD = null;
                    boardFunc.boardPosLogic.updateBoard(boardFunc);
                }
            }

        }
    }
}

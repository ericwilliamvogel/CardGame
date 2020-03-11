using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public abstract class RowLogic
    {
        protected SelectUnitAction selectAction;
        protected bool clickedInCardBox = false;

        public RowLogic()
        {
            selectAction = new SelectUnitAction();
        }
        public abstract void fieldLogic(MouseState mouseState, FunctionalRow row, Card card, BoardFunctionality boardFunc);

        public void setCardToView(MouseState mouseState, FunctionalRow row, BoardFunctionality boardFunc, bool friendly)
        {
            foreach (Card card in row.cardsInContainer)
            {
                if (card.isSelected())
                {
                    if (friendly)
                        boardFunc.SELECTEDCARD = card;
                    if (!friendly)
                        boardFunc.ENEMYSELECTEDCARD = card;

                    if (!boardFunc.cardView)
                    {
                        viewLogic(mouseState, row, card, boardFunc);
                        fieldLogic(mouseState, row, card, boardFunc);
                    }
                    else
                    {
                        boardFunc.viewFullSizeCard(mouseState, card);
                    }
                }
            }
        }
        public virtual void clickAndHold()
        {

        }
        public virtual void viewLogic(MouseState mouseState, FunctionalRow row, Card card, BoardFunctionality boardFunc)
        {
            if (mouseState.LeftButton == ButtonState.Pressed && row.isWithinModifiedPosition(mouseState, card))
            {
                //card.setPos(mouseState.X - card.getWidth() / 2, mouseState.Y - card.getHeight() / 2);
                clickedInCardBox = true;
            }
            if (mouseState.LeftButton == ButtonState.Released && clickedInCardBox && row.isWithinModifiedPosition(mouseState, card))
            {
                clickedInCardBox = false;
                boardFunc.cardView = true;

            }


        }
        public virtual void resetIfNoSelection(MouseState mouseState, HorizontalContainer container, Card card, BoardFunctionality boardFunc)
        {
            if (!boardFunc.boardActions.isActive() && mouseState.LeftButton == ButtonState.Released && clickedInCardBox && !container.isWithinModifiedPosition(mouseState, card))
            {
                clickedInCardBox = false;
                card.setRegular();
                card.resetCardSelector();
                boardFunc.SELECTEDCARD = null;
                boardFunc.boardPosLogic.updateBoard(boardFunc);
            }
        }

    }
    public class ArmyLogic : RowLogic
    {
        public override void fieldLogic(MouseState mouseState, FunctionalRow row, Card card, BoardFunctionality boardFunc)
        {

        }
    }
    public class GeneralLogic : RowLogic
    {
        public override void fieldLogic(MouseState mouseState, FunctionalRow row, Card card, BoardFunctionality boardFunc)
        {

        }
    }
    public class SelectUnitAction
    {

        private Action action;
        public void setAction(Action newAction)
        {
            action = newAction;
        }
        private Card targetedCard = null;
        public Card targetCard()
        {
            return targetedCard;
        }

        //needs testing, may break if selecting a friendly source
        public void SetTargetCard(MouseState mouseState, HorizontalContainer container, Card card, Side side)
        {
            if (mouseState.LeftButton == ButtonState.Pressed && !container.isWithinModifiedPosition(mouseState, card))
            {
                foreach (FunctionalRow row in side.Rows)
                {
                    if (row.playState == Card.PlayState.Revealed)
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
        }
        public Card TargetAnyCard(MouseState mouseState, HorizontalContainer container, Card card, BoardFunctionality boardFunc)
        {
            if(TargetCard(mouseState, container, card, boardFunc.friendlySide) != null)
            {
                return TargetCard(mouseState, container, card, boardFunc.friendlySide);
            }
            if (TargetCard(mouseState, container, card, boardFunc.enemySide) != null)
            {
                return TargetCard(mouseState, container, card, boardFunc.enemySide);
            }
            return null;
        }
        public Card TargetEnemyCard(MouseState mouseState, HorizontalContainer container, Card card, BoardFunctionality boardFunc)
        {
            return TargetCard(mouseState, container, card, boardFunc.enemySide);
        }
        public Card TargetFriendlyCard(MouseState mouseState, HorizontalContainer container, Card card, BoardFunctionality boardFunc)
        {
            return TargetCard(mouseState, container, card, boardFunc.friendlySide);
        }
        public Card TargetCard(MouseState mouseState, HorizontalContainer container, Card card, Side side)
        {
                if ((mouseState.LeftButton == ButtonState.Released && !container.isWithinModifiedPosition(mouseState, card)))
                {
                    foreach (FunctionalRow row in side.Rows)
                    {
                        if (row.playState == Card.PlayState.Revealed)
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
    }
    public class FieldUnitLogic : RowLogic
    {
        public FieldUnitLogic()
        {
        }
        public override void fieldLogic(MouseState mouseState, FunctionalRow row, Card card, BoardFunctionality boardFunc)
        {
            if (clickedInCardBox)
            {
                selectAction.SetTargetCard(mouseState, row, card, boardFunc.enemySide);
                if (selectAction.TargetEnemyCard(mouseState, row, card, boardFunc) != null)
                {
                    boardFunc.Fight(card, selectAction.TargetEnemyCard(mouseState, row, card, boardFunc));
                }
                resetIfNoSelection(mouseState, row, card, boardFunc);
            }
        }
    }
}

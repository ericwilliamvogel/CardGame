using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public class RowFunctionality
    {
        GeneralLogic generalLogic;
        FieldUnitLogic fieldUnitLogic;
        ArmyLogic armyLogic;
        public RowFunctionality()
        {
            generalLogic = new GeneralLogic();
            fieldUnitLogic = new FieldUnitLogic();
            armyLogic = new ArmyLogic();
        }
        public void rowLogic(MouseState mouseState, BoardFunctionality boardFunc)
        {
            rowLogicForSide(mouseState, boardFunc.friendlySide, boardFunc, true);
            rowLogicForSide(mouseState, boardFunc.enemySide, boardFunc, false);
        }

        private void rowLogicForSide(MouseState mouseState, Side side, BoardFunctionality boardFunc, bool friendly)
        {
            foreach (FunctionalRow row in side.Rows)
            {
                switch (row.type)
                {
                    case CardType.Army:
                        break;
                    case CardType.FieldUnit:
                        fieldUnitLogic.setCardToView(mouseState, row, boardFunc, friendly);
                        break;
                    case CardType.General:
                        generalLogic.setCardToView(mouseState, row, boardFunc, friendly);
                        break;
                }
            }
        }
        public void fieldLogic(MouseState mouseState, FunctionalRow row, Card card, BoardFunctionality boardFunc)
        {



        }
    }
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
                if (card.isSelected() && !card.cardProps.exhausted)
                {
                    if (friendly)
                        boardFunc.SELECTEDCARD = card;
                    if (!friendly)
                        boardFunc.ENEMYSELECTEDCARD = card;

                    if (boardFunc.state == BoardFunctionality.State.Regular)
                    {
                        viewLogic(mouseState, row, card, boardFunc);
                        fieldLogic(mouseState, row, card, boardFunc);
                    }
                    else
                    {
                        boardFunc.viewCardWithAbilities(mouseState, card);
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
                boardFunc.state = BoardFunctionality.State.CardView;

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
                if (selectAction.TargetEnemyCard(mouseState, boardFunc) != null && !row.isWithinModifiedPosition(mouseState, card))
                {
                    boardFunc.Fight(card, selectAction.TargetEnemyCard(mouseState,boardFunc));
                }
                resetIfNoSelection(mouseState, row, card, boardFunc);
            }
        }
    }
}

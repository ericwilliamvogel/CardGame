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
                        armyLogic.setCardToView(mouseState, row, boardFunc, friendly);
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

        public virtual void setCardToView(MouseState mouseState, FunctionalRow row, BoardFunctionality boardFunc, bool friendly)
        {
            foreach (Card card in row.cardsInContainer)
            {
                if (card.isSelected())
                {
                    if (friendly)
                    {
                        boardFunc.SELECTEDCARD = card;
                        if (boardFunc.state != BoardFunctionality.State.CardView)
                        {
                            
                            fieldLogic(mouseState, row, card, boardFunc);
                            viewLogic(mouseState, row, card, boardFunc);
                        }
                        else
                        {
                            boardFunc.cardViewer.viewCardWithAbilities(mouseState, card, boardFunc);

                        }
                    }
                    if (!friendly && row.revealed)
                    {
                        if (boardFunc.state != BoardFunctionality.State.CardView)
                        {
                            boardFunc.ENEMYSELECTEDCARD = card;
                            //boardFunc.SELECTEDCARD = card;
                            viewLogic(mouseState, row, card, boardFunc);
                        }
                        else
                        {
                            boardFunc.cardViewer.viewFullSizeCard(mouseState, card, boardFunc);
                        }
                    }
                }

                /*if (boardFunc.state == BoardFunctionality.State.Regular)
                {
                    viewLogic(mouseState, row, card, boardFunc);
                }*/
            }
        }
        public virtual void clickAndHold()
        {

        }
        public void viewLogic(MouseState mouseState, FunctionalRow row, Card card, BoardFunctionality boardFunc)
        {
            if (mouseState.LeftButton == ButtonState.Pressed && row.isWithinModifiedPosition(mouseState, card))
            {
                //card.setPos(mouseState.X - card.getWidth() / 2, mouseState.Y - card.getHeight() / 2);
                clickedInCardBox = true;
            }

            if (mouseState.LeftButton == ButtonState.Released && clickedInCardBox)
            {
                if(row.isWithinModifiedPosition(mouseState, card))
                {
                    clickedInCardBox = false;
                    boardFunc.state = BoardFunctionality.State.CardView;
                }
                clickedInCardBox = false;
                MouseTransformer.Set(MouseTransformer.State.Reg);
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
                boardFunc.ENEMYSELECTEDCARD = null;
                boardFunc.boardPosLogic.updateBoard(boardFunc);
                MouseTransformer.Set(MouseTransformer.State.Reg);
            }
        }

    }
    public class ArmyLogic : RowLogic
    {
        bool rightClickedInBox;
        public override void fieldLogic(MouseState mouseState, FunctionalRow row, Card card, BoardFunctionality boardFunc)
        {
            if (mouseState.RightButton == ButtonState.Pressed && row.isWithinModifiedPosition(mouseState, card) && !card.cardProps.exhausted)
            {
                rightClickedInBox = true;
            }
            if (mouseState.RightButton == ButtonState.Released && rightClickedInBox && row.isWithinModifiedPosition(mouseState, card))
            {
                rightClickedInBox = false;
                card.cardProps.exhausted = true;
                
                boardFunc.friendlySide.Resources.Add(card.race);
                boardFunc.cardViewer.resetSelectedCard(boardFunc);
                card.setRegular();
            }

            resetIfNoSelection(mouseState, row, card, boardFunc);
        }
        public override void setCardToView(MouseState mouseState, FunctionalRow row, BoardFunctionality boardFunc, bool friendly)
        {
            base.setCardToView(mouseState, row, boardFunc, friendly);
            foreach (Card card in row.cardsInContainer)
            {
                if(friendly)
                fieldLogic(mouseState, row, card, boardFunc);
            }
        }
    }
    public class GeneralLogic : RowLogic
    {
        public override void fieldLogic(MouseState mouseState, FunctionalRow row, Card card, BoardFunctionality boardFunc)
        {
            if (clickedInCardBox)
            {
                if ( !row.isWithinModifiedPosition(mouseState, card))
                {
                    resetIfNoSelection(mouseState, row, card, boardFunc);
                }
            }
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
                if (!row.isWithinModifiedPosition(mouseState, card))
                {
                        if (selectAction.TargetEnemyCard(mouseState, boardFunc, false) != null)
                    {

                        MouseTransformer.Set(MouseTransformer.State.Reg);
                        if (selectAction.TargetEnemyCard(mouseState, boardFunc, false).correctRow(boardFunc.enemySide).revealed)
                        {
                            switch (selectAction.TargetEnemyCard(mouseState, boardFunc, false).correctRow(boardFunc.enemySide).type)
                            {
                                case CardType.FieldUnit:
                                    boardFunc.Fight(card, selectAction.TargetEnemyCard(mouseState, boardFunc, false));
                                    break;
                                case CardType.Army:
                                    if (boardFunc.enemySide.Rows[Side.FieldUnit].isEmpty())
                                    {
                                        boardFunc.Fight(card, selectAction.TargetEnemyCard(mouseState, boardFunc, false));
                                    }
                                    else if (!boardFunc.enemySide.Rows[Side.FieldUnit].isEmpty())
                                    {
                                        boardFunc.BOARDMESSAGE.addMessage("Cannot fight an Army if a FieldUnit is present on the board!");

                                    }
                                    break;
                                case CardType.General:
                                    boardFunc.BOARDMESSAGE.addMessage("A FieldUnit cannot target a General!");
                                    break;
                            }




                        }

                    }
                    else
                    {
                        MouseTransformer.Set(MouseTransformer.State.Atk);
                        resetIfNoSelection(mouseState, row, card, boardFunc);

                    }
                }
                if(mouseState.LeftButton == ButtonState.Released && boardFunc.enemySide.Life.isWithinBox(mouseState) )
                {
                    if (boardFunc.enemySide.Rows[Side.FieldUnit].isEmpty() && boardFunc.enemySide.Rows[Side.Armies].isEmpty() && boardFunc.enemySide.Rows[Side.Armies].revealed)
                    {
                        boardFunc.LifeDamage(card);
                    }
                    else
                    {
                        boardFunc.BOARDMESSAGE.addMessage("---> or Fog");
                        boardFunc.BOARDMESSAGE.addMessage("---> an Enemy Army");
                        boardFunc.BOARDMESSAGE.addMessage("---> an Enemy FieldUnit");
                        boardFunc.BOARDMESSAGE.addMessage("Cannot deal damage to a player if there's:");
                    }
                    resetIfNoSelection(mouseState, row, card, boardFunc);
                }
                resetIfNoSelection(mouseState, row, card, boardFunc);

            }
        }
    }
}

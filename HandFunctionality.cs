using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public class HandFunctionality
    {
        public bool placingCard;
        private bool isWithinProperRow(MouseState mouseState, BoardFunctionality boardFunc)
        {
            if (boardFunc.SELECTEDCARD != null)
            {
                foreach (FunctionalRow row in boardFunc.friendlySide.Rows)
                {
                    if (row.isWithinBox(mouseState) && mouseState.LeftButton == ButtonState.Released && row.type == boardFunc.SELECTEDCARD.cardProps.type)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public void playManuever(MouseState mouseState, Card card, BoardFunctionality boardFunc)
        {
            if (!boardFunc.friendlySide.Hand.isWithinModifiedPosition(mouseState, card) && boardFunc.state != BoardFunctionality.State.CardView && card.cardProps.type == CardType.Manuever && mouseState.LeftButton == ButtonState.Released)
            {
                if (card == boardFunc.SELECTEDCARD)
                {
                    placingCard = false;
                    card.setRegular();
                    boardFunc.PlayCard(boardFunc.friendlySide,/* row,*/ card);

                    boardFunc.SELECTEDCARD = null;

                }

            }
        }
        public void playSelectedCard(MouseState mouseState, BoardFunctionality boardFunc)
        {
            if (isWithinProperRow(mouseState, boardFunc) && boardFunc.state != BoardFunctionality.State.CardView)
            {
                foreach (Card card in boardFunc.friendlySide.Hand.cardsInContainer)
                {
                    if (card == boardFunc.SELECTEDCARD)
                    {

                        foreach (FunctionalRow row in boardFunc.friendlySide.Rows)
                        {
                            if (row.type == card.cardProps.type)
                            {
                                placingCard = false;
                                card.setRegular();
                                boardFunc.PlayCard(boardFunc.friendlySide,/* row,*/ card);

                                boardFunc.SELECTEDCARD = null;
                            }
                        }


                    }
                }
            }

        }
        bool clickedInCardBox = false;

        public void setCardToMouse(MouseState mouseState, BoardFunctionality boardFunc)
        {
            foreach (Card card in boardFunc.friendlySide.Hand.cardsInContainer)
            {
                if (card.isSelected())
                {
                    if (boardFunc.state != BoardFunctionality.State.CardView)
                    {
                        updateCardInteractivityInHand(mouseState, card, boardFunc);
                    }
                    else
                    {
                        boardFunc.cardViewer.viewFullSizeCard(mouseState, card, boardFunc);
                    }
                }

            }
        }
        private void updateCardInteractivityInHand(MouseState mouseState, Card card, BoardFunctionality boardFunc)
        {
            boardFunc.SELECTEDCARD = card;
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                card.setPos(mouseState.X - card.getWidth() / 2, mouseState.Y - card.getHeight() / 2);
                clickedInCardBox = true;
                if (!boardFunc.friendlySide.Hand.isWithinModifiedPosition(mouseState, card))
                {
                    placingCard = true;
                }
            }

            if (mouseState.LeftButton == ButtonState.Released && !isWithinProperRow(mouseState, boardFunc) && !boardFunc.friendlySide.Hand.isWithinModifiedPosition(mouseState, card))
            {
                playManuever(mouseState, card, boardFunc);
                placingCard = false;
                clickedInCardBox = false;
                card.setRegular();
                boardFunc.SELECTEDCARD = null;
                boardFunc.boardPosLogic.updateBoard(boardFunc);
            }
            if (mouseState.LeftButton == ButtonState.Released && clickedInCardBox && boardFunc.friendlySide.Hand.isWithinModifiedPosition(mouseState, card))
            {
                placingCard = false;
                clickedInCardBox = false;
                boardFunc.state = BoardFunctionality.State.CardView;
                card.resetCardSelector();
            }
        }

    }
}

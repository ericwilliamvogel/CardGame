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
        public void playSelectedCard(MouseState mouseState, BoardFunctionality boardFunc)
        {
            if (isWithinProperRow(mouseState, boardFunc) && !boardFunc.cardView)
            {
                foreach (Card card in boardFunc.friendlySide.Hand.cardsInContainer)
                {
                    if (card == boardFunc.SELECTEDCARD)
                    {

                        foreach (FunctionalRow row in boardFunc.friendlySide.Rows)
                        {
                            if (row.type == card.cardProps.type)
                            {
                                card.setRegular();
                                boardFunc.PlayCard(boardFunc.friendlySide, row, card);

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
                    if (!boardFunc.cardView)
                    {
                        updateCardInteractivityInHand(mouseState, card, boardFunc);
                    }
                    else
                    {
                        boardFunc.viewFullSizeCard(mouseState, card);
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
            }
            if (mouseState.LeftButton == ButtonState.Released && !isWithinProperRow(mouseState, boardFunc) && !boardFunc.friendlySide.Hand.isWithinModifiedPosition(mouseState, card))
            {
                clickedInCardBox = false;
                card.setRegular();
                boardFunc.SELECTEDCARD = null;
                boardFunc.boardPosLogic.updateBoard(boardFunc);
            }
            if (mouseState.LeftButton == ButtonState.Released && clickedInCardBox && boardFunc.friendlySide.Hand.isWithinModifiedPosition(mouseState, card))
            {
                clickedInCardBox = false;
                boardFunc.cardView = true;

            }
        }

    }
}

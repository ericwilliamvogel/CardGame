using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public class BoardCardDrawer
    {
        public void drawSprite(SpriteBatch spriteBatch, BoardFunctionality boardFunc)
        {

            drawStack(boardFunc.enemySide.Deck, spriteBatch);
            drawStack(boardFunc.friendlySide.Deck, spriteBatch);

            drawStack(boardFunc.enemySide.Oblivion, spriteBatch);
            drawStack(boardFunc.friendlySide.Oblivion, spriteBatch);


            drawRows(boardFunc.enemySide, spriteBatch);
            drawRows(boardFunc.friendlySide, spriteBatch);

            drawHand(boardFunc.enemySide, spriteBatch);
            drawHand(boardFunc.friendlySide, spriteBatch);

            if (boardFunc.SELECTEDCARD != null)
            {
                boardFunc.SELECTEDCARD.drawSprite(spriteBatch);
            }
        }
        private void drawHand(Side side, SpriteBatch spriteBatch)
        {
            foreach (Card card in side.Hand.cardsInContainer)
            {
                card.drawSprite(spriteBatch);
            }
        }
        private void drawRows(Side side, SpriteBatch spriteBatch)
        {
            foreach (FunctionalRow row in side.Rows)
            {
                foreach (Card card in row.cardsInContainer)
                {
                    card.drawSprite(spriteBatch);
                }
            }

        }
        private void drawStack(CardContainer container, SpriteBatch spriteBatch)
        {
            if (!container.isEmpty())
            {
                if (!container.hasAtLeastTwoCards())
                {
                    container.cardsInContainer[1].setCardBackColor(Color.Green);
                    container.cardsInContainer[1].drawSprite(spriteBatch);

                }
                container.cardsInContainer[0].setCardBackColor(Color.White);
                container.cardsInContainer[0].drawSprite(spriteBatch);
            }
        }
    }
}

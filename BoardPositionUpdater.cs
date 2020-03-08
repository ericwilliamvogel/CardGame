using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public class BoardPositionUpdater
    {
        Card SELECTEDCARD;
        public void updateBoard(BoardFunctionality boardFunc) //after every Action
        {
            updateHandPositions(boardFunc);
            updateBoardPositions(boardFunc);
            updateDeckPositions(boardFunc.friendlySide);
            updateDeckPositions(boardFunc.enemySide);
            boardFunc.updateAllAssets();
            this.SELECTEDCARD = boardFunc.SELECTEDCARD;
        }


        private void updateDeckPositions(Side side)
        {
            foreach (Card card in side.Deck.cardsInContainer)
            {
                card.setPos(side.Deck.getPosition());
                scaleToBoard(card);
                card.playState = Card.PlayState.Hidden;
            }
        }
        private void setHandPositions(Side side)
        {
            int counter = 0;
            int spacing = 20;
            side.Hand.resetCardSpacingInHorizontalContainer();
            foreach (Card card in side.Hand.cardsInContainer)
            {
                if (card != SELECTEDCARD && !card.makingAction)
                {
                    scaleToHand(card);
                    Vector2 newPosition = new Vector2(side.Hand.getPosition().X + GraphicsSettings.toResolution(spacing) + counter * (card.getWidth() - side.Hand.horizontalSpacing), side.Hand.getPosition().Y);
                    card.setPos(newPosition);

                }

                counter++;
            }
        }
        private void updateHandPositions(BoardFunctionality boardFunc)
        {
            setHandPositions(boardFunc.friendlySide);
            setContainerPlayState(boardFunc.friendlySide.Hand, Card.PlayState.Revealed);
            setHandPositions(boardFunc.enemySide);
            setContainerPlayState(boardFunc.enemySide.Hand, Card.PlayState.Hidden);
        }
        private void setContainerPlayState(CardContainer container, Card.PlayState playState)
        {
            foreach (Card card in container.cardsInContainer)
            {
                card.playState = playState;
            }
        }
        private void updateBoardPositions(BoardFunctionality boardFunc)
        {
            for (int i = 0; i < Side.MaxRows; i++)
            {
                setBoardPosition(boardFunc.friendlySide.Rows[i]);
                setBoardPosition(boardFunc.enemySide.Rows[i]);
            }

        }
        private void setBoardPosition(FunctionalRow row)
        {
            int counter = 0;
            int spacing = 10; //already Preset
            row.resetCardSpacingInHorizontalContainer();

            foreach (Card card in row.cardsInContainer)
            {
                if (card != SELECTEDCARD && !card.makingAction)
                {
                    scaleToBoard(card);

                    row.setCorrectCenterSpacing(card);
                    Vector2 newPosition = new Vector2(row.getPosition().X - row.initialSpacing + spacing + counter * (card.getWidth() - row.horizontalSpacing), row.getPosition().Y);
                    card.setPos(newPosition);

                }
                counter++;
            }
        }
        public void scaleToView(Card card)
        {
            card.setScale(CardScale.View);
            card.initSupplements();
        }
        public void scaleToBoard(Card card)
        {
            card.setScale(CardScale.Board);
            card.initSupplements();
        }
        public void scaleToHand(Card card)
        {
            card.setScale(CardScale.Hand);
            card.initSupplements();
        }

    }
}

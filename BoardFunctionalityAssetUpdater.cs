using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public class BoardFunctionalityAssetUpdater
    {
        public void updateAllAssets(BoardFunctionality boardFunc)
        {
            foreach (Card card in boardFunc.friendlySide.Deck.cardsInContainer)
            {
                card.updateGameComponent();
            }
            foreach (Card card in boardFunc.enemySide.Deck.cardsInContainer)
            {
                card.updateGameComponent();
            }
            foreach (Card card in boardFunc.friendlySide.Oblivion.cardsInContainer)
            {
                card.updateGameComponent();
            }
            foreach (Card card in boardFunc.enemySide.Oblivion.cardsInContainer)
            {
                card.updateGameComponent();
            }
            foreach (Card card in boardFunc.friendlySide.Hand.cardsInContainer)
            {
                card.updateGameComponent();
            }
            foreach (Card card in boardFunc.enemySide.Hand.cardsInContainer)
            {
                card.updateGameComponent();
            }
            foreach(Card card in boardFunc.castManuever.cardsInContainer)
            {
                card.updateGameComponent();
            }

            foreach (FunctionalRow row in boardFunc.friendlySide.Rows)
            {
                foreach (Card card in row.cardsInContainer)
                {
                    card.updateGameComponent();
                }
            }
            foreach (FunctionalRow row in boardFunc.enemySide.Rows)
            {
                foreach (Card card in row.cardsInContainer)
                {
                    card.updateGameComponent();
                }
            }

            foreach (FunctionalRow row in boardFunc.enemySide.Rows)
            {
                if (row.playState != PlayState.Hidden)
                {
                    row.revealCardInContainer();
                }
            }

        }
    }
}

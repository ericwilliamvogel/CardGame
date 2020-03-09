using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public class Player
    {
        public enum TurnCycle
        {
            Beginning
        }

        public Deck deck;

        private bool control;
        public Player enemy;
        public int handSize = 7;
        public Player()
        {

        }
        public Player(Side side)
        {
            this.deck = deck;
        }

        public void DrawHand()
        {
            for (int i = 0; i < handSize; i++)
            {
                //hand
            }
        }
        public void Draw()
        {
            //Card card = deck.returnTopCardAndRemoveFromDeck();
            //hand.addCard(card);
        }
        public void ShuffleDeck()
        {
            deck.Shuffle();
        }
        public virtual void hasControl()
        {
            control = true;
        }
        public virtual void loseControl()
        {
            control = false;
        }
        public virtual void ResetPlayer()
        {
            hasPlayedArmyThisTurn = false;
        }
        public virtual void decide(BoardFunctionality boardFunc)
        {

        }
        protected bool hasPlayedArmyThisTurn;
    }
    public class AIPlayer : Player
    {

        int currentArmyCount;
        public override void decide(BoardFunctionality boardFunc)
        {
            foreach(Card card in boardFunc.enemySide.Hand.cardsInContainer)
            {
                boardFunc.PlayCard(boardFunc.enemySide, boardFunc.enemySide.Rows[Side.FieldUnit], card);
                //playArmies(card, boardFunc);
            }
            //playCardIfThereAreEnoughArmies(boardFunc);
            //throw new
            boardFunc.PassTurn();
        }
        private void playArmies(Card card, BoardFunctionality boardFunc)
        {
            if(card.cardProps.type == CardType.Army && !hasPlayedArmyThisTurn)
            {
                boardFunc.PlayCard(boardFunc.enemySide, boardFunc.enemySide.Rows[Side.Armies], card);
            }
        }
        private void playCardIfThereAreEnoughArmies(BoardFunctionality boardFunc)
        {
            int counter = 1;
            foreach(Card newCard in boardFunc.enemySide.Rows[Side.Armies].cardsInContainer)
            {
                if(!newCard.cardProps.exhausted)
                {
                    counter++;
                }

            }
            foreach(Card newCard in boardFunc.enemySide.Hand.cardsInContainer)
            {
                if(newCard.cardProps.cost <= counter)
                {
                    boardFunc.PlayCard(boardFunc.enemySide, boardFunc.enemySide.Rows[Side.FieldUnit], newCard);
                }
            }

        }
        
        private FunctionalRow getCorrectRow(Card card, BoardFunctionality boardFunc)
        {
            foreach(FunctionalRow row in boardFunc.enemySide.Rows)
            {
                if(row.type == card.cardProps.type)
                {
                    return row;
                }
            }
            return null;
        }
    }
}

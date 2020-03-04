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

        Deck deck;
        Hand hand;
        private bool control;
        public Player enemy;
        public int handSize = 7;
        public Player()
        {

        }
        public Player(Deck deck)
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
            Card card = deck.returnTopCardAndRemoveFromDeck();
            hand.addCard(card);
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
        public virtual void PassTurn()
        {

        }
    }
    public class AIPlayer
    {

    }
}

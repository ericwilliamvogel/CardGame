using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public class Hand
    {
        List<Card> cardsInHand = new List<Card>();
        public void addCard(Card card)
        {
            cardsInHand.Add(card);
        }
    }
    public class Deck
    {
        List<Card> cardsInDeck = new List<Card>();

        public void loadCardsInDeck(Dictionary<string, Texture2D> imageDictionary)
        {
            /*List<Card> cardsInDeck;
            foreach(Card card in cardsInDeck)
            {

            }*/
        }

        public void Shuffle()
        {
            int split = cardsInDeck.Count / 2;
            Card[] oneHalf = new Card[split];
            cardsInDeck.CopyTo(0, oneHalf, 0, split);
            cardsInDeck.RemoveRange(0, split);

            Card[] otherHalf = new Card[cardsInDeck.Count];
            cardsInDeck.CopyTo(otherHalf);
            cardsInDeck.Clear();

            cardsInDeck = rearrangeCards(oneHalf, otherHalf);
        }
        private List<Card> rearrangeCards(Card[] oneHalf, Card[] otherHalf)
        {
            oneHalf.Reverse();
            otherHalf.Reverse();

            List<Card> returnDeck = new List<Card>();
            int lowHalf = lowerHalf(oneHalf, otherHalf);
            for (int i = 0; i < lowHalf; i++)
            {
                returnDeck.Add(oneHalf[i]);
                returnDeck.Add(otherHalf[i]);
            }

            //refactor later
            if (oneHalf.Length > lowHalf)
            {
                for (int i = lowHalf; i < oneHalf.Length; i++)
                {
                    returnDeck.Add(oneHalf[i]);
                }
            }
            if (otherHalf.Length > lowHalf)
            {
                for (int i = lowHalf; i < otherHalf.Length; i++)
                {
                    returnDeck.Add(otherHalf[i]);
                }
            }

            return returnDeck;
        }


        private int lowerHalf(Card[] oneHalf, Card[] otherHalf)
        {
            if (oneHalf.Length < otherHalf.Length)
            {
                return oneHalf.Length;
            }
            else
            {
                return otherHalf.Length;
            }
        }


        public Card returnTopCardAndRemoveFromDeck()
        {
            Card card = cardsInDeck[0];
            cardsInDeck.Remove(card);
            return card;
        }
    }
    public class PlayerCollection
    {
        Dictionary<Card, int> cardCollection = new Dictionary<Card, int>();

        public void addCardToCollection(Card card)
        {
            if (cardCollection.ContainsKey(card))
            {
                cardCollection[card]++;
            }
            else
            {
                throw new Exception("card not found in collection");
            }
        }
    }
}

using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public class Deck : CardContainer
    {



        public void KidsShuffle()
        {
            int numberOfPiles = 10;
            List<List<Card>> piles = new List<List<Card>>();
            for(int i = 0; i < numberOfPiles; i++)
            {
                piles.Add(new List<Card>());
            }

            int counter = 0;
            foreach(Card card in cardsInContainer)
            {
                piles[counter].Add(card);

                counter++;
                if(counter >= numberOfPiles)
                {
                    counter = 0;
                }
            }

            for(int i = 0; i < numberOfPiles; i++)
            {

            }
            List<Card> shuffledDeck = new List<Card>();
        }
        public void importedShuffle()
        {
            Shuffle(cardsInContainer);
        }
        //took this off stackoverflow :D!
        private Random rng = new Random();

        public void Shuffle(List<Card> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Card value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
        public void Shuffle()
        {
            int split = cardsInContainer.Count / 2;
            Card[] oneHalf = new Card[split];
            cardsInContainer.CopyTo(0, oneHalf, 0, split);
            cardsInContainer.RemoveRange(0, split);

            Card[] otherHalf = new Card[cardsInContainer.Count];
            cardsInContainer.CopyTo(otherHalf);
            cardsInContainer.Clear();

            cardsInContainer = rearrangeCards(oneHalf, otherHalf);
        }


        private List<Card> rearrangeCards(Card[] oneHalf, Card[] otherHalf)
        {
            oneHalf.Reverse();
            //otherHalf.Reverse();

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

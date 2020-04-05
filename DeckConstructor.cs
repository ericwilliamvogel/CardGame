using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public class DeckConstructor
    {
        Deck deck;
        public CardBuilder cardBuilder = new CardBuilder();
        public CardConstructor cardConstructor = new CardConstructor();

        //


            //
        public void setDeck()
        {
            deck = new Deck();
        }
        public void addCardsToDeck(int numberOfThisCard, int identifier)
        {

            deck.cardsInContainer.AddRange(returnIdentifiedCards(numberOfThisCard, identifier));
        }
        private List<Card> returnIdentifiedCards(int numberOfThisCard, int identifier)
        {
            List<Card> tempList = new List<Card>();

            for (int i = 0; i < numberOfThisCard; i++)
            {
                Card card = cardBuilder.cardConstruct(cardConstructor, identifier);
                tempList.Add(card);
            }
            loadAllPossibleTextures();

            return tempList;
        }
        private void loadAllPossibleTextures()
        {
            Card card = cardBuilder.cardConstruct(cardConstructor, 10);
            card = cardBuilder.cardConstruct(cardConstructor, 11);
            card = cardBuilder.cardConstruct(cardConstructor, 6);
            card = cardBuilder.cardConstruct(cardConstructor, 1003);
        }
        public Deck getDeck()
        {
            for (int i = 0; i < 5; i++)
            {
                deck.importedShuffle();
            }

            return deck;
        }
    }
}

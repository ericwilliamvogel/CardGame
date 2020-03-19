using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public class DeckBuilder
    {

        public Deck getDeck(DeckConstructor constructor, string identifier)
        {
            constructor.setDeck();
            deckAssembly(constructor, identifier);
            return constructor.getDeck();
        }
        public void deckAssembly(DeckConstructor constructor, string identifier)
        {

            if(identifier == "TESTDECK")
            {
                constructor.addCardsToDeck(10, 1);
                constructor.addCardsToDeck(10, 2);
                constructor.addCardsToDeck(10, 3);
                constructor.addCardsToDeck(10, 4);
                constructor.addCardsToDeck(10, 0);
            }
            else if(identifier == "TESTDECK2")
            {
                constructor.addCardsToDeck(10, 0);
                constructor.addCardsToDeck(10, 10);
                constructor.addCardsToDeck(10, 11);
                constructor.addCardsToDeck(10, 0);
                constructor.addCardsToDeck(10, 0);
            }
            else
            {
                throw new Exception("deck didn't load my guy");
            }

        }
        
    }

}

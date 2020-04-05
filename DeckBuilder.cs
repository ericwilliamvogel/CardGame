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
                constructor.addCardsToDeck(10, 6);
                constructor.addCardsToDeck(4, 5);
                constructor.addCardsToDeck(4, 7);
                constructor.addCardsToDeck(16, 2);
                constructor.addCardsToDeck(4, 3);
                constructor.addCardsToDeck(4, 8);
                constructor.addCardsToDeck(4, 9);
                constructor.addCardsToDeck(10, 4);

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
                throw new Exception("deck didn't load my guy, need to check and see if your identifier is correct");
            }

        }
        
    }

}

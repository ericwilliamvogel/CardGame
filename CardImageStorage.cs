using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public class CardLibrary
    {

    }
    public class Set
    {
        public List<Card> cards = new List<Card>();
        public void addCard(int identifier)
        {
            if (!File.Exists(identifier.ToString()))
            {
                throw new Exception("couldn't find card image file per card identifier");
            }

        }
    }


    public class CardImageStorage
    {
        public Dictionary<int, Texture2D> cardTextureDictionary;
        public CardSupplementalTextures suppTextures;
        public CardImageStorage()
        {
            suppTextures = new CardSupplementalTextures();
            cardTextureDictionary = new Dictionary<int, Texture2D>();
        }
        public void fillDictionary(int identifier)
        {
            if(!cardTextureDictionary.ContainsKey(identifier))
            cardTextureDictionary.Add(identifier, null);
        }
        public void loadCardSupplementalTextures(ContentManager content)
        {
            suppTextures.cardBack.setTexture(content);
            suppTextures.cardBorder.setTexture(content);
            suppTextures.cardImageBorder.setTexture(content);
            suppTextures.cardFilling.setTexture(content);
        }
        public void loadAllDictionaryTextures(ContentManager content)
        {
            string defaultTexture = "notLoadedPortrait";
            Dictionary<int, Texture2D> newDictionary = new Dictionary<int, Texture2D>();
            foreach (KeyValuePair<int, Texture2D> pair in cardTextureDictionary)
            {
                int key = pair.Key;
                Texture2D checkIfNull = null;
                try {
                    checkIfNull = content.Load<Texture2D>(key.ToString());
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                }


                if (checkIfNull != null)
                {
                    newDictionary.Add(pair.Key, content.Load<Texture2D>(key.ToString()));
                }
                else
                {
                    newDictionary.Add(pair.Key, content.Load<Texture2D>(defaultTexture));
                }

            }
            cardTextureDictionary = newDictionary;

        }

    }
}

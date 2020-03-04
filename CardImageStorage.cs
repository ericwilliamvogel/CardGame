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
    public static class CardImageStorage
    {
        public static Dictionary<int, Texture2D> cardTextureDictionary = new Dictionary<int, Texture2D>();

        static CardImageStorage()
        {

        }
        private static void fillDictionary(int identifier)
        {
            cardTextureDictionary.Add(identifier, null);
        }
        public static void loadAllDictionaryTextures(ContentManager content)
        {
            foreach (KeyValuePair<int, Texture2D> pair in cardTextureDictionary)
            {
                int key = pair.Key;
                cardTextureDictionary.Remove(key);
                cardTextureDictionary.Add(pair.Key, content.Load<Texture2D>(key.ToString()));

            }
        }

    }
}

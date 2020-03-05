using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public class Side
    {
        public Side(Player player)
        {
            Deck = player.deck;
            Player = player;
            Deck = new Deck();
            Generals = new FunctionalRow(CardType.General);
            Armies = new FunctionalRow(CardType.Army);
            FieldUnit = new FunctionalRow(CardType.Field);
            Hand = new CardContainer();
            Oblivion = new CardContainer();
        }
        public Deck Deck;
        public Player Player;
        public FunctionalRow Generals;
        public FunctionalRow Armies;
        public FunctionalRow FieldUnit;
        public CardContainer Hand;
        public CardContainer Oblivion;
    }
}

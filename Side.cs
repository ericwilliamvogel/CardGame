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
            Rows = new FunctionalRow[MaxRows];
            Rows[General] = new FunctionalRow(CardType.General);
            Rows[Armies] = new FunctionalRow(CardType.Army);
            Rows[FieldUnit] = new FunctionalRow(CardType.FieldUnit);
            Hand = new Hand();
            Oblivion = new CardContainer();
        }
        public Deck Deck;
        public Player Player;
        public FunctionalRow[] Rows;
        public static int General = 0;
        public static int Armies = 1;
        public static int FieldUnit = 2;
        public static int MaxRows = 3;
        public Hand Hand;
        public CardContainer Oblivion;
    }
}

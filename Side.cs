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
            Resources = new List<Card.Race>();
            Rows[General] = new FunctionalRow(CardType.General);
            Rows[General].setVisibility(false);
            Rows[Armies] = new ArmyRow(CardType.Army);
            Rows[Armies].setVisibility(false);
            Rows[FieldUnit] = new FunctionalRow(CardType.FieldUnit);
            Rows[FieldUnit].setVisibility(true);
            boardFunc = new BoardFunctionality();
            Hand = new Hand();
            Oblivion = new CardContainer();
        }
        public bool canPlayArmy;
        public BoardFunctionality boardFunc;
        public Deck Deck;
        public Player Player;
        public FunctionalRow[] Rows;
        public static int General = 0;
        public static int Armies = 1;
        public static int FieldUnit = 2;
        public static int MaxRows = 3;
        public List<Card.Race> Resources;
        public Hand Hand;
        public CardContainer Oblivion;
    }
}

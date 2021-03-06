﻿using System;
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
            Resources = new List<Race>();
            Rows[General] = new FunctionalRow(CardType.General);
            Rows[General].setVisibility(false);
            Rows[Armies] = new ArmyRow(CardType.Army);
            Rows[Armies].setVisibility(false);
            Rows[FieldUnit] = new FunctionalRow(CardType.FieldUnit);
            Rows[FieldUnit].setVisibility(true);
            boardFunc = new BoardFunctionality();
            Hand = new Hand();
            Oblivion = new CardContainer();
            LifeTotal = SetLife;
            Life = new CardContainer();
        }
        public bool canPlayArmy;
        public BoardFunctionality boardFunc;
        public CardContainer Life;
        public Deck Deck;
        public Player Player;
        public FunctionalRow[] Rows;
        public static int General = 0;
        public static int Armies = 1;
        public static int FieldUnit = 2;
        public static int MaxRows = 3;
        public int LifeTotal = 0;
        private int SetLife = 25;
        public List<Race> Resources;
        public Hand Hand;
        public CardContainer Oblivion;
    }
}

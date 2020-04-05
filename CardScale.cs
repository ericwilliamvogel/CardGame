using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public static class CardScale
    {
        public static float Hand = -(Properties.globalScale.X - Properties.globalScale.X * 1/4);
        public static float View = 0f;
        public static float Cast = -(Properties.globalScale.X - Properties.globalScale.X * 2 / 4);
        public static float Board = -(Properties.globalScale.X - Properties.globalScale.X / 6);

    }
}

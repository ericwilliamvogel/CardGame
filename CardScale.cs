using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public static class CardScale
    {
        public static float Hand = -.6f;
        public static float View = 0f;
        public static float Board = -(Properties.globalScale.X - Properties.globalScale.X / 6);

    }
}

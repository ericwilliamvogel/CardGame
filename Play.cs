using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public class Play
    {
        public int value = 0;
        public Func<int> testPlay;
        public Action realPlay;
        public Action resetValues;
    }
}

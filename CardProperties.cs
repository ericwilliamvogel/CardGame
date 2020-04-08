using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public class CardProperties
    {
        public int identifier;
        public string name;
        public List<Effect> effects = new List<Effect>();
        public List<Ability> abilities = new List<Ability>();
        public Cost cost = new Cost();
        public int initialPower = 0;
        public int initialDefense = 1;
        public int aiCalcDefense = 1;
        public int power = 0;
        public int defense = 1;
        public CardType type;
        public bool exhausted;
        public bool doubleExhausted;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public class FunctionalRow : HorizontalContainer
    {
        public bool revealed;
        public FunctionalRow(CardType type)
        {
            this.type = type;
            containerScale = CardScale.Board;
            setCenterSpacing();
            revealed = false;
        }
        public CardType type;


    }
}

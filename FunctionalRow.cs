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
        public bool revealedTrueValue;
        public FunctionalRow(CardType type)
        {
            this.type = type;
            containerScale = CardScale.Board;
            setCenterSpacing();
            revealed = false;
        }
        public CardType type;
        public void setVisibility(bool revealed)
        {
            this.revealed = revealed;
            this.revealedTrueValue = revealed;
        }

    }
    public class ArmyRow : FunctionalRow
    {
        public ArmyRow(CardType type) : base(type)
        {

        }

            //
    }
}

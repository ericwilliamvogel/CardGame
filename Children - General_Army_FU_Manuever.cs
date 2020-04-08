using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public class General : Card
    {
        public General(int identifier) : base(identifier)
        {
            cardProps.type = CardType.General;
        }
    }
    public class Army : Card
    {
        public Army(int identifier) : base(identifier)
        {
            cardProps.type = CardType.Army;
        }
    }
    public class FieldUnit : Card
    {
        public FieldUnit(int identifier) : base(identifier)
        {
            cardProps.type = CardType.FieldUnit;
        }
    }
    public class Manuever : Card
    {
        public Manuever(int identifier) : base(identifier)
        {
            cardProps.type = CardType.Manuever;
        }
    }
}

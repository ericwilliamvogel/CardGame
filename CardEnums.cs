using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public enum CardType
    {
        General,
        Army,
        FieldUnit,
        Manuever
    }
    public enum Race
    {
        Orc,
        Elf,
        Human,
        Unanimous
    }
    public enum Rarity
    {
        Bronze,
        Silver,
        Gold,
        Diamond
    }
    public enum PlayState
    {
        Revealed,
        Hidden
    }
    public enum SelectState
    {
        Regular,
        Hovered,
        Selected
    }
}

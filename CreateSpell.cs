using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public class CreateSpell : NonTargetAbility
    {
        public int identifier;
        public string details;
        public CreateSpell(int identifier, string details)
        {
            this.identifier = identifier;
            description = "Create and draw " + details;

        }
        public CreateSpell(int identifier, int exchangeValue, string details) : this(identifier, details)
        {
            displayGeneralIncrements(exchangeValue);
        }
        public override void abilityImplementation(MouseState mouseState, BoardFunctionality boardFunc)
        {
            boardFunc.CreateAndDrawSpell(INITIALCARD, this);
        }
    }
}

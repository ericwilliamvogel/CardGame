using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CardGame
{
    public class UniqueButtonPopper<WindowType, ComponentType> : ButtonPopper<WindowType>
       where WindowType : UniqueWindow<ComponentType>
       where ComponentType : GameComponent
    {
        public UniqueButtonPopper(ContentManager content, WindowType input) : base(content, input) { }
        public void updateObj(ComponentType input)
        {
            window.updateObj(input);
        }

    }
}

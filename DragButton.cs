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
    public class DragButton : Button
    {
        public DragButton(ContentManager content, string imagesrc) : base(content, imagesrc)
        {
            type = Type.Hold;
        }
        public Action<MouseState> mouseAction;

        public void setAction(Action<MouseState> newAction)
        {
            setAction();
            mouseAction = newAction;

        }
        public override void performAction(MouseState mouseState)
        {
            mouseAction(mouseState);
        }
    }
}

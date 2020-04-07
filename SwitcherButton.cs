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
    public class SwitcherButton : Button
    {
        private int controller { get; set; }
        private Action<SwitcherButton> switcherAction;

        public SwitcherButton(ContentManager content, Vector2 position, int desiredController) : base(content, position)
        {
            setControllerToSwitch(desiredController);
        }

        public SwitcherButton(ContentManager content, Vector2 position, string imgSrc, int desiredController) : base(content, position, imgSrc)
        {
            setControllerToSwitch(desiredController);
            //settitle or something
        }

        public void setSwitcherAction(Action<SwitcherButton> action)
        {
            setAction();
            switcherAction = action;
        }
        public override void performAction(MouseState mouseState)
        {
            switcherAction(this);
        }
        public void setControllerToSwitch(int desiredController)
        {
            controller = desiredController;
        }
        public int returnComponentToSwitchTo()
        {
            return controller;
        }
    }
}

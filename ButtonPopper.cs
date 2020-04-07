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
   
    public class ButtonPopper<T> : Button where T : Window
    {
        public T window;
        public ButtonPopper(ContentManager content, T input) : base(content)
        {
            window = input;
            initializeGameComponent(content);
            setAction(() => {
                window.Toggle();
            });
        }
        public override void initializeGameComponent(ContentManager content)
        {
                window.initializeGameComponent(content);
        }
        public override void mouseStateLogic(MouseState mouseState, ContentManager content)
        {
            if (window.isShown())
                window.mouseStateLogic(mouseState, content);
            base.mouseStateLogic(mouseState, content);
        }
        public override void drawSprite(SpriteBatch spriteBatch)
        {
            base.drawSprite(spriteBatch);
            if (window.isShown())
                window.drawSprite(spriteBatch);

        }

    }
}

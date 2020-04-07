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
    public class UniqueWindow<T> : Window where T : GameComponent
    {
        T obj;
        public UniqueWindow(ContentManager content, int size) : base(content, size)
        {

        }
        public void updateObj(T input)
        {
            obj = input;
        }
        public override void mouseStateLogic(MouseState mouseState, ContentManager content)
        {
            if (isShown())
            {
                obj.updateGameComponent();
                obj.mouseStateLogic(mouseState, content);
                base.mouseStateLogic(mouseState, content);
            }
        }
        public override void drawSprite(SpriteBatch spriteBatch)
        {
            if (isShown())
            {
                drawFilling(spriteBatch);
                obj.drawSprite(spriteBatch);
                drawButtons(spriteBatch);
                drawExit(spriteBatch);
            }
        }
        public override void setPos(int x, int y)
        {
            base.setPos(x, y);
            obj.setPos(x, y);
        }

    }
}

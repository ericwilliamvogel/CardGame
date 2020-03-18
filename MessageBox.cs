using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CardGame
{
    public class DynamicMessage
    {
        public int spacing = GraphicsSettings.toResolution(45);
        public float transparency = 1f;
        public string message;
        public float size = 1.33f;
        public DynamicMessage(string message)
        {
            this.message = message;
        }
        public void drawSprite(SpriteBatch spriteBatch, MessageBox box)
        {
            spriteBatch.DrawString(Game1.spritefont, message.ToString(), new Vector2(box.getPosition().X, box.yPos), Color.Black * transparency, 0, new Vector2(0, 0), 1.33f * box.getScale(), SpriteEffects.None, 0);
        }
    }
    public class MessageBox : GameComponent
    {
        List<DynamicMessage> messages;
        public MessageBox()
        {
            messages = new List<DynamicMessage>();
            setPos(Game1.windowW / 5, Game1.windowH / 2);
        }
        public void addMessage(string message)
        {
            messages.Add(new DynamicMessage(message));
        }
        public float yPos;
        public override void drawSprite(SpriteBatch spriteBatch)
        {
            if (messages.Count > 0)
            {
                int iterator = 0;
                List<DynamicMessage> selector = new List<DynamicMessage>();
                foreach (DynamicMessage message in messages)
                {

                    yPos = getPosition().Y - message.spacing * iterator;
                    message.drawSprite(spriteBatch, this);
                    message.transparency -= .005f;
                    if (message.transparency <= 0)
                    {
                        selector.Add(message);
                    }
                    iterator++;
                }
                if (selector.Count > 0)
                {
                    foreach (DynamicMessage message in selector)
                    {
                        messages.Remove(message);
                    }
                }
            }


            //base.drawSprite(spriteBatch);
        }
    }

}

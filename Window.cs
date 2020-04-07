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
    public class WindowFilling : GameComponent
    {
        public static Texture2D texture;
    }
    public class Window : GameComponent
    {
        public Button closeWindowButton;
        public DragButton dragWindowButton;
        public List<GameComponent> buttons;
        private List<WindowFilling> filling;
        private bool shown;


        public Window(ContentManager content, int size)
        {
            buttons = new List<GameComponent>();
            filling = new List<WindowFilling>();
            int counter = 0;
            for (int i = 0; i < size; i++)
            {
                filling.Add(new WindowFilling());
                filling[i].setTexture(WindowFilling.texture);
                counter++;
            }
            properties.height = filling[0].getHeight() * counter;
            properties.width = filling[0].getWidth();
            closeWindowButton = new Button(content, "closeWindow");
            closeWindowButton.setAction(() => { Close(); });

            dragWindowButton = new DragButton(content, "moveBar");
            dragWindowButton.setType(Button.Type.Hold);
            dragWindowButton.setAction((MouseState newMouseState) => {
                setPos((int)(newMouseState.X - filling[0].getWidth() / 2), (int)(newMouseState.Y + dragWindowButton.getHeight() / 2));
            });
            dragWindowButton.setButtonText("Past Moves");
            dragWindowButton.wantedScale = .7f;
            dragWindowButton.setPresetCentering(10);
            initializeGameComponent(content);
        }
        public override void setPos(int x, int y)
        {

            base.setPos(x, y);
            closeWindowButton.setPos((int)getPosition().X + filling[0].getWidth() - closeWindowButton.getWidth(), (int)getPosition().Y - closeWindowButton.getHeight());
            dragWindowButton.setPos((int)getPosition().X, (int)getPosition().Y - dragWindowButton.getHeight());
            for (int i = 0; i < filling.Count; i++)
            {
                filling[i].setPos((int)(getPosition().X), (int)(getPosition().Y + filling[i].getHeight() * i));
            }
            foreach(GameComponent button in buttons)
            {
                button.setPos((int)getPosition().X, (int)getPosition().Y);
            }
        }
        public void Toggle()
        {
            if (isShown())
            {
                Close();
            }
            else
            {
                Show();
            }
        }
        public void Show()
        {
            shown = true;
        }
        public void Close()
        {
            shown = false;
        }
        public bool isShown()
        {
            if (shown)
            {
                return true;
            }
            return false;
        }


        public override void initializeGameComponent(ContentManager content)
        {
            if (isShown())
            {
                foreach (Button button in buttons)
                {
                    button.initializeGameComponent(content);
                }
            }
        }

        public override void mouseStateLogic(MouseState mouseState, ContentManager content)
        {
            if (isShown())
            {
                dragWindowButton.mouseStateLogic(mouseState, content);
                closeWindowButton.mouseStateLogic(mouseState, content);
                foreach (Button button in buttons)
                {
                    button.mouseStateLogic(mouseState, content);
                }
                base.mouseStateLogic(mouseState, content);
            }
        }
        public virtual void addNewComponent(Button button)
        {
            button.setPos((int)getPosition().X, (int)getPosition().Y);
            buttons.Add(button);
        }
        public override void drawSprite(SpriteBatch spriteBatch)
        {
            if (isShown())
            {
                drawFilling(spriteBatch);
                drawButtons(spriteBatch);
                drawExit(spriteBatch);
            }
        }

        protected void drawFilling(SpriteBatch spriteBatch)
        {
            foreach (WindowFilling bg in filling)
            {
                bg.drawSprite(spriteBatch);
            }
        }
        protected void drawButtons(SpriteBatch spriteBatch)
        {
            foreach (Button button in buttons)
            {
                button.drawSprite(spriteBatch);
            }
        }
        protected void drawExit(SpriteBatch spriteBatch)
        {
            dragWindowButton.drawSprite(spriteBatch);
            closeWindowButton.drawSprite(spriteBatch);
        }
    }
}

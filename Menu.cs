using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SiegewithCleanCode
{
    public class Menu : PrimaryComponent
    {
        private List<Button> buttons;
        private LoadingScreenImage loadScreenImage;


        public void setButtons(ContentManager content)
        {
            int buttonPositionX = Game1.windowW / 2 + 300;
            buttons = new List<Button>();

            buttons.Add(new Button(content, new Vector2(buttonPositionX - 300, 200), "Exit Game"));
            switcherButtons.Add(new SwitcherButton(content, new Vector2(buttonPositionX, 900), "Start", 4));
        }

        public override void unloadGameComponent()
        {
            buttons = null;
            switcherButtons.Remove(switcherButtons[0]);

        }
        public void closeWindowLogic(Game1 game)
        {
            if (buttons[0].isPressed())
            {
                game.Exit();
                buttons[0].setClickToFalse();
            }
        }
        public override void initializeGameComponent(ContentManager content)
        {
            loadScreenImage = new LoadingScreenImage(content);
            setButtons(content);
            switcherButtons[0].satisfyCondition();

        }

        public override void updateGameComponent(ContentManager content)
        {
            foreach (Button button in buttons)
            {
                button.updateGameComponent();

            }
        }

        public override void mouseStateLogic(MouseState mouseState, ContentManager content)
        {
            foreach (Button button in buttons)
            {
                button.Update(mouseState);
            }
        }

        public override void drawSprite(SpriteBatch spriteBatch)
        {
            loadScreenImage.Draw(spriteBatch);
            foreach (Button button in buttons)
            {
                button.drawSprite(spriteBatch);
            }
        }

        public void handleButtons()
        {
            setSwitchStatusToOff();
        }
    }


}
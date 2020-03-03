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
using System.IO;
using System.Text.RegularExpressions;

namespace CardGame
{
    public class Background : GameComponent
    {
        
    }
    
    public class Menu : PrimaryComponent
    {
        protected List<Button> buttons;
        protected GameComponent loadScreenImage;

        public virtual void setButtons(ContentManager content)
        {
            Button tempButton = new Button(content, new Vector2(0,0));
            int counter = 0;
            int buttonPositionX = GraphicsSettings.realScreenWidth()/2 - tempButton.getWidth()/2;
            int buttonPositionY = counter * tempButton.getHeight() + tempButton.getHeight() / 3;

            buttons = new List<Button>();

            switcherButtons.Add(new SwitcherButton(content, new Vector2(buttonPositionX, buttonPositionY), 1));
            switcherButtons[0].setButtonText("RELOAD SCREEN");
            counter++;
            buttonPositionY = counter * tempButton.getHeight() + tempButton.getHeight() / 3;
            switcherButtons.Add(new SwitcherButton(content, new Vector2(buttonPositionX, buttonPositionY), 0));
            switcherButtons[1].setButtonText("TO SETTINGS");
            counter++;
            buttonPositionY = counter * tempButton.getHeight() + tempButton.getHeight() / 3;
            buttons.Add(new Button(content, new Vector2(buttonPositionX, buttonPositionY)));
            buttons[0].setAction();
            buttons[0].setButtonText("Exit");
            
        }

        public override void unloadGameComponent()
        {
            //buttons = null;
            //switcherButtons.Remove(switcherButtons[0]);

        }
        public void closeWindowLogic(Game1 game)
        {
            if(buttons != null)
            buttons[0].setAction(() => { game.Exit();  });
        }
        public override void initializeGameComponent(ContentManager content)
        {
            loadScreenImage = new GameComponent();
            loadScreenImage.setContentName("simplemenu");
            loadScreenImage.setTexture(content);

            if(buttons == null)
            setButtons(content);

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
            loadScreenImage.drawSprite(spriteBatch);

            foreach (Button button in buttons)
            {
                button.drawSprite(spriteBatch);
            }
        }
    }


}
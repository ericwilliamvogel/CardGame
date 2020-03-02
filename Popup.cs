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
    public class Popup : PrimaryComponent
    {
        Sprite popupSprite;
        public bool end = false;
        string buttonTitle;
        int mapToSelect;
        public Popup(string buttonText, int switchToThisMap)
        {
            mapToSelect = switchToThisMap;
            buttonTitle = buttonText;

        }

        public override void drawSprite(SpriteBatch spriteBatch)
        {
            if (end == true)
            {
                spriteBatch.Draw(popupSprite.getLoadedTexture(), new Vector2(Game1.windowW / 2 - popupSprite.getTextureParamaters().X / 2, Game1.windowH / 2 - popupSprite.getTextureParamaters().Y / 2), Color.Green);
            }
        }

        public override void initializeGameComponent(ContentManager content)
        {
            popupSprite = new Sprite(content, "popup");
            switcherButtons = new List<SwitcherButton>();
            switcherButtons.Add(new SwitcherButton(content, new Vector2(Game1.windowW / 2 - 100, Game1.windowH / 2 - 100), mapToSelect));
        }

        public override void mouseStateLogic(MouseState mouseState, ContentManager content)
        {

        }

        public override void unloadGameComponent()
        {

        }

        public override void updateGameComponent(ContentManager content)
        {

        }
    }
}

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
        string buttonTitle;
        int mapToSelect;
        bool reveal;
        public Popup(int switchToThisMap)
        {
            mapToSelect = switchToThisMap;
        }

        public override void drawSprite(SpriteBatch spriteBatch)
        {
            if(reveal)
            {
                spriteBatch.Draw(popupSprite.getLoadedTexture(), new Vector2(Game1.windowW / 2 - popupSprite.getTextureParamaters().X / 2, Game1.windowH / 2 - popupSprite.getTextureParamaters().Y / 2), Color.White);
            }
            
        }

        public override void initializeGameComponent(ContentManager content)
        {
            popupSprite = new Sprite(content, "popup");
            switcherButtons = new List<SwitcherButton>();
            switcherButtons.Add(new SwitcherButton(content, new Vector2(-300, -300), "secondButtonTexture",mapToSelect));
            if(switcherButtons[0].getTexture() == null)
            {
                throw new Exception("switcher didn't load");
            }
            switcherButtons[0].setButtonText("Continue");
        }

        public void SetPopup()
        {
            switcherButtons[0].setPos(Game1.windowW / 2 - switcherButtons[0].getWidth() / 2, Game1.windowH / 2 - switcherButtons[0].getHeight() / 2);
            reveal = true;
        }
    }
}

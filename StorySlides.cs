
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
    public class Slide
    {
        public Slide(string incomingMessage, CharacterProfile incomingCharacter)
        {
            message = incomingMessage;
            character = incomingCharacter;
        }
        public string message;
        public CharacterProfile character;
    }
    public class StorySlides : PrimaryComponent
    {
        Popup popup;
        public List<Slide> slides;
        public int slideController;
        private bool slideSwitch;
        private string backgroundSelector;
        Sprite background;
        Sprite textBox;
        CharBuilder charBuilder;
        public StorySlides()
        {
            slides = new List<Slide>(); 
            charBuilder = new CharBuilder();
        }
        public void setBackground(string background)
        {
            backgroundSelector = background;
        }
        public override void drawSprite(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background.getLoadedTexture(), new Vector2(0, 0), Color.White);
            spriteBatch.Draw(textBox.getLoadedTexture(), new Vector2(0, Game1.windowH - textBox.getTextureParamaters().Y), Color.White);
            spriteBatch.Draw(slides[slideController].character.characterImage.getLoadedTexture(), new Vector2(100, Game1.windowH - 900), Color.White);

            spriteBatch.DrawString(Game1.spritefont, charBuilder.returnMessageStillBeingBuilt(), new Vector2(Game1.windowW / 2 - 450, Game1.windowH - 300), Color.Gold, 0, new Vector2(0, 0), 2f, SpriteEffects.None, 0);
            spriteBatch.DrawString(Game1.spritefont, slides[slideController].character.name + ":", new Vector2(200, Game1.windowH - 300), Color.White, 0, new Vector2(0, 0), 2f, SpriteEffects.None, 0);

            spriteBatch.DrawString(Game1.spritefont, "Left click to advance slide", new Vector2(Game1.windowW / 2 - 300, 0), Color.White, 0, new Vector2(0, 0), 3f, SpriteEffects.None, 0);
            popup.drawSprite(spriteBatch);
        }


        public override void initializeGameComponent(ContentManager content)
        {
            setBackground("blueBackground");

            //popup = new Popup("To Battle", 1);
            background = new Sprite(content, backgroundSelector);
            textBox = new Sprite(content, "textBackground");
            foreach (Slide slide in slides)
            {
                slide.character.setTexture(content);
            }

            popup.initializeGameComponent(content);
        }

        public override void mouseStateLogic(MouseState mouseState, ContentManager content)
        {
            if (mouseState.LeftButton == ButtonState.Pressed && slideSwitch == false)
            {
                slideSwitch = true;
                if (slideController < slides.Count - 1)
                {
                    slideController++;
                    charBuilder.resetStringBuilder();
                }
                else
                {
                    throwPopup();
                }
                throwPopup();
            }
            if (mouseState.LeftButton == ButtonState.Released)
            {
                slideSwitch = false;
            }

            popup.mouseStateLogic(mouseState, content);
        }

        public override void unloadGameComponent()
        {
            popup.unloadGameComponent();
            switcherButtons = new List<SwitcherButton>();
            slides = new List<Slide>();
        }

        public override void updateGameComponent(ContentManager content)
        {
            popup.updateGameComponent(content);
            charBuilder.updateMessage(slides[slideController].message);
        }

        private void throwPopup()
        {
            /*if (popup.end == false)
            {
                if (slideController == slides.Count - 1)
                {
                    switcherButtons = popup.switcherButtons;
                    popup.end = true;
                }
            }*/

        }
    }



    public class StorySlideConstructor
    {
        StorySlides storySlides;
        public void setSlide()
        {
            storySlides = new StorySlides();
        }
        public void addSlide(string message, CharacterProfile character)
        {
            storySlides.slides.Add(new Slide(message, character));
        }
        public void selectBackground(string background)
        {
            storySlides.setBackground(background);
        }
        public StorySlides getSlides()
        {
            return storySlides;
        }
    }

    public class CharacterProfile
    {
        public CharacterProfile(string profileName, string image)
        {
            name = profileName;
            imageFile = image;
        }
        public void setTexture(ContentManager content)
        {
            characterImage = new Sprite(content, imageFile);
        }
        public string imageFile { get; set; }
        public string name { get; set; }
        public Sprite characterImage { get; set; }
    }


}
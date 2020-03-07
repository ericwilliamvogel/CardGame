using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CardGame
{
    // 02/29/2020 changed ABSTRACT to VIRTUAL in attempt to make classes a little cleaner, yet to test
    public abstract class DrawAndUpdate 
    {
        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void Update();
    }

    public class ComponentSkeleton
    {
        public ComponentSkeleton()
        {
            properties = new Properties();
            properties.color = Color.White;
        }

        private string contentName;
        public Properties properties;

        public bool isWithinBox(int x, int y)
        {
            int width = getWidth();
            int height = getHeight();

            if (x > getPosition().X && x < getPosition().X + width)
            {
                if (y > getPosition().Y && y < getPosition().Y + height)
                {
                    return true;
                }
            }
            return false;
        }
        public bool isWithinBox(MouseState state)
        {
            if(isWithinBox(state.X, state.Y))
            {
                return true;
            }
            return false;
        }
        public void setSprite(ContentManager content, string image)
        {
            setContentName(image);
            setTexture(content);
        }
        public void setTexture(ContentManager content)
        {
            properties.sprite = new Sprite(content, contentName);
            setSpriteParams();
        }
        public void setTexture(Texture2D newTexture)
        {

            properties.sprite.setTexture(newTexture);
            setSpriteParams();
        }

        private void setSpriteParams()
        {
            properties.Width = ((int)properties.sprite.getTextureParamaters().X);
            properties.Height = ((int)properties.sprite.getTextureParamaters().Y);
        }
        public void setContentName(string image)
        {
            contentName = image;
        }

        public virtual void unloadGameComponent() { }
        public virtual void mouseStateLogic(MouseState mouseState, ContentManager content) { }
        public virtual void drawSprite(SpriteBatch spriteBatch)
        {
            
            if(getTexture() != null)
            spriteBatch.Draw(getTexture(), getPosition(), null, null, null, getRotation(), getScale(), getColor(), properties.spriteEffects, 0);
        }

        public virtual void setPos(int x, int y)
        {
            properties.POS = new Vector2(x, y);
        }
        public virtual void setPos(Vector2 input)
        {
            properties.POS = input;
        }
        public Texture2D getTexture()
        {
            return properties.sprite.getLoadedTexture();
        }
        public Vector2 getPosition()
        {
            return properties.POS;
        }
        public int getWidth()
        {
            return properties.Width;
        }
        public int getHeight()
        {
            return properties.Height;
        }

        public Color getColor()
        {
            return properties.color;
        }
        public Vector2 getScale()
        {
            float x = Properties.globalScale.X + properties.scale.X;
            float y = Properties.globalScale.Y + properties.scale.Y;
            Vector2 newScale = new Vector2(x, y);
            return newScale;
            //return Properties.globalScale;
        }
        public float getLocalScale()
        {
            return ToAbsolute(properties.scale.X);
        }
        public virtual void setScale(float setting)
        {
            properties.scale = new Vector2(setting, setting);
        }
        public virtual void incrementScale(float increment)
        {
            properties.scale = new Vector2(properties.scale.X + increment, properties.scale.Y + increment);
        }
        public float getRotation()
        {
            return properties.rotation;
        }

        public static float ToAbsolute(float x)
        {
            if (x < 0)
            {
                return -x;
            }
            else
                return x;
        }
    }
    public class CardContainer : GameComponent
    {
        public List<Card> cardsInContainer = new List<Card>();

        public int horizontalSpacing = 0;



        public void modifyCardInteractivity(MouseState mouseState)
        {
            foreach (Card card in cardsInContainer)
            {
                cardStateHandler(mouseState, card);
            }
        }
        public bool isWithinModifiedPosition(MouseState mouseState, Card card)
        {
            if (mouseState.X > getPositionOfCardInContainer(card).X && mouseState.X < getPositionOfCardInContainer(card).X + (cardsInContainer.IndexOf(card)) * (card.getWidth() - horizontalSpacing))
            {
                if (mouseState.Y > getPositionOfCardInContainer(card).Y && mouseState.Y < getPositionOfCardInContainer(card).Y + card.getHeight())
                {
                    return true;
                }
            }
            return false;
        }

        public int notZero(int i)
        {
            if(i < 1)
            {
                return 1;
            }
            return i;
        }
        public Vector2 getPositionOfCardInContainer(Card card)
        {
            int spacing = 20;
            int initialPosX = (int)getPosition().X + GraphicsSettings.toResolution(spacing);
            int initialPosY = (int)getPosition().Y + GraphicsSettings.toResolution(spacing);
            int multiplier = cardsInContainer.IndexOf(card);
            //throw new Exception(multiplier.ToString());
            int finalPosX = initialPosX + multiplier * card.getWidth() ;
            int finalPosY = initialPosY;

            Vector2 returnValue = new Vector2(finalPosX, finalPosY);

            return returnValue;
        }
        public int getWidthOfCardInContainer(Card card)
        {
            int spacing = 20;
            int multiplier = cardsInContainer.IndexOf(card);
            int initialPosX = (int)getPosition().X + GraphicsSettings.toResolution(spacing);
            int finalPosX = initialPosX + multiplier * card.getWidth() + (card.getWidth() - horizontalSpacing);
            return finalPosX;
        }
        private void cardStateHandler(MouseState mouseState, Card card)
        {
            switch(card.selectState)
            {
                case Card.SelectState.Regular:
                    if(isWithinModifiedPosition(mouseState, card))
                    {
                        card.setHovered();
                    }

                    break;
                case Card.SelectState.Hovered:
                    if (!isWithinModifiedPosition(mouseState, card))
                    {
                        card.setRegular();
                    }
                    if(mouseState.LeftButton == ButtonState.Pressed)
                    {
                        card.setSelected();
                    }
                        break;
                case Card.SelectState.Selected:
                    if (mouseState.MiddleButton == ButtonState.Pressed)
                    {
                        card.setRegular();
                    }
                    break;
            }
        }
        public CardContainer()
        {
            properties = new Properties();
        }

        public void setValuesToImage(GameComponent image)
        {
            properties.Width = image.properties.width;
            properties.Height = image.properties.height;
            setPos(image.getPosition());
        }
        public void moveCard(CardContainer container, Card card)
        {
            if(cardsInContainer.Contains(card))
            {
                cardsInContainer.Remove(card);
                container.cardsInContainer.Add(card);
            }
            else
            {
                Console.WriteLine("Card not found in this container");
            }
        }

        public bool isEmpty()
        {
            if (cardsInContainer.Count < 1) //this will bite me in the butt later~!
            {
                return true;
            }
            return false;
        }
        public bool hasAtLeastTwoCards()
        {
            if (cardsInContainer.Count < 2) //this will bite me in the butt later~!
            {
                return true;
            }
            return false;
        }
        public int Count()
        {
            return cardsInContainer.Count;
        }
        public void resetCardPositionsInHorizontalContainer()
        {
            if(!isEmpty())
            {
                if (Count() * cardsInContainer[0].getWidth() > getWidth())
                {
                    horizontalSpacing = cardsInContainer[0].getWidth() - getWidth() * 1 / Count();
                }
            }

        }
    }
    public class GameComponent : ComponentSkeleton
    {
        
        public virtual void updateGameComponent() { }
        public virtual void initializeGameComponent() { }

    }
    public class CardSupplement : GameComponent
    {
        public Vector2 relativePosition;
        public Vector2 offSet; 

        public void setOffset(float x, float y)
        {
            offSet.X = x;
            offSet.Y = y;
        }
        public void setRelativePosition(GameComponent component)
        {
            relativePosition = component.getPosition();
        }
        public override void drawSprite(SpriteBatch spriteBatch)
        {
            properties.POS = relativePosition + offSet;
            base.drawSprite(spriteBatch);
        }
    }
    public class PrimaryComponent : ComponentSkeleton
    {
        public List<SwitcherButton> switcherButtons;

        public PrimaryComponent()
        {
            switcherButtons = new List<SwitcherButton>();
        }

        public override void unloadGameComponent()
        {
            switcherButtons = null; 
            base.unloadGameComponent();
        }

        public virtual void updateGameComponent(ContentManager content) { }
        public virtual void initializeGameComponent(ContentManager content) { }


    }

}
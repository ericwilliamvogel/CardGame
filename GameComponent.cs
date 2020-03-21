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
            spriteBatch.Draw(getTexture(), getPosition(), null, null, null, getRotation(), getScale() , getColor() * properties.transparency, properties.spriteEffects, 0);
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

    public class ShadowComponent : GameComponent
    {
        public ShadowComponent(GameComponent input)
        {
            setTexture(input.getTexture());
            setPos(input.getPosition());
            setScale(input.properties.scale.X);

        }
        
        public void updateScaleAndPosition(GameComponent input)
        {
            setPos(input.getPosition());
            setScale(input.properties.scale.X);
        }
    }
    public class GameComponent : ComponentSkeleton
    {
        public virtual void updateGameComponent() { }
        public virtual void initializeGameComponent() { }

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
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
namespace SiegewithCleanCode
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

        protected bool isWithinBox(int x, int y)
        {
            if (x > properties.ACTUALPOSITION.X && x < properties.ACTUALPOSITION.X + properties.getWidth())
            {
                if (y > properties.ACTUALPOSITION.Y && y < properties.ACTUALPOSITION.Y + properties.getHeight())
                {
                    return true;
                }
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
        }
        public void setContentName(string image)
        {
            contentName = image;
        }

        public virtual void unloadGameComponent() { }
        public virtual void mouseStateLogic(MouseState mouseState, ContentManager content) { }
        public virtual void drawSprite(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(properties.sprite.getLoadedTexture(), properties.ACTUALPOSITION, properties.color);
        }

        public Texture2D getTexture()
        {
            return properties.sprite.getLoadedTexture();
        }
        public Vector2 getPosition()
        {
            return properties.ACTUALPOSITION;
        }
        public Rectangle getParams()
        {
            return properties.getParams();
        }
        public Color getColor()
        {
            return properties.color;
        }
    }
    public class GameComponent : ComponentSkeleton
    {
        
        public virtual void updateGameComponent() { }
        public virtual void initializeGameComponent() { }

    }

    public abstract class PrimaryComponent : ComponentSkeleton
    {
        public List<SwitcherButton> switcherButtons;

        public PrimaryComponent()
        {
            switcherButtons = new List<SwitcherButton>();
        }

        protected bool switchStatus;
        public bool getSwitchStatus()
        {
            return switchStatus;
        }

        public void setSwitchStatusToOff()
        {
            switchStatus = false;
        

        public virtual void updateGameComponent(ContentManager content) { }
        public virtual void initializeGameComponent(ContentManager content) { }


    }

}
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
    public class Button : GameComponent
    {
        protected ButtonProperties buttonProperties;
        protected Dictionary<ButtonProperties.State, Color> buttonColor;
        protected Action action;

        int presetCentering = GraphicsSettings.toResolution(20);
        public float wantedScale = 1f;
        public Type type = Type.Standard;

        int textOffsetX = 0;
        int textOffsetY = 0;

        public enum Type
        {
            Standard,
            Hold
        }

        public void setType(Type type)
        {
            this.type = type;
        }
        public Button() //only if loading a static image
        {
            setButtonStateColors();
            buttonProperties = new ButtonProperties();
        }
        public Button(ContentManager content) : this()
        {

            setSprite(content, "regularButton");
        }
        public Button(ContentManager content, Vector2 position) : this(content) //default
        {
            setGeneralProperties(position);
        }
        public Button(ContentManager content,string imagesrc) : this(content)
        {
            setSprite(content, imagesrc);
        }
        public Button(ContentManager content, Vector2 position, string imagesrc) : this(content, position)//new image
        {
            setSprite(content, imagesrc);
        }

        public void centerText(int x, int y)
        {
            textOffsetX = x;
            textOffsetY = y;
        }
        protected void setGeneralProperties(Vector2 position)
        {

            properties.POS = position;
        }
        protected virtual void setButtonStateColors()
        {
            buttonColor = new Dictionary<ButtonProperties.State, Color>();
            buttonColor.Add(ButtonProperties.State.Inactive, Color.Black);
            buttonColor.Add(ButtonProperties.State.Waiting, Color.White);
            buttonColor.Add(ButtonProperties.State.Hover, Color.Firebrick);
            buttonColor.Add(ButtonProperties.State.Press, Color.DarkGreen);
            buttonColor.Add(ButtonProperties.State.ReleaseAndSend, Color.Gray);
            checkIfColorsMatchStates();
        }

        private void checkIfColorsMatchStates()
        {
            if (ButtonProperties.State.ReleaseAndSend.Equals(buttonColor.Count))
            {
                throw new Exception("color count doesn't match state count");
            }
        }

        public virtual void addActionToCurrentAction(Action inputAction)
        {
            Action currentAction = action;
            Action newAction = () =>
            {
                currentAction();
                inputAction();
            };
            action = newAction;
        }

        public virtual void setAction(Action action)
        {
            this.action = action;
        }
        public virtual void setAction()
        {
            action = () => { };
        }
        public virtual void performAction(MouseState mouseState)
        {
            action();
        }

        public void setButtonText(string title)
        {
            buttonProperties.message = title;
        }


        public override void drawSprite(SpriteBatch spriteBatch)
        {
            
            base.drawSprite(spriteBatch);
            if(buttonProperties.message != null)
            spriteBatch.DrawString(Game1.spritefont, buttonProperties.message, new Vector2(centerText(getPosition().X) + textOffsetX, centerText(getPosition().Y) + textOffsetY), Color.Black, 0, new Vector2(0, 0), wantedScale * getScale(), SpriteEffects.None, 0);
        }


        protected float centerText(float value)
        {
            return value + presetCentering;
        }
        public void setPresetCentering(int val)
        {
            presetCentering = val;
        }

        public override void mouseStateLogic(MouseState mouseState, ContentManager content)
        {
            try
            {
                changeButtonStates(mouseState);
                changeButtonColors();
            }
            catch(Exception e)
            {
                Console.Write(e);
            }
        }


        private void changeButtonColors()
        {
            properties.color = buttonColor[buttonProperties.state];
        }


        private void changeButtonStates(MouseState mouseState)
        {
            switch(buttonProperties.state)
            {
                case ButtonProperties.State.Inactive:
                    buttonProperties.state = waitToActivate();
                    break;
                case ButtonProperties.State.Waiting:
                    buttonProperties.state = changeStateIfHovering(mouseState);
                    break;
                case ButtonProperties.State.Hover:
                    buttonProperties.state = changeStateIfClicking(mouseState);
                    break;
                case ButtonProperties.State.Press:
                    buttonProperties.state = sendActionWhenClicked(mouseState);
                    break;
                case ButtonProperties.State.ReleaseAndSend:
                    performAction(mouseState);
                    resetButton(mouseState);
                    break;
            }

        }
        
        protected virtual void resetButton(MouseState mouseState)
        {
            if(type == Type.Standard)
            {
                buttonProperties.state = ButtonProperties.State.Waiting;
            }
            else
            {
                if(mouseState.LeftButton == ButtonState.Released)
                {
                    buttonProperties.state = ButtonProperties.State.Waiting;
                }
            }
        }

        protected virtual ButtonProperties.State waitToActivate()
        {
            if(action != null)
            {
                return ButtonProperties.State.Waiting;
            }
            return ButtonProperties.State.Inactive;

        }
        protected virtual ButtonProperties.State changeStateIfHovering(MouseState mouseState)
        {
            if (isWithinBox(mouseState.X, mouseState.Y))
            {
                return ButtonProperties.State.Hover;
            }
            return ButtonProperties.State.Waiting;
        }


        protected virtual ButtonProperties.State sendActionWhenClicked(MouseState mouseState)
        {
            if(type == Type.Standard)
            {
                if (mouseState.LeftButton == ButtonState.Released)
                {
                    return ButtonProperties.State.ReleaseAndSend;
                }
                if (!isWithinBox(mouseState.X, mouseState.Y))
                {
                    return ButtonProperties.State.Waiting;
                }
                return ButtonProperties.State.Press;
            }
            else
            {
                return ButtonProperties.State.ReleaseAndSend;
            }


        }

        protected virtual ButtonProperties.State changeStateIfClicking(MouseState mouseState)
        {
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                return ButtonProperties.State.Press;
            }
            if (!isWithinBox(mouseState.X, mouseState.Y))
            {
                return ButtonProperties.State.Waiting;
            }
            return ButtonProperties.State.Hover;
        }


    }

  
}
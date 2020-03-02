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


        public Button(ContentManager content, Vector2 position) //default
        {
            setSprite(content, "button");
            buttonProperties = new ButtonProperties();
            setButtonStateColors();
            setGeneralProperties(position);
        }
        public Button(ContentManager content, Vector2 position, string imagesrc) : this(content, position)//new image
        {
            setSprite(content, imagesrc);
        }

        protected void setGeneralProperties(Vector2 position)
        {
            properties.Width = ((int)properties.sprite.getTextureParamaters().X);
            properties.Height = ((int)properties.sprite.getTextureParamaters().Y);
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

        public void setAction(Action action)
        {
            this.action = action;
        }
        public virtual void setAction()
        {
            action = () => { };
        }
        public virtual void performAction()
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
            spriteBatch.DrawString(Game1.spritefont, buttonProperties.message, new Vector2(centerText(getPosition().X), centerText(getPosition().Y)), Color.Gold, 0, new Vector2(0, 0), properties.scale, SpriteEffects.None, 0);
        }

        protected float centerText(float value)
        {
            return value + 20;
        }


        public void Update(MouseState mouseState)
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
                    performAction();
                    resetButton();
                    break;
            }

        }
        
        protected virtual void resetButton()
        {
            buttonProperties.state = ButtonProperties.State.Waiting;
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
            if (mouseState.LeftButton == ButtonState.Released)
            {
                return ButtonProperties.State.ReleaseAndSend;
            }
            if(!isWithinBox(mouseState.X, mouseState.Y))
            {
                return ButtonProperties.State.Waiting;
            }
            return ButtonProperties.State.Press;
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

    //work on this mayb
    public class SwitcherButton : Button
    {
        private int controller { get; set; }
        private Action<SwitcherButton> switcherAction;

        public SwitcherButton(ContentManager content, Vector2 position, int desiredController) : base(content, position)
        {
            setControllerToSwitch(desiredController);
            //settitle or something
        }
        
        public void setSwitcherAction(Action<SwitcherButton> action)
        {
            setAction();
            switcherAction = action;
        }
        public override void performAction()
        {
            switcherAction(this);
        }
        public void setControllerToSwitch(int desiredController)
        {
            controller = desiredController;
        }
        public int returnComponentToSwitchTo()
        {
            return controller;
        }
    }

    public class ButtonProperties
    {
        public ButtonProperties()
        {
            state = State.Inactive;
        }

        public enum State
        {
            Inactive,
            Waiting,
            Hover,
            Press,
            ReleaseAndSend
        }
        public State state { get; set; }
        public string message { get; set; }


    }
}
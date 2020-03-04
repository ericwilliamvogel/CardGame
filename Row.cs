using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public class Row : GameComponent
    {
        public int maxCards = 12;
        public Row()
        {
            setContentName("row");
        }
        public virtual void fillRowWithCard(Card card)
        {

        }

        public override void mouseStateLogic(MouseState mouseState, ContentManager content)
        {

            switch (state)
            {
                case State.Hovered:
                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {

                    }

                    if (isWithinBox(mouseState.X, mouseState.Y))
                    {
                        state = State.Regular;
                    }
                    break;
                case State.Regular:
                    if (isWithinBox(mouseState.X, mouseState.Y))
                    {
                        state = State.Hovered;
                    }
                    break;
            }
            if (isWithinBox(mouseState.X, mouseState.Y))
            {
                state = State.Hovered;
            }
            else
            {
                state = State.Regular;
            }

            changeColorsToState();
        }
        bool canPlace;
        /*public void updateBoard(GamePlay game)
        {
            if(game.isHoldingCard)
            {
                canPlace = true;
            }
            else
            {

            }
        }*/
        public void changeColorsToState()
        {
            if (state == State.Hovered)
            {
                properties.color = Color.Yellow;
            }
            if (state == State.Regular)
            {
                properties.color = Color.White;
            }
        }
        private enum State
        {
            Hovered,
            Regular
        }
        State state;
        public override void drawSprite(SpriteBatch spriteBatch)
        {
            if (state == State.Hovered)
            {
                base.drawSprite(spriteBatch);
            }
            base.drawSprite(spriteBatch);
        }
    }
    public class PortraitWidget : GameComponent
    {
        public PortraitWidget()
        {
            setContentName("heroWidget");
        }
    }
    public class StackPlaceholder : GameComponent
    {
        public StackPlaceholder()
        {
            setContentName("deckPlaceholder");
        }
    }
}

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



    public class CardContainer : GameComponent
    {
        public List<Card> cardsInContainer = new List<Card>();

        public int initialSpacing = 0;
        public int horizontalSpacing = 0;
        public int spacing = 20;


        public void modifyCardInteractivity(MouseState mouseState, BoardFunctionality boardFunc)
        {
            foreach (Card card in cardsInContainer)
            {
                cardStateHandler(mouseState, boardFunc, card);
            }
        }
        public bool isWithinModifiedPosition(MouseState mouseState, Card card)
        {
            if (mouseState.X > getPositionOfCardInContainer(card).X - initialSpacing && mouseState.X < getPositionOfCardInContainer(card).X -initialSpacing+ (card.getWidth() - ifNotLastInContainer(card, horizontalSpacing)))
            {
                if (mouseState.Y > getPositionOfCardInContainer(card).Y && mouseState.Y < getPositionOfCardInContainer(card).Y + card.getHeight())
                {
                    return true;
                }
            }
            return false;
        }
        private int ifNotLastInContainer(Card card, int input)
        {
            if (cardsInContainer.IndexOf(card) >= Count() - 1)
            {
                return 0;
            }
            return input;
        }
        public int notZero(int i)
        {
            if (i < 1)
            {
                return 1;
            }
            return i;
        }
        public Vector2 getPositionOfCardInContainer(Card card)
        {

            int initialPosX = (int)getPosition().X + GraphicsSettings.toResolution(spacing);
            int initialPosY = (int)getPosition().Y + GraphicsSettings.toResolution(spacing);
            int multiplier = cardsInContainer.IndexOf(card);
            //throw new Exception(multiplier.ToString());
            int finalPosX = initialPosX + multiplier * (card.getWidth() - horizontalSpacing);
            int finalPosY = initialPosY;

            Vector2 returnValue = new Vector2(finalPosX, finalPosY);

            return returnValue;
        }
        public int getWidthOfCardInContainer(Card card)
        {

            int multiplier = cardsInContainer.IndexOf(card);
            int initialPosX = (int)getPosition().X + GraphicsSettings.toResolution(spacing);
            int finalPosX = initialPosX + multiplier * card.getWidth() + (card.getWidth() - horizontalSpacing);
            return finalPosX;
        }
        protected virtual void additionalAction(Card card)
        {

        }
        protected virtual void resetAction(Card card)
        {

        }



        protected void cardStateHandler(MouseState mouseState, BoardFunctionality boardFunc, Card card)
        {
            //setCorrectCenterSpacing(card);
            switch (card.selectState)
            {
                case Card.SelectState.Regular:
                    if (isWithinModifiedPosition(mouseState, card))
                    {
                        card.setHovered();
                        additionalAction(card);
                    }

                    break;
                case Card.SelectState.Hovered:
                    if (!isWithinModifiedPosition(mouseState, card))
                    {
                        card.setRegular();
                        resetAction(card);
                    }
                    
                    if (mouseState.LeftButton == ButtonState.Pressed && noCardsOnBoardAreSelected(boardFunc))
                    {
                        card.setSelected();
                        //resetAction(card);
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
        private bool noCardsOnBoardAreSelected(BoardFunctionality game)
        {
            if(game.SELECTEDCARD == null)
            {
                return true;
            }
            return false;
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
            if (cardsInContainer.Contains(card))
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
        
    }
    public class HorizontalContainer : CardContainer
    {
        public float containerScale;
        public virtual void setCorrectCenterSpacing(Card card)
        {
            if (centerSpacing)
                initialSpacing = -(getWidth() / 2);

            if (!isEmpty() && centerSpacing)
            {
                additionalSpacing = (Count() * card.getWidth() / 2);
                float startingWidth = 0;
                startingWidth = trueContainerWidth(cardsInContainer[0]);
                if ((Count()) * startingWidth > getWidth())
                additionalSpacing = getWidth() / 2;
                initialSpacing = -(getWidth() / 2 - additionalSpacing);
            }
        }
        public Vector2 newCardPositionInContainer(Card card)
        {
            int multiplier = Count();
            int initialPosX = (int)getPosition().X + GraphicsSettings.toResolution(spacing);
            int finalPosX = initialPosX + multiplier * ((int)trueContainerWidth(card) - horizontalSpacing);
            Vector2 newPos = new Vector2(finalPosX, getPosition().Y);
            return newPos;
        }
        private float trueContainerWidth(Card card)
        {
            float trueStartingWidth = 0;
            if (card.properties.scale.X != containerScale)
            {
                float initialScale = card.properties.scale.X;
                card.setScale(containerScale);
                trueStartingWidth = card.getWidth();
                card.setScale(initialScale);
            }
            else
            {
                trueStartingWidth = card.getWidth();

            }
            return trueStartingWidth;
        }

        protected bool centerSpacing = false;
        public void setCenterSpacing()
        {
            centerSpacing = true;
        }
        protected int additionalSpacing = 0;
        public void resetCardSpacingInHorizontalContainer()
        {
            if (!isEmpty())
            {
                int additionalPadding = 2;
                float goalWidth = getWidth() * 1 / Count() - additionalPadding;
                float startingWidth = 0;
                startingWidth = trueContainerWidth(cardsInContainer[0]);
                if (hasExceededContainerWidth(startingWidth))
                {
                    //throw new Exception();
                    /**solve for spacing given startValue and endValue**/
                    /**startValue - spacing = endValue;**/
                    /**spacing = -(startValue - endValue)**/
                    //we are using negative on implementation
                    horizontalSpacing = (int)(startingWidth - goalWidth);

                }
                else
                {
                    horizontalSpacing = 0;
                    
                }

            }
        }
        private bool hasExceededContainerWidth(float input)
        {
            if((Count()) * input > getWidth())
            {
                return true;
            }
            return false;
        }
    }
}

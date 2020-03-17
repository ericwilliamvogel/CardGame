using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public class ActionConstructor
    {
        public Action moveToAction;

        public void addNewAction(Action action, BoardFunctionality boardFunc)
        {
            boardFunc.boardActions.AddAction(action);
        }
        public void moveTo(CardContainer startingContainer, CardContainer endingContainer, Card card, BoardFunctionality boardFunc)
        {
            //Vector2 toMoveTo = container.getPosition();
            //Vector2 from = card.getPosition();
            moveToAction = () => {
                movementLogic(startingContainer, endingContainer, card, boardFunc);

            };
            boardFunc.boardActions.AddAction(moveToAction);
        }

        public void addWaitAction(Action action, int time, BoardFunctionality boardFunc)
        {
            int x = 0;
            moveToAction = () =>
            {
                x++;
                if (x > time)
                {
                    boardFunc.boardActions.AddAction(action);
                    x = 0;
                    boardFunc.boardActions.nextAction();
                }
            };
            boardFunc.boardActions.AddAction(moveToAction);

        }
        public void addDrawAction(CardContainer startingContainer, CardContainer endingContainer, BoardFunctionality boardFunc)
        {
            moveToAction = () => {
                drawCardLogic(startingContainer, endingContainer, boardFunc);
            };
            boardFunc.boardActions.AddAction(moveToAction);
        }
        public void drawCardLogic(CardContainer startingContainer, CardContainer endingContainer, BoardFunctionality boardFunc)
        {
            Card card = startingContainer.cardsInContainer[0];
            card.makingAction = true;
            movementLogic(startingContainer, endingContainer, card, boardFunc);
        }

        public void movementLogic(CardContainer startingContainer, CardContainer endingContainer, Card card, BoardFunctionality boardFunc)
        {
            card.makingAction = true;
            Vector2 newPosition = endingContainer.getPosition();
            if (newPosition.X > Game1.windowW || newPosition.X < 0)
            {
                throw new Exception(endingContainer.getPosition().ToString());
            }
            if (newPosition.Y > Game1.windowH || newPosition.Y < -200)
            {
                throw new Exception(endingContainer.getPosition().ToString());
            }
            int timeUntilArrival = 3;
            int speedX = (int)GameComponent.ToAbsolute((newPosition.X - card.getPosition().X)) / timeUntilArrival;

            if (speedX < 1)
            {
                speedX = 1;
            }
            int speedY = (int)GameComponent.ToAbsolute((newPosition.Y - card.getPosition().Y)) / timeUntilArrival;
            if (speedY < 1)
            {
                speedY = 1;
            }
            Vector2 adjustingPosition;
            bool xAxisFinished = false;
            bool yAxisFinished = false;

            if (card.getPosition().X < newPosition.X)
            {
                adjustingPosition = new Vector2(card.getPosition().X + speedX, card.getPosition().Y);
                card.setPos(adjustingPosition);
            }
            else if (card.getPosition().X > newPosition.X)
            {
                adjustingPosition = new Vector2(card.getPosition().X - speedX, card.getPosition().Y);
                card.setPos(adjustingPosition);
            }
            else
            {
                xAxisFinished = true;
            }

            if (card.getPosition().Y < newPosition.Y)
            {
                adjustingPosition = new Vector2(card.getPosition().X, card.getPosition().Y + speedY);
                card.setPos(adjustingPosition);
            }
            else if (card.getPosition().Y > newPosition.Y)
            {
                adjustingPosition = new Vector2(card.getPosition().X, card.getPosition().Y - speedY);
                card.setPos(adjustingPosition);
            }
            else
            {
                yAxisFinished = true;
            }

            if (xAxisFinished && yAxisFinished)
            {
                try
                {
                    startingContainer.moveCard(endingContainer, card);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                card.makingAction = false;
                boardFunc.boardPosLogic.updateBoard(boardFunc);
                boardFunc.boardActions.nextAction();
            }


        }
    }

}

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
    public static class CardScale
    {
        public static float Hand = -.6f;
        public static float View = 0f;
        public static float Board = -(Properties.globalScale.X - Properties.globalScale.X/6);

    }

    public class FunctionalRow : CardContainer
    {
        public FunctionalRow(CardType type)
        {
            this.type = type;
        }
        public CardType type;

        
    }
    public class GamePlay : PrimaryComponent
    {
        
        public enum GameState //LATER
        {
            DrawHand,
            Play,
            Pause,
            Pass
        }
        BoardActions boardActions = new BoardActions();
        GameState state;
        public GamePlay()
        {
            state = GameState.DrawHand;
        }
        public void assignAction()
        {

        }
        /*public enum TurnState
        {
            Player1Start,
            Player1Decision,
            Player2Start,
            Player2Decision
        }*/
        public override void initializeGameComponent(ContentManager content)
        {
            //load all card textures
            //load decks of players

        }

        Player controllingPlayer;
        public void handlePlayers(Player firstPlayer, Player secondPlayer)
        {
            player1 = firstPlayer;
            player2 = secondPlayer;

            controllingPlayer = firstPlayer;

            //controllingPlayer.Draw
        }
        Player player1;

        Player player2;
        public void StartGame(Player firstPlayer, Player secondPlayer)
        {
            firstPlayer.DrawHand();
            secondPlayer.DrawHand();
        }
        public void newTurnSwitchPlayer()
        {
            controllingPlayer = getOtherPlayer();
        }
        public void handleGameStates()
        {
            switch (state)
            {
                case GameState.DrawHand:
                    controllingPlayer.hasControl();
                    break;
                case GameState.Play:
                    controllingPlayer.hasControl();
                    break;
                case GameState.Pause:
                    controllingPlayer.loseControl();
                    break;
                case GameState.Pass:
                    newTurnSwitchPlayer();
                    break;
            }
        }
        private Player getOtherPlayer()
        {
            Player player = null;
            if (controllingPlayer == player1)
            {
                player = player2;
            }

            if (controllingPlayer == player2)
            {
                player = player1;
            }

            return player;
        }
        private void sendCardToOblivion(CardContainer container, Card card)
        {

        }




        public void scaleToView(Card card)
        {
            card.setScale(CardScale.View);
            card.initSupplements();
        }
        public void scaleToBoard(Card card)
        {
            card.setScale(CardScale.Board);
            card.initSupplements();
        }
        public void scaleToHand(Card card)
        {
            card.setScale(CardScale.Hand);
            card.initSupplements();
        }

        public Action moveToAction;

        public void moveTo(CardContainer startingContainer, CardContainer endingContainer, Card card)
        {
            //Vector2 toMoveTo = container.getPosition();
            //Vector2 from = card.getPosition();
            moveToAction = () => {
                movementLogic(startingContainer, endingContainer, card);
            };
            boardActions.AddAction(moveToAction);
        }

        public void addDrawAction(CardContainer startingContainer, CardContainer endingContainer)
        {
            moveToAction = () => {
                drawCardLogic(startingContainer, endingContainer);
            };
            boardActions.AddAction(moveToAction);
        }
        public void drawCardLogic(CardContainer startingContainer, CardContainer endingContainer)
        {
            Card card = startingContainer.cardsInContainer[0];
            movementLogic(startingContainer, endingContainer, card);
        }
        public void movementLogic(CardContainer startingContainer, CardContainer endingContainer, Card card)
        {
            Vector2 newPosition = endingContainer.getPosition();
            if(newPosition.X > Game1.windowW || newPosition.X < 0)
            {
                throw new Exception(endingContainer.getPosition().ToString());
            }
            if(newPosition.Y > Game1.windowH || newPosition.Y < -200)
            {
                throw new Exception(endingContainer.getPosition().ToString());
            }
            //Vector2 initialPosition = startingContainer.getPosition();
            int timeUntilArrival = 10;
            int speedX = (int)ToAbsolute((newPosition.X - card.getPosition().X)) / timeUntilArrival;

            if (speedX < 1)
            {
                speedX = 1;
            }
            int speedY = (int)ToAbsolute((newPosition.Y - card.getPosition().Y)) / timeUntilArrival;
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
            else if(card.getPosition().X > newPosition.X)
            {
                adjustingPosition = new Vector2(card.getPosition().X - speedX, card.getPosition().Y);
                card.setPos(adjustingPosition);
            }
            else
            {
                xAxisFinished = true;
            }

            if(card.getPosition().Y < newPosition.Y)
            {
                adjustingPosition = new Vector2(card.getPosition().X, card.getPosition().Y + speedY);
                card.setPos(adjustingPosition);
            }
            else if(card.getPosition().Y > newPosition.Y)
            {
                adjustingPosition = new Vector2(card.getPosition().X, card.getPosition().Y - speedY);
                card.setPos(adjustingPosition);
            }
            else
            {
                yAxisFinished = true;
            }

            if(xAxisFinished && yAxisFinished)
            {
                try
                {
                    startingContainer.moveCard(endingContainer, card);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                }

                updateBoard();
                boardActions.nextAction();
            }
        
        }



        private void DeckLogic(Side side)//both sides
        {
            foreach(Card card in side.Deck.cardsInContainer)
            {
                scaleToBoard(card);
                card.playState = Card.PlayState.Hidden;
            }
        }
        public void DrawCard(Side side)
        {
            addDrawAction(side.Deck, side.Hand);
        }
        private void PlayCard(Side side, FunctionalRow row, Card card)
        {
            moveTo(side.Hand, row, card);
            card.playState = Card.PlayState.Revealed;
        }
        private void DiscardCard(Side side, Card card)
        {
            moveTo(side.Hand, side.Oblivion, card);
        }
        private void DrawHand(Side side)
        {
            for(int i = 0; i < 7; i++)
            {
                DrawCard(side);
            }
        }

        public void updateBoard() //after every Action
        {
            updateHandPositions();
            updateBoardPositions();

        }

        public void StartGame(Board board)
        {
            this.friendlySide = board.friendlySide;
            this.enemySide = board.enemySide;
            updateBoard();
            updateDeckPositions(friendlySide);
            updateDeckPositions(enemySide);
            assignCardPositionsOnHandExtension(board);
        }

        private void updateDeckPositions(Side side)
        {
            foreach(Card card in side.Deck.cardsInContainer)
            {
                card.setPos(side.Deck.getPosition());
            }
        }
        
        private void setHandPositions(Side side)
        {
            int counter = 0;
            int spacing = 20;
            side.Hand.resetCardPositionsInHorizontalContainer();
            foreach (Card card in side.Hand.cardsInContainer)
            {
                if (card != SELECTEDCARD)
                {
                    scaleToHand(card);
                    Vector2 newPosition = new Vector2(side.Hand.getPosition().X + GraphicsSettings.toResolution(spacing) + counter * (card.getWidth() - side.Hand.horizontalSpacing), side.Hand.getPosition().Y);
                    card.setPos(newPosition);
                    counter++;
                }
            }
        }

        private void updateHandPositions()
        {
            setHandPositions(friendlySide);
            setContainerPlayState(friendlySide.Hand, Card.PlayState.Revealed);
            setHandPositions(enemySide);
            setContainerPlayState(enemySide.Hand, Card.PlayState.Hidden);
        }
        private void setContainerPlayState(CardContainer container, Card.PlayState playState)
        {
            foreach (Card card in container.cardsInContainer)
            {
                card.playState = playState;
            }
        }
        private void updateBoardPositions()
        {
            for(int i = 0; i < Side.MaxRows; i++)
            {
                setBoardPosition(friendlySide.Rows[i]);
                setBoardPosition(enemySide.Rows[i]);
            }

        }
        private void setBoardPosition(FunctionalRow row)
        {
            int counter = 0;
            int spacing = row.getWidth()/2;
            row.resetCardPositionsInHorizontalContainer();
            foreach (Card card in row.cardsInContainer)
            {
                scaleToBoard(card);
                Vector2 newPosition = new Vector2(row.getPosition().X - row.Count() * card.getWidth()/2 + spacing + counter * (card.getWidth()- row.horizontalSpacing), row.getPosition().Y);
                card.setPos(newPosition);
                counter++;
            }
        }

        public override void drawSprite(SpriteBatch spriteBatch)
        {
            drawStack(friendlySide.Deck, spriteBatch);
            drawStack(enemySide.Deck, spriteBatch);
            drawStack(friendlySide.Oblivion, spriteBatch);
            drawStack(enemySide.Oblivion, spriteBatch);

            drawRows(friendlySide, spriteBatch);
            drawRows(enemySide, spriteBatch);
            drawHand(friendlySide, spriteBatch);

            drawHand(enemySide, spriteBatch);

        }
        private void drawHand(Side side, SpriteBatch spriteBatch)
        {
            foreach(Card card in side.Hand.cardsInContainer)
            {
                card.drawSprite(spriteBatch);
            }
        }
        private void drawRows(Side side, SpriteBatch spriteBatch)
        {
            foreach(FunctionalRow row in side.Rows)
            {
                foreach (Card card in row.cardsInContainer)
                {
                    card.drawSprite(spriteBatch);
                }
            }

        }
        private void drawStack(CardContainer container, SpriteBatch spriteBatch)
        {
            if(!container.isEmpty())
            {
                if(!container.hasAtLeastTwoCards())
                {
                    container.cardsInContainer[1].setCardBackColor(Color.Green);
                    container.cardsInContainer[1].drawSprite(spriteBatch);

                }
                container.cardsInContainer[0].setCardBackColor(Color.White);
                container.cardsInContainer[0].drawSprite(spriteBatch);
            }
        }

        Side friendlySide;
        Side enemySide;
        public void Update(Board board)
        {
            //throw new Exception(friendlySide.Deck.cardsInContainer[0].getPosition().ToString());
            
            
            this.friendlySide = board.friendlySide;
            this.enemySide = board.enemySide;
            //updateBoard();
            DeckLogic(friendlySide);
            DeckLogic(enemySide);

            boardActions.updateAnimations();
            foreach(Card card in friendlySide.Deck.cardsInContainer)
            {
                card.updateGameComponent();
            }
            foreach (Card card in enemySide.Deck.cardsInContainer)
            {
                card.updateGameComponent();
            }

            foreach (Card card in friendlySide.Hand.cardsInContainer)
            {
                card.updateGameComponent();
            }
            foreach (Card card in enemySide.Hand.cardsInContainer)
            {
                card.updateGameComponent();
            }

            foreach(FunctionalRow row in friendlySide.Rows)
            {
                foreach(Card card in row.cardsInContainer)
                {
                    card.updateGameComponent();
                }
            }
            /*if (enemySide.Hand.cardsInContainer.Count > 10)
            {
                throw new Exception("");
            }*/
            //updateBoard();
        }

        public override void mouseStateLogic(MouseState mouseState, ContentManager content)
        {
            if(SELECTEDCARD==null)
            friendlySide.Hand.modifyCardInteractivity(mouseState);

            setCardToMouse(mouseState);
            playSelectedCard(mouseState);
            base.mouseStateLogic(mouseState, content);
        }

        public void assignCardPositionsOnHandExtension(Board board)
        {
            Action action = () =>
            {
                updateBoard();
            };
            board.handSpace[1].action = action;
        }
        Card SELECTEDCARD;
        private bool isWithinProperRow(MouseState mouseState)
        {
            if(SELECTEDCARD!=null)
            {
                foreach(FunctionalRow row in friendlySide.Rows)
                {
                    if(row.isWithinBox(mouseState) && mouseState.LeftButton == ButtonState.Released && row.type == SELECTEDCARD.type)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        private void playSelectedCard(MouseState mouseState)
        {
            if(isWithinProperRow(mouseState) && !cardView)
            {
                foreach (Card card in friendlySide.Hand.cardsInContainer)
                {
                    if (card == SELECTEDCARD)
                    {

                        foreach(FunctionalRow row in friendlySide.Rows)
                        {
                            if(row.type == card.type)
                            {
                                PlayCard(friendlySide, row, card);
                                card.setRegular();
                                SELECTEDCARD = null;
                            }
                        }


                    }
                }
            }

        }
        bool clickedInCardBox = false;
        bool cardView = false;
        private void setCardToMouse(MouseState mouseState)
        {
            foreach (Card card in friendlySide.Hand.cardsInContainer)
            {
                if (card.isSelected())
                {
                    if (!cardView)
                    {
                        SELECTEDCARD = card;
                        if (mouseState.LeftButton == ButtonState.Pressed)
                        {
                            card.setPos(mouseState.X - card.getWidth() / 2, mouseState.Y - card.getHeight() / 2);
                            clickedInCardBox = true;
                        }
                        if (mouseState.LeftButton == ButtonState.Released && !isWithinProperRow(mouseState) && !friendlySide.Hand.isWithinModifiedPosition(mouseState, card))
                        {
                            clickedInCardBox = false;
                            card.setRegular();
                            SELECTEDCARD = null;
                            //updateBoard();
                        }
                        if (mouseState.LeftButton == ButtonState.Released && clickedInCardBox && friendlySide.Hand.isWithinModifiedPosition(mouseState, card))
                        {
                            clickedInCardBox = false;
                            cardView = true;
                            SELECTEDCARD = null;
                        }
                    }
                    else
                    {
                        viewFullSizeCard(mouseState, card);
                    }
                }

            }
        }
        private void viewFullSizeCard(MouseState mouseState, Card card)
        {
            if (cardView)
            {
                scaleToView(card);
                card.setPos(Game1.windowW / 2 - card.getWidth() / 2, Game1.windowH / 2 - card.getHeight() / 2);
                if(mouseState.RightButton == ButtonState.Pressed)
                {
                    cardView = false;
                    card.setRegular();
                    SELECTEDCARD = null;
                    updateBoard();
                }
            }
        }
    }
}

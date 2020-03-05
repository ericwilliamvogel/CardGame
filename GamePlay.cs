using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
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
        CardType type;
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
        }
        public void scaleToBoard(Card card)
        {
            card.setScale(CardScale.Board);
        }
        public void scaleToHand(Card card)
        {
            card.setScale(CardScale.Hand);
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
            int speedX = ToAbsolute((int)(newPosition.X - card.getPosition().X)) / timeUntilArrival;

            if (speedX < 1)
            {
                speedX = 1;
            }
            int speedY = ToAbsolute((int)(newPosition.Y - card.getPosition().Y)) / timeUntilArrival;
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
                //**************************
                //startingContainer.moveCard(endingContainer, card);
                startingContainer.moveCard(endingContainer, startingContainer.cardsInContainer[0]);
                boardActions.nextAction();
                updateBoard();

                //throw new Exception("");
            }
        
        }

        
        private int ToAbsolute(int x)
        {
            if (x < 0)
            {
                return -x;
            }
            else
                return x;
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
            Card card = side.Deck.cardsInContainer[0];
            moveTo(side.Deck, side.Hand, card);
            if(side == friendlySide)
            {
                card.playState = Card.PlayState.Revealed;
            }
            if(side == enemySide)
            {
                card.playState = Card.PlayState.Hidden;
            }

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
        public void StartGame(Side friendlySide, Side enemySide)
        {
            this.friendlySide = friendlySide;
            this.enemySide = enemySide;
            updateBoard();
            updateDeckPositions(friendlySide);
            updateDeckPositions(enemySide);
        }
        private void updateDeckPositions(Side side)
        {
            foreach(Card card in side.Deck.cardsInContainer)
            {
                card.setPos(side.Deck.POS);
            }
        }
        private void updateHandPositions()
        {
            int counter = 0;
            int spacing = 20;
            foreach (Card card in friendlySide.Hand.cardsInContainer)
            {
                Vector2 newPosition = new Vector2(friendlySide.Hand.POS.X + spacing + counter * card.getWidth(), friendlySide.Hand.POS.Y);
                scaleToHand(card);
                card.playState = Card.PlayState.Revealed;
                card.setPos(newPosition);
                counter++;
            }

            counter = 0;
            foreach (Card card in enemySide.Hand.cardsInContainer)
            {
                Vector2 newPosition = new Vector2(enemySide.Hand.POS.X + spacing + counter * card.getWidth(), enemySide.Hand.POS.Y);
                scaleToHand(card);
                card.playState = Card.PlayState.Hidden;
                card.setPos(newPosition);
                counter++;
            }
        }
        private void updateBoardPositions()
        {
            setBoardPosition(friendlySide.Generals);
            setBoardPosition(enemySide.Generals);
            setBoardPosition(friendlySide.Armies);
            setBoardPosition(enemySide.Armies);
            setBoardPosition(friendlySide.FieldUnit);
            setBoardPosition(enemySide.FieldUnit);
        }
        private void setBoardPosition(FunctionalRow row)
        {
            int counter = 0;
            int spacing = 20;
            foreach (Card card in row.cardsInContainer)
            {
                Vector2 newPosition = new Vector2(row.POS.X + spacing + counter * card.getWidth(), friendlySide.Hand.POS.Y);
                scaleToBoard(card);
                card.setPos(newPosition);
                counter++;
            }
        }

        public override void drawSprite(SpriteBatch spriteBatch)
        {
            drawDeck(friendlySide, spriteBatch);
            drawDeck(enemySide, spriteBatch);
            drawOblivion(friendlySide, spriteBatch);
            drawOblivion(enemySide, spriteBatch);
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
        private void drawDeck(Side side, SpriteBatch spriteBatch)
        {
            if (!side.Deck.isEmpty())
            {
                
                side.Deck.cardsInContainer[0].drawSprite(spriteBatch);
            }
        }
        private void drawOblivion(Side side, SpriteBatch spriteBatch)
        {
            if (!side.Oblivion.isEmpty())
            {
                
                side.Oblivion.cardsInContainer[0].drawSprite(spriteBatch);
            }
        }
        Side friendlySide;
        Side enemySide;
        public void Update(Side friendlySide, Side enemySide)
        {
            //throw new Exception(friendlySide.Deck.cardsInContainer[0].getPosition().ToString());
            
            
            this.friendlySide = friendlySide;
            this.enemySide = enemySide;
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
            if(enemySide.Hand.cardsInContainer.Count > 10)
            {
                throw new Exception("");
            }
        }

    }
}

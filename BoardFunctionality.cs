using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Utilities;

namespace CardGame
{



    public class BoardFunctionality : PrimaryComponent
    {
        public BoardPositionUpdater boardPosLogic = new BoardPositionUpdater();
        public BoardActions boardActions = new BoardActions();
        ActionConstructor actionConstructor = new ActionConstructor();
        HandFunctionality handFunction = new HandFunctionality();
        RowFunctionality rowFunction = new RowFunctionality();
        public enum GameState //LATER
        {
            DrawHand,
            Play,
            Pause,
            Pass
        }

        GameState state;
        public BoardFunctionality()
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




        




        public void DrawCard(Side side)
        {
            actionConstructor.addDrawAction(side.Deck, side.Hand, this);
        }
        public void PlayCard(Side side, FunctionalRow row, Card card)
        {
            actionConstructor.moveTo(side.Hand, row, card, this);
            card.playState = Card.PlayState.Revealed;
        }
        public void DiscardCard(Side side, Card card)
        {
            actionConstructor.moveTo(side.Hand, side.Oblivion, card, this);
        }
        public void DrawHand(Side side)
        {
            for(int i = 0; i < 7; i++)
            {
                DrawCard(side);
            }
        }


        public void StartGame(Board board)
        {
            this.friendlySide = board.friendlySide;
            this.enemySide = board.enemySide;
            DrawHand(friendlySide);
            DrawHand(enemySide);
            boardPosLogic.updateBoard(this);

            assignCardPositionsOnHandExtension(board);
        }


        public override void drawSprite(SpriteBatch spriteBatch)
        {
            cardDrawer.drawSprite(spriteBatch, this);
        }


        BoardCardDrawer cardDrawer = new BoardCardDrawer();
        

        public Side friendlySide;
        public Side enemySide;
        public void Update(Board board)
        {
            //throw new Exception(friendlySide.Deck.cardsInContainer[0].getPosition().ToString());
            
            
            this.friendlySide = board.friendlySide;
            this.enemySide = board.enemySide;


            boardActions.updateAnimations();
            updateAllAssets();
                    /*if (enemySide.Hand.cardsInContainer.Count > 10)
                    {
                        throw new Exception("");
                    }*/
                    //updateBoard();
                }

        public void updateAllAssets()
        {
            foreach (Card card in friendlySide.Deck.cardsInContainer)
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

            foreach (FunctionalRow row in friendlySide.Rows)
            {
                foreach (Card card in row.cardsInContainer)
                {
                    card.updateGameComponent();
                    //row.setCorrectCenterSpacing(card);
                }
            }
            foreach (FunctionalRow row in enemySide.Rows)
            {
                foreach (Card card in row.cardsInContainer)
                {
                    card.updateGameComponent();
                }
            }
        }

        public override void mouseStateLogic(MouseState mouseState, ContentManager content)
        {
            if(SELECTEDCARD==null)
            friendlySide.Hand.modifyCardInteractivity(mouseState, this);
            
            foreach(FunctionalRow row in friendlySide.Rows)
            {
                if(SELECTEDCARD==null)
                row.modifyCardInteractivity(mouseState, this);
            }
            handFunction.setCardToMouse(mouseState, this);
            handFunction.playSelectedCard(mouseState, this);

            rowFunction.rowLogic(mouseState, this);

            base.mouseStateLogic(mouseState, content);
        }

        public void assignCardPositionsOnHandExtension(Board board)
        {
            Action action = () =>
            {
                boardPosLogic.updateBoard(this);
            };
            board.handSpace[1].action = action;
        }
        public Card SELECTEDCARD;


        public bool cardView = false;
        public void viewFullSizeCard(MouseState mouseState, Card card)
        {
            if (cardView)
            {
                boardPosLogic.scaleToView(card);
                card.setPos(Game1.windowW / 2 - card.getWidth() / 2, Game1.windowH / 2 - card.getHeight() / 2);
                /*SELECTEDCARD = null;
                SELECTEDCARD = new Card(card);
                boardPosLogic.scaleToView(SELECTEDCARD);
                SELECTEDCARD.setPos(Game1.windowW / 2 - card.getWidth() / 2, Game1.windowH / 2 - card.getHeight() / 2);
                SELECTEDCARD.updateGameComponent();*/
                if (mouseState.RightButton == ButtonState.Pressed)
                {
                    cardView = false;
                    card.setRegular();
                    SELECTEDCARD = null;
                    boardPosLogic.updateBoard(this);
                    //boardPosLogic.scaleToHand(card);
                }
            }
        }

        
        
    }
    public class RowFunctionality
    {
        public void rowLogic(MouseState mouseState, BoardFunctionality boardFunc)
        {
            foreach(FunctionalRow row in boardFunc.friendlySide.Rows)
            {
                if(row.type == CardType.Army)
                {
                    setCardToView(mouseState, row, boardFunc);
                }

                if(row.type == CardType.FieldUnit)
                {
                   setCardToView(mouseState, row, boardFunc);
                }
                if(row.type == CardType.General)
                {
                    setCardToView(mouseState, row, boardFunc);
                }
            }
        }
        public void setCardToView(MouseState mouseState, FunctionalRow row, BoardFunctionality boardFunc)
        {
            foreach (Card card in row.cardsInContainer)
            {
                if (card.isSelected())
                {
                    boardFunc.SELECTEDCARD = card;
                    if (!boardFunc.cardView)
                    {
                        fieldLogic(mouseState, row, card, boardFunc);
                    }
                    else
                    {
                        boardFunc.viewFullSizeCard(mouseState, card);
                    }
                }
            }
        }
        bool clickedInCardBox = false;
        public void fieldLogic(MouseState mouseState, FunctionalRow row, Card card, BoardFunctionality boardFunc)
        {
            if (mouseState.LeftButton == ButtonState.Pressed && row.isWithinModifiedPosition(mouseState, card))
            {
                //card.setPos(mouseState.X - card.getWidth() / 2, mouseState.Y - card.getHeight() / 2);
                clickedInCardBox = true;
            }
            if (mouseState.LeftButton == ButtonState.Released && clickedInCardBox && row.isWithinModifiedPosition(mouseState, card))
            {
                clickedInCardBox = false;
                boardFunc.cardView = true;

            }
            if(mouseState.LeftButton == ButtonState.Pressed && clickedInCardBox && !row.isWithinModifiedPosition(mouseState, card))
            {
                card.setPos(mouseState.X - card.getWidth() / 2, mouseState.Y - card.getHeight() / 2);
            }
            if(mouseState.LeftButton == ButtonState.Released && !row.isWithinModifiedPosition(mouseState, card))
            {
                clickedInCardBox = false;
                card.setRegular();
                boardFunc.SELECTEDCARD = null;
                boardFunc.boardPosLogic.updateBoard(boardFunc);
            }
        }

    }
}

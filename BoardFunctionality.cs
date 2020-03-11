﻿using Microsoft.Xna.Framework;
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

        public override void initializeGameComponent(ContentManager content)
        {
            //load all card textures
            //load decks of players

        }

        Side controllingPlayer;
        /*public void handleGameStates()
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
        }*/


        public void DrawCard(Side side)
        {
            actionConstructor.addDrawAction(side.Deck, side.Hand, this);
        }
        public void PlayCard(Side side, FunctionalRow row, Card card)
        {
            actionConstructor.moveTo(side.Hand, row, card, this);
           //card.playState = Card.PlayState.Revealed;
        }
        public void DiscardCard(Side side, Card card)
        {
            actionConstructor.moveTo(side.Hand, side.Oblivion, card, this);
        }
        public void DrawHand(Side side)
        {
            for (int i = 0; i < 7; i++)
            {
                DrawCard(side);
            }
        }
        public void StartTurn(Side side)
        {
            side.Player.ResetPlayer();
            DrawCard(side);

            /*
             * foreach effect in startturn effect -> trigger
             * 
             */
        }
        private void resetAllExhaustedCardsOnSide(Side side)
        {
            foreach(FunctionalRow row in side.Rows)
                {
                foreach(Card card in row.cardsInContainer)
                {
                    card.cardProps.exhausted = false;
                }
            }
        }
        public void PassTurn()
        {
            if (controllingPlayer == friendlySide)
            {
                controllingPlayer = enemySide;
            }
            else if(controllingPlayer == enemySide)
            {
                controllingPlayer = friendlySide;

            }

            StartTurn(controllingPlayer);
        }


        public void StartGame(Board board)
        {
            this.friendlySide = board.friendlySide;
            this.enemySide = board.enemySide;
            DrawHand(friendlySide);
            DrawHand(enemySide);

            controllingPlayer = friendlySide;

            boardPosLogic.updateBoard(this);

            assignCardPositionsOnHandExtension(board);
        }


        public override void drawSprite(SpriteBatch spriteBatch)
        {
            cardDrawer.drawSprite(spriteBatch, this);
            if(abilityButtons != null)
            {
                foreach (Button button in abilityButtons)
                {
                    button.drawSprite(spriteBatch);
                }
            }

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
            if (boardActions.isActive())
            {
                resetSelectedCard();
            }
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
            foreach (Card card in friendlySide.Oblivion.cardsInContainer)
            {
                card.updateGameComponent();
            }
            foreach (Card card in enemySide.Oblivion.cardsInContainer)
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

            foreach (FunctionalRow row in enemySide.Rows)
            {
                if (row.playState != Card.PlayState.Hidden)
                {
                    row.revealCardInContainer();
                }
            }

        }

        public override void mouseStateLogic(MouseState mouseState, ContentManager content)
        {
            if(controllingPlayer == friendlySide)
            {
                if (SELECTEDCARD == null && !boardActions.isActive())
                    friendlySide.Hand.modifyCardInteractivity(mouseState, this);

                foreach (FunctionalRow row in friendlySide.Rows)
                {
                    if (SELECTEDCARD == null && !boardActions.isActive())
                        row.modifyCardInteractivity(mouseState, this);
                }
                foreach (FunctionalRow row in enemySide.Rows)
                {
                    if (!boardActions.isActive())
                        row.modifyCardInteractivity(mouseState, this);
                }
                handFunction.setCardToMouse(mouseState, this);
                handFunction.playSelectedCard(mouseState, this);
                rowFunction.rowLogic(mouseState, this);
                if (abilityButtons != null)
                {
                    foreach (Button button in abilityButtons)
                    {
                        button.mouseStateLogic(mouseState, content);
                    }
                }

                    base.mouseStateLogic(mouseState, content);
            }
            else if(controllingPlayer == enemySide && !boardActions.isActive())
            {
                enemySide.Player.decide(this);
            }

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
        public Card ENEMYSELECTEDCARD;

        public bool cardView = false;
        public bool createButtonsOnView;
        public void viewFullSizeCard(MouseState mouseState, Card card)
        {
            if (cardView)
            {
                boardPosLogic.scaleToView(card);
                card.setPos(Game1.windowW / 2 - card.getWidth() / 2, Game1.windowH / 2 - card.getHeight() / 2);
                SELECTEDCARD = null;
                SELECTEDCARD = new Card(card);
                if (SELECTEDCARD != null)
                {
                    boardPosLogic.scaleToView(SELECTEDCARD);
                    SELECTEDCARD.setPos(Game1.windowW / 2 - card.getWidth() / 2, Game1.windowH / 2 - card.getHeight() / 2);
                    SELECTEDCARD.updateGameComponent();
                }
                if(ENEMYSELECTEDCARD != null)
                {

                }
                if(createButtonsOnView == false)
                {
                    showButtonsOnView(card);
                }

                if (mouseState.RightButton == ButtonState.Pressed)
                {
                    resetSelectedCard();
                    card.setRegular();
                    boardPosLogic.updateBoard(this);
                    abilityButtons = new List<Button>();
                    createButtonsOnView = false;
                    /*cardView = false;
                    card.setRegular();
                    SELECTEDCARD = null;
                    boardPosLogic.updateBoard(this);*/
                    //boardPosLogic.scaleToHand(card);
                }
            }
        }
        List<Button> abilityButtons = new List<Button>();
        private void showButtonsOnView(Card card)
        {
            foreach (Ability ability in card.cardProps.abilities)
            {
                Vector2 throwAwayLocation = new Vector2(0, 0);
                abilityButtons.Add(new Button(null, throwAwayLocation));
            }
            int counter = 0;
            foreach (Button button in abilityButtons)
            {
                button.setTexture(card.suppTextures.supplements[card.suppTextures.abilityDisplay].getTexture());
                button.setPos(new Vector2(card.getPosition().X + card.getWidth(), card.getPosition().Y + button.getHeight() * counter));


                ///

                //
                button.setAction(() => { /*ability[counter].target*/ });
                //
                //
                //

                counter++;

            }
            createButtonsOnView = true;
        }
        public void viewCardAndAbilities(MouseState mouseState, Card card)
        {
            boardPosLogic.scaleToView(card);
            card.setPos(Game1.windowW / 2 - card.getWidth(), Game1.windowH / 2 - card.getHeight() / 2);
            //card.displayAbilities

            if (mouseState.RightButton == ButtonState.Pressed)
            {
                resetSelectedCard();
                boardPosLogic.updateBoard(this);
                abilityButtons = null;
            }
        }
        public void resetSelectedCard()
        {
            cardView = false;
            if (SELECTEDCARD != null)
            {
                SELECTEDCARD.setRegular();
                SELECTEDCARD.resetCardSelector();
            }
            /*if (ENEMYSELECTEDCARD != null)
            {
                ENEMYSELECTEDCARD.setRegular();
                ENEMYSELECTEDCARD.resetCardSelector();
            }
            ENEMYSELECTEDCARD = null;*/
            SELECTEDCARD = null;
            //
        }

        public void Fight(Card card, Card otherCard)
        {
            boardPosLogic.scaleToHand(card);
            boardPosLogic.scaleToHand(otherCard);
            card.setRegular();
            otherCard.setRegular();
            int xpos = 50 + GraphicsSettings.toResolution(600);
            int ypos = Game1.windowH / 2 - card.getWidth() * 2;
            card.setPos(xpos - card.getWidth(), ypos);
            otherCard.setPos(xpos, ypos);
            showEnemyCard = true;

            Action newAction = () =>
            {
                deductAttributesAndDecideWinner(card, otherCard);
                showEnemyCard = false;
                boardActions.nextAction();
            };
            actionConstructor.addWaitAction(newAction, 60, this);
            
            
        }
        public bool showEnemyCard = false;
        public void deductAttributesAndDecideWinner(Card card, Card otherCard)
        {
            //throw new Exception();
            int firstcardpower = card.cardProps.power;
            int secondcardpower = otherCard.cardProps.power;
            int firstcarddefense = card.cardProps.defense;
            int secondcarddefense = otherCard.cardProps.defense;

            card.cardProps.defense -= secondcardpower;
            otherCard.cardProps.defense -= firstcardpower;

            card.setRegular();
            otherCard.setRegular();
            card.cardProps.exhausted = true;
            otherCard.cardProps.exhausted = true;
            boardPosLogic.scaleToBoard(card);
            boardPosLogic.scaleToBoard(otherCard);
            checkBothSidesForDead();
            boardPosLogic.updateBoard(this);

        }
        private void checkBothSidesForDead()
        {
            checkSideForDead(enemySide);
            checkSideForDead(friendlySide);
        }
        private void checkSideForDead(Side side)
        {
            foreach(FunctionalRow row in side.Rows)
            {
                foreach(Card card in row.cardsInContainer)
                {
                    checkIfDead(side, row, card);
                }
            }
        }
        private void checkIfDead(Side side, FunctionalRow row, Card card)
        {
            if (card.cardProps.defense <= 0)
            {
                Kill(side, row, card);
            }
        }
        public void Kill(Side side, FunctionalRow row, Card card)
        {
            actionConstructor.moveTo(row, side.Oblivion, card, this);
        }
    }
    public class RowFunctionality
    {
        GeneralLogic generalLogic;
        FieldUnitLogic fieldUnitLogic;
        ArmyLogic armyLogic;
        public RowFunctionality()
        {
            generalLogic = new GeneralLogic();
            fieldUnitLogic = new FieldUnitLogic();
            armyLogic = new ArmyLogic();
        }
        public void rowLogic(MouseState mouseState, BoardFunctionality boardFunc)
        {
            rowLogicForSide(mouseState, boardFunc.friendlySide, boardFunc, true);
            rowLogicForSide(mouseState, boardFunc.enemySide, boardFunc, false);
        }

        private void rowLogicForSide(MouseState mouseState, Side side, BoardFunctionality boardFunc, bool friendly)
        {
            foreach (FunctionalRow row in side.Rows)
            {
                switch (row.type)
                {
                    case CardType.Army:
                        break;
                    case CardType.FieldUnit:
                        fieldUnitLogic.setCardToView(mouseState, row, boardFunc, friendly);
                        break;
                    case CardType.General:
                        break;
                }
            }
        }
        public void fieldLogic(MouseState mouseState, FunctionalRow row, Card card, BoardFunctionality boardFunc)
        {



        }
    }

}
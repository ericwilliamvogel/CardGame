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

        public void assignAction()
        {

        }

        public override void initializeGameComponent(ContentManager content)
        {
            //load all card textures
            //load decks of players

        }

        Side controllingPlayer;

        public void DrawCard(Side side)
        {
            actionConstructor.addDrawAction(side.Deck, side.Hand, this);
        }
        public void PlayCard(Side side, FunctionalRow row, Card card)
        {
            actionConstructor.moveTo(side.Hand, row, card, this);
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
            resetSide(side);
        }
        private void resetSide(Side side)
        {
            foreach (FunctionalRow row in side.Rows)
            {
                foreach (Card card in row.cardsInContainer)
                {
                    card.cardProps.exhausted = false;
                }
            }
            foreach (FunctionalRow row in side.Rows)
            {
                foreach (Card card in row.cardsInContainer)
                {
                    if(card.cardProps.doubleExhausted == true)
                    {
                        card.cardProps.exhausted = true;
                        card.cardProps.doubleExhausted = false;
                    }
                }
            }
            foreach(FunctionalRow row in side.Rows)
            {
                foreach(Card card in row.cardsInContainer)
                {
                    card.cardProps.defense = card.cardProps.initialDefense;
                    card.cardProps.power = card.cardProps.initialPower;
                }
            }
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

            controllingPlayer = friendlySide;
            StartTurn(controllingPlayer);
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


                if (state != State.Selection)
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
            if (state == State.Selection && selection != null)
            {
                selection(mouseState);
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

        //public bool cardView = false;
        public bool createButtonsOnView;
        public void viewFullSizeCard(MouseState mouseState, Card card)
        {
            if (state == State.CardView)
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


                if (mouseState.RightButton == ButtonState.Pressed)
                {
                    resetSelectedCard();
                    card.setRegular();
                    boardPosLogic.updateBoard(this);
                    state = State.Regular;
                    /*cardView = false;
                    card.setRegular();
                    SELECTEDCARD = null;
                    boardPosLogic.updateBoard(this);*/
                    //boardPosLogic.scaleToHand(card);
                }
            }
        }
        public void viewCardWithAbilities(MouseState mouseState, Card card)
        {
            if (state == State.CardView)
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
                if (ENEMYSELECTEDCARD != null)
                {

                }
                if (createButtonsOnView == false)
                {
                    showButtonsOnView(mouseState, card);
                }

                if (mouseState.RightButton == ButtonState.Pressed)
                {
                    resetSelectedCard();
                    card.setRegular();
                    boardPosLogic.updateBoard(this);
                    createButtonsOnView = false;
                    abilityButtons = new List<Button>();
                    state = State.Regular;
                    /*cardView = false;
                    card.setRegular();
                    SELECTEDCARD = null;
                    boardPosLogic.updateBoard(this);*/
                    //boardPosLogic.scaleToHand(card);
                }
            }
        }
        List<Button> abilityButtons = new List<Button>();
        public enum State
        {
            Regular,
            CardView,
            Selection
        }
        public State state = State.Regular;
        private void showButtonsOnView(MouseState mouseState, Card card)
        {
            createButtonsOnView = true;
            int counter = 0;
            for (int i = 0; i < card.cardProps.abilities.Count; i++)
            {
                Vector2 throwAwayLocation = new Vector2(0, 0);
                abilityButtons.Add(new Button(null, throwAwayLocation));
                abilityButtons[i].setTexture(card.suppTextures.supplements[card.suppTextures.abilityDisplay].getTexture());
                abilityButtons[i].setPos(new Vector2(card.getPosition().X + card.getWidth(), card.getPosition().Y + abilityButtons[i].getHeight() * i));
                abilityButtons[i].setButtonText(card.cardProps.abilities[i].description);
                abilityButtons[i].wantedScale = 4f;
                card.cardProps.abilities[i].clickedInAbilityBox = false;
                //THE REASONING BEHIND THIS IS THAT THE I ITERATOR WILL END OUTSIDE OF THE ARRAY, AND EACH TIME THE BUTTONS ARE PRESSED THEY
                //WILL TRIGGER THE FUNCTION AT ITS MAXIMUM

                //IS THERE A BETTER WAY TO DO THIS?
                //PROBABLY
                //BUT ITS OK
                Action<MouseState> action = (MouseState newMouseState) => { };
                if (i == 0)
                {
                    action = (MouseState newMouseState) => {
                        card.cardProps.abilities[0].setTarget(newMouseState, this);
                        card.cardProps.abilities[0].activateAbilityOnSelection(newMouseState, this);
                        resetCardSelection(newMouseState);
                    };
                }
                if (i == 1)
                {
                    action = (MouseState newMouseState) => {
                        card.cardProps.abilities[1].setTarget(newMouseState, this);
                        card.cardProps.abilities[1].activateAbilityOnSelection(newMouseState, this);
                        resetCardSelection(newMouseState);
                    };
                }
                if (i == 2)
                {
                    action = (MouseState newMouseState) => {
                        card.cardProps.abilities[2].setTarget(newMouseState, this);
                        card.cardProps.abilities[2].activateAbilityOnSelection(newMouseState, this);
                        resetCardSelection(newMouseState);
                    };
                }
                if (i == 3)
                {
                    action = (MouseState newMouseState) => {
                        card.cardProps.abilities[3].setTarget(newMouseState, this);
                        card.cardProps.abilities[3].activateAbilityOnSelection(newMouseState, this);
                        resetCardSelection(newMouseState);
                    };
                }
                else
                {
                    Console.WriteLine("too many abilities in card");
                }
                //THIS ACTUALLY WORKED PERFECTLY IM SO MAD

                abilityButtons[i].setAction(() => {
                    state = State.Selection;

                    createButtonsOnView = false;
                    //int receivedint = i;
                    selection = action;
                    resetSelectedCard();
                    card.setRegular();
                    boardPosLogic.updateBoard(this);
                    abilityButtons = new List<Button>();

                });
                counter = i;
            }
            //throw new Exception(counter.ToString());

        }
        public Action<MouseState> selection;
        public void resetCardSelection(MouseState mouseState)
        {
            if(mouseState.RightButton == ButtonState.Pressed)
            {
                state = State.Regular;
                selection = null;
                resetSelectedCard();
                boardPosLogic.updateBoard(this);
                abilityButtons = new List<Button>();
                createButtonsOnView = false;
                if(selection!=null)
                {
                    throw new Exception("what is going on");

                }
            }
        }
        public void resetSelectedCard()
        {

            if (SELECTEDCARD != null)
            {
                SELECTEDCARD.setRegular();
                SELECTEDCARD.resetCardSelector();
            }

            SELECTEDCARD = null;

        }
        public void Exhaust(Card fromCard, Card targetCard)
        {
            boardPosLogic.scaleToHand(fromCard);
            boardPosLogic.scaleToHand(targetCard);
            fromCard.setRegular();
            targetCard.setRegular();
            int xpos = 50 + GraphicsSettings.toResolution(600);
            int ypos = Game1.windowH / 2 - fromCard.getWidth() * 2;
            fromCard.setPos(xpos - fromCard.getWidth(), ypos);
            targetCard.setPos(xpos, ypos);
            showEnemyCard = true;

            Action newAction = () =>
            {
                targetCard.cardProps.doubleExhausted = true;
                finalizeCardInteraction(fromCard, targetCard);

                //exhausts both units anyway

                boardActions.nextAction();
            };
            actionConstructor.addWaitAction(newAction, 60, this);
        }
        public void BoardDamage(Card fromCard, Ability ability)
        {
            boardPosLogic.scaleToHand(fromCard);
            fromCard.setRegular();
            int xpos = 50 + GraphicsSettings.toResolution(600);
            int ypos = Game1.windowH / 2 - fromCard.getWidth() * 2;
            fromCard.setPos(xpos - fromCard.getWidth(), ypos);
            showEnemyCard = true;


            Action newAction = () =>
            {
                dealBoardDamageAndDisposeOfDead(fromCard, ability);

                boardActions.nextAction();
            };
            actionConstructor.addWaitAction(newAction, 60, this);
        }
        public void DirectDamage(Card fromCard, Ability ability, Card targetCard)
        {

            boardPosLogic.scaleToHand(fromCard);
            boardPosLogic.scaleToHand(targetCard);
            fromCard.setRegular();
            targetCard.setRegular();
            int xpos = 50 + GraphicsSettings.toResolution(600);
            int ypos = Game1.windowH / 2 - fromCard.getWidth() * 2;
            fromCard.setPos(xpos - fromCard.getWidth(), ypos);
            targetCard.setPos(xpos, ypos);
            showEnemyCard = true;


            Action newAction = () =>
            {
                dealDirectDamageAndDisposeOfDead(fromCard, ability, targetCard);

                boardActions.nextAction();
            };
            actionConstructor.addWaitAction(newAction, 60, this);
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
            //int firstcarddefense = card.cardProps.defense;
            //int secondcarddefense = otherCard.cardProps.defense;

            card.cardProps.defense -= secondcardpower;
            otherCard.cardProps.defense -= firstcardpower;

            finalizeCardInteraction(card, otherCard);

        }
        public void dealDirectDamageAndDisposeOfDead(Card fromCard, Ability ability, Card targetCard)
        {
            int damage = ability.power;
            targetCard.cardProps.defense -= damage;
            finalizeCardInteraction(fromCard, targetCard);

        }
        public void dealBoardDamageAndDisposeOfDead(Card fromCard, Ability ability)
        {
            int damage = ability.power;
            foreach(Card card in enemySide.Rows[Side.FieldUnit].cardsInContainer)
            {
                card.cardProps.defense -= damage;
            }
            finalizeCardInteraction(fromCard, fromCard);
        }
        private void finalizeCardInteraction(Card card, Card otherCard)
        {
            card.setRegular();
            otherCard.setRegular();
            card.cardProps.exhausted = true;
            otherCard.cardProps.exhausted = true;
            boardPosLogic.scaleToBoard(card);
            boardPosLogic.scaleToBoard(otherCard);
            checkBothSidesForDead();
            showEnemyCard = false;
            selection = null;

            state = State.Regular;
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
    

}
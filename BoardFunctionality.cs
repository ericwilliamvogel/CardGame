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
        public ActionConstructor actionConstructor = new ActionConstructor();
        public HandFunctionality handFunction = new HandFunctionality();
        public RowFunctionality rowFunction = new RowFunctionality();
        GameComponent rowFog = new GameComponent();
        public void assignAction()
        {

        }

        public override void initializeGameComponent(ContentManager content)
        {
            rowFog.setSprite(content, "rowNotRevealed");
            //load all card textures
            //load decks of players

        }

        Side controllingPlayer;

        public void DrawCard(Side side)
        {

            actionConstructor.addDrawAction(side.Deck, side.Hand, this);
        }
        public void PlayCard(Side side,  Card card)
        {
            switch(card.cardProps.type)
            {
                case CardType.Army:
                    if(side.canPlayArmy)
                    {
                        playCardIfSideHasSufficientResources(side, card);

                    }
                    else
                    {
                        returnToHand(side, card);
                    }
                    break;
                default:
                    playCardIfSideHasSufficientResources(side, card);
                    break;
            }

            
        }
        private void playCardIfSideHasSufficientResources(Side side, Card card)
        {
            if (card.canBePlayed(side))
            {
                //int temporaryCounter = card.cardProps.cost.raceCost.Count;
                deductCostFromResourcesAndPlayCard(side, card);

            }
            else
            {
                returnToHand(side, card);
                //boardPosLogic.updateBoard(this);
                //throw new Exception("does not have enough MANA");
            }
        }
        private void deductCostFromResourcesAndPlayCard(Side side, Card card)
        {
            if (card.cardProps.cost.raceCost != null)
            {
                List<Card.Race> deductedResources = new List<Card.Race>();

                foreach (Card.Race cardResource in card.cardProps.cost.raceCost)
                {
                    bool check = false;
                    foreach (Card.Race resource in side.Resources)
                    {
                        if (cardResource == resource && check == false)
                        {
                            deductedResources.Add(resource);
                            check = true;
                        }
                    }
                }
                if (deductedResources.Count < card.cardProps.cost.raceCost.Count)
                {

                    returnToHand(side, card);

                }
                else
                {
                    foreach (Card.Race resource in deductedResources)
                    {
                        side.Resources.Remove(resource);

                    }
                    for (int i = 0; i < card.cardProps.cost.unanimousCost; i++)
                    {
                        side.Resources.Remove(0);
                    }
                    resumeWithPlayingCard(side, card);
                }
            }
            else
            {
                for (int i = 0; i < card.cardProps.cost.totalCost; i++)
                {
                    side.Resources.Remove(0);
                }
                resumeWithPlayingCard(side, card);
            }
        }
        private void resumeWithPlayingCard(Side side, Card card)
        {
            actionConstructor.moveTo(side.Hand, card.correctRow(side), card, this);

            if (card.cardProps.type == CardType.Army)
            {
                side.canPlayArmy = false;
            }
            else
            {
                card.cardProps.exhausted = true;
            }
        }
        private void returnToHand(Side side, Card card)
        {
            actionConstructor.moveTo(side.Hand, side.Hand, card, this);

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
                    if (card.cardProps.type != CardType.General)
                    {

                        card.cardProps.defense = card.cardProps.initialDefense;
                        card.cardProps.power = card.cardProps.initialPower;
                    }
                }
            }
            resetFog(enemySide);
            resetFog(friendlySide);
            enemySide.Resources = new List<Card.Race>();
            friendlySide.Resources = new List<Card.Race>();
            side.canPlayArmy = true;
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

        private void resetFog(Side side)
        {
            foreach (FunctionalRow row in side.Rows)
            {
                row.revealed = row.revealedTrueValue;
            }
        }
        public void PassTurn()
        {
            if (controllingPlayer == friendlySide)
            {
                controllingPlayer = enemySide;
                enemySide.boardFunc.controllingPlayer = enemySide.boardFunc.friendlySide;
            }
            else if(controllingPlayer == enemySide)
            {
                controllingPlayer = friendlySide;

            }

            StartTurn(controllingPlayer);
        }

        Action<Board> updateSide;
        public void initSide(Board board, Side RELATIVEfriendlySide)
        {
            if (board.enemySide == RELATIVEfriendlySide)
            {
                this.friendlySide = RELATIVEfriendlySide;
                updateSide = (Board newBoard) => {
                    this.enemySide = newBoard.friendlySide;
                    this.friendlySide = newBoard.enemySide;
                    newBoard.controllingPlayer = this.controllingPlayer;
                };
                updateSide(board);
            }
            if (board.friendlySide == RELATIVEfriendlySide)
            {
                this.friendlySide = RELATIVEfriendlySide;
                updateSide = (Board newBoard) => {
                    this.enemySide = newBoard.enemySide;
                    this.friendlySide = newBoard.friendlySide;
                    newBoard.controllingPlayer = this.controllingPlayer;
                };
            }
            updateSide(board);
        }
        public void StartGame(Board board, Side side)
        {



            controllingPlayer = friendlySide;
            StartTurn(controllingPlayer);
            DrawHand(friendlySide);
            DrawHand(enemySide);


            boardPosLogic.updateBoard(this);

            assignCardPositionsOnHandExtension(board);
        }


        public override void drawSprite(SpriteBatch spriteBatch)
        {
            foreach (FunctionalRow row in enemySide.Rows)
            {
                if (!row.revealed)
                {
                    rowFog.setPos(row.getPosition());
                    rowFog.drawSprite(spriteBatch);
                }
            }
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

            updateSide(board);
            boardActions.updateAnimations();
            updateAllAssets();
            if (boardActions.isActive())
            {
                resetSelectedCard();
            }

            /*if(enemySide.boardFunc.controllingPlayer == enemySide.boardFunc.enemySide)
            {
                controllingPlayer = friendlySide;
            }
            else
            {
                controllingPlayer = enemySide;
            }*/
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
                friendlySide.Player.decide(mouseState, content, this);
                    
            }
            else if(controllingPlayer == enemySide && !boardActions.isActive())
            {
                //enemySide.Player.decide(mouseState, content, this);
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
        public List<Button> abilityButtons = new List<Button>();
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
        public void RevealBoard(Card fromCard, Ability ability)
        {
            boardPosLogic.scaleToHand(fromCard);
            fromCard.setRegular();
            int xpos = 50 + GraphicsSettings.toResolution(600);
            int ypos = Game1.windowH / 2 - fromCard.getWidth() * 2;
            fromCard.setPos(xpos - fromCard.getWidth(), ypos);
            showEnemyCard = true;

            Action newAction = () =>
            {
                revealBoardForRemainderOfTurn(fromCard, ability);
                finalizeCardInteraction(fromCard, fromCard);

                //exhausts both units anyway

                boardActions.nextAction();
            };
            actionConstructor.addWaitAction(newAction, 60, this);
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
        public void BoardDamage(Card fromCard, Ability ability, Card targetCard)
        {
            boardPosLogic.scaleToHand(fromCard);
            fromCard.setRegular();
            int xpos = 50 + GraphicsSettings.toResolution(600);
            int ypos = Game1.windowH / 2 - fromCard.getWidth() * 2;
            fromCard.setPos(xpos - fromCard.getWidth(), ypos);
            showEnemyCard = true;


            Action newAction = () =>
            {
                dealBoardDamageAndDisposeOfDead(fromCard, ability, targetCard);

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
        public void revealBoardForRemainderOfTurn(Card fromCard, Ability ability)
        {
            foreach(FunctionalRow row in enemySide.Rows)
            {
                row.revealed = true;
            }

        }
        public void dealBoardDamageAndDisposeOfDead(Card fromCard, Ability ability, Card targetCard)
        {

            int damage = ability.power;
            foreach(Card card in targetCard.correctRow(enemySide).cardsInContainer)
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
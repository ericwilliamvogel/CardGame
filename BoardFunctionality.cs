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

        public MessageBox BOARDMESSAGE = new MessageBox();
        public BoardCardDrawer cardDrawer = new BoardCardDrawer();
        public BoardPositionUpdater boardPosLogic = new BoardPositionUpdater();
        public BoardActions boardActions = new BoardActions();
        public ActionConstructor actionConstructor = new ActionConstructor();
        public HandFunctionality handFunction = new HandFunctionality();
        public RowFunctionality rowFunction = new RowFunctionality();
        public BoardDefinitions boardDef = new BoardDefinitions();
        public SideSetter sideSetter = new SideSetter();
        public BoardCardViewer cardViewer = new BoardCardViewer();
        public CardImageStorage library = new CardImageStorage();
        public GameComponent rowFog = new GameComponent();
        public CardBuilder cardBuilder = new CardBuilder();
        public CardConstructor cardConstructor = new CardConstructor();
        public BoardFunctionalityAssetUpdater assetUpdater = new BoardFunctionalityAssetUpdater();
        public MoveHistory moveHistory = new MoveHistory();
        public Popup endGamePopup = new Popup(1);
        public FunctionalRow castManuever = new FunctionalRow(CardType.Manuever);
        public enum State
        {
            Regular,
            CardView,
            Selection
        }
        public Side friendlySide;
        public Side enemySide;
        public Side controllingPlayer;
        public Card SELECTEDCARD;
        public Card ENEMYSELECTEDCARD;
        public State state = State.Regular;
        public bool showEnemyCard = false;
        public UniqueButtonPopper<UniqueWindow<GameComponent>, GameComponent> historyButton;
        public UniqueWindow<GameComponent> historyWindow;
        public UniqueWindow<GameComponent> enemyHistoryWindow;
        public void passDown(CardImageStorage library, CardConstructor constructor)
        {
            cardConstructor = constructor;
            this.library = library;
        }
        public T duplicate<T>(T input)
        {
            T item = input;
            return item;
        }
        public override void initializeGameComponent(ContentManager content)
        {
            enemyHistoryWindow = new UniqueWindow<GameComponent>(content, 6);
            //newHistory = duplicate(moveHistory);
            //newHistory.type = MoveHistory.Type.Previous;
            enemyHistoryWindow.updateObj(moveHistory);
            enemyHistoryWindow.setPos(Game1.windowW / 2, Game1.windowH / 2);
            historyWindow = new UniqueWindow<GameComponent>(content, 6);
            //historyWindow.setPos(Game1.windowW - GraphicsSettings.toResolution(500), 100);
            historyButton = new UniqueButtonPopper<UniqueWindow<GameComponent>, GameComponent>(content, historyWindow);
            historyButton.setSprite(content, "pastMovesButton");
            historyButton.setButtonText("Past Moves");
            historyButton.wantedScale = .5f;
            historyButton.centerText(historyButton.getWidth() / 2 - GraphicsSettings.toResolution(80), historyButton.getHeight() / 2 - GraphicsSettings.toResolution(40));
            historyButton.updateObj(moveHistory);
            historyButton.setPos(Game1.windowW - GraphicsSettings.toResolution(500), 0);

            historyWindow.setPos(Game1.windowW - GraphicsSettings.toResolution(500), 100);
            UpButton upButton = new UpButton();
            upButton.setTexture(UpButton.texture);
            upButton.setOffset(historyWindow.getWidth(), 0);

            DownButton downButton = new DownButton();
            downButton.setTexture(DownButton.texture);
            downButton.setOffset(historyWindow.getWidth() , upButton.getHeight() * 2);

            LeftButton leftButton = new LeftButton();
            leftButton.setTexture(LeftButton.texture);
            leftButton.setOffset(0, historyWindow.getHeight()/2);

            RightButton rightButton = new RightButton();
            rightButton.setTexture(RightButton.texture);
            rightButton.setOffset(historyWindow.getWidth(), historyWindow.getHeight() / 2);

            upButton.setAction(()=> {
                moveHistory.ScrollUp();
            });
            downButton.setAction(() => {
                moveHistory.ScrollDown();
            });
            rightButton.setAction();
            leftButton.setAction();

            historyWindow.addNewComponent(upButton);
            historyWindow.addNewComponent(downButton);
            historyWindow.addNewComponent(rightButton);
            historyWindow.addNewComponent(leftButton);

            castManuever.setPos(Game1.windowW - GraphicsSettings.toResolution(400), GraphicsSettings.toResolution(200));
            endGamePopup.initializeGameComponent(content);
            rowFog.setSprite(content, "rowNotRevealed");
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
            cardViewer.drawSprite(spriteBatch);
            BOARDMESSAGE.drawSprite(spriteBatch);
            //moveHistory.drawSprite(spriteBatch);
            historyButton.drawSprite(spriteBatch);
            enemyHistoryWindow.drawSprite(spriteBatch);
            endGamePopup.drawSprite(spriteBatch);
        }
        MoveHistory newHistory;
        public void Update(Board board)
        {
            //newHistory = duplicate(moveHistory);
            //newHistory.type = MoveHistory.Type.Previous;
            //enemyHistoryWindow.updateObj(newHistory);
            historyButton.updateObj(moveHistory);

            boardActions = board.boardActions;
            //moveHistory = board.moveHistory;
            sideSetter.updateSide(board);
            //boardActions.updateAnimations();
            assetUpdater.updateAllAssets(this);
            if(boardActions.isActive())
            {
                cardViewer.resetSelectedCard(this);
            }
            //moveHistory.updateGameComponent();
        }
        public override void mouseStateLogic(MouseState mouseState, ContentManager content)
        {
            if (controllingPlayer == friendlySide)
            {
                friendlySide.Player.decide(mouseState, content, this);

            }
            if (cardViewer.SelectionStillActive())
            {
                cardViewer.selection[0](mouseState);
            }

            enemyHistoryWindow.mouseStateLogic(mouseState, content);
            historyButton.mouseStateLogic(mouseState, content);

            endGameIfOver();
        }







        public void DrawCard(Side side)
        {
            actionConstructor.addDrawAction(side.Deck, side.Hand, this);

        }
        public void PlayCard(Side side,  Card card)
        {
            boardDef.performChecksThenPlayCard(side, card, this);
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
            sideSetter.resetSide(side, this);
        }
        
        
        public void PassTurn()
        {
            boardActions.AddAction(() =>
           {
               //enemySide.boardFunc.enemyHistoryWindow.Show();
               moveHistory.storeTurnAndReset();
               enemySide.boardFunc.moveHistory.storeTurnAndReset();

               if (controllingPlayer == friendlySide)
               {
                   controllingPlayer = enemySide;
                   enemySide.boardFunc.controllingPlayer = enemySide.boardFunc.friendlySide;
               }
               else if (controllingPlayer == enemySide)
               {
                   controllingPlayer = friendlySide;

               }

               StartTurn(controllingPlayer);
               boardActions.nextAction();
           });

        }

        
        public void StartGame(Board board, Side side)
        {



            controllingPlayer = friendlySide;
            Update(board);
            StartTurn(controllingPlayer);
            DrawHand(friendlySide);
            DrawHand(enemySide);


            boardPosLogic.updateBoard(this);

            //assignCardPositionsOnHandExtension(board);
        }

        private Card duplicate(Card card)
        {
            Card duplicate = cardBuilder.cardConstruct(cardConstructor, card.cardProps.identifier);
            duplicate.suppTextures.supplements[duplicate.suppTextures.portrait].setTexture(library.cardTextureDictionary[duplicate.cardProps.identifier]);
            duplicate.setSupplementalTextures(library);
            duplicate.setColorForRace();
            duplicate.setScale(CardScale.Board);
            duplicate.initSupplements();
            return duplicate;
        }

        
        public void CreateAndDrawSpell(Card fromCard, CreateSpell ability)
        {
            setUpCard(fromCard);
            sendActionToQueue(() => {
                hideMoveFromEnemyAndDisplay(fromCard, ability);
                boardDef.drawSpecifiedSpell(fromCard, ability, this);
                finalizeCardInteraction(fromCard, fromCard);
            });
        }

        public void AbilityDrawCard(Card fromCard, Ability ability, Side side)
        {
            setUpCard(fromCard);
            sendActionToQueue(() => {
                hideMoveFromEnemyAndDisplay(fromCard, ability);
                for(int i = 0; i < ability.power; i++)
                {
                    DrawCard(side);
                    finalizeCardInteraction(fromCard, fromCard);
                }
            });
        }

        public void AbilityBothDrawCard(Card fromCard, Ability ability, Side side)
        {
            setUpCard(fromCard);
            sendActionToQueue(() => {
                hideMoveFromEnemyAndDisplay(fromCard, ability);
                for (int i = 0; i < ability.power; i++)
                {
                    DrawCard(side);
                    DrawCard(side.boardFunc.enemySide);
                    finalizeCardInteraction(fromCard, fromCard);
                }
            });
        }
        public void LifeDamage(Card fromCard)
        {
            setUpCard(fromCard);
            sendActionToQueue(() => {
                Card placeholder = duplicate(fromCard);
                placeholder.playState = Card.PlayState.Hidden;
                moveHistory.AddNewAttackMove(duplicate(fromCard), placeholder);
                enemySide.boardFunc.moveHistory.AddNewAttackMove(duplicate(fromCard), placeholder);
                boardDef.dealPlayerDamage(fromCard, this);
                finalizeCardInteraction(fromCard, fromCard);
                endGameIfOver();
            });
        }
        public void LifeDamage(Card fromCard, Ability ability)
        {
            setUpCard(fromCard);
            sendActionToQueue(() => {
                Card placeholder = duplicate(fromCard);
                placeholder.playState = Card.PlayState.Hidden;
                moveHistory.AddTargetAbilityMove(duplicate(fromCard), ability, placeholder);
                enemySide.boardFunc.moveHistory.AddTargetAbilityMove(duplicate(fromCard), ability, placeholder);
                boardDef.dealPlayerDamage(fromCard, ability, this);
                finalizeCardInteraction(fromCard, fromCard);
                endGameIfOver();
            });
        }
        public void RevealBoard(Card fromCard, Ability ability)
        {
            setUpCard(fromCard);
            sendActionToQueue(() => {
                hideMoveFromEnemyAndDisplay(fromCard, ability);
                boardDef.revealBoardForRemainderOfTurn(fromCard, ability, this);
                finalizeCardInteraction(fromCard, fromCard);
            });
        }
        private void hideMoveFromEnemyAndDisplay(Card fromCard, Ability ability)
        {
            moveHistory.AddSoloAbilityMove(duplicate(fromCard), ability);
            enemySide.boardFunc.moveHistory.AddHiddenMove(duplicate(fromCard));
        }
        public void Exhaust(Card fromCard, Ability ability, Card targetCard)
        {
            setUpCard(fromCard, targetCard);
            sendActionToQueue(() => {
                moveHistory.AddTargetAbilityMove(duplicate(fromCard), ability, duplicate(targetCard));
                targetCard.cardProps.doubleExhausted = true;
                finalizeCardInteraction(fromCard, targetCard);
            });
        }
        public void BoardDamage(Card fromCard, Ability ability, Card targetCard)
        {
            setUpCard(fromCard);
            sendActionToQueue(() => {
                moveHistory.AddTargetAbilityMove(duplicate(fromCard), ability, duplicate(targetCard));
                enemySide.boardFunc.moveHistory.AddTargetAbilityMove(duplicate(fromCard), ability, duplicate(targetCard));
                boardDef.dealBoardDamageAndDisposeOfDead(fromCard, ability, targetCard, this);
                finalizeCardInteraction(fromCard, fromCard);
            });
        }
        public void DirectDamage(Card fromCard, Ability ability, Card targetCard)
        {
            setUpCard(fromCard, targetCard);
            sendActionToQueue(() => {
                moveHistory.AddTargetAbilityMove(duplicate(fromCard), ability, duplicate(targetCard));
                enemySide.boardFunc.moveHistory.AddTargetAbilityMove(duplicate(fromCard), ability, duplicate(targetCard));
                boardDef.dealDirectDamageAndDisposeOfDead(fromCard, ability, targetCard);
                finalizeCardInteraction(fromCard, targetCard);
            });
        }
        public void DirectDamage(Card fromCard, Ability ability)
        {
            setUpCard(fromCard);
            sendActionToQueue(() => {
                LifeDamage(fromCard);
            });
        }
        public void KillTarget(Card fromCard, Ability ability, Card targetCard)
        {
            setUpCard(fromCard, targetCard);
            sendActionToQueue(() => {
                moveHistory.AddTargetAbilityMove(duplicate(fromCard), ability, duplicate(targetCard));
                enemySide.boardFunc.moveHistory.AddTargetAbilityMove(duplicate(fromCard), ability, duplicate(targetCard));
                Kill(enemySide, targetCard.correctRow(enemySide), targetCard);
                finalizeCardInteraction(fromCard, targetCard);
            });
        }
        public void SpawnCard(Card fromCard, SpawnCard spawn)
        {
            setUpCard(fromCard);
            sendActionToQueue(() => {
                hideMoveFromEnemyAndDisplay(fromCard, spawn);
                boardDef.spawnSpecifiedUnit(fromCard, spawn, this);
                finalizeCardInteraction(fromCard, fromCard);
            });
        }

        public void Fight(Card card, Card otherCard)
        {
            setUpCard(card, otherCard);
            sendActionToQueue(() => {
                moveHistory.AddNewAttackMove(duplicate(card), duplicate(otherCard));
                enemySide.boardFunc.moveHistory.AddNewAttackMove(duplicate(card), duplicate(otherCard));
                boardDef.deductAttributesAndDecideWinner(card, otherCard);
                finalizeCardInteraction(card, otherCard);
            }, 30);
        }

        private void sendActionToQueue(Action action, int waitTime)
        {
            Action newAction = () =>
            {
                action();
                boardActions.nextAction();
            };
            actionConstructor.addWaitAction(newAction, waitTime, this);
        }
        private void sendActionToQueue(Action action)
        {
            Action newAction = () =>
            {
                action();
                boardActions.nextAction();
            };
            actionConstructor.addWaitAction(newAction, 30, this);
        }
        private void setUpCard(Card card)
        {
            boardPosLogic.scaleToHand(card);
            card.setRegular();
            int xpos = 50 + GraphicsSettings.toResolution(600);
            int ypos = Game1.windowH / 2 - card.getWidth() * 2;
            card.setPos(xpos - card.getWidth(), ypos);
            showEnemyCard = true;
        }
        private void setUpCard(Card fromCard, Card targetCard)
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
        }

        public void finalizeCardInteraction(Card card, Card otherCard)
        {
            finalizeSingularCard(card);
            finalizeSingularCard(otherCard);
            checkBothSidesForDead();
            showEnemyCard = false;
            state = State.Regular;
            boardPosLogic.updateBoard(this);
        }
        private void endGameIfOver()
        {
            if(friendlySide.LifeTotal <= 0 || enemySide.LifeTotal <= 0)
            {
                endGamePopup.SetPopup();
                enemySide.boardFunc.endGamePopup.SetPopup();
                boardActions.AddAction(() => { /*this prevents any more moves*/ });
            }
        }
        private void finalizeSingularCard(Card card)
        {
            card.setRegular();
            card.cardProps.exhausted = true;
            boardPosLogic.scaleToBoard(card);
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
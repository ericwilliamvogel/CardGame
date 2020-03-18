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

        public void passDown(CardImageStorage library, CardConstructor constructor)
        {
            cardConstructor = constructor;
            this.library = library;
        }

        public override void initializeGameComponent(ContentManager content)
        {
            cardBuilder = new CardBuilder();
            cardConstructor = new CardConstructor();
            Card card = cardBuilder.cardConstruct(cardConstructor, 10);
            card = cardBuilder.cardConstruct(cardConstructor, 11);
            library = cardConstructor.tempStorage;
            library.loadCardSupplementalTextures(content);
            library.loadAllDictionaryTextures(content);
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


        }
        public void Update(Board board)
        {
            sideSetter.updateSide(board);
            boardActions.updateAnimations();
            assetUpdater.updateAllAssets(this);
            if(boardActions.isActive())
            {
                cardViewer.resetSelectedCard(this);
            }
        }
        public override void mouseStateLogic(MouseState mouseState, ContentManager content)
        {
            if (controllingPlayer == friendlySide)
            {
                friendlySide.Player.decide(mouseState, content, this);

            }
            if (state == State.Selection && cardViewer.selection != null)
            {
                cardViewer.selection(mouseState);
            }
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
            StartTurn(controllingPlayer);
            DrawHand(friendlySide);
            DrawHand(enemySide);


            boardPosLogic.updateBoard(this);

            //assignCardPositionsOnHandExtension(board);
        }


        


        public void LifeDamage(Card fromCard)
        {
            setUpCard(fromCard);
            sendActionToQueue(() => {
                boardDef.dealPlayerDamage(fromCard, this);
                finalizeCardInteraction(fromCard, fromCard);
            });
        }
        public void RevealBoard(Card fromCard, Ability ability)
        {
            setUpCard(fromCard);
            sendActionToQueue(() => {
                boardDef.revealBoardForRemainderOfTurn(fromCard, ability, this);
                finalizeCardInteraction(fromCard, fromCard);
            });
        }
        public void Exhaust(Card fromCard, Card targetCard)
        {
            setUpCard(fromCard, targetCard);
            sendActionToQueue(() => {
                targetCard.cardProps.doubleExhausted = true;
                finalizeCardInteraction(fromCard, targetCard);
            });
        }
        public void BoardDamage(Card fromCard, Ability ability, Card targetCard)
        {
            setUpCard(fromCard);
            sendActionToQueue(() => {
                boardDef.dealBoardDamageAndDisposeOfDead(fromCard, ability, targetCard, this);
                finalizeCardInteraction(fromCard, fromCard);
            });
        }
        public void DirectDamage(Card fromCard, Ability ability, Card targetCard)
        {
            setUpCard(fromCard, targetCard);
            sendActionToQueue(() => {
                boardDef.dealDirectDamageAndDisposeOfDead(fromCard, ability, targetCard);
                finalizeCardInteraction(fromCard, targetCard);
            });
        }
        public void SpawnCard(Card fromCard, SpawnCard ability)
        {
            setUpCard(fromCard);
            sendActionToQueue(() => {
                boardDef.spawnSpecifiedUnit(fromCard, ability, this);
                finalizeCardInteraction(fromCard, fromCard);
            });
        }

        public void Fight(Card card, Card otherCard)
        {
            setUpCard(card, otherCard);
            sendActionToQueue(() => {
                boardDef.deductAttributesAndDecideWinner(card, otherCard);
                finalizeCardInteraction(card, otherCard);
            });
        }

        private void sendActionToQueue(Action action)
        {
            Action newAction = () =>
            {
                action();
                boardActions.nextAction();
            };
            actionConstructor.addWaitAction(newAction, 60, this);
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
            cardViewer.selection = null;

            state = State.Regular;
            boardPosLogic.updateBoard(this);
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
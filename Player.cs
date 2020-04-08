using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using static CardGame.BoardFunctionality;

namespace CardGame
{

    public class Player
    {
        public enum TurnCycle
        {
            Beginning
        }

        public Deck deck;

        private bool control;
        public Player enemy;
        public int handSize = 7;
        public Player()
        {

        }
        public Player(Side side)
        {
            this.deck = deck;
        }

        public void DrawHand()
        {
            for (int i = 0; i < handSize; i++)
            {
                //hand
            }
        }
        public void Draw()
        {
            //Card card = deck.returnTopCardAndRemoveFromDeck();
            //hand.addCard(card);
        }
        public void ShuffleDeck()
        {
            deck.Shuffle();
        }
        public virtual void hasControl()
        {
            control = true;
        }
        public virtual void loseControl()
        {
            control = false;
        }
        public virtual void ResetPlayer()
        {
            hasPlayedArmyThisTurn = false;
        }
        public virtual void decide(MouseState mouseState, ContentManager content, BoardFunctionality boardFunc)
        {

        }
        public bool hasPlayedArmyThisTurn;
    }
    public class ActivePlayer : Player
    {

        public ActivePlayer()
        {

        }
        public override void decide(MouseState mouseState, ContentManager content, CardGame.BoardFunctionality boardFunc)
        {

            if (/*boardFunc.SELECTEDCARD == null && */!boardFunc.boardActions.isActive() && !boardFunc.handFunction.placingCard)
                boardFunc.friendlySide.Hand.modifyCardInteractivity(mouseState, boardFunc);

            boardFunc.handFunction.setCardToMouse(mouseState, boardFunc);
            boardFunc.handFunction.playSelectedCard(mouseState, boardFunc);
            foreach (FunctionalRow row in boardFunc.friendlySide.Rows)
            {
                if (boardFunc.SELECTEDCARD == null && !boardFunc.boardActions.isActive())
                    row.modifyCardInteractivity(mouseState, boardFunc);
            }
            foreach (FunctionalRow row in boardFunc.enemySide.Rows)
            {
                if (!boardFunc.boardActions.isActive() && row.revealed)
                    row.modifyCardInteractivity(mouseState, boardFunc);
            }


            if (boardFunc.state != State.Selection)
                boardFunc.rowFunction.rowLogic(mouseState, boardFunc);





            boardFunc.cardViewer.updateButtonsOnPopup(mouseState, content);

            if (boardFunc.cardViewer.SelectionStillActive())
            {
                boardFunc.cardViewer.selection[0](mouseState);
            }
            if (boardFunc.enemySide.boardFunc.cardViewer.SelectionStillActive())
            {
                boardFunc.enemySide.boardFunc.cardViewer.selection[0](mouseState);
            }
        }
    }

    public class AIPlayer : Player
    {
        List<Card> knownEnemyGenerals = new List<Card>();
        List<Card> knownEnemyArmies = new List<Card>();
        AIFunctionality AI;
        bool endFirstDec = false;
        bool endSecondDec = false; //so that the army container updates by the time we decide to play cards
        public AIPlayer()
        {
            AI = new AIFunctionality(this);
        }

        public override void ResetPlayer()
        {
            endFirstDec = false;
            endSecondDec = false;
            base.ResetPlayer();
        }
        public override void decide(MouseState mouseState, ContentManager content, BoardFunctionality boardFunc)
        {

            if (!endFirstDec && boardFunc.boardActions.actions.Count < 1)
            {
                AI.playArmies(boardFunc);
                AI.generalAbiltiies(mouseState, boardFunc);
                AI.attackIfBeneficial(boardFunc);
                endFirstDec = true;
            }


            if (boardFunc.boardActions.actions.Count < 1)
            {
                if (!endSecondDec)
                {
                    AI.playCardIfThereAreEnoughArmies(boardFunc);
                    boardFunc.PassTurn();
                }
                endSecondDec = true;
            }

        }

        public void VERYBASICATTACKLOGIC(BoardFunctionality boardFunc)
        {
            foreach (Card card in boardFunc.friendlySide.Rows[Side.FieldUnit].cardsInContainer)
            {
                foreach (Card enemyCard in boardFunc.enemySide.Rows[Side.FieldUnit].cardsInContainer)
                {
                    if (!card.cardProps.exhausted)
                    {
                        if (enemyCard.cardProps.defense - card.cardProps.power <= 0)
                        {
                            card.cardProps.exhausted = true;
                            boardFunc.Fight(card, enemyCard);
                        }
                        else if (card.cardProps.defense - enemyCard.cardProps.power > 0)
                        {
                            card.cardProps.exhausted = true;
                            boardFunc.Fight(card, enemyCard);
                        }

                    }
                }
            }
        }

    }

    
    
}

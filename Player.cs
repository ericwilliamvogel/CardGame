using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
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
        protected bool hasPlayedArmyThisTurn;
    }
    public class ActivePlayer : Player
    {
        public override void decide(MouseState mouseState, ContentManager content, BoardFunctionality boardFunc)
        {
            if (boardFunc.SELECTEDCARD == null && !boardFunc.boardActions.isActive())
                boardFunc.friendlySide.Hand.modifyCardInteractivity(mouseState, boardFunc);

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
            boardFunc.handFunction.setCardToMouse(mouseState, boardFunc);
            boardFunc.handFunction.playSelectedCard(mouseState, boardFunc);


            if (boardFunc.state != State.Selection)
                boardFunc.rowFunction.rowLogic(mouseState, boardFunc);

            if (boardFunc.abilityButtons != null)
            {
                foreach (Button button in boardFunc.abilityButtons)
                {
                    button.mouseStateLogic(mouseState, content);
                }
            }

           
        }
    }
    public class AIPlayer : Player
    {

        int currentArmyCount;
        public override void decide(MouseState mouseState, ContentManager content, BoardFunctionality boardFunc)
        {
            foreach(Card card in boardFunc.friendlySide.Hand.cardsInContainer)
            {
                //boardFunc.PlayCard(boardFunc.friendlySide, /*boardFunc.enemySide.Rows[Side.FieldUnit],*/ card);
                playArmies(card, boardFunc);
            }
            playCardIfThereAreEnoughArmies(boardFunc);
            attackIfBeneficial(boardFunc);
            //throw new
            boardFunc.PassTurn();
        }
        private void attackIfBeneficial(BoardFunctionality boardFunc)
        {
            List<int> valueOfPlay = new List<int>();
            foreach(Card card in boardFunc.friendlySide.Rows[Side.General].cardsInContainer)
            {

            }
            foreach (Card card in boardFunc.friendlySide.Rows[Side.FieldUnit].cardsInContainer)
            {

            }
        }
        private void exchangeValues(Card card, Card otherCard)
        {

        }
        private void playArmies(Card card, BoardFunctionality boardFunc)
        {
            if(card.cardProps.type == CardType.Army && !hasPlayedArmyThisTurn)
            {
                boardFunc.PlayCard(boardFunc.friendlySide,/* boardFunc.enemySide.Rows[Side.Armies],*/ card);
            }
        }
        private void playCardIfThereAreEnoughArmies(BoardFunctionality boardFunc)
        {
            int counter = 0;
            foreach(Card newCard in boardFunc.friendlySide.Rows[Side.Armies].cardsInContainer)
            {
                if(!newCard.cardProps.exhausted)
                {
                    counter++;
                }

            }
            foreach(Card newCard in boardFunc.friendlySide.Hand.cardsInContainer)
            {
                if(newCard.cardProps.cost.totalCost <= counter)
                {
                    exhaustArmies(boardFunc.friendlySide, newCard); 
                    boardFunc.PlayCard(boardFunc.friendlySide, /*boardFunc.enemySide.Rows[Side.FieldUnit]*/ newCard);
                }
            }

        }

        public void exhaustArmies(Side side, Card card)
        {
            if (card.cardProps.cost.raceCost != null)
            {
                List<Card> deductedResources = new List<Card>();

                foreach (Card.Race cardResource in card.cardProps.cost.raceCost)
                {
                    bool check = false;
                    foreach (Card army in side.Rows[Side.Armies].cardsInContainer)
                    {
                        if (cardResource == army.race && check == false)
                        {
                            deductedResources.Add(army);
                            check = true;
                        }
                    }
                }
                if (deductedResources.Count >= card.cardProps.cost.raceCost.Count)
                {
                    foreach (Card newCard in deductedResources)
                    {
                        exhaustUnit(side, newCard);
                    }
                }
            }
            else
            {
                for (int i = 0; i < card.cardProps.cost.totalCost; i++)
                {
                    exhaustUnit(side, side.Rows[Side.Armies].cardsInContainer[i]);
                }
            }
        }
        private void exhaustUnit(Side side, Card card)
        {
            card.cardProps.exhausted = true;
            side.Resources.Add(card.race);
        }

        private FunctionalRow getCorrectRow(Card card, BoardFunctionality boardFunc)
        {
            foreach(FunctionalRow row in boardFunc.friendlySide.Rows)
            {
                if(row.type == card.cardProps.type)
                {
                    return row;
                }
            }
            return null;
        }
    }
}

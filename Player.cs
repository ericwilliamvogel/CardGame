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
        protected bool hasPlayedArmyThisTurn;
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

           
        }
    }

    public class AIPlayer : Player
    {
        bool endTurnOnce = false;
        public override void ResetPlayer()
        {
            endTurnOnce = false;
            base.ResetPlayer();
        }
        int currentArmyCount;
        public override void decide(MouseState mouseState, ContentManager content, BoardFunctionality boardFunc)
        {
            if (!endTurnOnce && boardFunc.boardActions.actions.Count < 1)
            {
                playArmies(boardFunc);
                playCardIfThereAreEnoughArmies(boardFunc);
                VERYBASICATTACKLOGIC(boardFunc);
                //attackIfBeneficial(boardFunc);
                //throw new
                if (!endTurnOnce )
                {
                    boardFunc.PassTurn();

                }
                endTurnOnce = true;
            }

        }

        public void VERYBASICATTACKLOGIC(BoardFunctionality boardFunc)
        {
            foreach(Card card in boardFunc.friendlySide.Rows[Side.FieldUnit].cardsInContainer)
            {
                foreach(Card enemyCard in boardFunc.enemySide.Rows[Side.FieldUnit].cardsInContainer)
                {
                    if(!card.cardProps.exhausted)
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
        private void playArmies(BoardFunctionality boardFunc)
        {
            bool trigger = false;
            if(!hasPlayedArmyThisTurn)
            {
                foreach(Card card in boardFunc.friendlySide.Hand.cardsInContainer)
                {
                    if (card.cardProps.type == CardType.Army && trigger == false)
                    {
                        boardFunc.PlayCard(boardFunc.friendlySide, card);
                        trigger = true;
                    }
                }

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
                    //exhaustArmies(boardFunc.friendlySide, newCard); 
                    boardFunc.PlayCard(boardFunc.friendlySide, /*boardFunc.enemySide.Rows[Side.FieldUnit]*/ newCard);
                    counter -= newCard.cardProps.cost.totalCost;
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

    /*private void attackIfBeneficial(MouseState mouseState, BoardFunctionality boardFunc)
{
    Board tempBoard = new Board();
    BoardFunctionality tempBoardFunc = new BoardFunctionality();
    List<int> valueOfPlay = new List<int>();
    List<Outcome> outcomes = new List<Outcome>();

    int counter = 0;
    int thisSideValue = 0;
    int thatSideValue = 0;
    int valueDifference = 0;
    int finalValueDifference = 0;

    foreach (Card card in boardFunc.friendlySide.Rows[Side.General].cardsInContainer)
    {
        thisSideValue += card.returnValue();
    }
    foreach (Card card in boardFunc.friendlySide.Rows[Side.FieldUnit].cardsInContainer)
    {
        thisSideValue += card.returnValue();
    }
    foreach (Card card in boardFunc.enemySide.Rows[Side.FieldUnit].cardsInContainer)
    {
        thatSideValue += card.returnValue();
    }

    valueDifference = (int)thisSideValue - thatSideValue;

    foreach (Card card in boardFunc.friendlySide.Rows[Side.General].cardsInContainer)
    {
        outcomes.Add(new Outcome(card));
        foreach (Ability ability in card.cardProps.abilities)
        {
            foreach (FunctionalRow row in boardFunc.enemySide.Rows)
            {
                if (row.revealed)
                {
                    foreach (Card enemyCard in row.cardsInContainer)
                    {
                        Action action = () =>
                        {
                            ability.useAIAbility(this, boardFunc, enemyCard);
                        };
                        outcomes[counter].actions.Add(action);

                        Action<BoardFunctionality> realAction = (BoardFunctionality realFunc) =>
                        {
                            ability.useAbility(mouseState, realFunc);
                        };
                        outcomes[counter].REALACTIONS.Add(realAction);
                    }


                }
            }
            Action noAction = () =>
            {

            };
            outcomes[counter].actions.Add(noAction);
        }
        counter++;
    }



    foreach (Card card in boardFunc.friendlySide.Rows[Side.FieldUnit].cardsInContainer)
    {
        outcomes.Add(new Outcome(card));
        foreach (FunctionalRow row in boardFunc.enemySide.Rows)
        {
            if (row.revealed)
            {
                foreach (Card enemyCard in row.cardsInContainer)
                {
                    Action action = () =>
                    {
                        card.cardProps.aiCalcDefense -= enemyCard.cardProps.power;
                        enemyCard.cardProps.aiCalcDefense -= card.cardProps.power;
                    };
                    outcomes[counter].actions.Add(action);

                    Action<BoardFunctionality> realAction = (BoardFunctionality realFunc) =>
                    {
                        realFunc.Fight(card, enemyCard);
                    };
                    outcomes[counter].REALACTIONS.Add(realAction);
                }


            }
        }
        Action noAction = () =>
        {

        };
        outcomes[counter].actions.Add(noAction);

        counter++;
    }


}
public void startFirstAction(List<Outcome> outcomes)
{

}
public void goThroughAllActions(List<Outcome> outcomes, Outcome usedOutcome)
{

    int selector = 1;

    Strategy strategy = new Strategy();

    List<Action> actions = new List<Action>();


            int strategyCounter = 0;

            foreach (Action newAction in outcomes[0].actions)
            {
                actions = new List<Action>();
                bool finished = false;
                while (!finished)
                {
                    for (int j = selector; j < outcomes.Count; j++)
                    {
                        int internalCounter = outcomes[j].actionCounter;
                        for (int k = outcomes[j].actionCounter; k < outcomes[j].actions.Count; k++)
                        {
                            actions = new List<Action>();


                            if (outcomes[j].actionCounter > outcomes[j].actions.Count)
                            {
                                throw new Exception("shouldn't happen in this loop...");
                                outcomes[j].actionCounter = outcomes[j].actionCounter;
                            }
                            foreach (Outcome OUTCOME in outcomes)
                            {

                                actions.Add(OUTCOME.actions[OUTCOME.actionCounter]);
                            }
                            outcomes[j].actionCounter++;
                            strategy.strat.Add(actions);
                        }
                        outcomes[j].actionCounter = internalCounter;

                    }
                    outcomes[selector].actionCounter++;

                    selector++;
                    if(selector >= outcomes.Count)
                    {
                        selector = 1;
                        outcomes[selector].actionCounter++;
                    }

                    if(outcomes[outcomes.Count - 1].actionCounter >= outcomes[outcomes.Count - 1].actions.Count)
                    {
                        finished = true;
                    }
                }
                selector = 1;


                //NBEED TO LOOP BEFORE THIS
                //strategy.strat.Add(actions);
                outcomes[0].actionCounter++;
                if(outcomes[0].actionCounter > outcomes[0].actions.Count)
                {
                    outcomes[0].actionCounter = 0;
                }
            }




            strategyCounter++;
}
public class Strategy
{
    public List<List<Action>> strat;
    public void Add(List<Outcome> outcomes, Outcome currentOutcome, Action action, int counter)
    {
        List<Action> actions = new List<Action>();
        actions.Add(action);
        foreach (Outcome outcome in outcomes)
        {
            if (outcome != currentOutcome)
            {

            }
        }
        strat.Add(actions);
    }

}
public class Outcome
{
    public int actionCounter = 0;
    public Card card;
    public List<Action> actions;
    public List<Action<BoardFunctionality>> REALACTIONS;
    public Outcome(Card card)
    {
        this.card = card;
    }
}
*/
}

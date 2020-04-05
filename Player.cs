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
        List<Card> knownEnemyGenerals = new List<Card>();
        List<Card> knownEnemyArmies = new List<Card>();

        bool endFirstDec = false;
        bool endSecondDec = false; //so that the army container updates by the time we decide to play cards
        public override void ResetPlayer()
        {
            endFirstDec = false;
            endSecondDec = false;
            base.ResetPlayer();
        }
        int currentArmyCount;
        public override void decide(MouseState mouseState, ContentManager content, BoardFunctionality boardFunc)
        {
            if (!endFirstDec && boardFunc.boardActions.actions.Count < 1)
            {
                playArmies(boardFunc);

                generalAbiltiies(mouseState, boardFunc);
                attackIfBeneficial(boardFunc);
                //throw new
                endFirstDec = true;
            }
            if(boardFunc.boardActions.actions.Count < 1)
            {
                if (!endSecondDec)
                {
                    playCardIfThereAreEnoughArmies(boardFunc);
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
        private void playArmies(BoardFunctionality boardFunc)
        {
            bool trigger = false;
            if (!hasPlayedArmyThisTurn)
            {
                foreach (Card card in boardFunc.friendlySide.Hand.cardsInContainer)
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
            int counter = 0; //set to 1 because the army in hand won't register 
            foreach (Card newCard in boardFunc.friendlySide.Rows[Side.Armies].cardsInContainer)
            {
                if (!newCard.cardProps.exhausted)
                {
                    counter++;
                }

            }
            foreach (Card newCard in boardFunc.friendlySide.Hand.cardsInContainer)
            {
                if (newCard.cardProps.cost.totalCost <= counter)
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
            foreach (FunctionalRow row in boardFunc.friendlySide.Rows)
            {
                if (row.type == card.cardProps.type)
                {
                    return row;
                }
            }
            return null;
        }

        private Dictionary<Card, CardPlayShell> playDictionary;
        public GroupPlayManager groupPlayManager= new GroupPlayManager();

        private void attackRow(FunctionalRow row, BoardFunctionality boardFunc)
        {
            playDictionary = new Dictionary<Card, CardPlayShell>();
            foreach (Card friendlyCard in boardFunc.friendlySide.Rows[Side.FieldUnit].cardsInContainer)
            {
                if (!friendlyCard.cardProps.exhausted)
                {
                    CardPlayShell cardPlayShell = new CardPlayShell();
                    foreach (Card enemyCard in row.cardsInContainer)
                    {
                        cardPlayShell.AddEnemy(enemyCard);
                    }
                    cardPlayShell.SetCardToMakePlay(friendlyCard);
                    cardPlayShell.SetPlays(boardFunc);
                    playDictionary.Add(friendlyCard, cardPlayShell);
                }
            }

            List<Card> list = boardFunc.friendlySide.Rows[Side.FieldUnit].cardsInContainer;
            int lastMember = boardFunc.friendlySide.Rows[Side.FieldUnit].unexhaustedCount() - 1;
            Iterate(lastMember, list); //biggish load of work for comp, maybe make this async?
            groupPlayManager.executeBestPlay(boardFunc); //assign those sweet actions
        }
        private void attackIfBeneficial(BoardFunctionality boardFunc)
        {
            if (rowCanBeAttacked(boardFunc.friendlySide.Rows[Side.FieldUnit]))
            {
                attackRow(boardFunc.enemySide.Rows[Side.FieldUnit], boardFunc);
            }
            else if(rowCanBeAttacked(boardFunc.friendlySide.Rows[Side.Armies]))
            {
                attackRow(boardFunc.enemySide.Rows[Side.Armies], boardFunc);
            }
            else
            {
                if(isRowRevealed(boardFunc.enemySide.Rows[Side.Armies]) && isRowRevealed(boardFunc.enemySide.Rows[Side.FieldUnit]))
                {
                    foreach(Card card in boardFunc.friendlySide.Rows[Side.FieldUnit].cardsInContainer)
                    {
                        if(!card.cardProps.exhausted)
                        {
                            boardFunc.LifeDamage(card);
                        }
                    }
                }
            }

            /*else
            {
                boardFunc.enemySide.boardFunc.BOARDMESSAGE.addMessage("AI has no attacks this round");
            }*/
        }

        private void revealBoardIfOptimal(BoardFunctionality boardFunc)
        {
            if(shouldRevealBoard(boardFunc))
            {
                //revealBoardWithBestCard(BoardFunctionality boardFunc);
                //still need to see if the code in Card.containsReveal works@!~
            }
        }
        private bool shouldRevealBoard(BoardFunctionality boardFunc)
        {
            if(getCumulativeAttack(boardFunc.friendlySide.Rows[Side.FieldUnit]) * 1.2 > getCumulativeDefense(boardFunc.friendlySide.Rows[Side.FieldUnit]))
            {
                if(haveARevealCard(boardFunc))
                return true;
            }
            return false;
        }
        private bool haveARevealCard(BoardFunctionality boardFunc)
        {
            foreach(FunctionalRow row in boardFunc.friendlySide.Rows)
            {
                if(containsReveal(row))
                {
                    return true;
                }
            }
            if(containsReveal(boardFunc.friendlySide.Hand))
            {
                //return true;
            }
            return false;
        }
        private bool containsReveal(HorizontalContainer container)
        {
            foreach(Card card in container.cardsInContainer)
            {
                if(card.containsReveal())
                {
                    return true;
                }
            }
            return false;
        }
        private int getCumulativeDefense(FunctionalRow row)
        {
            int counter = 0;
            foreach (Card card in row.cardsInContainer)
            {
                counter += card.cardProps.defense;
            }
            return counter;
        }
        private int getCumulativeAttack(FunctionalRow row)
        {
            int counter = 0;
            foreach (Card card in row.cardsInContainer)
            {
                counter += card.cardProps.power;
            }
            return counter;
        }
        private int getCumulativeValue(FunctionalRow row)
        {
            int counter = getCumulativeAttack(row) + getCumulativeDefense(row);
            foreach (Card card in row.cardsInContainer)
            {
                counter += card.cardProps.cost.totalCost;
            }
            return counter;
        }
        private bool isRowRevealed(FunctionalRow row)
        {
            if (row.revealed)
            {
                return true;
            }
            return false;
        }
        private bool rowCanBeAttacked(FunctionalRow row)
        {
            if(row.cardsInContainer.Count > 0 && row.revealed)
            {
                return true;
            }
            return false;
        }
        private void generalAbiltiies(MouseState mouseState, BoardFunctionality boardFunc)
        {
            foreach (Card friendlyCard in boardFunc.friendlySide.Rows[Side.General].cardsInContainer)
            {
                List<Ability> usableAbilities = new List<Ability>();
                foreach (Ability ability in friendlyCard.cardProps.abilities)
                {
                    if(ability.exchangeValue < 0)
                    {
                        if(GameComponent.ToAbsolute(ability.exchangeValue) <= friendlyCard.cardProps.defense)
                        {
                            usableAbilities.Add(ability);
                        }
                    }
                    else
                    {
                        usableAbilities.Add(ability);
                    }
                }
                Random random = new Random();
                int selector = random.Next(0, usableAbilities.Count);
                usableAbilities[selector].useAbility(mouseState, boardFunc);
            }
        }

        private void Iterate(int selector, List<Card> list)
        {
            CardPlayShell shell = playDictionary[list[selector]];
            foreach (KeyValuePair<int, Play> play in shell.play)
            {
                foreach (Card card in list)
                {
                    groupPlayManager.AddGroup(playDictionary);
                }
                shell.increaseIterator();
                if (selector > 0)
                {
                    Iterate(selector - 1, list);
                }

            }
            shell.iterator = 0;
        }
    }

    public class GroupPlayManager
    {
        public List<GroupPlay> groupPlays = new List<GroupPlay>();
        
        public void AddGroup(Dictionary<Card, CardPlayShell> friendlyCards)
        {
            GroupPlay groupPlay = new GroupPlay();
            groupPlay.addGroupPlay(friendlyCards);
            groupPlays.Add(groupPlay);
        }
        public void executeBestPlay(BoardFunctionality boardFunc)
        {

            int PLAYCOUNTER = 0;
            int highestValue = 0;
            GroupPlay selectedCombination = null;
            foreach(GroupPlay group in groupPlays)
            {
                if (highestValue == 0)
                {
                    highestValue = group.fullPlayValue;
                    selectedCombination = group;
                }
                if (group.fullPlayValue > highestValue)
                {
                    highestValue = group.fullPlayValue;
                    selectedCombination = group;
                }
                PLAYCOUNTER++;
            }
            boardFunc.enemySide.boardFunc.BOARDMESSAGE.addMessage("Current play value = " + highestValue.ToString());
            boardFunc.enemySide.boardFunc.BOARDMESSAGE.addMessage("Amount of group combos parsed = " + PLAYCOUNTER.ToString());
            //throw new Exception(highestValue.ToString());

            if(selectedCombination == null)
            {
                boardFunc.BOARDMESSAGE.addMessage("For some reason the AI is not functioning~");
            }
            else
            {
                foreach (Play play in selectedCombination.plays)
                {
                    play.realPlay();
                }
            }
            groupPlays = new List<GroupPlay>();
        }
    }
    public class GroupPlay
    {
        public List<Play> plays;
        public int fullPlayValue;
        public void addGroupPlay(List<Play> newGroupPlay)
        {
            plays = new List<Play>();
            plays = newGroupPlay;
            fullPlayValue = calcPlayValue();
        }
        public void addGroupPlay(Dictionary<Card, CardPlayShell> friendlyCards)
        {
            plays = new List<Play>();
            foreach (KeyValuePair<Card, CardPlayShell> pair in friendlyCards)
            {
                Play play = new Play();
                play = pair.Value.play[pair.Value.iterator];
                plays.Add(play);
            }
            fullPlayValue = calcPlayValue();
        }
        public int calcPlayValue()
        {
            int totalValue = 0;
            foreach (Play play in plays)
            {
                totalValue += play.testPlay();

            }
            foreach(Play play in plays)
            {
                play.resetValues();
            }
            return totalValue;
        }
    }
    public class Play
    {
        public int value = 0;
        public Func<int> testPlay;
        public Action realPlay;
        public Action resetValues;
    }
    public class CardPlayShell
    {
        private Card OPERATOR;
        private List<Card> enemyCards;
        public int iterator = 0;
        public int maxPlays;
        public Dictionary<int, Play> play;

        public CardPlayShell()
        {
            enemyCards = new List<Card>();
            play = new Dictionary<int, Play>();
        }
        public void increaseIterator()
        {
            iterator++;
            if (iterator >= play.Count)
            {
                iterator = 0;
            }
        }
        public void SetCardToMakePlay(Card card)
        {
            OPERATOR = card;
        }
        public void AddEnemy(Card card)
        {
            enemyCards.Add(card);
        }
        private int getValue(Card card)
        {
            int value = card.cardProps.initialDefense + card.cardProps.initialPower + card.cardProps.cost.totalCost + card.cardProps.abilities.Count * 2;
            return value;
        }
        public void SetPlays(BoardFunctionality boardFunc)
        {
            int counter = 0;
            /*foreach(Ability ability in OPERATOR.cardProps.abilities)
            {

            }*/

            foreach(Card card in enemyCards)
            {
                Play newPlay = new Play();
                Func<int> testAction = () =>
                {
                    int value = 0;
                    if (card.cardProps.aiCalcDefense > 0)
                    {
                        card.cardProps.aiCalcDefense -= OPERATOR.cardProps.power;
                        OPERATOR.cardProps.aiCalcDefense -= card.cardProps.power;



                        if (card.cardProps.aiCalcDefense <= 0)
                        {
                            value += getValue(card) + 1;
                        }
                        if (OPERATOR.cardProps.aiCalcDefense <= 0)
                        {
                            value -= getValue(OPERATOR);
                        }

                    }
                    else
                    {
                        card.cardProps.aiCalcDefense -= OPERATOR.cardProps.power;
                        OPERATOR.cardProps.aiCalcDefense -= card.cardProps.power;
                        if (OPERATOR.cardProps.aiCalcDefense <= 0)
                        {
                            value -= getValue(OPERATOR);
                        }
                    }
                    return value;

                };
                Action realAction = () =>
                {
                    boardFunc.Fight(OPERATOR, card);
                };
                Action resetAction = () =>
                {
                    card.cardProps.aiCalcDefense = card.cardProps.defense;
                    OPERATOR.cardProps.aiCalcDefense = OPERATOR.cardProps.defense;
                };

                newPlay.testPlay = testAction;
                newPlay.realPlay = realAction;
                newPlay.resetValues = resetAction;
                play.Add(counter, newPlay);
                counter++;
            }

            Play nothingPlay = new Play();
            Action nothingAction = () => { };
            Func<int> nothingReturn = () => { return 0; };
            nothingPlay.realPlay = nothingAction;
            nothingPlay.testPlay = nothingReturn;
            nothingPlay.resetValues = nothingAction;
            play.Add(counter, nothingPlay);
            counter++;

            maxPlays = counter;
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

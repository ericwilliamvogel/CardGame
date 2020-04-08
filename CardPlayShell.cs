using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
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

            foreach (Card card in enemyCards)
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
}

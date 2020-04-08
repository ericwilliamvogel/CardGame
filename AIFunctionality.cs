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
    public class AIFunctionality
    {
        AIPlayer player;
        public AIFunctionality(AIPlayer player)
        {
            this.player = player;
        }
        public void playArmies(BoardFunctionality boardFunc)
        {
            bool trigger = false;
            if (!player.hasPlayedArmyThisTurn)
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
        public void playCardIfThereAreEnoughArmies(BoardFunctionality boardFunc)
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
        public FunctionalRow getCorrectRow(Card card, BoardFunctionality boardFunc)
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
        public GroupPlayManager groupPlayManager = new GroupPlayManager();

        public void attackRow(FunctionalRow row, BoardFunctionality boardFunc)
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
        public void attackIfBeneficial(BoardFunctionality boardFunc)
        {
            if (rowCanBeAttacked(boardFunc.friendlySide.Rows[Side.FieldUnit]))
            {
                attackRow(boardFunc.enemySide.Rows[Side.FieldUnit], boardFunc);
            }
            else if (rowCanBeAttacked(boardFunc.friendlySide.Rows[Side.Armies]))
            {
                attackRow(boardFunc.enemySide.Rows[Side.Armies], boardFunc);
            }
            else
            {
                if (isRowRevealed(boardFunc.enemySide.Rows[Side.Armies]) && isRowRevealed(boardFunc.enemySide.Rows[Side.FieldUnit]))
                {
                    foreach (Card card in boardFunc.friendlySide.Rows[Side.FieldUnit].cardsInContainer)
                    {
                        if (!card.cardProps.exhausted)
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
            if (shouldRevealBoard(boardFunc))
            {
                //revealBoardWithBestCard(BoardFunctionality boardFunc);
                //still need to see if the code in Card.containsReveal works@!~
            }
        }
        private bool shouldRevealBoard(BoardFunctionality boardFunc)
        {
            if (getCumulativeAttack(boardFunc.friendlySide.Rows[Side.FieldUnit]) * 1.2 > getCumulativeDefense(boardFunc.friendlySide.Rows[Side.FieldUnit]))
            {
                if (haveARevealCard(boardFunc))
                    return true;
            }
            return false;
        }
        private bool haveARevealCard(BoardFunctionality boardFunc)
        {
            foreach (FunctionalRow row in boardFunc.friendlySide.Rows)
            {
                if (containsReveal(row))
                {
                    return true;
                }
            }
            if (containsReveal(boardFunc.friendlySide.Hand))
            {
                //return true;
            }
            return false;
        }
        private bool containsReveal(HorizontalContainer container)
        {
            foreach (Card card in container.cardsInContainer)
            {
                if (card.containsReveal())
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
            if (row.cardsInContainer.Count > 0 && row.revealed)
            {
                return true;
            }
            return false;
        }
        public void generalAbiltiies(MouseState mouseState, BoardFunctionality boardFunc)
        {
            foreach (Card friendlyCard in boardFunc.friendlySide.Rows[Side.General].cardsInContainer)
            {
                List<Ability> usableAbilities = new List<Ability>();
                foreach (Ability ability in friendlyCard.cardProps.abilities)
                {
                    if (ability.exchangeValue < 0)
                    {
                        if (GameComponent.ToAbsolute(ability.exchangeValue) <= friendlyCard.cardProps.defense)
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

}

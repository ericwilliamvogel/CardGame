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
    public class BoardDefinitions
    {
        

        public void performChecksThenPlayCard(Side side, Card card, BoardFunctionality boardFunc)
        {
            switch (card.cardProps.type)
            {
                case CardType.Army:
                    if (side.canPlayArmy)
                    {
                        playCardIfSideHasSufficientResources(side, card, boardFunc);

                    }
                    else
                    {
                        returnToHand(side, card, boardFunc);
                    }
                    break;
                default:
                    playCardIfSideHasSufficientResources(side, card, boardFunc);
                    break;
            }
        }
        public void playCardIfSideHasSufficientResources(Side side, Card card, BoardFunctionality boardFunc)
        {
            if (card.canBePlayed(side))
            {
                //int temporaryCounter = card.cardProps.cost.raceCost.Count;
                deductCostFromResourcesAndPlayCard(side, card, boardFunc);

            }
            else
            {
                if (side.Rows[Side.Armies].cardsInContainer.Count >= card.cardProps.cost.totalCost)
                {
                    List<Card> deductedResources = new List<Card>();
                    List<Card> otherAvailableResources = new List<Card>();
                    foreach (Card resource in side.Rows[Side.Armies].cardsInContainer)
                    {
                        if (!resource.cardProps.exhausted)
                            otherAvailableResources.Add(resource);
                    }
                    foreach (Card.Race cardResource in card.cardProps.cost.raceCost)
                    {
                        bool check = false;
                        foreach (Card armyCard in side.Rows[Side.Armies].cardsInContainer)
                        {
                            if (cardResource == armyCard.race && check == false && !armyCard.cardProps.exhausted && !deductedResources.Contains(armyCard))
                            {
                                deductedResources.Add(armyCard);
                                ///otherAvailableResources.Remove(armyCard);
                                check = true;
                            }
                        }
                    }
                    foreach (Card resource in deductedResources)
                    {
                        otherAvailableResources.Remove(resource);
                    }


                    if (deductedResources.Count < card.cardProps.cost.raceCost.Count || deductedResources.Count + otherAvailableResources.Count < card.cardProps.cost.totalCost)
                    {

                        returnToHand(side, card, boardFunc);

                    }
                    else
                    {
                        for (int i = 0; i < deductedResources.Count; i++)
                        {
                            deductedResources[i].cardProps.exhausted = true;
                            side.Resources.Add(deductedResources[i].race);
                        }
                        for (int i = 0; i < card.cardProps.cost.unanimousCost; i++)
                        {
                            otherAvailableResources[i].cardProps.exhausted = true;
                            side.Resources.Add(otherAvailableResources[i].race);
                        }
                        boardFunc.PlayCard(side, card);
                    }
                }
                else
                    returnToHand(side, card, boardFunc);
                //boardPosLogic.updateBoard(this);
                //throw new Exception("does not have enough MANA");
            }
        }
        private void deductCostFromResourcesAndPlayCard(Side side, Card card, BoardFunctionality boardFunc)
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

                    returnToHand(side, card, boardFunc);

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
                    resumeWithPlayingCard(side, card, boardFunc);
                }
            }
            else
            {
                for (int i = 0; i < card.cardProps.cost.totalCost; i++)
                {
                    side.Resources.Remove(0);
                }
                resumeWithPlayingCard(side, card, boardFunc);
            }
        }
        public void resumeWithPlayingCard(Side side, Card card, BoardFunctionality boardFunc)
        {
            boardFunc.actionConstructor.moveTo(side.Hand, card.correctRow(side), card, boardFunc);

            if (card.cardProps.type == CardType.Army)
            {
                side.canPlayArmy = false;
            }
            else
            {
                card.cardProps.exhausted = true;
            }
        }
        public void returnToHand(Side side, Card card, BoardFunctionality boardFunc)
        {
            boardFunc.actionConstructor.moveTo(side.Hand, side.Hand, card, boardFunc);

        }

        public void spawnSpecifiedUnit(Card card, SpawnCard ability, BoardFunctionality boardFunc)
        {

            Card newCard = boardFunc.cardBuilder.cardConstruct(boardFunc.cardConstructor, ability.identifier);
            boardFunc.library = boardFunc.cardConstructor.tempStorage;
            newCard.setSupplementalTextures(boardFunc.library);

            newCard.correctRow(boardFunc.friendlySide).cardsInContainer.Add(newCard);
            newCard.correctRow(boardFunc.friendlySide).loadCardImage(boardFunc.library.cardTextureDictionary, newCard);
            newCard.cardProps.exhausted = true;
            //newCard.correctRow(friendlySide).loadCardImagesInContainer(library.cardTextureDictionary);
        }
        public void deductAttributesAndDecideWinner(Card card, Card otherCard)
        {
            int firstcardpower = card.cardProps.power;
            int secondcardpower = otherCard.cardProps.power;

            card.cardProps.defense -= secondcardpower;
            otherCard.cardProps.defense -= firstcardpower;

        }
        public void dealDirectDamageAndDisposeOfDead(Card fromCard, Ability ability, Card targetCard)
        {
            int damage = ability.power;
            targetCard.cardProps.defense -= damage;

        }
        public void revealBoardForRemainderOfTurn(Card fromCard, Ability ability, BoardFunctionality boardFunc)
        {
            foreach (FunctionalRow row in boardFunc.enemySide.Rows)
            {
                row.revealed = true;
            }

        }
        public void dealBoardDamageAndDisposeOfDead(Card fromCard, Ability ability, Card targetCard, BoardFunctionality boardFunc)
        {

            int damage = ability.power;
            foreach (Card card in targetCard.correctRow(boardFunc.enemySide).cardsInContainer)
            {
                card.cardProps.defense -= damage;
            }
        }
    }
}

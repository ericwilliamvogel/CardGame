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
                    foreach (Race cardResource in card.cardProps.cost.raceCost)
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
                        boardFunc.BOARDMESSAGE.addMessage("Not enough armies to produce the needed resources!");
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
                {
                    boardFunc.BOARDMESSAGE.addMessage("Not enough armies to produce the needed resources!");
                    returnToHand(side, card, boardFunc);
                }

                //boardPosLogic.updateBoard(this);
                //throw new Exception("does not have enough MANA");
            }
        }
        private void deductCostFromResourcesAndPlayCard(Side side, Card card, BoardFunctionality boardFunc)
        {
            if (card.cardProps.cost.raceCost != null)
            {
                List<Race> deductedResources = new List<Race>();

                foreach (Race cardResource in card.cardProps.cost.raceCost)
                {
                    bool check = false;
                    foreach (Race resource in side.Resources)
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
                    boardFunc.BOARDMESSAGE.addMessage("Not enough mana of the correct type!");
                }
                else
                {
                    foreach (Race resource in deductedResources)
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
            if(card.cardProps.type != CardType.Manuever)
            {
                boardFunc.actionConstructor.moveTo(side.Hand, card.correctRow(side), card, boardFunc);
                boardFunc.hidePlayCardFromEnemyAndDisplay(card);
                foreach (Effect effect in card.cardProps.effects)
                {
                    if (effect.trigger == Effect.Trigger.OnEnterPlay)
                    {
                        assignAbilityToNextSelection(card, effect.ability, boardFunc);
                    }
                }
            }


            if (card.cardProps.type == CardType.Army)
            {
                side.canPlayArmy = false;
            }
            else if (card.cardProps.type == CardType.Manuever)
            {
                boardFunc.actionConstructor.moveTo(side.Hand, boardFunc.castManuever, card, boardFunc);
                int counter = 0;
                foreach(Ability ability in card.cardProps.abilities)
                {
                    assignAbilityToNextSelection(card, ability, boardFunc);
                    counter++;
                }

            }
            else
            {
                card.cardProps.exhausted = true;
            }
        }

        //changed from int selector to ability, should work fine. If not can revert back this code to 4/7/2020 on github
        public void assignAbilityToNextSelection(Card card, Ability ability, BoardFunctionality boardFunc)
        {
            Action<MouseState> setSelection = (MouseState mouseState) => {
                ability.setTarget();
                ability.activateAbilityOnSelection(mouseState, boardFunc);
                if (mouseState.RightButton == ButtonState.Pressed)
                {
                    MouseTransformer.Set(MouseTransformer.State.Reg);
                    boardFunc.cardViewer.NextSelection();
                    
                }
            };
            boardFunc.cardViewer.setSelectionState(setSelection, card, boardFunc);
        }
        public void returnToHand(Side side, Card card, BoardFunctionality boardFunc)
        {
            boardFunc.actionConstructor.moveTo(side.Hand, side.Hand, card, boardFunc);

        }
        
        public void dealPlayerDamage(Card card, BoardFunctionality boardFunc)
        {
            boardFunc.enemySide.LifeTotal -= card.cardProps.power;
        }
        public void dealPlayerDamage(Card card, Ability ability, BoardFunctionality boardFunc)
        {
            boardFunc.enemySide.LifeTotal -= ability.power;
        }
        public void spawnSpecifiedUnit(Card card, SpawnCard ability, BoardFunctionality boardFunc)
        {

            Card newCard = boardFunc.cardBuilder.cardConstruct(boardFunc.cardConstructor, ability.identifier);
            boardFunc.library = boardFunc.cardConstructor.tempStorage;
            newCard.setSupplementalTextures(boardFunc.library);
            newCard.correctRow(boardFunc.friendlySide).cardsInContainer.Add(newCard);
            newCard.correctRow(boardFunc.friendlySide).loadCardImage(boardFunc.library, newCard);
            newCard.cardProps.exhausted = true;
            //newCard.correctRow(friendlySide).loadCardImagesInContainer(library.cardTextureDictionary);
        }
        public void drawSpecifiedSpell(Card card, CreateSpell spell, BoardFunctionality boardFunc)
        {

            Card newCard = boardFunc.cardBuilder.cardConstruct(boardFunc.cardConstructor, spell.identifier);
            boardFunc.library = boardFunc.cardConstructor.tempStorage;
            newCard.setSupplementalTextures(boardFunc.library);
            boardFunc.friendlySide.Deck.cardsInContainer.Insert(0, newCard);
            boardFunc.friendlySide.Deck.loadCardImage(boardFunc.library, newCard);
            boardFunc.DrawCard(boardFunc.friendlySide);
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
            if (targetCard != null)
            {
                foreach (Card card in targetCard.correctRow(boardFunc.enemySide).cardsInContainer)
                {
                    card.cardProps.defense -= damage;
                }
            }
            else
            { //used by ai
                foreach (Card card in boardFunc.enemySide.Rows[Side.FieldUnit].cardsInContainer)
                {
                    card.cardProps.defense -= damage;
                }
            }
        }
    }
}

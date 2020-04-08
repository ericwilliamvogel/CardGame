using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public class NonTargetAbility : Ability
    {

    }
    public class Ability
    {
        public bool clickedInAbilityBox;
        public SelectUnitAction selectAction = new SelectUnitAction();
        public Card INITIALCARD;
        public string name;
        public string description;
        public int power;
        public int exchangeValue;

        public void displayGeneralIncrements(int exchangeValue)
        {
            this.exchangeValue = exchangeValue;
            string startingString = exchangeValue.ToString();
            if (exchangeValue == 0)
            {
                name = startingString + ":";

            }
            else if (exchangeValue > 0)
            {
                name = "+" + startingString + ":";
            }
            else
            {
                name = "" + startingString + ":";
            }
        }
        public enum Type
        {
            Targeted,
            Global,
            Upkeep
        }
        public Ability()
        {
            description = "no description";
        }

        public virtual void setCard(Card card)
        {
            INITIALCARD = card;
            if (name == null || name == "")
                name = getTrueName();
        }
        public virtual void activateAbilityOnSelection(MouseState mouseState, BoardFunctionality boardFunc)
        {
            if (INITIALCARD.cardProps.exhausted == false && clickedInAbilityBox == false)
            {

                boardFunc.cardViewer.resetCardSelectionOnRightClick(mouseState, boardFunc);
                useAbility(mouseState, boardFunc);
                clickedInAbilityBox = true;
                resetAllCards(boardFunc);
            }
            else if (INITIALCARD.cardProps.type == CardType.Manuever)
            {
                boardFunc.cardViewer.resetCardSelectionOnRightClick(mouseState, boardFunc);
                useAbility(mouseState, boardFunc);
                clickedInAbilityBox = true;
                resetAllCards(boardFunc);
            }
        }
        public virtual void setTarget()
        {
            MouseTransformer.Set(MouseTransformer.State.Tgt);
        }
        public virtual Card returnSelectedCard(MouseState mouseState, BoardFunctionality boardFunc)
        {
            return null;
        }
        public void resetAllCards(BoardFunctionality boardFunc)
        {
            resetSide(boardFunc.enemySide);
            resetSide(boardFunc.friendlySide);
            MouseTransformer.Set(MouseTransformer.State.Reg);
        }
        public string getTrueName()
        {
            string name;
            if (INITIALCARD.cardProps.type == CardType.Manuever)
            {
                name = "";
            }
            else
            {
                name = "->: ";
            }
            return name;
        }
        public virtual void useAbility(MouseState mouseState, BoardFunctionality boardFunc)
        {
            if (INITIALCARD.cardProps.type == CardType.General)
            {
                if (exchangeValue < 0)
                {
                    if (INITIALCARD.cardProps.defense >= GameComponent.ToAbsolute(exchangeValue))
                    {
                        INITIALCARD.cardProps.defense += exchangeValue;
                        abilityImplementation(mouseState, boardFunc);
                        boardFunc.cardViewer.NextSelection();
                    }
                    else
                    {
                        boardFunc.BOARDMESSAGE.addMessage(INITIALCARD.cardProps.name + " does not have enough power yet.");
                        clickedInAbilityBox = false;
                        resetAllCards(boardFunc);
                        boardFunc.cardViewer.hardResetSelection(boardFunc);
                    }

                }
                else
                {
                    INITIALCARD.cardProps.defense += exchangeValue;

                    abilityImplementation(mouseState, boardFunc);
                    boardFunc.cardViewer.NextSelection();
                }
            }
            else if (INITIALCARD.cardProps.type == CardType.Manuever)
            {
                abilityImplementation(mouseState, boardFunc);
                manueverImplementation(boardFunc);
            }
            else
            {

                abilityImplementation(mouseState, boardFunc);
                boardFunc.cardViewer.NextSelection();
            }


        }
        public virtual void manueverImplementation(BoardFunctionality boardFunc)
        {
            boardFunc.cardViewer.NextSelection();
            if (!boardFunc.cardViewer.SelectionStillActive())
                boardFunc.actionConstructor.moveTo(boardFunc.castManuever, boardFunc.friendlySide.Oblivion, INITIALCARD, boardFunc);
        }
        public virtual void abilityImplementation(MouseState mouseState, BoardFunctionality boardFunc)
        {

        }
        public virtual void useAIAbility(AIPlayer player, BoardFunctionality boardFunc, Card targetCard)
        {

        }
        private void resetSide(Side side)
        {
            foreach (FunctionalRow row in side.Rows)
            {
                foreach (Card card in row.cardsInContainer)
                {
                    card.setRegular();
                }
            }
        }
    }
}

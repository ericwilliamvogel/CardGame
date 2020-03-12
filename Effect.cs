using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public class Ability
    {
        public bool clickedInAbilityBox;
        public SelectUnitAction selectAction = new SelectUnitAction();
        public Card INITIALCARD;
        public string name;
        public string description;
        public int power;
        protected int exchangeValue;

        public enum Type
        {
            Targeted,
            Global,
            Upkeep
        }
        public Ability()
        {
            name = "->:";
            description = "no description";
        }
        
        public virtual void setCard(Card card)
        {
            INITIALCARD = card;
        }
        public virtual void activateAbilityOnSelection(MouseState mouseState, BoardFunctionality boardFunc)
        {

        }
        public virtual void setTarget(MouseState mouseState, BoardFunctionality boardFunc)
        {

        }
        public virtual Card returnSelectedCard(MouseState mouseState, BoardFunctionality boardFunc)
        {
            return null;
        }
        public void resetAllCards(BoardFunctionality boardFunc)
        {
            resetSide(boardFunc.enemySide);
            resetSide(boardFunc.friendlySide);
        }
        private void resetSide(Side side)
        {
            foreach (FunctionalRow row in side.Rows)
            {
                foreach(Card card in row.cardsInContainer)
                {
                    card.setRegular();
                }
            }
        }
    }
    public class Exhaust : Ability
    {
        public Exhaust()
        {
            description = "Exhaust enemy unit";
        }
        public Exhaust(int exchangeValue)
        {
            this.exchangeValue = exchangeValue;
        }
        public override Card returnSelectedCard(MouseState mouseState, BoardFunctionality boardFunc)
        {

            if (selectAction.TargetEnemyCard(mouseState, boardFunc) != null)
            {
                return selectAction.TargetEnemyCard(mouseState, boardFunc);
            }
            else
                return null;

            //throw new Exception();
        }
        public override void activateAbilityOnSelection(MouseState mouseState, BoardFunctionality boardFunc)
        {
            if (returnSelectedCard(mouseState, boardFunc) != null && INITIALCARD.cardProps.exhausted == false && clickedInAbilityBox == false)
            {
                //INITIALCARD.finalizeAbilities();
                boardFunc.resetCardSelection(mouseState);
                INITIALCARD.cardProps.defense += exchangeValue;
                boardFunc.Exhaust(INITIALCARD, returnSelectedCard(mouseState, boardFunc));
                clickedInAbilityBox = true;
                resetAllCards(boardFunc);
            }
            selectAction.resetIfNoSelection(mouseState, INITIALCARD.getCurrentContainer(boardFunc.friendlySide), INITIALCARD, boardFunc);
        }
    }
    public class BoardDamage : TargetDamage
    {
        public BoardDamage(int damage, int exchangeValue) : base(damage, exchangeValue)
        {
            description = "Deal " + damage + "to all enemy units.";
        }
        public override void activateAbilityOnSelection(MouseState mouseState, BoardFunctionality boardFunc)
        {
            if (returnSelectedCard(mouseState, boardFunc) != null && INITIALCARD.cardProps.exhausted == false && clickedInAbilityBox == false)
            {
                //INITIALCARD.finalizeAbilities();
                boardFunc.resetCardSelection(mouseState);
                INITIALCARD.cardProps.defense += exchangeValue;
                boardFunc.BoardDamage(INITIALCARD, this);
                clickedInAbilityBox = true;
                resetAllCards(boardFunc);
            }
            selectAction.resetIfNoSelection(mouseState, INITIALCARD.getCurrentContainer(boardFunc.friendlySide), INITIALCARD, boardFunc);
        }
    }
    public class TargetDamage : Ability
    {
        
        public TargetDamage(int damage)
        {
            power = damage;
            description = "Deal " + damage + " damage.";
            name = "Exhaust:";
        }
        public TargetDamage(int damage, int exchangeValue) : this(damage)
        {
            this.exchangeValue = exchangeValue;
            string startingString = exchangeValue.ToString();
            if(exchangeValue == 0)
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
        public override void activateAbilityOnSelection(MouseState mouseState, BoardFunctionality boardFunc)
        {
            if(returnSelectedCard(mouseState,boardFunc) != null && INITIALCARD.cardProps.exhausted == false && clickedInAbilityBox == false)
            {
                //INITIALCARD.finalizeAbilities();
                boardFunc.resetCardSelection(mouseState);
                INITIALCARD.cardProps.defense += exchangeValue;
                boardFunc.DirectDamage(INITIALCARD, this, returnSelectedCard(mouseState, boardFunc));
                clickedInAbilityBox = true;
                resetAllCards(boardFunc);
            }
            selectAction.resetIfNoSelection(mouseState, INITIALCARD.getCurrentContainer(boardFunc.friendlySide), INITIALCARD, boardFunc);
        }
        public override void setTarget(MouseState mouseState, BoardFunctionality boardFunc)
        {
            //selectAction.SetTargetCard(mouseState, INITIALCARD.getCurrentContainer(boardFunc.friendlySide), INITIALCARD, boardFunc.friendlySide);
            //selectAction.SetTargetCard(mouseState, INITIALCARD.getCurrentContainer(boardFunc.friendlySide), INITIALCARD, boardFunc.enemySide);
            //base.activateAbility(mouseState, boardFunc);
        }
        public override Card returnSelectedCard(MouseState mouseState, BoardFunctionality boardFunc)
        {

            if (selectAction.TargetEnemyCard(mouseState, boardFunc) != null)
            {
                return selectAction.TargetEnemyCard(mouseState, boardFunc);
            }
            else
                return null;

            //throw new Exception();
        }
    }


    /*public class GeneralAbility : Ability
    {
        //public GeneralAbility() { }
    }*/
    public class EffectSensor //LATER
    {

    }
    public class Effect //LATER
    {
        public enum EffectType
        {
            SpawnUnit,
            DestroyUnitWithPowerUnder,
            DestroyUnitWithPowerOver,
            DamageDirectUnit,
            DamageAllUnits,
            Illuminate
        }
        public enum EffectTrigger
        {
            OnTurnStart,
            OnYourTurnStart,
            OnAttack,
            OnDefense,
            OnBaseHit,
            OnLeavePlay,
            OnEnterPlay
        }
        string text;
        //private Effect effect;
        public Effect(EffectType type, EffectTrigger trigger, int value)
        {

        }
        /*public Effect(EffectType type, EffectTrigger trigger, Card card, Effect newEffect)
        {
            effect = newEffect;
        }
        public bool Trigger(EffectTrigger trigger)
        {
            return true;/////
        }*/

        public void setTrigger()
        {

        }
    }
}

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
        protected Card INITIALCARD;
        public Ability(Card card)
        {
            INITIALCARD = card;
        }
        public virtual void activateAbility(MouseState mouseState, BoardFunctionality boardFunc)
        {

        }
    }
    public class Attack : Ability
    {
        public Attack(Card card) : base(card)
        {

        }

        public override void activateAbility(MouseState mouseState, BoardFunctionality boardFunc)
        {
            foreach(FunctionalRow row in boardFunc.enemySide.Rows)
            {
                row.modifyCardInteractivity(mouseState, boardFunc);
                foreach(Card card in row.cardsInContainer)
                {
                    if(card.isSelected())
                    {
                        //EXCHANGE VALUES AND IF ONE IS LESS THAN 0 BOARDFUNC KILL THOSE WITH LESS THAN 0
                    }
                }
            }
        }

        private void fightCards(Card cardOne, Card cardTwo, BoardFunctionality boardFunc)
        {
            boardFunc.boardPosLogic.scaleToView(cardOne);
            int spacing = GraphicsSettings.toResolution(100);
            cardOne.setPos(spacing, (int)cardOne.getPosition().Y);
            cardTwo.setPos(Game1.windowW - spacing, (int)cardTwo.getPosition().Y);
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

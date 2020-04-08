using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    /*public class TargetedEffect : Effect
    {
        public TargetedEffect(Ability ability, Trigger trigger)
        {
            this.trigger = trigger;
            this.ability = ability;

        }
    }
    public class NonTargetedEffect IDK IDK IDK*/
    public class Effect
    {
        public NonTargetAbility ability;
        public Trigger trigger;
        public Effect(NonTargetAbility ability, Trigger trigger)
        {
            this.trigger = trigger;
            this.ability = ability;
            
        }
        public string getName()
        {
            string name = "not loaded";
            switch (trigger)
            {
                case Trigger.OnAttack:
                    name = "On Attack: ";
                    break;
                /*case Trigger.OnDefense:
                    name = "On Defense: ";
                    break;*/
                /*case Trigger.OnLeavePlay:
                    name = "On Death: ";
                    break;*/
                case Trigger.OnEnterPlay:
                    name = "On Entry: ";
                    break;
            }
            return name;
        }
        public enum Trigger
        {
            //need to fix the turn overlap w/ AI. maybe async?
            //OnTurnStart,
            //OnYourTurnStart,
            OnAttack,
            //OnDefense, //must be nontargeted / passive ability
            //OnBaseHit,
            //OnLeavePlay,
            OnEnterPlay
        }
    }



}

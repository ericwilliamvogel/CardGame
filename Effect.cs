﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public class Ability
    {
        public Ability()
        {

        }
    }
    public class GeneralAbility : Ability
    {
        public GeneralAbility() { }
    }
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
        private Effect effect;
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

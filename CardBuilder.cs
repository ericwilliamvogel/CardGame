﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public class CardBuilder
    {
        //DECLARE ALL ABILITIES HERE

            //DICTIONARY THAT EXCHANGES ACTION
        public CardBuilder()
        {
            //Effect effect = new Effect()
        }
        public Card cardConstruct(CardConstructor constructor, int identifier)
        {
            Assemble(constructor, identifier);
            
            //constructor.addEffect(new Effect(Effect.EffectType.DamageAllUnits, Effect.EffectTrigger.OnDefense, 3));
            return constructor.getCard();
        }
        public void Assemble(CardConstructor constructor, int identifier)
        {
            if (identifier == 0)
            {
                constructor.setFieldUnit(identifier);
                constructor.setPower(3);
                constructor.setRace(Card.Race.Elf);
                constructor.setDefense(3);
                constructor.setRarity(Card.Rarity.Bronze);
                constructor.setName("Elf");
                constructor.addAbility(new Exhaust()); /**/

            }
            else if(identifier == 1)
            {
                constructor.setGeneral(identifier);
                constructor.setPower(0);
                constructor.setRace(Card.Race.Human);
                constructor.setDefense(4);
                constructor.setRarity(Card.Rarity.Bronze);
                constructor.setName("Guy");
                constructor.addAbility(new TargetDamage(2, 1));
                constructor.addAbility(new TargetDamage(5, -2));
                constructor.addAbility(new TargetDamage(12, -8));
            }
            else if(identifier == 2)
            {
                constructor.setArmy(identifier);
                constructor.setPower(1);
                constructor.setRace(Card.Race.Orc);
                constructor.setDefense(1);
                constructor.setRarity(Card.Rarity.Bronze);
                constructor.setName("Thing");
            }
            else if(identifier == 3)
            {
                constructor.setFieldUnit(identifier);
                constructor.setPower(1);
                constructor.setRace(Card.Race.Orc);
                constructor.setDefense(2);
                constructor.setRarity(Card.Rarity.Bronze);
                constructor.setName("NEWGUY");
            }
            else if(identifier == 4)
            {
                constructor.setGeneral(identifier);
                constructor.setPower(0);
                constructor.setRace(Card.Race.Orc);
                constructor.setDefense(2);
                constructor.setRarity(Card.Rarity.Bronze);
                constructor.setName("COOL GENERAL");
                constructor.addAbility(new TargetDamage(1, 2));
                constructor.addAbility(new BoardDamage(1, -1));
                constructor.addAbility(new BoardDamage(12, -10));
            }
        }
        /*public void Assemble2(CardConstructor constructor)
        {
            if (identifier == 1)
            {
                constructor.setFieldUnit(1);
                constructor.setPower(2);
                constructor.setRace(Card.Race.Elf);
                constructor.setDefense(3);
            }
        }*/
    }
    public class CardConstructor
    {
        protected Card card;
        public CardImageStorage tempStorage;
        //
        public CardConstructor()
        {
            tempStorage = new CardImageStorage();
        }
        public void setGeneral(int identifier)
        {
            card = new General(identifier);
            tempStorage.fillDictionary(identifier);
        }
        public void setName(string name)
        {
            card.cardProps.name = name;
        }
        public void setPower(int power)
        {
            card.setPower(power);
        }
        public void setRarity(Card.Rarity rarity)
        {
            card.rarity = rarity;
        }
        public void setDefense(int defense)
        {

                card.setDefense(defense);
            card.cardProps.initialDefense = defense;
            
        }
        public void setRace(Card.Race race)
        {
            card.race = race;
        }
        public void addEffect(Effect effect)
        {
            card.cardProps.effects.Add(effect);
        }
        public void addAbility(Ability ability)
        {
            card.cardProps.abilities.Add(ability);
        }

        public void setArmy(int identifier)
        {
            card = new Army(identifier);
            tempStorage.fillDictionary(identifier);
        }
        public void setFieldUnit(int identifier)
        {
            card = new FieldUnit(identifier);
            tempStorage.fillDictionary(identifier);
        }
        public Card getCard()
        {
            card.finalizeAbilities();
            return card;
        }
    }
}

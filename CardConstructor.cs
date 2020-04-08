using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public class CardConstructor
    {
        protected Card card;
        public CardImageStorage tempStorage;
        public CardConstructor()
        {
            tempStorage = new CardImageStorage();
        }
        public void setCost(Cost cost)
        {
            card.cardProps.cost = cost;
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
            card.cardProps.initialPower = power;
        }
        public void setRarity(Rarity rarity)
        {
            card.rarity = rarity;
        }
        public void setDefense(int defense)
        {

            card.setDefense(defense);
            card.cardProps.initialDefense = defense;
            card.cardProps.aiCalcDefense = defense;

        }
        public void setRace(Race race)
        {
            card.race = race;
        }
        public void addEffect(Effect effect)
        {
            effect.ability.setCard(card);
            card.cardProps.effects.Add(effect);
        }
        public void addAbility(Ability ability)
        {
            ability.setCard(card);
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
        public void setManuever(int identifier)
        {
            card = new Manuever(identifier);
            tempStorage.fillDictionary(identifier);
        }
        public Card getCard()
        {
            card.finalizeAbilities();
            return card;
        }
    }
}

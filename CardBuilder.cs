using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public class CardBuilder
    {
        //DECLARE ALL ABILITIES HERE

        public CardBuilder()
        {
            //Effect effect = new Effect()
        }
        public Card cardConstruct(CardConstructor constructor)
        {
            constructor.addEffect(new Effect(Effect.EffectType.DamageAllUnits, Effect.EffectTrigger.OnDefense, 3));
            return constructor.getCard();
        }
    }
    public class CardConstructor
    {
        protected Card card;
        public void setGeneral(int identifier)
        {
            card = new General(identifier);
        }
        public void setPower(int power)
        {
            card.setPower(power);
        }
        public void setDefense(int defense)
        {
            if (card.type == CardType.General)
            {

            }
            else
            {
                card.setDefense(defense);
            }
        }
        public void addEffect(Effect effect)
        {
            card.effects.Add(effect);
        }
        public void addAbility(Ability ability)
        {
            card.abilities.Add(ability);
        }
        public void setArmy(int identifier)
        {
            card = new Army(identifier);
        }
        public void setFieldUnit(int identifier)
        {
            card = new FieldUnit(identifier);
        }
        public Card getCard()
        {
            return card;
        }
    }
}

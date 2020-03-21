using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public class CardBuilder
    {
        public Card cardConstruct(CardConstructor constructor, int identifier)
        {
            Assemble(constructor, identifier);

            return constructor.getCard();
        }
        public void Assemble(CardConstructor constructor, int identifier)
        {
            if (identifier == 0)
            {
                constructor.setFieldUnit(identifier);
                Card.Race[] cost = { Card.Race.Elf, Card.Race.Elf };
                constructor.setCost(new Cost(2, cost));
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
                Card.Race[] cost = { Card.Race.Human };
                constructor.setCost(new Cost(3, cost));
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
                constructor.setPower(2);
                constructor.setRace(Card.Race.Orc);
                constructor.setDefense(4);
                constructor.setRarity(Card.Rarity.Bronze);
                constructor.setName("Thing");
            }
            else if(identifier == 3)
            {
                constructor.setFieldUnit(identifier);
                Card.Race[] cost = { Card.Race.Orc };
                constructor.setCost(new Cost(0, cost));
                constructor.setPower(1);
                constructor.setRace(Card.Race.Orc);
                constructor.setDefense(2);
                constructor.setRarity(Card.Rarity.Bronze);
                constructor.setName("NEWGUY");
            }
            else if(identifier == 4)
            {
                constructor.setGeneral(identifier);
                Card.Race[] cost = { Card.Race.Orc };
                constructor.setCost(new Cost(2, cost));
                constructor.setPower(0);
                constructor.setRace(Card.Race.Orc);
                constructor.setDefense(2);
                constructor.setRarity(Card.Rarity.Bronze);
                constructor.setName("COOL GENERAL");
                constructor.addAbility(new Reveal(2));
                constructor.addAbility(new BoardDamage(1, -1));
                constructor.addAbility(new BoardDamage(12, -10));
            }
            else if (identifier == 5)
            {
                constructor.setGeneral(identifier);
                Card.Race[] cost = { Card.Race.Orc, Card.Race.Orc };
                constructor.setCost(new Cost(0, cost));
                constructor.setPower(0);
                constructor.setRace(Card.Race.Orc);
                constructor.setDefense(1);
                constructor.setRarity(Card.Rarity.Bronze);
                constructor.setName("Spawner General");
                constructor.addAbility(new SpawnCard(10, +1));
                constructor.addAbility(new SpawnCard(11, -1));
                constructor.addAbility(new BoardDamage(12, -10));
            }
            else if(identifier == 6)
            {
                constructor.setGeneral(identifier);
                Card.Race[] cost = { Card.Race.Orc, Card.Race.Orc };
                constructor.setCost(new Cost(0, cost));
                constructor.setPower(0);
                constructor.setRace(Card.Race.Orc);
                constructor.setDefense(6);
                constructor.setRarity(Card.Rarity.Bronze);
                constructor.setName("Jess's General");
                constructor.addAbility(new BothSidesDrawCard(1, +2));
                constructor.addAbility(new SpawnCard(6, -5));
                constructor.addAbility(new BoardDamage(12, -10));
            }
            else if (identifier == 10)
            {
                constructor.setFieldUnit(identifier);
                constructor.setPower(4);
                constructor.setRace(Card.Race.Orc);
                constructor.setDefense(1);
                constructor.setRarity(Card.Rarity.Bronze);
                constructor.setName("Spawned");

                constructor.addAbility(new TargetDamage(1));

            }
            else if (identifier == 11)
            {
                constructor.setArmy(identifier);
                constructor.setPower(1);
                constructor.setRace(Card.Race.Orc);
                constructor.setDefense(1);
                constructor.setRarity(Card.Rarity.Bronze);
                constructor.setName("Spawn Army");

            }
        }
    }
    
}

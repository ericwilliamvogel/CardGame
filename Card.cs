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
    public class Card : GameComponent
    {
        public bool selected;
                Texture2D cardBorder;
                Texture2D cardHighlighting;
        private int identifier;
        public string name;
        public CardType type;
        public List<Effect> effects = new List<Effect>();
        public List<Ability> abilities = new List<Ability>();
        public int power = 0;
        public int defense = 1;

        public void setPower(int power)
        {
            this.power = power;
        }
        public void setDefense(int defense)
        {
            this.defense = defense;
        }

        public Card(int identifier)
        {
            this.name = name;
            this.identifier = identifier;
        }

        public override void mouseStateLogic(MouseState mouseState, ContentManager content)
        {

        }

        public virtual void takeDamage()
        {

        }
        public virtual void removeDamage()
        {

        }
        public virtual void checkEffects(GameState state)
        {
            if(effects != null)
            {
                foreach(Effect effect in effects)
                {
                    //if (effect.Trigger(state)) { }
                }
            }
            //ONABILITYACTIVATE
        }
        public virtual void activateAbility()
        {

        }
        //card logio here
        //generals / armies / soldiers have different capabilities
    }
    public class General : Card
    {
        public General(int identifier) : base(identifier)
        {
            type = CardType.General;
        }
    }
    public class Army : Card
    {
        public Army(int identifier) : base(identifier)
        {
            type = CardType.Army;
        }
    }
    public class FieldUnit : Card
    {
        public FieldUnit(int identifier) : base(identifier)
        {
            type = CardType.Field;
        }
    }
    public enum CardType
    {
        General,
        Army,
        Field
    }



   

    


}

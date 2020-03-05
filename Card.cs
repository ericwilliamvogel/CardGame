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
    public class Card : GameComponent
    {
        public bool makingAction;
        public Vector2 storedPosition;


        //
        //
        public CardSupplementalTextures suppTextures;
        public int identifier;
        public string name;
        public CardType type;
        public List<Effect> effects = new List<Effect>();
        public List<Ability> abilities = new List<Ability>();
        public int power = 0;
        public int defense = 1;

        public SelectState selectState;
        public PlayState playState;
        public Rarity rarity;
        public Race race;
        public enum Race
        {
            Orc,
            Elf,
            Human
        }
        public enum Rarity
        {
            Bronze,
            Silver,
            Gold,
            Diamond
        }
        public enum PlayState
        {
            Revealed,
            Hidden
        }
        public enum SelectState
        {
            Regular,
            Hovered,
            Selected
        }

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
            suppTextures = new CardSupplementalTextures();
            setContentName("cardBack");
            initSupplements();
        }

        

        public override void mouseStateLogic(MouseState mouseState, ContentManager content)
        {
            switch (selectState)
            {
                case SelectState.Regular:
                    if(isWithinBox(mouseState))
                    {
                        selectState = SelectState.Hovered;
                    }
                    
                    break;
                case SelectState.Hovered:
                    if(mouseState.LeftButton == ButtonState.Pressed)
                    {
                        selectState = SelectState.Selected;
                    }
                    break;
                case SelectState.Selected:
                    //pass a delegate depending on where the card is on board???
                    /*foreach card card in row
                     * if(card.state == STate.selected)
                     * {
                     *  //in board
                     *  FromRowLogic(Card card)
                     *  {
                     *      //give action
                     *  }
                     *  orrrr
                     *  FromHandLogic(Card card)
                     *  {
                     *      card.toMouse;
                     *      if(mouse is within Row[0])
                     *      {
                     *      if(card.type == general)
                     *      {
                     *      play}
                     *      else
                     *      {
                     *      popup
                     *      }
                     *      }
                     *      
                     *  }
                     *  if(mouesState.right click)
                     *  {
                     *  card.setBackTOLocastion;
                     *  card .state= Regular;
                        }
                        */
                    break;
            }
        }

        public override void drawSprite(SpriteBatch spriteBatch)
        {
            switch (playState)
            {
                case PlayState.Hidden:
                    suppTextures.cardBack.drawSprite(spriteBatch);
                    break;
                case PlayState.Revealed:

                    suppTextures.cardBorder.drawSprite(spriteBatch);
                    suppTextures.cardFilling.drawSprite(spriteBatch);
                    suppTextures.cardImageBorder.drawSprite(spriteBatch);
                    suppTextures.portrait.drawSprite(spriteBatch);


                    break;
            }

        }
        public override void updateGameComponent()
        {
            updateSupplementPositions();

        }

        public override void setPos(int x, int y)
        {
            suppTextures.moveAllTo(new Vector2(x, y));
            base.setPos(x, y);
        }
        public override void setPos(Vector2 input)
        {
            suppTextures.moveAllTo(input);
            base.setPos(input);
        }
        public override void setScale(float setting)
        {
            suppTextures.scaleAllTo(setting);
            initSupplements();
            base.setScale(setting);
        }
        private void initSupplements()
        {
            float w = Properties.globalScale.X + properties.scale.X;
            //
            suppTextures.portrait.setOffset(75*w, 105*w);
            suppTextures.cardBorder.setOffset(0, 0);
            suppTextures.cardFilling.setOffset(20*w, 20*w);
            suppTextures.cardImageBorder.setOffset(65*w, 95*w);
            suppTextures.cardBack.setOffset(0, 0);
            properties.width = suppTextures.cardBack.getWidth();
            properties.height = suppTextures.cardBack.getHeight();
        }
        public void setSupplementalTextures(CardImageStorage storage)
        {
            suppTextures.cardBorder.setTexture(storage.suppTextures.cardBorder.getTexture());
            suppTextures.cardImageBorder.setTexture(storage.suppTextures.cardImageBorder.getTexture());
            suppTextures.cardFilling.setTexture(storage.suppTextures.cardFilling.getTexture());
            suppTextures.cardBack.setTexture(storage.suppTextures.cardBack.getTexture());
            properties.width = suppTextures.cardBack.getWidth();
            properties.height = suppTextures.cardBack.getHeight();
        }
        public void setColorForRace()
        {
            suppTextures.cardFilling.properties.color = retrieveRaceColor();
        }
        private Color retrieveRaceColor()
        {
            if(race == Race.Elf)
            {
                return Color.Turquoise;
            }
            if(race == Race.Human)
            {
                return Color.Orange;
            }
            if (race == Race.Orc)
            {
                return Color.DarkViolet;
            }

            return Color.White;
        }
        private void updateSupplementPositions()
        {
            suppTextures.setAllPositionsRelativeTo(this);
        }
        public virtual void takeDamage()
        {

        }
        public virtual void removeDamage()
        {

        }
        /*public virtual void checkEffects(GameState state)
        {
            if(effects != null)
            {
                foreach(Effect effect in effects)
                {
                    //if (effect.Trigger(state)) { }
                }
            }
            //ONABILITYACTIVATE
        }*/
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

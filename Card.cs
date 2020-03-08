﻿using Microsoft.Xna.Framework;
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
        public class CardProperties
        {
            public int identifier;
            public string name;
            public List<Effect> effects = new List<Effect>();
            public List<Ability> abilities = new List<Ability>();
            public int cost = 0;
            public int power = 0;
            public int defense = 1;
            public CardType type;
        }




        public CardProperties cardProps;
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
            this.cardProps.power = power;
        }
        public void setDefense(int defense)
        {
            this.cardProps.defense = defense;
        }

        public Card(int identifier)
        {
            cardProps = new CardProperties();
            this.cardProps.identifier = identifier;
            suppTextures = new CardSupplementalTextures();
            setContentName("cardBack");
            initSupplements();
        }
        public Card(Card card)
        {
            this.suppTextures = card.suppTextures;
            this.properties.POS = card.getPosition();
            this.setScale(card.properties.scale.X);
            this.cardProps = card.cardProps;
            this.properties.Width = card.properties.width;
            this.properties.Height = card.properties.height;
            //this.setTexture(card.getTexture());
            initSupplements();
        }

        bool firstSwitch;

        public void setHovered()
        {
            selectState = SelectState.Hovered;
        }
        public void setRegular()
        {
            selectState = SelectState.Regular;
        }
        public void setSelected()
        {
            selectState = SelectState.Selected;
        }
        public bool isSelected()
        {
            if(selectState == SelectState.Selected)
            {
                return true;
            }
            return false;
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
                    if(mouseState.MiddleButton == ButtonState.Pressed)
                    {
                        selectState = SelectState.Regular;
                    }
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
                    if(selectState == SelectState.Hovered)
                    {
                        drawHighlight(spriteBatch);

                    }
                    spriteBatch.DrawString(Game1.spritefont, cardProps.name, new Vector2(getPosition().X + 50 * getScale().X, getPosition().Y + 40 * getScale().X), Color.Black, 0, new Vector2(0, 0), 3f * getScale(), SpriteEffects.None, 0);
                    spriteBatch.DrawString(Game1.spritefont, cardProps.cost.ToString(), new Vector2(getPosition().X + getWidth() - 70 * getScale().X, getPosition().Y + 30 * getScale().X), Color.Red, 0, new Vector2(0, 0), 4f * getScale(), SpriteEffects.None, 0);
                    break;
            }

        }
        public void drawHighlight(SpriteBatch spriteBatch)
        {
            ShadowComponent highlight = new ShadowComponent(suppTextures.cardFilling);
            highlight.properties.color = Color.Black * .4f;
            //highlight.updateScaleAndPosition(suppTextures.cardFilling);

            highlight.drawSprite(spriteBatch);
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
        public void setCardBackColor(Color color)
        {
            suppTextures.cardBack.properties.color = color;
        }

        public void initSupplements()
        {
            float w = Properties.globalScale.X + properties.scale.X;
            //
            suppTextures.portrait.setOffset(20*w, 105*w);
            suppTextures.cardBorder.setOffset(0, 0);
            suppTextures.cardFilling.setOffset(20*w, 20*w);
            suppTextures.cardImageBorder.setOffset(0*w, 95*w);
            suppTextures.cardBack.setOffset(0, 0);
            properties.width = suppTextures.cardBack.properties.width;
            properties.height = suppTextures.cardBack.properties.height;
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
            cardProps.type = CardType.General;
        }
    }
    public class Army : Card
    {
        public Army(int identifier) : base(identifier)
        {
            cardProps.type = CardType.Army;
        }
    }
    public class FieldUnit : Card
    {
        public FieldUnit(int identifier) : base(identifier)
        {
            cardProps.type = CardType.FieldUnit;
        }
    }
    public enum CardType
    {
        General,
        Army,
        FieldUnit
    }



   

    


}

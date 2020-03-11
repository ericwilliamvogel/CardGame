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
            public bool exhausted;
        }


        //public List<Button> abilityButtons = new List<Button>();
       // public bool showAbilities;
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
                    suppTextures.supplements[suppTextures.cardBack].drawSprite(spriteBatch);
                    break;
                case PlayState.Revealed:

                    suppTextures.supplements[suppTextures.cardBorder].drawSprite(spriteBatch);
                    suppTextures.supplements[suppTextures.cardFilling].drawSprite(spriteBatch);
                    suppTextures.supplements[suppTextures.cardImageBorder].drawSprite(spriteBatch);
                    suppTextures.supplements[suppTextures.portrait].drawSprite(spriteBatch);
                    if (selectState == SelectState.Hovered)
                    {
                        drawHighlight(spriteBatch);

                    }
                    if (selectState == SelectState.Selected)
                    {
                        drawHighlight(spriteBatch);
                        drawHighlight(spriteBatch);
                    }
                    drawCardSelectionBorder(spriteBatch);
                    spriteBatch.DrawString(Game1.spritefont, cardProps.name, new Vector2(getPosition().X + 50 * getScale().X, getPosition().Y + 40 * getScale().X), Color.Black, 0, new Vector2(0, 0), 3f * getScale(), SpriteEffects.None, 0);
                    spriteBatch.DrawString(Game1.spritefont, cardProps.cost.ToString(), new Vector2(getPosition().X + getWidth() - 70 * getScale().X, getPosition().Y + 30 * getScale().X), Color.Red, 0, new Vector2(0, 0), 4f * getScale(), SpriteEffects.None, 0);

                    /*for(int i = 0; i < 3; i++)
                    {
                        suppTextures.supplements[suppTextures.abilityDisplay].drawSprite(spriteBatch);
                        spriteBatch.Draw(suppTextures.supplements[suppTextures.abilityDisplay].getTexture(), new Vector2(suppTextures.supplements[suppTextures.abilityDisplay].getPosition().X, suppTextures.supplements[suppTextures.abilityDisplay].getPosition().Y + suppTextures.supplements[suppTextures.abilityDisplay].getHeight() * i), null, null, null, getRotation(), getScale(), getColor(), properties.spriteEffects, 0);
                    }*/
                    /*int i = 0;
                    foreach(Ability ability in cardProps.abilities)
                    {
                        spriteBatch.Draw(suppTextures.supplements[suppTextures.abilityDisplay].getTexture(), new Vector2(suppTextures.supplements[suppTextures.abilityDisplay].getPosition().X, suppTextures.supplements[suppTextures.abilityDisplay].getPosition().Y + suppTextures.supplements[suppTextures.abilityDisplay].getHeight() * i), null, null, null, getRotation(), getScale(), getColor(), properties.spriteEffects, 0);
                        //suppTextures.supplements[suppTextures.abilityDisplay].drawSprite(spriteBatch);
                        i++;
                    }*/
                    break;
            }

        }
        public void drawHighlight(SpriteBatch spriteBatch)
        {
            ShadowComponent highlight = new ShadowComponent(suppTextures.supplements[suppTextures.cardFilling]);
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
            suppTextures.supplements[suppTextures.cardBack].properties.color = color;
        }

        public void resetCardSelector()
        {
            selectorX = 0;
            selectorY = 0;
            velocityX = 2;
            velocityY = -1;
        }
        public void initSupplements()
        {
            float w = Properties.globalScale.X + properties.scale.X;
            //

            suppTextures.supplements[suppTextures.portrait].setOffset(20 * w, 105 * w);
            suppTextures.supplements[suppTextures.cardBorder].setOffset(0 * w, 0 * w);
            suppTextures.supplements[suppTextures.cardFilling].setOffset(20 * w, 20 * w);
            suppTextures.supplements[suppTextures.cardImageBorder].setOffset(0 * w, 95 * w);
            suppTextures.supplements[suppTextures.cardBack].setOffset(0 * w, 0 * w);
            suppTextures.supplements[suppTextures.abilityDisplay].setOffset(properties.width * w, 0);



            properties.width = suppTextures.supplements[suppTextures.cardBack].properties.width;
            properties.height = suppTextures.supplements[suppTextures.cardBack].properties.height;
        }
        private int selectorX, selectorY;
        int velocityX = 2;
        int velocityY = -1;
        public void drawCardSelectionBorder(SpriteBatch spriteBatch)
        {

            if(isSelected())
            {
                int speed = (int)(50 * getScale().X);
                int width = suppTextures.supplements[suppTextures.selectionIndicator].getWidth();
                int height = suppTextures.supplements[suppTextures.selectionIndicator].getHeight();
                selectorX += velocityX;
                selectorY += velocityY;
                if(selectorY < 0)
                {
                    velocityX = speed;
                    velocityY = 0;

                }
                if(selectorX > getWidth() + width)
                {
                    velocityY = speed;
                    velocityX = 0;

                }

                if (selectorY > getHeight() + height)
                {
                    velocityY = 0;
                    velocityX = -speed;
                    if (selectorX < 0)
                    {
                        velocityY = -speed;
                        velocityX = 0;

                    }
                }


                suppTextures.supplements[suppTextures.selectionIndicator].setOffset(selectorX - width, selectorY - height);
                suppTextures.supplements[suppTextures.selectionIndicator].properties.color = Color.Yellow;
                suppTextures.supplements[suppTextures.selectionIndicator].drawSprite(spriteBatch);
            }

        }
        public void setSupplementalTextures(CardImageStorage storage)
        {
            for(int i = 0; i < suppTextures.TOTAL; i++)
            {
                if(i != suppTextures.portrait)
                suppTextures.supplements[i].setTexture(storage.suppTextures.supplements[i].getTexture());
            }
            /*for(int i = 0; i < abilityButtons.Count; i++)
            {
                Vector2 nullablePosition = new Vector2(0, 0);
                abilityButtons[i] = new Button(null, nullablePosition);
                abilityButtons[i].setTexture(storage.suppTextures.supplements[suppTextures.abilityDisplay].getTexture());
            }*/
            //suppTextures.cardBorder.setTexture(storage.suppTextures.cardBorder.getTexture());
            //suppTextures.cardImageBorder.setTexture(storage.suppTextures.cardImageBorder.getTexture());
            //suppTextures.cardFilling.setTexture(storage.suppTextures.cardFilling.getTexture());
            //suppTextures.cardBack.setTexture(storage.suppTextures.cardBack.getTexture());
            properties.width = suppTextures.supplements[suppTextures.cardBack].getWidth();
            properties.height = suppTextures.supplements[suppTextures.cardBack].getHeight();
        }
        public void setColorForRace()
        {
            suppTextures.supplements[suppTextures.cardFilling].properties.color = retrieveRaceColor();
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

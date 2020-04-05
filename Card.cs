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
    public class Cost
    {
        public Cost()
        {

        }
        public Cost(int unanimousCost, Card.Race[] races)
        {
            raceCost = new List<Card.Race>();
            this.unanimousCost = unanimousCost;
            raceCost.AddRange(races.ToList());
            totalCost = unanimousCost + raceCost.Count;
        }
        public List<Card.Race> raceCost;
        public int unanimousCost = 0;
        public int totalCost = 0;
    }
    public class Card : GameComponent
    {
        public bool makingAction;
        public Vector2 storedPosition;
        public CardProperties cardProps;
        public SelectState selectState;
        public PlayState playState;
        public Rarity rarity;
        public Race race;
        public CardSupplementalTextures suppTextures;

        public class CardProperties
        {
            public int identifier;
            public string name;
            public List<Effect> effects = new List<Effect>();
            public List<Ability> abilities = new List<Ability>();
            public Cost cost = new Cost();
            public int initialPower = 0;
            public int initialDefense = 1;
            public int aiCalcDefense = 1;
            public int power = 0;
            public int defense = 1;
            public CardType type;
            public bool exhausted;
            public bool doubleExhausted;
        }


        public enum Race
        {
            Orc,
            Elf,
            Human,
            Unanimous
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

        public int returnValue()
        {
            int value = cardProps.power + cardProps.defense + cardProps.cost.totalCost;
            return value;
        }

        public HorizontalContainer getCurrentContainer(Side side)
        {
            foreach(FunctionalRow container in side.Rows)
            {
                if(container.type == cardProps.type)
                {
                    return container;
                }
            }
            return null;
        }



        public void setPower(int power)
        {
            this.cardProps.power = power;
        }
        public void setDefense(int defense)
        {
            this.cardProps.defense = defense;
        }

        public FunctionalRow correctRow(Side side)
        {
            foreach(FunctionalRow row in side.Rows)
            {
                if(row.type == cardProps.type)
                {
                    return row;
                }
            }
            return null;
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
            //this.properties.POS = card.getPosition();
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
            if (!cardProps.exhausted)
            {
                switch (selectState)
                {
                    case SelectState.Regular:
                        if (isWithinBox(mouseState))
                        {
                            selectState = SelectState.Hovered;
                        }

                        break;
                    case SelectState.Hovered:
                        if (mouseState.LeftButton == ButtonState.Pressed)
                        {
                            selectState = SelectState.Selected;
                        }
                        break;
                    case SelectState.Selected:
                        if (mouseState.MiddleButton == ButtonState.Pressed)
                        {
                            selectState = SelectState.Regular;
                        }
                        break;
                }
            }
            else
            {
                selectState = SelectState.Hovered;
            }
        }

        public bool canBePlayed(Side side)
        {
            if(cardProps.cost.totalCost <= side.Resources.Count)
            {
                return true;
            }
            return false;
        }
        public bool isFirstArmy(Side side)
        {
                if(side.canPlayArmy)
                {
                    return true;
                }
            
            return false;
        }

        public bool containsReveal()
        {
            foreach(Ability ability in cardProps.abilities)
            {
                if(ability is Reveal)
                {
                    throw new Exception("IN FILE EFFECT / CLASS REVEAL / CODE IS WORKING!~");
                    return true;
                }
            }
            return false;
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
                    suppTextures.supplements[suppTextures.cardBody].drawSprite(spriteBatch);


                    suppTextures.supplements[suppTextures.portrait].drawSprite(spriteBatch);
                    suppTextures.supplements[suppTextures.cardShading].drawSprite(spriteBatch);
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
                    spriteBatch.DrawString(Game1.spritefont, cardProps.name, new Vector2(getPosition().X + 55 * getScale().X, getPosition().Y + 30 * getScale().X), Color.Black, 0, new Vector2(0, 0), getScale(), SpriteEffects.None, 0);

                    switch(cardProps.type)
                    {
                        case CardType.Army:
                            suppTextures.supplements[suppTextures.armySymbol].drawSprite(spriteBatch);
                            break;
                        case CardType.General:
                            suppTextures.supplements[suppTextures.generalSymbol].drawSprite(spriteBatch);
                            break;
                        case CardType.FieldUnit:
                            suppTextures.supplements[suppTextures.fieldUnitSymbol].drawSprite(spriteBatch);
                            break;
                        case CardType.Manuever:
                            suppTextures.supplements[suppTextures.manueverSymbol].drawSprite(spriteBatch);
                            break;
                    }

                    if(cardProps.type != CardType.Manuever)
                    {
                        if (cardProps.type != CardType.General)
                        {
                            suppTextures.supplements[suppTextures.attackIcon].drawSprite(spriteBatch);
                            spriteBatch.DrawString(Game1.spritefont, cardProps.power.ToString(), new Vector2(getPosition().X + 40 * getScale().X, getPosition().Y + getHeight() - 100 * getScale().X), Color.Black, 0, new Vector2(0, 0), 1.66f * getScale(), SpriteEffects.None, 0);
                        }
                        suppTextures.supplements[suppTextures.defenseIcon].drawSprite(spriteBatch);
                        spriteBatch.DrawString(Game1.spritefont, cardProps.defense.ToString(), new Vector2(getPosition().X + getWidth() - 80 * getScale().X, getPosition().Y + getHeight() - 100 * getScale().X), Color.Black, 0, new Vector2(0, 0), 1.66f * getScale(), SpriteEffects.None, 0);
                    }



                    for(int i = 0; i < cardProps.abilities.Count; i++)
                    {
                        spriteBatch.DrawString(Game1.spritefont, cardProps.abilities[i].name.ToString() + " "+cardProps.abilities[i].description.ToString(), new Vector2(getPosition().X + 50 * getScale().X, getPosition().Y + getHeight() * 2/3 + (65 * getScale().X) * i), Color.Black, 0, new Vector2(0, 0), getScale(), SpriteEffects.None, 0);
                    }


                    int counter = 0;
                    int selector = 0;
                    int tokenWidth = (int) (suppTextures.supplements[suppTextures.elfToken].getWidth() - 20 * getScale().X);
                    int borderOffset = (int)(15 * getScale().X);
                    float iconScale = .72f;

                    if (cardProps.cost.raceCost != null)
                    {
                        foreach (Race resource in cardProps.cost.raceCost)
                        {

                            if (resource == Race.Elf)
                            {
                                selector = suppTextures.elfToken;
                            }
                            else if (resource == Race.Orc)
                            {
                                selector = suppTextures.orcToken;
                            }
                            else if (resource == Race.Human)
                            {
                                selector = suppTextures.humanToken;
                            }


                            spriteBatch.Draw(suppTextures.supplements[selector].getTexture(), new Vector2(getPosition().X + getWidth() - borderOffset * 2 - tokenWidth - tokenWidth * counter * iconScale, getPosition().Y + borderOffset), null, null, null, getRotation(), iconScale * getScale(), getColor(), properties.spriteEffects, 0);

                            counter++;
                        }
                    }

                    if (cardProps.cost.unanimousCost > 0 && cardProps.cost.raceCost != null)
                    {

                        selector = suppTextures.unanimousToken;
                        spriteBatch.Draw(suppTextures.supplements[selector].getTexture(), new Vector2(getPosition().X + getWidth() - borderOffset * 2 - tokenWidth - tokenWidth * counter * iconScale, getPosition().Y + borderOffset), null, null, null, getRotation(), iconScale * getScale(), getColor(), properties.spriteEffects, 0);
                        spriteBatch.DrawString(Game1.spritefont, cardProps.cost.unanimousCost.ToString(), new Vector2(getPosition().X + getWidth() - borderOffset * 2- tokenWidth - tokenWidth * counter * iconScale + tokenWidth / 2 - borderOffset * 4/5, getPosition().Y + borderOffset * 2), Color.Black, 0, new Vector2(0, 0), 1f * getScale(), SpriteEffects.None, 0);
                    }

                    if (cardProps.cost.unanimousCost >= 0 && cardProps.cost.raceCost == null)
                    {

                        selector = suppTextures.unanimousToken;
                        spriteBatch.Draw(suppTextures.supplements[selector].getTexture(), new Vector2(getPosition().X + getWidth() - borderOffset * 2 - tokenWidth - tokenWidth * counter * iconScale, getPosition().Y + borderOffset), null, null, null, getRotation(), iconScale * getScale(), getColor(), properties.spriteEffects, 0);
                        spriteBatch.DrawString(Game1.spritefont, cardProps.cost.unanimousCost.ToString(), new Vector2(getPosition().X + getWidth() - borderOffset * 2 - tokenWidth - tokenWidth * counter * iconScale + tokenWidth / 2 - borderOffset * 4 / 5, getPosition().Y + borderOffset * 2), Color.Black, 0, new Vector2(0, 0), 1f * getScale(), SpriteEffects.None, 0);
                    }

                    break;
            }

        }
        public void drawHighlight(SpriteBatch spriteBatch)
        {
            ShadowComponent highlight = new ShadowComponent(suppTextures.supplements[suppTextures.cardFilling]);
            highlight.properties.color = Color.Black * .4f;
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

            suppTextures.supplements[suppTextures.portrait].setOffset(20 * w, 100 * w);
            suppTextures.supplements[suppTextures.cardBorder].setOffset(0 * w, 0 * w);
            suppTextures.supplements[suppTextures.cardShading].setOffset(0 * w, 0 * w);
            suppTextures.supplements[suppTextures.cardBody].setOffset(0 * w, 0 * w);
            suppTextures.supplements[suppTextures.cardBack].setOffset(0 * w, 0 * w);
            suppTextures.supplements[suppTextures.cardFilling].setOffset(0 * w, 0 * w);
            suppTextures.supplements[suppTextures.abilityDisplay].setOffset(properties.width * w, 0);

            int afterCardImageBorder = (int)(540 * w);
            suppTextures.supplements[suppTextures.generalSymbol].setOffset(0 * w, afterCardImageBorder);
            suppTextures.supplements[suppTextures.armySymbol].setOffset(0 * w, afterCardImageBorder);
            suppTextures.supplements[suppTextures.fieldUnitSymbol].setOffset(0 * w, afterCardImageBorder);
            suppTextures.supplements[suppTextures.manueverSymbol].setOffset(0 * w, afterCardImageBorder);

            float symbolTransparency = .2f;
            suppTextures.supplements[suppTextures.generalSymbol].properties.transparency = symbolTransparency;
            suppTextures.supplements[suppTextures.armySymbol].properties.transparency = symbolTransparency;
            suppTextures.supplements[suppTextures.fieldUnitSymbol].properties.transparency = symbolTransparency;
            suppTextures.supplements[suppTextures.manueverSymbol].properties.transparency = symbolTransparency;

            Color symbolColor = Color.Gray;
            suppTextures.supplements[suppTextures.generalSymbol].properties.color = symbolColor;
            suppTextures.supplements[suppTextures.armySymbol].properties.color = symbolColor;
            suppTextures.supplements[suppTextures.fieldUnitSymbol].properties.color = symbolColor;
            suppTextures.supplements[suppTextures.manueverSymbol].properties.color = symbolColor;


            int attackPosY = (int)(properties.height * w - suppTextures.supplements[suppTextures.attackIcon].getHeight() * 7/8);
            int attackPosX = (int)-(suppTextures.supplements[suppTextures.attackIcon].getWidth() * 3/16);

            int defensePosY = (int)(properties.height*w - suppTextures.supplements[suppTextures.attackIcon].getHeight() * 7/8);
            int defensePosX = (int)(properties.width*w -suppTextures.supplements[suppTextures.attackIcon].getWidth() * 7/8);

            suppTextures.supplements[suppTextures.attackIcon].setOffset(attackPosX , attackPosY );
            suppTextures.supplements[suppTextures.defenseIcon].setOffset(defensePosX , defensePosY );


            properties.width = suppTextures.supplements[suppTextures.cardBack].properties.width;
            properties.height = suppTextures.supplements[suppTextures.cardBack].properties.height;
        }
        private int selectorX, selectorY;
        int velocityX = 2;
        int velocityY = -1;

        public void finalizeAbilities()
        {
            foreach(Ability ability in cardProps.abilities)
            {
                ability.setCard(this);
            }
        }

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
                return Color.Green;
            }
            if(race == Race.Human)
            {
                return Color.Orange;
            }
            if (race == Race.Orc)
            {
                return Color.SteelBlue;
            }

            return Color.White;
        }
        private void updateSupplementPositions()
        {
            suppTextures.setAllPositionsRelativeTo(this);
        }

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
    public class Manuever : Card
    {
        public Manuever(int identifier) : base(identifier)
        {
            cardProps.type = CardType.Manuever;
        }
    }
    public enum CardType
    {
        General,
        Army,
        FieldUnit,
        Manuever
    }



   

    


}

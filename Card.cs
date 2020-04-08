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
        public CardProperties cardProps;
        public SelectState selectState;
        public PlayState playState;
        public Rarity rarity;
        public Race race;
        public CardSupplementalTextures suppTextures;
        public CardDrawLogic cardDrawer;
        public CardSupplementSetter supplementSetter;



        public Card(int identifier)
        {
            cardProps = new CardProperties();
            this.cardProps.identifier = identifier;
            suppTextures = new CardSupplementalTextures();
            setContentName("cardBack");
            setNestedClasses();
            initSupplements();
        }
        public Card(Card card) //a better functionality exists in BoardFunctioanlity -> duplicate. only known use in CardViewer, will look into patching later//
        {
            setNestedClasses();
            this.suppTextures = card.suppTextures;
            this.setScale(card.properties.scale.X);
            this.cardProps = card.cardProps;
            this.properties.Width = card.properties.width;
            this.properties.Height = card.properties.height;
            initSupplements();
        }

        public void setNestedClasses()
        {
            cardDrawer = new CardDrawLogic();
            supplementSetter = new CardSupplementSetter();
        }
       
        
        public override void drawSprite(SpriteBatch spriteBatch)
        {
            cardDrawer.updCardValues(spriteBatch, this);
            switch (playState)
            {
                case PlayState.Hidden:
                    cardDrawer.DrawCardBack();
                    break;
                case PlayState.Revealed:
                    cardDrawer.DrawCardFront();
                    cardDrawer.DrawSelectionHighlights();
                    cardDrawer.DrawCardTypeSymbol();
                    cardDrawer.DrawPowerAndDefense();
                    cardDrawer.DrawAbilityAndEffectText();
                    cardDrawer.DrawCost();
                    break;
            }

        }

        public override void updateGameComponent()
        {
            updateSupplementPositions();
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



        public void resetCardSelector()
        {
            cardDrawer.resetCardSelector();
        }


        //Setters
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
        public void setPower(int power)
        {
            this.cardProps.power = power;
        }
        public void setDefense(int defense)
        {
            this.cardProps.defense = defense;
        }
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
            if (selectState == SelectState.Selected)
            {
                return true;
            }
            return false;
        }
        public int returnValue()
        {
            int value = cardProps.power + cardProps.defense + cardProps.cost.totalCost;
            return value;
        }
        public HorizontalContainer getCurrentContainer(Side side)
        {
            foreach (FunctionalRow container in side.Rows)
            {
                if (container.type == cardProps.type)
                {
                    return container;
                }
            }
            return null;
        }
        public FunctionalRow correctRow(Side side)
        {
            foreach (FunctionalRow row in side.Rows)
            {
                if (row.type == cardProps.type)
                {
                    return row;
                }
            }
            return null;
        }
        public bool canBePlayed(Side side)
        {
            if (cardProps.cost.totalCost <= side.Resources.Count)
            {
                return true;
            }
            return false;
        }

        public bool containsReveal()
        {
            foreach (Ability ability in cardProps.abilities)
            {
                if (ability is Reveal) // test pls
                {
                    throw new Exception("IN FILE EFFECT / CLASS REVEAL / CODE IS WORKING!~");
                    return true;
                }
            }
            return false;
        }




        public void setCardBackColor(Color color)
        {
            suppTextures.supplements[suppTextures.cardBack].properties.color = color;
        }
        public void setColorForRace()
        {
            suppTextures.supplements[suppTextures.cardFilling].properties.color = retrieveRaceColor();
        }
        private Color retrieveRaceColor()
        {
            if (race == Race.Elf)
            {
                return Color.Green;
            }
            if (race == Race.Human)
            {
                return Color.Orange;
            }
            if (race == Race.Orc)
            {
                return Color.SteelBlue;
            }

            return Color.White;
        }





        public void initSupplements()
        {
            supplementSetter.initSupplements(this);
        }
        private void updateSupplementPositions()
        {
            suppTextures.setAllPositionsRelativeTo(this);
        }
        public void setSupplementalTextures(CardImageStorage storage)
        {
            for (int i = 0; i < suppTextures.TOTAL; i++)
            {
                if (i != suppTextures.portrait)
                    suppTextures.supplements[i].setTexture(storage.suppTextures.supplements[i].getTexture());
            }
            properties.width = suppTextures.supplements[suppTextures.cardBack].getWidth();
            properties.height = suppTextures.supplements[suppTextures.cardBack].getHeight();
        }





        public void finalizeAbilities()
        {
            foreach(Ability ability in cardProps.abilities)
            {
                ability.setCard(this);
            }
        }
    }
}

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
using static CardGame.Card;

namespace CardGame
{
    public class CardDrawLogic
    {
        private CardSelectorLogic selectorLogic;
        private SpriteBatch spriteBatch;
        private Card card;
        public CardDrawLogic()
        {
            selectorLogic = new CardSelectorLogic();
        }
        public void updCardValues(SpriteBatch spriteBatch, Card card)
        {
            this.card = card;
            this.spriteBatch = spriteBatch;
        }
        public void resetCardSelector()
        {
            selectorLogic.resetCardSelector();
        }
        public void DrawCardBack()
        {
            card.suppTextures.supplements[card.suppTextures.cardBack].drawSprite(spriteBatch);
        }
        public void DrawCardFront()
        {
            card.suppTextures.supplements[card.suppTextures.cardBorder].drawSprite(spriteBatch);
            card.suppTextures.supplements[card.suppTextures.cardFilling].drawSprite(spriteBatch);
            card.suppTextures.supplements[card.suppTextures.cardBody].drawSprite(spriteBatch);
            card.suppTextures.supplements[card.suppTextures.portrait].drawSprite(spriteBatch);
            card.suppTextures.supplements[card.suppTextures.cardShading].drawSprite(spriteBatch);
            spriteBatch.DrawString(Game1.spritefont, card.cardProps.name, new Vector2(card.getPosition().X + 55 * card.getScale().X, card.getPosition().Y + 30 * card.getScale().X), Color.Black, 0, new Vector2(0, 0), card.getScale(), SpriteEffects.None, 0);
        }
        public void DrawSelectionHighlights()
        {
            switch (card.selectState)
            {
                case SelectState.Hovered:
                    DrawHighlight();
                    break;
                case SelectState.Selected:
                    DrawHighlight();
                    DrawHighlight();
                    selectorLogic.DrawCardSelectionBorder(spriteBatch, card);
                    break;

            }
        }
        private void DrawHighlight()
        {
            ShadowComponent highlight = new ShadowComponent(card.suppTextures.supplements[card.suppTextures.cardFilling]);
            highlight.properties.color = Color.Black * .4f;
            highlight.drawSprite(spriteBatch);
        }

        public void DrawCardTypeSymbol()
        {
            switch (card.cardProps.type)
            {
                case CardType.Army:
                    card.suppTextures.supplements[card.suppTextures.armySymbol].drawSprite(spriteBatch);
                    break;
                case CardType.General:
                    card.suppTextures.supplements[card.suppTextures.generalSymbol].drawSprite(spriteBatch);
                    break;
                case CardType.FieldUnit:
                    card.suppTextures.supplements[card.suppTextures.fieldUnitSymbol].drawSprite(spriteBatch);
                    break;
                case CardType.Manuever:
                    card.suppTextures.supplements[card.suppTextures.manueverSymbol].drawSprite(spriteBatch);
                    break;
            }
        }
        public void DrawPowerAndDefense()
        {
            if (card.cardProps.type != CardType.Manuever)
            {
                float textScale = 1.66f;
                float trueTextScale = textScale * card.getScale().X;
                if (card.cardProps.type != CardType.General)
                {

                    float powerOffsetX = 40 * card.getScale().X;
                    float x = card.getPosition().X + powerOffsetX;
                    float powerOffsetY = -100 * card.getScale().X;
                    float y = card.getPosition().Y + card.getHeight() + powerOffsetY;

                    card.suppTextures.supplements[card.suppTextures.attackIcon].drawSprite(spriteBatch);
                    spriteBatch.DrawString(Game1.spritefont, card.cardProps.power.ToString(), new Vector2(x, y), Color.Black, 0, new Vector2(0, 0), trueTextScale, SpriteEffects.None, 0);
                }
                card.suppTextures.supplements[card.suppTextures.defenseIcon].drawSprite(spriteBatch);

                float defOffsetX = -80 * card.getScale().X;
                float posX = card.getPosition().X + card.getWidth() + defOffsetX;
                float defOffsetY = -100 * card.getScale().X;
                float posY = card.getPosition().Y + card.getHeight() + defOffsetY;
                spriteBatch.DrawString(Game1.spritefont, card.cardProps.defense.ToString(), new Vector2(posX, posY), Color.Black, 0, new Vector2(0, 0), trueTextScale, SpriteEffects.None, 0);
            }
        }
        public void DrawAbilityAndEffectText()
        {
            int counter = 0;
            for (int i = 0; i < card.cardProps.effects.Count; i++)
            {
                card.cardProps.effects[i].ability.name = card.cardProps.effects[i].getName();
                spriteBatch.DrawString(Game1.spritefont, card.cardProps.effects[i].ability.name.ToString() + " " + card.cardProps.effects[i].ability.description.ToString(), new Vector2(card.getPosition().X + 50 * card.getScale().X, card.getPosition().Y + card.getHeight() * 2 / 3 + (65 * card.getScale().X) * counter), Color.Black, 0, new Vector2(0, 0), card.getScale(), SpriteEffects.None, 0);
                counter++;
            }
            for (int i = 0; i < card.cardProps.abilities.Count; i++)
            {
                spriteBatch.DrawString(Game1.spritefont, card.cardProps.abilities[i].name.ToString() + " " + card.cardProps.abilities[i].description.ToString(), new Vector2(card.getPosition().X + 50 * card.getScale().X, card.getPosition().Y + card.getHeight() * 2 / 3 + (65 * card.getScale().X) * counter), Color.Black, 0, new Vector2(0, 0), card.getScale(), SpriteEffects.None, 0);
                counter++;
            }
        }

        public void DrawCost()
        {
            int counter = 0;
            int selector = 0;
            int tokenWidth = (int)(card.suppTextures.supplements[card.suppTextures.elfToken].getWidth() - 20 * card.getScale().X);
            int borderOffset = (int)(15 * card.getScale().X);
            float iconScale = .72f;

            if (card.cardProps.cost.raceCost != null)
            {
                foreach (Race resource in card.cardProps.cost.raceCost)
                {
                    selector = correctSymbol(resource, card);
                    spriteBatch.Draw(card.suppTextures.supplements[selector].getTexture(), new Vector2(card.getPosition().X + card.getWidth() - borderOffset * 2 - tokenWidth - tokenWidth * counter * iconScale, card.getPosition().Y + borderOffset), null, null, null, card.getRotation(), iconScale * card.getScale(), card.getColor(), card.properties.spriteEffects, 0);
                    counter++;
                }
            }

            if (card.cardProps.cost.unanimousCost > 0 && card.cardProps.cost.raceCost != null)
            {

                selector = card.suppTextures.unanimousToken;
                spriteBatch.Draw(card.suppTextures.supplements[selector].getTexture(), new Vector2(card.getPosition().X + card.getWidth() - borderOffset * 2 - tokenWidth - tokenWidth * counter * iconScale, card.getPosition().Y + borderOffset), null, null, null, card.getRotation(), iconScale * card.getScale(), card.getColor(), card.properties.spriteEffects, 0);
                spriteBatch.DrawString(Game1.spritefont, card.cardProps.cost.unanimousCost.ToString(), new Vector2(card.getPosition().X + card.getWidth() - borderOffset * 2 - tokenWidth - tokenWidth * counter * iconScale + tokenWidth / 2 - borderOffset * 4 / 5, card.getPosition().Y + borderOffset * 2), Color.Black, 0, new Vector2(0, 0), 1f * card.getScale(), SpriteEffects.None, 0);
            }

            if (card.cardProps.cost.unanimousCost >= 0 && card.cardProps.cost.raceCost == null)
            {

                selector = card.suppTextures.unanimousToken;
                spriteBatch.Draw(card.suppTextures.supplements[selector].getTexture(), new Vector2(card.getPosition().X + card.getWidth() - borderOffset * 2 - tokenWidth - tokenWidth * counter * iconScale, card.getPosition().Y + borderOffset), null, null, null, card.getRotation(), iconScale * card.getScale(), card.getColor(), card.properties.spriteEffects, 0);
                spriteBatch.DrawString(Game1.spritefont, card.cardProps.cost.unanimousCost.ToString(), new Vector2(card.getPosition().X + card.getWidth() - borderOffset * 2 - tokenWidth - tokenWidth * counter * iconScale + tokenWidth / 2 - borderOffset * 4 / 5, card.getPosition().Y + borderOffset * 2), Color.Black, 0, new Vector2(0, 0), 1f * card.getScale(), SpriteEffects.None, 0);
            }

        }
        private int correctSymbol(Race resource, Card card)
        {
            int selector = 0;
            if (resource == Race.Elf)
            {
                selector = card.suppTextures.elfToken;
            }
            else if (resource == Race.Orc)
            {
                selector = card.suppTextures.orcToken;
            }
            else if (resource == Race.Human)
            {
                selector = card.suppTextures.humanToken;
            }
            else
            {
                selector = card.suppTextures.orcToken;
                Console.WriteLine("resource in card was not recognized");
            }
            return selector;
        }
    }
}

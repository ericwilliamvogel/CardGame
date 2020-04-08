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
    public class CardSelectorLogic
    {
        private int selectorX, selectorY;
        int velocityX = 2;
        int velocityY = -1;
        public void resetCardSelector()
        {
            selectorX = 0;
            selectorY = 0;
            velocityX = 2;
            velocityY = -1;
        }
        public void DrawCardSelectionBorder(SpriteBatch spriteBatch, Card card)
        {
            int speed = (int)(50 * card.getScale().X);
            int width = card.suppTextures.supplements[card.suppTextures.selectionIndicator].getWidth();
            int height = card.suppTextures.supplements[card.suppTextures.selectionIndicator].getHeight();
            selectorX += velocityX;
            selectorY += velocityY;
            if (selectorY < 0)
            {
                velocityX = speed;
                velocityY = 0;

            }
            if (selectorX > card.getWidth() + width)
            {
                velocityY = speed;
                velocityX = 0;
            }

            if (selectorY > card.getHeight() + height)
            {
                velocityY = 0;
                velocityX = -speed;
                if (selectorX < 0)
                {
                    velocityY = -speed;
                    velocityX = 0;

                }
            }
            card.suppTextures.supplements[card.suppTextures.selectionIndicator].setOffset(selectorX - width, selectorY - height);
            card.suppTextures.supplements[card.suppTextures.selectionIndicator].properties.color = Color.Yellow;
            card.suppTextures.supplements[card.suppTextures.selectionIndicator].drawSprite(spriteBatch);
        }
    }
}

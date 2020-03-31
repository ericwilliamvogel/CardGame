
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CardGame
{
    public class LifeTotal : GameComponent
    {
        int life;
        int borderOffset = GraphicsSettings.toResolution(40);
        public LifeTotal()
        {
            setContentName("lifeCounterImage");
        }
        public void updateLifeValue(int life)
        {

            this.life = life;
        }
        public override void drawSprite(SpriteBatch spriteBatch)
        {
            int yPos = 0;
            base.drawSprite(spriteBatch);
            if(properties.spriteEffects == SpriteEffects.None)
            {
                yPos = (int)getPosition().Y + borderOffset * 2;
            }
            else
            {
                yPos = (int)getPosition().Y + getHeight() - borderOffset * 3;
            }
            spriteBatch.DrawString(Game1.spritefont, life.ToString(), new Vector2(getPosition().X + getWidth()/2 - borderOffset, yPos), Color.Black, 0, new Vector2(0, 0), 1.33f * getScale(), SpriteEffects.None, 0);
        }
    }
    public class BoardTextures
    {
        public int enemy = 0;
        public int friendly = 1;
        Board board;
        public BoardTextures(Board board)
        {
            this.board = board;
        }

        public void initTextures(ContentManager content)
        {
            setRowTextures(content);
            setRowPositions();
            setHolderTextures(content);
            setHolderPositions();
            setPortraitWidgetTextures(content);
            setPortraitWidgetPositions();
            setHandSpaceTextures(content);
            setHandSpacePositions();
            setTokens(content);
            setLifeTotalTexture(content);
            setLifeTotalPositions();
        }
        private void setLifeTotalTexture(ContentManager content)
        {
            board.lifeTotal.Add(new LifeTotal());
            board.lifeTotal.Add(new LifeTotal());
            foreach(LifeTotal total in board.lifeTotal)
            {
                total.setTexture(content);
            }
        }
        private void setLifeTotalPositions()
        {
            int startingPosX = (int)board.rows[5].getPosition().X + board.rows[5].getWidth() + GraphicsSettings.toResolution(50);
            int yPos = 0;
            board.lifeTotal[enemy].setPos(startingPosX, yPos);
            board.lifeTotal[enemy].properties.spriteEffects = SpriteEffects.FlipVertically;

            yPos = Game1.windowH - board.lifeTotal[friendly].getHeight();
            board.lifeTotal[friendly].setPos(startingPosX, yPos);
        }
        private void setTokens(ContentManager content)
        {
            int startingPosX = (int)board.rows[5].getPosition().X + board.rows[5].getWidth() + GraphicsSettings.toResolution(50);
            int yPos = (int)board.rows[5].getPosition().Y;
            board.unanimousToken.setSprite(content, "unanimousToken");
            board.unanimousToken.setPos(startingPosX, yPos);

            startingPosX += GraphicsSettings.toResolution(100);
            board.elfToken.setSprite(content, "elfToken");
            board.elfToken.setPos(startingPosX, yPos);

            startingPosX += GraphicsSettings.toResolution(100);
            board.orcToken.setSprite(content, "orcToken");
            board.orcToken.setPos(startingPosX, yPos);

            startingPosX += GraphicsSettings.toResolution(100);
            board.humanToken.setSprite(content, "humanToken");
            board.humanToken.setPos(startingPosX, yPos);
        }
        private void setHandSpaceTextures(ContentManager content)
        {
            board.handSpace.Add(new HandSpace());
            board.handSpace.Add(new HandSpace());
            foreach (HandSpace hand in board.handSpace)
            {
                hand.setTexture(content);
            }
        }
        private void setHandSpacePositions()
        {
            int xPos = GraphicsSettings.toResolution(200);
            int yPos = Game1.windowH - board.handSpace[0].getHeight() / 2;
            board.handSpace[enemy].setPos(xPos, GraphicsSettings.toResolution(-120));
            board.handSpace[enemy].properties.spriteEffects = SpriteEffects.FlipVertically;
            board.handSpace[friendly].setPos(xPos, yPos);
            board.handSpace[friendly].initializeGameComponent();
        }
        private void setPortraitWidgetPositions()
        {
            int yPos = GraphicsSettings.toResolution(100);
            int xPos = Game1.windowW - board.portraitWidgets[enemy].getWidth();
            board.portraitWidgets[enemy].setPos(xPos, yPos);
            board.portraitWidgets[friendly].setPos(xPos, Game1.windowH - yPos - board.portraitWidgets[enemy].getHeight());
        }
        private void setPortraitWidgetTextures(ContentManager content)
        {
            board.portraitWidgets.Add(new PortraitWidget());
            board.portraitWidgets.Add(new PortraitWidget());
            foreach (PortraitWidget widget in board.portraitWidgets)
            {
                widget.setTexture(content);
            }
        }
        private void setHolderPositions()
        {
            int borderOffset = GraphicsSettings.toResolution(60);
            int yOffset = borderOffset * 4;
            int xPos = borderOffset * 2 + board.rows[enemy].getWidth();
            board.deckHolder[enemy].setPos(xPos, yOffset);
            board.deckHolder[friendly].setPos(xPos, Game1.windowH - board.deckHolder[friendly].getHeight() - yOffset);

            int newXPOS = xPos + borderOffset + board.oblivionHolder[0].getWidth();
            //int newYPOS = borderOffset * 4;
            //int newYPOS = board.deckHolder[ene]
            board.oblivionHolder[enemy].setPos(newXPOS, yOffset);
            board.oblivionHolder[friendly].setPos(newXPOS, Game1.windowH - board.deckHolder[friendly].getHeight() - yOffset);
        }
        private void setHolderTextures(ContentManager content)
        {
            board.deckHolder.Add(new StackPlaceholder());
            board.deckHolder.Add(new StackPlaceholder());
            foreach (StackPlaceholder holder in board.deckHolder)
            {
                holder.setTexture(content);
            }

            board.oblivionHolder.Add(new StackPlaceholder());
            board.oblivionHolder.Add(new StackPlaceholder());
            foreach (StackPlaceholder holder in board.oblivionHolder)
            {
                holder.setTexture(content);
            }
        }
        private void setRowTextures(ContentManager content)
        {
            int numberOfRowsOnBoard = 6;
            for (int i = 0; i < numberOfRowsOnBoard; i++)
            {
                board.rows.Add(new Row());
                board.rows[i].setTexture(content);
            }
        }
        private void setRowPositions()
        {
            int changingYPOS = 0;
            int borderOffset = GraphicsSettings.toResolution(50);
            int staticXPOS = GraphicsSettings.toResolution(60);
            for (int i = 0; i < 3; i++)
            {
                changingYPOS = borderOffset + i * board.rows[i].getHeight();
                board.rows[i].setPos(staticXPOS, changingYPOS);
            }
            for (int i = 0; i < 3; i++)
            {
                int selector = 3 + i;
                changingYPOS = Game1.windowH - borderOffset - board.rows[selector].getHeight() - i * board.rows[selector].getHeight();
                board.rows[selector].setPos(staticXPOS, changingYPOS);
            }
        }
    }
}

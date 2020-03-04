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

    
    public enum GameState //LATER
    {
        DrawHand,
        Play,
        Pause,
        Pass
    }


    public class Board : PrimaryComponent
    {
        List<PortraitWidget> portraitWidgets = new List<PortraitWidget>();
        GameComponent background;
        List<Row> rows = new List<Row>();
        List<StackPlaceholder> deckHolder = new List<StackPlaceholder>();
        List<StackPlaceholder> oblivionHolder = new List<StackPlaceholder>();
        public override void initializeGameComponent(ContentManager content)
        {
            oblivionHolder = new List<StackPlaceholder>();
            deckHolder = new List<StackPlaceholder>();
            rows = new List<Row>();
            portraitWidgets = new List<PortraitWidget>();
            background = new GameComponent();
            background.setSprite(content, "board");
            setRowTextures(content);
            setRowPositions();
            setHolderTextures(content);
            setHolderPositions();
            setPortraitWidgetTextures(content);
            setPortraitWidgetPositions();
            switcherButtons = new List<SwitcherButton>();
            switcherButtons.Add(new SwitcherButton(content, new Vector2(0, 0), "exitImage", 1));
            int switcherButtonPosX = Game1.windowW - switcherButtons[0].getWidth();
            switcherButtons[0].setPos(switcherButtonPosX, 0);
                }
        private void setPortraitWidgetPositions()
        {
            int yPos = 100;
            int xPos = Game1.windowW - portraitWidgets[0].getWidth();
            portraitWidgets[0].setPos(xPos, yPos);
            portraitWidgets[1].setPos(xPos, Game1.windowH - yPos - portraitWidgets[0].getHeight());
        }
        private void setPortraitWidgetTextures(ContentManager content)
        {
            portraitWidgets.Add(new PortraitWidget());
            portraitWidgets.Add(new PortraitWidget());
            foreach(PortraitWidget widget in portraitWidgets)
            {
                widget.setTexture(content);
            }
        }
        private void setHolderPositions()
        {
            int borderOffset = 50;
            int xPos = borderOffset * 2 + rows[0].getWidth();
            deckHolder[0].setPos(xPos, borderOffset*2);
            deckHolder[1].setPos(xPos, Game1.windowH - borderOffset*2 - deckHolder[1].getHeight());

            int newXPOS = xPos + borderOffset + oblivionHolder[0].getWidth();
            int newYPOS = borderOffset * 4;
            oblivionHolder[0].setPos(newXPOS, newYPOS);
            oblivionHolder[1].setPos(newXPOS, Game1.windowH - newYPOS - oblivionHolder[1].getHeight());
        }
        private void setHolderTextures(ContentManager content)
        {
            deckHolder.Add(new StackPlaceholder());
            deckHolder.Add(new StackPlaceholder());
            foreach (StackPlaceholder holder in deckHolder)
            {
                holder.setTexture(content);
            }

            oblivionHolder.Add(new StackPlaceholder());
            oblivionHolder.Add(new StackPlaceholder());
            foreach (StackPlaceholder holder in oblivionHolder)
            {
                holder.setTexture(content);
            }
        }
        private void setRowTextures(ContentManager content)
        {
            int numberOfRowsOnBoard = 6;
            for (int i = 0; i < numberOfRowsOnBoard; i++)
            {
                rows.Add(new Row());
                rows[i].setTexture(content);
            }
        }
        private void setRowPositions()
        {
            int changingYPOS = 0;
            int borderOffset = 50;
            int staticXPOS = 60;
            for (int i = 0; i < 3; i++)
            {
                changingYPOS = borderOffset + i * rows[i].getHeight();
                rows[i].setPos(staticXPOS, changingYPOS);
            }
            for (int i = 0; i < 3; i++)
            {
                int selector = rows.Count - 1 - i;
                changingYPOS = Game1.windowH - borderOffset - rows[selector].getHeight() - i * rows[selector].getHeight();
                rows[selector].setPos(staticXPOS, changingYPOS);
            }
        }
        public override void drawSprite(SpriteBatch spriteBatch)
        {
            background.drawSprite(spriteBatch);
            foreach(Row row in rows)
            {
                row.drawSprite(spriteBatch);
            }
            foreach(StackPlaceholder holder in oblivionHolder)
            {
                holder.drawSprite(spriteBatch);
            }
            foreach(StackPlaceholder holder in deckHolder)
            {
                holder.drawSprite(spriteBatch);
            }
            foreach(PortraitWidget widget in portraitWidgets)
            {
                widget.drawSprite(spriteBatch);
            }
            //base.drawSprite(spriteBatch);
        }
        public override void mouseStateLogic(MouseState mouseState, ContentManager content)
        {
            foreach(Row row in rows)
            {
                row.mouseStateLogic(mouseState, content);
            }
        }
    }
    
    
    

}

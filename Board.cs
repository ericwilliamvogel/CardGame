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



   
    public class Board : PrimaryComponent
    {
        List<PortraitWidget> portraitWidgets = new List<PortraitWidget>();
        GameComponent background;
        List<Row> rows = new List<Row>();
        List<StackPlaceholder> deckHolder = new List<StackPlaceholder>();
        List<StackPlaceholder> oblivionHolder = new List<StackPlaceholder>();
        public List<HandSpace> handSpace = new List<HandSpace>();

        GamePlay gameLoop;

        public Side friendlySide;
        public Side enemySide;

        int sideCounter = 0;
        public override void initializeGameComponent(ContentManager content)
        {
            gameLoop = new GamePlay();
            oblivionHolder = new List<StackPlaceholder>();
            deckHolder = new List<StackPlaceholder>();
            rows = new List<Row>();
            portraitWidgets = new List<PortraitWidget>();
            background = new GameComponent();
            handSpace = new List<HandSpace>();
            background.setSprite(content, "board");
            setRowTextures(content);
            setRowPositions();
            setHolderTextures(content);
            setHolderPositions();
            setPortraitWidgetTextures(content);
            setPortraitWidgetPositions();
            setHandSpaceTextures(content);
            setHandSpacePositions();
        
            switcherButtons = new List<SwitcherButton>();
            switcherButtons.Add(new SwitcherButton(content, new Vector2(0, 0), "exitImage", 1));
            int switcherButtonPosX = Game1.windowW - switcherButtons[0].getWidth();
            switcherButtons[0].setPos(switcherButtonPosX, 0);

            //
            //
            //
            CardImageStorage library = new CardImageStorage();
            library.loadCardSupplementalTextures(content);

            Card card1;
            CardBuilder cardBuilder = new CardBuilder();
            CardConstructor cardConstructor = new CardConstructor();

            Deck deck = new Deck();
            Deck TEST = new Deck();
            int deckLimitForTesting = 30;
            int identifierCounter = 0;
            for (int i = 0; i < deckLimitForTesting; i++)
            {
                Card card = cardBuilder.cardConstruct(cardConstructor, identifierCounter);
                card.setSupplementalTextures(library);
                Card car2 = cardBuilder.cardConstruct(cardConstructor, identifierCounter);
                car2.setSupplementalTextures(library);
                TEST.cardsInContainer.Add(car2);
                deck.cardsInContainer.Add(card);
                identifierCounter++;
                if(identifierCounter > 2)
                {
                    identifierCounter = 0;
                }
            }

            library = cardConstructor.tempStorage;
            //library.cardTextureDictionary = new Dictionary<int, Texture2D>();
            library.loadCardSupplementalTextures(content);
            library.loadAllDictionaryTextures(content);

            //

            //
            Player player1 = new Player();
            Player player2 = new Player();

            friendlySide = new Side(player1);
            enemySide = new Side(player2);
            deck.loadCardsInDeck(library.cardTextureDictionary);
            TEST.loadCardsInDeck(library.cardTextureDictionary);
            friendlySide.Deck = deck;
            enemySide.Deck = TEST;
            setSide(enemySide);
            setSide(friendlySide);





            //gameLoop.initializeGameComponent(content);
            gameLoop.StartGame(this);
        }
        int multiplier = 0;
        private void updateHandPosition()
        {
            friendlySide.Hand.setPos(handSpace[friendly].getPosition());
        }
        private void setSide(Side side)
        {
            side.Oblivion.setValuesToImage(oblivionHolder[sideCounter]);
            side.Deck.setValuesToImage(deckHolder[sideCounter]);
            side.Rows[Side.General].setValuesToImage(rows[multiplier]);
            side.Rows[Side.Armies].setValuesToImage(rows[multiplier + 1]);
            side.Rows[Side.FieldUnit].setValuesToImage(rows[multiplier + 2]);
            side.Hand.setValuesToImage(handSpace[sideCounter]);

            sideCounter++;
            multiplier += 3;
            if(sideCounter > 1)
            {
                sideCounter = 0;
                multiplier = 0;
            }
            //throw new Exception(side.Deck.POS.ToString());
        }
        int enemy = 0;
        int friendly = 1;
        private void setHandSpaceTextures(ContentManager content)
        {
            handSpace.Add(new HandSpace());
            handSpace.Add(new HandSpace());
            foreach(HandSpace hand in handSpace)
            {
                hand.setTexture(content);
            }
        }
        private void setHandSpacePositions()
        {
            int xPos = 200;
            int yPos = Game1.windowH - handSpace[0].getHeight() / 4;
            handSpace[0].setPos(xPos, 0);
            handSpace[0].properties.spriteEffects = SpriteEffects.FlipVertically;
            handSpace[1].setPos(xPos, yPos);
            handSpace[1].initializeGameComponent();
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
            foreach(HandSpace hand in handSpace)
            {
                //hand.drawSprite(spriteBatch);
            }
            gameLoop.drawSprite(spriteBatch);
        }
        bool pressed;
        public override void mouseStateLogic(MouseState mouseState, ContentManager content)
        {
            foreach(Row row in rows)
            {
                row.mouseStateLogic(mouseState, content);
            }
            handSpace[friendly].mouseStateLogic(mouseState, content);
            gameLoop.mouseStateLogic(mouseState, content);
            
            ////
            if(mouseState.MiddleButton == ButtonState.Pressed && pressed == false)
            {
                gameLoop.DrawCard(friendlySide);
                gameLoop.DrawCard(enemySide);
                pressed = true;
            }
            if(mouseState.MiddleButton == ButtonState.Released)
            {
                pressed = false;
            }
        }
        public override void updateGameComponent(ContentManager content)
        {
            gameLoop.Update(this);
            updateHandPosition();
        }
    }
    
    
    

}

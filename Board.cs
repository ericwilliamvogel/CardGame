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



    public class Token : GameComponent
    {
        public int resources;
        public Card.Race race;
        public Token(Card.Race race)
        {
            this.race = race;
        }
        public override void drawSprite(SpriteBatch spriteBatch)
        {

            base.drawSprite(spriteBatch);
            spriteBatch.DrawString(Game1.spritefont, resources.ToString(), new Vector2(getPosition().X + getWidth()/2 - 10 * getScale().X, getPosition().Y + getHeight() / 2 - 20 * getScale().X), Color.Black, 0, new Vector2(0, 0), getScale(), SpriteEffects.None, 0);
        }
        public void adjustResourceValue(Side side)
        {
            int x = 0;
            foreach(Card.Race resource in side.Resources)
            {
                if(resource == race)
                {
                    x++;
                }
            }

            resources = x;
        }
    }
    public class Board : PrimaryComponent
    {
        public List<PortraitWidget> portraitWidgets = new List<PortraitWidget>();
        GameComponent background;
        public List<Row> rows = new List<Row>();
        public List<StackPlaceholder> deckHolder = new List<StackPlaceholder>();
        public List<StackPlaceholder> oblivionHolder = new List<StackPlaceholder>();
        public List<HandSpace> handSpace = new List<HandSpace>();
        public Token unanimousToken;
        public Token elfToken;
        public Token orcToken;
        public Token humanToken;

        //BoardFunctionality gameLoop;

        public Side friendlySide;
        public Side enemySide;

        int sideCounter = 0;
        BoardTextures textures;
        Button button;
        public Side controllingPlayer;

        public override void initializeGameComponent(ContentManager content)
        {

            unanimousToken = new Token(Card.Race.Unanimous);
            elfToken = new Token(Card.Race.Elf);
            orcToken = new Token(Card.Race.Orc);
            humanToken = new Token(Card.Race.Human);
            //gameLoop = new BoardFunctionality();
            oblivionHolder = new List<StackPlaceholder>();
            deckHolder = new List<StackPlaceholder>();
            rows = new List<Row>();
            portraitWidgets = new List<PortraitWidget>();
            background = new GameComponent();
            handSpace = new List<HandSpace>();
            background.setSprite(content, "board");
            textures = new BoardTextures(this);
            textures.initTextures(content);

            button = new Button(content, new Vector2(Game1.windowW - 100, Game1.windowH / 2 + 100), "secondButtonTexture");
            button.setPos(new Vector2(Game1.windowW - 100 - button.getWidth(), Game1.windowH / 2 + 100));
            button.setAction(() => { friendlySide.boardFunc.PassTurn(); });


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
            int deckLimitForTesting = 100;
            int identifierCounter = 0;
            for (int i = 0; i < deckLimitForTesting; i++)
            {
                /*if (identifierCounter == 0)
                {
                    identifierCounter = 4;
                }
                else
                    identifierCounter = 0;
*/
                Card card = cardBuilder.cardConstruct(cardConstructor,identifierCounter);
                card.setSupplementalTextures(library);
                Card car2 = cardBuilder.cardConstruct(cardConstructor,identifierCounter);
                car2.setSupplementalTextures(library);
                TEST.cardsInContainer.Add(car2);
                deck.cardsInContainer.Add(card);
                identifierCounter++;
                if(identifierCounter > 4)
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
            Player player1 = new ActivePlayer();
            Player player2 = new AIPlayer();

            friendlySide = new Side(player1);
            enemySide = new Side(player2);
            deck.loadCardsInDeck(library.cardTextureDictionary);
            TEST.loadCardsInDeck(library.cardTextureDictionary);
            friendlySide.Deck = deck;
            enemySide.Deck = TEST;

            setSide(enemySide);
            setSide(friendlySide);

            enemySide.boardFunc.initSide(this, enemySide);
            friendlySide.boardFunc.initSide(this, friendlySide);
            friendlySide.boardFunc.initializeGameComponent(content);



            //gameLoop.initializeGameComponent(content);
            friendlySide.boardFunc.StartGame(this, friendlySide);
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
            side.boardFunc.initSide(this, side);
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
            unanimousToken.drawSprite(spriteBatch);
            elfToken.drawSprite(spriteBatch);
            orcToken.drawSprite(spriteBatch);
            humanToken.drawSprite(spriteBatch);

            button.drawSprite(spriteBatch);
            friendlySide.boardFunc.drawSprite(spriteBatch);
        }
        bool pressed;
        public override void mouseStateLogic(MouseState mouseState, ContentManager content)
        {
            setContainerPlayState(friendlySide.Hand, Card.PlayState.Revealed);
            setContainerPlayState(enemySide.Hand, Card.PlayState.Hidden);
            foreach (Row row in rows)
            {
                row.mouseStateLogic(mouseState, content);
            }
            //
            //
            //handSpace[friendly].mouseStateLogic(mouseState, content);
            
            friendlySide.boardFunc.mouseStateLogic(mouseState, content);
            enemySide.boardFunc.mouseStateLogic(mouseState, content);
            ////
            if(mouseState.MiddleButton == ButtonState.Pressed && pressed == false)
            {
                friendlySide.boardFunc.DrawHand(friendlySide);
                friendlySide.boardFunc.DrawHand(enemySide);
                pressed = true;
            }
            if(mouseState.MiddleButton == ButtonState.Released)
            {
                pressed = false; 
            }
            button.mouseStateLogic(mouseState, content);
        }

        private void setContainerPlayState(CardContainer container, Card.PlayState playState)
        {
            foreach (Card card in container.cardsInContainer)
            {
                card.playState = playState;
            }
        }
        public override void updateGameComponent(ContentManager content)
        {
            friendlySide.boardFunc.Update(this);
            enemySide.boardFunc.Update(this);
            updateHandPosition();
            unanimousToken.adjustResourceValue(friendlySide);
            elfToken.adjustResourceValue(friendlySide);
            orcToken.adjustResourceValue(friendlySide);
            humanToken.adjustResourceValue(friendlySide);
        }

        public int enemy = 0;
        public int friendly = 1;
    }
    
    
    

}

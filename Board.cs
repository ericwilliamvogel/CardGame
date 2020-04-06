using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CardGame
{



    public class Board : PrimaryComponent
    {
        public List<PortraitWidget> portraitWidgets = new List<PortraitWidget>();
        GameComponent background;
        public List<Row> rows = new List<Row>();
        public List<StackPlaceholder> deckHolder = new List<StackPlaceholder>();
        public List<StackPlaceholder> oblivionHolder = new List<StackPlaceholder>();
        public List<HandSpace> handSpace = new List<HandSpace>();
        public List<LifeTotal> lifeTotal = new List<LifeTotal>();
        public Token unanimousToken;
        public Token elfToken;
        public Token orcToken;
        public Token humanToken;

        public Side friendlySide;
        public Side enemySide;
        int multiplier = 0;
        int sideCounter = 0;
        BoardTextures textures;
        Button button;
        public Side controllingPlayer;
        CardBuilder cardBuilder = new CardBuilder();
        CardConstructor cardConstructor = new CardConstructor();
        DeckBuilder deckBuilder = new DeckBuilder();
        DeckConstructor deckConstructor = new DeckConstructor();
        CardImageStorage library = new CardImageStorage();
        public BoardActions boardActions = new BoardActions();
        Player player1;
        Player player2;
        public MoveHistory moveHistory = new MoveHistory();
        public override void initializeGameComponent(ContentManager content)
        {
            //moveHistory = new MoveHistory();
            imageAndFunctionsSetup(content);
            loadBuildersAndConstructors();

            /**********************/

            Deck playerDeck = new Deck();
            Deck aiDeck = new Deck();
            playerDeck = deckBuilder.getDeck(deckConstructor, "TESTDECK");
            aiDeck = deckBuilder.getDeck(deckConstructor, "TESTDECK");

            /*********************/


            loadLibraryAssets(content);
            loadPlayers();

            friendlySide = new Side(player1);
            enemySide = new Side(player2);
            playerDeck.loadCardImagesInContainer(library);
            aiDeck.loadCardImagesInContainer(library);
            friendlySide.Deck = playerDeck;
            enemySide.Deck = aiDeck;

            setSide(enemySide);
            setSide(friendlySide);

            friendlySide.boardFunc.passDown(library, deckConstructor.cardConstructor);
            enemySide.boardFunc.passDown(library, deckConstructor.cardConstructor);

            friendlySide.boardFunc.initializeGameComponent(content);
            enemySide.boardFunc.initializeGameComponent(content);

            friendlySide.boardFunc.StartGame(this, friendlySide);
        }

        private void loadBuildersAndConstructors()
        {
            cardBuilder = new CardBuilder();
            cardConstructor = new CardConstructor();
            deckBuilder = new DeckBuilder();
            deckConstructor = new DeckConstructor();

        }
        private void loadPlayers()
        {
            player1 = new ActivePlayer();
            player2 = new AIPlayer();
        }
        private void loadLibraryAssets(ContentManager content)
        {



            library = new CardImageStorage();
            library = deckConstructor.cardConstructor.tempStorage;
            library.loadCardSupplementalTextures(content);
            library.loadAllDictionaryTextures(content);
        }
        public void imageAndFunctionsSetup(ContentManager content)
        {
            initAllComponents();
            background.setSprite(content, "board");
            textures = new BoardTextures(this);
            textures.initTextures(content);
            initButtons(content);
        }
        private void initAllComponents()
        {
            unanimousToken = new Token(Card.Race.Unanimous);
            elfToken = new Token(Card.Race.Elf);
            orcToken = new Token(Card.Race.Orc);
            humanToken = new Token(Card.Race.Human);
            lifeTotal = new List<LifeTotal>();
            oblivionHolder = new List<StackPlaceholder>();
            deckHolder = new List<StackPlaceholder>();
            rows = new List<Row>();
            portraitWidgets = new List<PortraitWidget>();
            background = new GameComponent();
            handSpace = new List<HandSpace>();
        }
        private void initButtons(ContentManager content)
        {
            int offSet = GraphicsSettings.toResolution(100);
            button = new Button(content, new Vector2(Game1.windowW - offSet, Game1.windowH / 2 + offSet), "secondButtonTexture");
            button.setPos(new Vector2(Game1.windowW - offSet - button.getWidth(), Game1.windowH / 2 + offSet));
            button.setButtonText("END");
            button.setAction(() => { friendlySide.boardFunc.PassTurn(); });

            switcherButtons = new List<SwitcherButton>();
            switcherButtons.Add(new SwitcherButton(content, new Vector2(0, 0), "exitImage", 1));
            int switcherButtonPosX = Game1.windowW - switcherButtons[0].getWidth();
            switcherButtons[0].setPos(switcherButtonPosX, 0);
        }
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
            side.Life.setValuesToImage(lifeTotal[sideCounter]);

            sideCounter++;
            multiplier += 3;
            if(sideCounter > 1)
            {
                sideCounter = 0;
                multiplier = 0;
            }
            side.boardFunc.sideSetter.initSide(this, side, side.boardFunc);
        }
        
        public enum Mode
        {
            Normal,
            Developer
        }
        public void showDeveloperTools(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Game1.spritefont, "Selection count: "+ friendlySide.boardFunc.cardViewer.selection.Count.ToString(), new Vector2(0, 100), Color.Black);
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

            foreach(LifeTotal total in lifeTotal)
            {
                total.drawSprite(spriteBatch);
            }
            button.drawSprite(spriteBatch);
            friendlySide.boardFunc.drawSprite(spriteBatch);
            //moveHistory.drawSprite(spriteBatch);
        }
        bool pressed;
        bool handHidden;
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


                KeyboardState state = Keyboard.GetState();
                if (state.IsKeyDown(Keys.X) && handHidden == false)
                {
                if (!friendlySide.boardFunc.handFunction.placingCard)
                {
                    friendlySide.boardFunc.handFunction.placingCard = true;
                }
                else if (friendlySide.boardFunc.handFunction.placingCard)
                    friendlySide.boardFunc.handFunction.placingCard = false;

                handHidden = true;
                }
                if (state.IsKeyUp(Keys.X))
                {

                    handHidden = false;
                }
            
            //handSpace[friendly].mouseStateLogic(mouseState, content);*/
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


            lifeTotal[enemy].updateLifeValue(enemySide.LifeTotal);
            lifeTotal[friendly].updateLifeValue(friendlySide.LifeTotal);
            boardActions.updateAnimations();
            //moveHistory.updateGameComponent();
        }

        public int enemy = 0;
        public int friendly = 1;
    }
    
    
    

}

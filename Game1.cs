using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CardGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static SpriteFont spritefont;
        public static int windowH, windowW;
        public static Vector2 windowParams;
        private ComponentManager componentManager;
        public MouseState mouseState;
        public Game1()
        {

            mouseState = new MouseState();
            componentManager = new ComponentManager(this.Content);
            windowParams = new Vector2((int)windowW, (int)windowH);
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            componentManager.adjustSettingsOnStartup(graphics);

            //windowH = (int)GraphicsSettings.resolutions[GraphicsSettings.currentResolution].Y;
            //windowW = (int)GraphicsSettings.resolutions[GraphicsSettings.currentResolution].X;
            this.IsMouseVisible = true;
            //battlefield = new Battlefield(this.Content);
            //throw new Exception(windowH.ToString());
        }

        public void Close()
        {
            Exit();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {

            componentManager.initializeGameComponent(this.Content);

            componentManager.assignExitLogic(this);
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            try
            {
                spritefont = Content.Load<SpriteFont>("File");
            }
            catch (Exception e)
            {
                Console.Write(e);
            }

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            componentManager.unloadGameComponent();
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if(componentManager.controller == 0)
            {
                componentManager.giveSettingsMenuPermToModifyGraphics(graphics);
            }
            mouseState = Mouse.GetState();

            // TODO: Add your update logic here

            componentManager.updateGameComponent(this.Content);
            componentManager.mouseStateLogic(mouseState, this.Content);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            GraphicsDevice.Clear(Color.CornflowerBlue);
            componentManager.drawSprite(spriteBatch);
            // TODO: Add your drawing code here
            spriteBatch.DrawString(spritefont, mouseState.X.ToString(), new Vector2(0, 0), Color.Red);
            spriteBatch.DrawString(spritefont, mouseState.Y.ToString(), new Vector2(30, 0), Color.Blue);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }

}
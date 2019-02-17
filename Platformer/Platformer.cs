using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Global {
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Platformer : Game {

        #region private attributes
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        KeyboardState oldKeyboardState;
        KeyboardState keyboardState;
        ContentOrganizer content;
        FrameCounter frameCounter;
        World world;
        Random rnd;
        float dt;

        //AnimatedSprite animatedSprite;

        string[] texturesToLoad;
        string[] fontsToLoad;
        #endregion

        #region public attributes
        public double time = 0;
        public bool isPaused = false;
        #endregion

        #region constructor
        public Platformer() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        #endregion

        #region initialization
        protected override void Initialize() {
            content = new ContentOrganizer(GraphicsDevice);
            //Prepare what texture and fonts will be loaded
            texturesToLoad = new string[]{
                "collines1", "collines2", "collines3", "collines4", "collines5", "collines6",
                "player",
                "dirt"
            };
            fontsToLoad = new string[]{
                "arial"
            };

            IsMouseVisible = true;

            rnd = new Random();

            //Window size
            graphics.PreferredBackBufferWidth = 1200;
            graphics.PreferredBackBufferHeight = 800;
            graphics.ApplyChanges();

            //FPS counter
            frameCounter = new FrameCounter();

            //Create world
            world = new World(Window.ClientBounds, 40, 18, rnd);

            int visibleBlocsHorizontaly = world.wdowDimensions.Width / Bloc.SIZE + 1;
            int visibleBlocsVerticaly = world.wdowDimensions.Height / Bloc.SIZE + 1;

            world.debugGrid = Tools.GenerateGridTexture2D(GraphicsDevice, visibleBlocsHorizontaly, visibleBlocsVerticaly, Bloc.SIZE, Color.White);
            world.debugRectangleBackground = Tools.GenerateFilledRectangleTexture2D(GraphicsDevice, 200, 130, Color.White);

            base.Initialize();
        }
        #endregion

        #region content
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Load all content
            foreach (string textureString in texturesToLoad) {
                content.textures.Add(textureString, Content.Load<Texture2D>(textureString));
            }

            //Add rectangle texture for the world
            content.textures.Add("worldBounds", Tools.GenerateRectangleTexture2D(GraphicsDevice, (int)world.width, (int)world.height, 2, Color.White));

            //Load all fonts
            foreach (string fontString in fontsToLoad) {
                content.fonts.Add(fontString, Content.Load<SpriteFont>(fontString));
            }

            world.LoadContent(content);

            //animatedSprite = new AnimatedSprite(Content.Load<Texture2D>("explosion"), 17, 64, 64, 6, 3);
        }

        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
        }
        #endregion

        #region update
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            //Get the time between frame
            dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Get keyboard state
            keyboardState = Keyboard.GetState();

            Controls(dt);

            if (!isPaused) {
                //Increment time
                time += gameTime.ElapsedGameTime.TotalSeconds;

                world.Update(dt, keyboardState);
            }

            oldKeyboardState = keyboardState;

            //Do monogame stuff related to update
            base.Update(gameTime);
        }
        #endregion

        #region draw
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            //Set background color
            GraphicsDevice.Clear(Color.Black);

            //Draw in screen
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);

            world.Draw(spriteBatch);

            //FPS
            frameCounter.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            string stringFPS = string.Format("FPS:{0}", Math.Round(frameCounter.AverageFramesPerSecond).ToString());
            Vector2 fpsPosition = new Vector2(Window.ClientBounds.Width - content.fonts["arial"].MeasureString(stringFPS).X, 0);
            spriteBatch.Draw(content.textures["rect," + (int)content.fonts["arial"].MeasureString(stringFPS).X + "," + (int)content.fonts["arial"].MeasureString(stringFPS).Y],fpsPosition, Color.White);
            spriteBatch.DrawString(content.fonts["arial"], stringFPS, fpsPosition, Color.Black);

            spriteBatch.End();

            base.Draw(gameTime);
        }
        #endregion

        #region controls
        void Controls(float dt) {
            //Press escape to quit the game
            if (keyboardState.IsKeyDown(Keys.Escape)) Exit();

            //display debug
            if (keyboardState.IsKeyDown(Keys.G) && oldKeyboardState.IsKeyUp(Keys.G))
                world.displayDebug = !world.displayDebug;

            //Pause
            if (keyboardState.IsKeyDown(Keys.P) && oldKeyboardState.IsKeyUp(Keys.P))
                isPaused = !isPaused;

            //TEMP Move camera
            float speedMovementWorld = 200;
            if (keyboardState.IsKeyDown(Keys.Up)) world.y += speedMovementWorld*dt;
            if (keyboardState.IsKeyDown(Keys.Down)) world.y -= speedMovementWorld*dt;
            if (keyboardState.IsKeyDown(Keys.Left)) world.x += speedMovementWorld*dt;
            if (keyboardState.IsKeyDown(Keys.Right)) world.x -= speedMovementWorld*dt;
        }
        #endregion
    }
}

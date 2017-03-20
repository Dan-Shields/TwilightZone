using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TwilightZone
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class TwilightZone : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Settings file name
        protected const string SETTINGS_FILE = "settings.txt";

        // Textures
        private Texture2D background;
        private Texture2D spaceship;
        private Texture2D laser1;
        private Texture2D metal;
        private Texture2D asteroid;
        
        //Objects
        Ship playerShip = new Ship();
        List<Laser> laserList = new List<Laser>();
        List<Asteroid> asteroidList = new List<Asteroid>();

        //general class properties
        private int timeSinceLastLaser = 0;
        private int timeSinceLastAsteroid = 360;
        public static int screenWidth;
        public static int screenHeight;
        public static int virtualWidth = 1280;
        public static int virtualHeight = 720;

        public Random random = new Random();

        public TwilightZone()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            screenHeight = GraphicsDevice.DisplayMode.Height;
            screenWidth = GraphicsDevice.DisplayMode.Width;

            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.IsFullScreen = true;
            graphics.ApplyChanges();

            Mouse.SetPosition(screenWidth / 2, screenWidth /2);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            background = Content.Load<Texture2D>("starry_background");
            spaceship = Content.Load<Texture2D>("ship_1");
            laser1 = Content.Load<Texture2D>("laser_1");
            metal = Content.Load<Texture2D>("metal_tile");
            asteroid = Content.Load<Texture2D>("asteroid");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardState keyboardState = Keyboard.GetState();

            playerShip.UpdatePosition(keyboardState);
            
            timeSinceLastLaser++;

            //Update position of all current laser objects
            foreach (Laser laserObj in laserList)
            {
                laserObj.Tick();
            }

            //Create new laser if LMB is pressed
            if (keyboardState.IsKeyDown(Keys.Space))
            {
                if (timeSinceLastLaser > 8)
                {
                    Point laserPosition = new Point(playerShip.currentPosition.X + 20, playerShip.currentPosition.Y + 5);
                    laserList.Add(new Laser(laserPosition));
                    timeSinceLastLaser = 0;
                }
            }

            timeSinceLastAsteroid++;

            foreach (Asteroid asteroidObj in asteroidList)
            {
                asteroidObj.Tick();
            }

            //Create new laser if LMB is pressed
            if (random.Next(0, 100) == 1)
            { 
                Point asteroidPosition = new Point(random.Next(350, virtualWidth - 400), -50);
                asteroidList.Add(new Asteroid(asteroidPosition));
                timeSinceLastAsteroid = 0;
            }
            
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            var scaleX = (float)screenWidth / virtualWidth;
            var scaleY = (float)screenHeight / virtualHeight;
            var matrix = Matrix.CreateScale(scaleX, scaleY, 1.0f);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.LinearWrap, transformMatrix: matrix);
            const int rows = 3;
            const int columns = 2;

            //make this once per frame

            //frametimer == 1 when our game runs at 60 hz
            float frametimer = (float)(gameTime.TotalGameTime.TotalMilliseconds * 60 / 1000);

            const float scrollDirectionY = -2;

            int starty = (int)(scrollDirectionY * frametimer);

            Rectangle backgroundReadRectangle = new Rectangle(0,
                starty,
                background.Width * columns,
                background.Height * rows);


            spriteBatch.Draw(background, new Rectangle(350, 0, virtualWidth - 700, virtualHeight), backgroundReadRectangle, Color.White);

            foreach (Laser laserObj in laserList)
            {
                spriteBatch.Draw(laser1, new Rectangle(laserObj.currentPosition.X, laserObj.currentPosition.Y, 10,15), Color.White);
            }

            foreach (Asteroid asteroidObj in asteroidList)
            {
                spriteBatch.Draw(asteroid, new Rectangle(asteroidObj.currentPosition.X, asteroidObj.currentPosition.Y, 50, 50), Color.White);
            }

            spriteBatch.Draw(spaceship, new Rectangle(playerShip.currentPosition.X, playerShip.currentPosition.Y, 50, 50), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

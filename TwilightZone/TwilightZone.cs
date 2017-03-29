using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TwilightZone
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class TwilightZone : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        //Settings file name
        protected const string SettingsFile = "settings.txt";

        // Textures
        private Texture2D background;
        private Texture2D spaceship;
        private Texture2D laser1;
        private Texture2D asteroid;
        
        //Sounds
        private SoundEffect pewpew;
        
        //Objects
        Background backgroundObj = new Background();
        Ship shipObj = new Ship();
        List<Laser> laserList = new List<Laser>();
        List<Asteroid> asteroidList = new List<Asteroid>();

        //general class properties
        private int timeSinceLastLaser = 8;
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
            asteroid = Content.Load<Texture2D>("asteroid");

            pewpew = Content.Load<SoundEffect>("pewpew");
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

            shipObj.Update(keyboardState);
            
            timeSinceLastLaser++;

            //Update position of all current laser objects
            foreach (Laser laserObj in laserList)
            {
                laserObj.Update();

                //laser cleanup
                var item = laserList.SingleOrDefault(x => x.currentPosition.X < -1 * x.hitbox.Height);
                if (item != null)
                    laserList.Remove(item);
                
            }

            //Create new laser if space is pressed
            if (keyboardState.IsKeyDown(Keys.Space) && shipObj.timeSinceDamage > 60)
            {
                if (timeSinceLastLaser > 8)
                {
                    Point laserPosition = new Point(shipObj.currentPosition.X + 20, shipObj.currentPosition.Y + 5);
                    laserList.Add(new Laser(laserPosition));
                    timeSinceLastLaser = 0;
                    pewpew.Play();
                }
            }

            timeSinceLastAsteroid++;

            foreach (Asteroid asteroidObj in asteroidList)
            {
                asteroidObj.Update();

                //asteroid cleanup
                var item = asteroidList.SingleOrDefault(x => x.currentPosition.X > screenHeight);
                if (item != null)
                    asteroidList.Remove(item);

                //Test for collision with player
                if (asteroidObj.hitbox.Intersects(shipObj.hitbox))
                {
                    shipObj.SustainDamage(10);
                }

                for (var i = 0; i < laserList.Count(); i++)
                {
                    var test = laserList.SingleOrDefault(x => asteroidObj.hitbox.Intersects(x.hitbox));
                    if (test != null)
                        laserList.Remove(test);
                }
                
            }

            //Create new asteroids
            if (random.Next(0, 100) == 1 && timeSinceLastAsteroid > 360)
            { 
                Point newAsteroidPosition = new Point(random.Next(350, virtualWidth - 400), -50);
                asteroidList.Add(new Asteroid(newAsteroidPosition, random.Next(20, 50)));
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
            

            //draw background
            spriteBatch.Draw(background, new Rectangle(350, 0, virtualWidth - 700, virtualHeight), backgroundObj.ReadRectangle(background, gameTime), Color.White);

            //draw all lasers
            foreach (Laser laserObj in laserList)
            {
                spriteBatch.Draw(laser1, laserObj.hitbox, Color.White);
            }

            //draw all asteroids
            foreach (Asteroid asteroidObj in asteroidList)
            {
                spriteBatch.Draw(asteroid, new Rectangle(asteroidObj.hitbox.X - (asteroidObj.asteroidSize/2), asteroidObj.hitbox.Y - (asteroidObj.asteroidSize / 2), asteroidObj.hitbox.Radius * 2, asteroidObj.hitbox.Radius * 2), Color.White);
            }

            //draw ship
            Color maskColor;
            if (shipObj.timeSinceDamage < 60 && (shipObj.timeSinceDamage / 10) % 2 == 0)
            {
                maskColor = Color.Red;
            }
            else
            {
                maskColor = Color.White;
            }
            
            spriteBatch.Draw(spaceship, shipObj.hitbox, maskColor);

            //draw health bar
            //spriteBatch.Draw();
            Texture2D healthBar = new Texture2D(graphics.GraphicsDevice, shipObj.health + 1, 20);
            Color[] data = new Color[(shipObj.health + 1) * 20];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.Red;
            healthBar.SetData(data);

            Vector2 coor = new Vector2(40, virtualHeight - 40);
            spriteBatch.Draw(healthBar, coor, Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        Texture2D createCircleText(int radius)
        {
            Texture2D texture = new Texture2D(GraphicsDevice, radius, radius);
            Color[] colorData = new Color[radius * radius];

            float diam = radius / 2f;
            float diamsq = diam * diam;

            for (int x = 0; x < radius; x++)
            {
                for (int y = 0; y < radius; y++)
                {
                    int index = x * radius + y;
                    Vector2 pos = new Vector2(x - diam, y - diam);
                    if (pos.LengthSquared() <= diamsq)
                    {
                        colorData[index] = Color.White;
                    }
                    else
                    {
                        colorData[index] = Color.Transparent;
                    }
                }
            }

            texture.SetData(colorData);
            return texture;
        }
    }




}

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;

namespace CreatureGame
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Creature test;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            short temporary = 2345;
            this.IsMouseVisible = true;
            test = new Creature(temporary);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.OldLace);

            base.Draw(gameTime);
        }

        private void ReadConfig()
        {
            try
            {
                TextReader tr = new StreamReader(Directory.GetCurrentDirectory() + "\\config.txt");
                string fScreen = tr.ReadLine();
                string[] split = fScreen.Split('=');
                if (split[1] == "true")
                    graphics.IsFullScreen = true;
                else if (split[1] == "false")
                    graphics.IsFullScreen = false;
                else
                {
                    graphics.IsFullScreen = false;
                    DebugLog.WriteLine("Error on fullscreen line of config.txt.");
                }

                if (graphics.IsFullScreen == false)
                {
                    string screenWidth = tr.ReadLine();
                    split = screenWidth.Split('=');
                    graphics.PreferredBackBufferWidth = Convert.ToInt32(split[1]);
                    string screenHeight = tr.ReadLine();
                    split = screenHeight.Split('=');
                    graphics.PreferredBackBufferHeight = Convert.ToInt32(split[1]);
                }
            }
            catch (FileNotFoundException e)
            {
                DebugLog.WriteLine("Could not find config file.");
            }
            catch (Exception e)
            {
                DebugLog.WriteLine("Error reading config file. Message: " + e.Message);
            }

        }
    }
}
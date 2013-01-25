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

        Texture2D handlerTex;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            DebugLog.Init();
           
            this.IsMouseVisible = true;
            ReadConfig();
            base.Initialize();

            HandlerEntity handler = new HandlerEntity(this);

            Components.Add(new InputHandler(this));
            Components.Add(new SpriteBatchComponent(this));
            Components.Add(new Viewport(this));
            Components.Add(handler);

            handler.Enabled = true;
            HandlerEntity.TileMap = new TileMap("TestMap.txt", graphics.GraphicsDevice);
            HandlerEntity.player = new Creature(2345, handlerTex, HandlerEntity.TileMap.PlayerSpawn, 1f);
        }

        protected override void LoadContent()
        {
            handlerTex = Content.Load<Texture2D>("handler");
        }

        protected override void UnloadContent()
        {
            DebugLog.Close();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            if (InputHandler.KeyPressed(Keys.Escape))
                this.Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.OldLace);
            base.Draw(gameTime);
        }

        private void ReadConfig()
        {
            try
            {
                TextReader tr = new StreamReader(Directory.GetCurrentDirectory() + "//config.txt");
                string fScreen = tr.ReadLine();
                string[] split = fScreen.Split('=');
                if (split[1] == "true")
                {
                    graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                    graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                    graphics.ApplyChanges();
                    graphics.ToggleFullScreen();
                }
                else if (split[1] == "false")
                {
                    if (graphics.IsFullScreen == true)
                        graphics.ToggleFullScreen();
                }
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
                    graphics.ApplyChanges();
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
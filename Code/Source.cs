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
using VOiD.Components;

namespace VOiD
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public static LevelEditor LevelEditor;

        public Game1()
        {
            this.IsMouseVisible = true;
            Content.RootDirectory = "Content";
            Components.Add(new Configuration(this));//Very Important DO NOT MOVE
        }

        protected override void Initialize()
        {
            Components.Add(new InputHandler(this));
            Components.Add(new SpriteManager(this));
            Components.Add(new DebugLog(this));
            Components.Add(new GameHandler(this));
            Components.Add(new Interface(this));
            Components.Add(new BattleHandler(this));

            Audio.Initialize();
            Audio.Play("IntroMusic");

            LevelEditor = new LevelEditor(this); 
            Components.Add(LevelEditor);
            LevelEditor.Enabled = false;
            LevelEditor.Visible = false;

            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            if ((GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) | (InputHandler.KeyPressed(Keys.Escape)))
                this.Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Interface.BackgroundColor);
            base.Draw(gameTime);
        }
        
        protected override void UnloadContent()
        {
            // Delete temporary minimap file from folder.
            File.Delete("~minimap.png");
            Audio.StopAll();
            base.LoadContent();
        }
    }
}
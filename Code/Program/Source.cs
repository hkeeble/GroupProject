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
        public Game1()
        {
            this.IsMouseVisible = true;
            Content.RootDirectory = "Content";
            Components.Add(new Configuration(this));//Very Important DO NOT MOVE
        }

        protected override void Initialize()
        {
            Components.Add(new InputHandler(this));
            Components.Add(new SpriteBatchComponent(this));
            Components.Add(new DebugLog(this));
            Components.Add(new GameHandler(this));
            Components.Add(new Interface(this));
            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            if ((GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) | (InputHandler.KeyPressed(Keys.Escape)))
                this.Exit();

            if (InputHandler.KeyPressed(Keys.W))
                Interface.currentScreen=Screens.BLANK;
            if (InputHandler.KeyPressed(Keys.A))
                Interface.currentScreen = Screens.Intro;
            if (InputHandler.KeyPressed(Keys.S))
                Interface.currentScreen = Screens.MainMenu;
            if (InputHandler.KeyPressed(Keys.D))
                Interface.currentScreen = Screens.LevelMenu;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Interface.BackgroundColor);
            base.Draw(gameTime);
        }
    }
}
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
        GraphicsDeviceManager graphics;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            this.IsMouseVisible = true;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            Components.Add(new InputHandler(this));
            Components.Add(new SpriteBatchComponent(this));
            Components.Add(new VOiD.Components.Viewport(this));
            Components.Add(new DebugLog(this));
            Components.Add(new GameHandler(this));
            Components.Add(new Configuration(this, graphics));
            Components.Add(new Interface(this));
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
            GraphicsDevice.Clear(Color.OldLace);
            base.Draw(gameTime);
        }
    }
}
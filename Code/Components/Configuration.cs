using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace VOiD.Components
{
    class Configuration : GameComponent
    {
        public static int Width 
        {
            get { return graphics.GraphicsDevice.Viewport.Width; }
            set
            {
                graphics.PreferredBackBufferWidth = value;
                graphics.ApplyChanges();
            }
        }
        public static int Height
        {
            get { return graphics.GraphicsDevice.Viewport.Height; }
            set
            {
                graphics.PreferredBackBufferHeight = value;
                graphics.ApplyChanges();
            }
        }

        
        private static GraphicsDeviceManager graphics;
        public static float AspectRatio { get { return graphics.GraphicsDevice.Viewport.AspectRatio; } }
        public static Rectangle Bounds { get { return graphics.GraphicsDevice.Viewport.Bounds; } }

        public static bool Fullscreen
        {
            get { return graphics.IsFullScreen; }
            set
            {
                if (graphics.IsFullScreen == true && value == false)
                    graphics.ToggleFullScreen();
                if (graphics.IsFullScreen == false && value == true)
                    graphics.ToggleFullScreen();
            }
        }

        public static void Toggle()
        {
            graphics.ToggleFullScreen();
        }

        public Configuration(Game game)
            : base(game)
        {
            graphics = new GraphicsDeviceManager(game);
            Game_Library.Configuration config = game.Content.Load<Game_Library.Configuration>("Configuration");
            Width = config.Width;
            Height = config.Height;
            Fullscreen = config.Fullscreen;

            if (Fullscreen == true)
            {
                graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                graphics.ApplyChanges();
                if (graphics.IsFullScreen == false)
                    graphics.ToggleFullScreen();
            }
            else
            {
                graphics.PreferredBackBufferHeight = Height;
                graphics.PreferredBackBufferWidth = Width;
                graphics.ApplyChanges();
                if (graphics.IsFullScreen == true)
                    graphics.ToggleFullScreen();
            }
            game.Window.ClientSizeChanged += new EventHandler<EventArgs>(OnResize);
        }

        void OnResize(object sender, EventArgs e)
        {
            Width = Game.GraphicsDevice.Viewport.Width;
            Height = Game.GraphicsDevice.Viewport.Height;
        }
    }
}

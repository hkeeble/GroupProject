using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CreatureGame
{
    public class Viewport : GameComponent
    {
        public static int Width = 0;
        public static int Height = 0;
        public static float AspectRatio = 1.0f;
        public static Rectangle Bounds;
        public static Rectangle TitleSafeArea;

        public Viewport(Game game)
            : base(game)
        {
            game.Window.ClientSizeChanged += new EventHandler<EventArgs>(Window_ClientSizeChanged);
        }

        void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            Width = Game.GraphicsDevice.Viewport.Width;
            Height = Game.GraphicsDevice.Viewport.Height;
            Bounds = Game.GraphicsDevice.Viewport.Bounds;
            TitleSafeArea = Game.GraphicsDevice.Viewport.TitleSafeArea;
            AspectRatio = Game.GraphicsDevice.Viewport.AspectRatio;
        }
    }
}


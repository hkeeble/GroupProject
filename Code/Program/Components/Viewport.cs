using System;
using Microsoft.Xna.Framework;

namespace VOiD.Components
{
    public class Viewport : GameComponent
    {
        public static int Width = 640;
        public static int Height = 480;
        public static float AspectRatio = 1.0f;
        public static Rectangle Bounds;
        public static Rectangle TitleSafeArea;

        public Viewport(Game game)
            : base(game)
        {
            game.Window.ClientSizeChanged += new EventHandler<EventArgs>(Window_ClientSizeChanged);
            Width = Game.GraphicsDevice.Viewport.Width;
            Height = Game.GraphicsDevice.Viewport.Height;
            Bounds = Game.GraphicsDevice.Viewport.Bounds;
            TitleSafeArea = Game.GraphicsDevice.Viewport.TitleSafeArea;
            AspectRatio = Game.GraphicsDevice.Viewport.AspectRatio;
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

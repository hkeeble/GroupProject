using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameLibrary;

namespace VOiD.Components
{
    enum Screens
    {
        Intro,
        MainMenu,
        BLANK
    }

    class Interface : DrawableGameComponent
    {
        public static Color BackgroundColor { get { return _color; } }
        private static Color _color=Color.Black;
        private static Screens lastScreen;
        public static Screens currentScreen;
        private static List<Object2D> content = new List<Object2D>();

        private void DrawComponent(List<Object2D> Interface)
        {
            Object2D sam = new Object2D();
            foreach (Object2D thing in Interface)
            {
                DrawComponent(thing, ref sam);
            }
        }

        private void DrawComponent(Object2D component, ref Object2D parent)
        {
            if (component.GetType() == typeof(GraphicObject))
            {
                DrawGraphicComponent((component as GraphicObject), ref parent);
            }
            else if (component.GetType() == typeof(TextObject))
            {
                DrawTextComponent((component as TextObject), ref parent);
            }

            DrawComponent(component.Children, ref component);
        }

        private void DrawComponent(List<Object2D> children, ref Object2D parent)
        {
            foreach (Object2D thing in children)
            {
                DrawComponent(thing, ref parent);
            }
        }

        private void DrawTextComponent(TextObject component, ref Object2D parent)
        {
            if (!component.Init)
            {
                Vector2 ye=Vector2.Zero;
                if (component.Location == "TopLeft")
                    ye = new Vector2(0, 0);
                else if (component.Location == "TopCenter")
                    ye = new Vector2(parent.Size.X / 2, 0);
                else if (component.Location == "TopRight")
                    ye = new Vector2(parent.Size.X, 0);
                else
                    ye = new Vector2(parent.Size.X, parent.Size.Y);



                component.Position = ye + parent.Position;
                component.Init = true;
                if (component.isCentered)
                    component.Position-=Game.Content.Load<SpriteFont>("SegoeUI").MeasureString(component.Text) / 2; 

            }


            SpriteBatchComponent.spriteBatch.DrawString(Game.Content.Load<SpriteFont>("SegoeUI"), component.Text, component.Position, Color.White);
        }

        private void DrawGraphicComponent(GraphicObject component, ref Object2D parent)
        {
            // if texture is not yet loaded load it
            if (component.Texture == null)
                component.Texture = Game.Content.Load<Texture2D>((component as GraphicObject).TextureLocation);

            if (!component.Init)
            {
                component.Init = true;
                Vector2 ye = Vector2.Zero;
                if (component.Location == "TopLeft")
                    ye = new Vector2(0, 0);
                else if (component.Location == "TopCenter")
                    ye = new Vector2(parent.Size.X / 2, 0);
                else if (component.Location == "TopRight")
                    ye = new Vector2(parent.Size.X, 0);
                else
                    ye = new Vector2(parent.Size.X, parent.Size.Y);



                component.Position = ye + parent.Position;

                if(component.isCentered)
                    component.Position -= new Vector2(component.Texture.Width, component.Texture.Height) / 2;

            }

            if (component.fullscreen)
            {
                component.Position.X = 0;
                component.Position.Y = 0;
                component.Size.X = Game.GraphicsDevice.Viewport.Width;
                component.Size.Y = Game.GraphicsDevice.Viewport.Height;

                SpriteBatchComponent.spriteBatch.Draw(component.Texture, Configuration.Bounds, Color.White);
            }
            else
            {
                SpriteBatchComponent.spriteBatch.Draw(component.Texture, component.Position, Color.White);
            }
        }








        public Interface(Game game)
            : base(game)
        {

            currentScreen = Screens.Intro;
            lastScreen = Screens.BLANK;
        }

        public override void Update(GameTime gameTime)
        {
            if (currentScreen != lastScreen)
            {
                // if screen has changed
                content.Clear();
                if (currentScreen == Screens.Intro)
                {
                    GameLibrary.Interface temp = (Game.Content.Load<GameLibrary.Interface>("Intro"));
                    content = temp.content;
                    _color = temp.backgroundColor;
                }

            }
            else
            {
                // Do any logic required for this type of screen
            }

            base.Update(gameTime);
            lastScreen = currentScreen;
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatchComponent.spriteBatch.Begin();
            DrawComponent(content);
            SpriteBatchComponent.spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

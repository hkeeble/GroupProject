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

        private void DrawGraphicComponent(GraphicObject component, ref Object2D parent)
        {
            // if texture is not yet loaded load it
            if (component.Texture == null)
                component.Texture = Game.Content.Load<Texture2D>((component as GraphicObject).TextureLocation);

            if (!component.Init)
            {
                component.Rotation = MathHelper.ToRadians(component.Rotation);
                component.currentRotation = MathHelper.ToRadians(component.Rotation);
                component.Init = true;
                component.Position.X = component.Area.X;
                component.Position.Y = component.Area.Y;
                //if (component.isCentered)
                component.offset += new Vector2(component.Texture.Width, component.Texture.Height) / 2;

            }

            if (component.RotationAni)
                component.currentRotation += component.Rotation;

            if (component.fullscreen)
            {
                component.Area.X = 0;
                component.Area.Y = 0;
                component.Area.Height = Game.GraphicsDevice.Viewport.Height;
                component.Area.Width = Game.GraphicsDevice.Viewport.Width;
                SpriteBatchComponent.spriteBatch.Draw(component.Texture, component.Area, Color.White);
            }
            else
            {
                component.currentRotation2 = component.currentRotation + parent.currentRotation2;
                component.Area.X = (int)component.Position.X + parent.Area.X + (int)((float)parent.Area.X * Math.Cos(parent.currentRotation2));
                component.Area.Y = (int)component.Position.Y + parent.Area.Y + (int)((float)parent.Area.Y * Math.Sin(parent.currentRotation2));
                SpriteBatchComponent.spriteBatch.Draw(component.Texture, component.Area, null, Color.White, component.currentRotation2, component.offset, SpriteEffects.None, 0);
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
                    content = (Game.Content.Load<List<Object2D>>("Intro"));

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

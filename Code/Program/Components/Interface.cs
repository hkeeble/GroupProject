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
        LevelMenu,
        BLANK
    }

    class Interface : DrawableGameComponent
    {
        private static GameLibrary.Interface temp;
        public static Color BackgroundColor { get { return temp.backgroundColor; } }
        private static Screens lastScreen;
        public static Screens currentScreen;

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
                if (parent.GetType() == typeof(GraphicObject))
                    DrawTextComponent((component as TextObject), (parent as GraphicObject));
            }

            if(component.GetType() == typeof(GraphicObject))
                DrawComponent((component as GraphicObject).Children, ref component);
        }

        private void DrawComponent(List<Object2D> children, ref Object2D parent)
        {
            foreach (Object2D thing in children)
            {
                DrawComponent(thing, ref parent);
            }
        }

        private void DrawTextComponent(TextObject component, GraphicObject parent)
        {
            if (!component.Init)
            {


                component.offset += yep(component, parent);

                //component.Position += ye + parent.Position;
                component.Init = true;
                
                //if (component.isCentered)
                    component.offset-=Game.Content.Load<SpriteFont>("SegoeUI").MeasureString(component.Text) / 2; 
                
            }

            SpriteBatchComponent.spriteBatch.DrawString(Game.Content.Load<SpriteFont>("SegoeUI"), component.Text, parent.Position+component.offset, Color.White);
        }


        private void DrawGraphicComponent(GraphicObject component, ref Object2D parent)
        {
            // if texture is not yet loaded load it
            if (component.Texture == null)
                if (component.TextureLocation != "")
                    component.Texture = Game.Content.Load<Texture2D>((component as GraphicObject).TextureLocation);
                else
                    component.Texture = new Texture2D(Game.GraphicsDevice, 1, 1);

            if (!component.Init)
            {
                component.Init = true;
                Vector2 yesh = Vector2.Zero;
                if ((component.GetType() == typeof(GraphicObject)) && (parent.GetType() == typeof(GraphicObject)))
                {
                    yesh = (parent as GraphicObject).Position;
                    component.Position += yesh;

                    component.Position.X = ((parent as GraphicObject).Size.X / 100 * component.Position.X);
                    component.Position.Y = ((parent as GraphicObject).Size.Y / 100 * component.Position.Y);
                }


                

                
                /*
                if(component.isCentered)
                    component.offset = new Vector2(component.Size.X, component.Size.Y) / 2;
                */
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
                SpriteBatchComponent.spriteBatch.Draw(component.Texture, new Rectangle((int)component.Position.X, (int)component.Position.Y, (int)component.Size.X, (int)component.Size.Y), null, Color.White);
            }
        }

        public Interface(Game game)
            : base(game)
        {
            currentScreen = Screens.MainMenu;
            lastScreen = Screens.BLANK;
        }



        private Vector2 yep(TextObject component,GraphicObject parent)
        {
            Vector2 ye;
            
            if (component.Location == "TopCenter")
                ye = new Vector2(parent.Size.X / 2, 0);
            else if (component.Location == "TopRight")
                ye = new Vector2(parent.Size.X, 0);
            else if (component.Location == "MiddleLeft")
                ye = new Vector2(0, parent.Size.Y / 2);
            else if (component.Location == "MiddleCenter")
                ye = new Vector2(parent.Size.X / 2, parent.Size.Y / 2);
            else if (component.Location == "MiddleRight")
                ye = new Vector2(parent.Size.X, parent.Size.Y / 2);
            else if (component.Location == "BottomLeft")
                ye = new Vector2(0, parent.Size.Y);
            else if (component.Location == "BottomCenter")
                ye = new Vector2(parent.Size.X / 2, parent.Size.Y);
            else if (component.Location == "BottomRight")
                ye = new Vector2(parent.Size.X, parent.Size.Y);
            else // TopLeft
                ye = new Vector2(0, 0);

            return ye;
        }


        private void ClickableComponent(GraphicObject component)
        {
            Rectangle TextureRectangle = new Rectangle((int)component.Position.X, (int)component.Position.Y, (int)component.Size.X, (int)component.Size.Y);

            if (TextureRectangle.Contains(InputHandler.mouseState.X, InputHandler.mouseState.Y)&&InputHandler.mouseState.LeftButton== Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                if (component.Action.Equals("continue"))
                    currentScreen = Screens.LevelMenu;
                if (component.Action.Equals("Quit"))
                    Game.Exit();
                DebugLog.WriteLine(string.Format("Button Clicked Action =  {0} ", component.Action));
            }
        }


        private void UpdateComponent(List<Object2D> components)
        {
            foreach (Object2D thing in components)
            {
                UpdateComponent(thing);
            }
        }

        private void UpdateComponent(Object2D component)
        {
            if (component.GetType() == typeof(GraphicObject))
            {
                UpdateComponent((component as GraphicObject).Children);
                if ((component as GraphicObject).isClickable)
                    ClickableComponent((component as GraphicObject));
            }
        }








        public override void Update(GameTime gameTime)
        {
            if (currentScreen != lastScreen)
            {
                // if screen has changed
                temp = new GameLibrary.Interface();

                if (currentScreen == Screens.MainMenu)
                {
                    temp = (Game.Content.Load<GameLibrary.Interface>("MainMenu"));
                }
                else if (currentScreen == Screens.LevelMenu)
                {
                    temp = (Game.Content.Load<GameLibrary.Interface>("LevelMenu"));
                }
                else
                {
                    temp = new GameLibrary.Interface();
                }

            }

            // Do any logic required for this type of screen
            lastScreen = currentScreen;
            UpdateComponent(temp.content);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (temp.Overlay)
                GraphicsDevice.Clear(BackgroundColor);
            SpriteBatchComponent.spriteBatch.Begin();
            DrawComponent(temp.content);
            SpriteBatchComponent.spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

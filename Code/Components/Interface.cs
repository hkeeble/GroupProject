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
        Battle,
        Lab,
        BLANK
    }

    class Interface : DrawableGameComponent
    {
        private static GameLibrary.Interface temp;
        private static GameLibrary.Interface subMenu;
        public static Color BackgroundColor { get { return temp.backgroundColor; } }
        private static Screens lastScreen;
        public static Screens currentScreen;
        private static short ResID = 0;
        private DisplayMode[] dm;
        RasterizerState _scissorState = new RasterizerState() { ScissorTestEnable = true };

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
            else if (component.GetType() == typeof(TextBoxObject))
            {
                if(parent.GetType() == typeof(GraphicObject))
                    DrawTextBoxComponent((component as TextBoxObject), (parent as GraphicObject));
            }
            else if (component.GetType() == typeof(ListBox))
            {
                if(parent.GetType() == typeof(GraphicObject))
                    DrawListBox((component as ListBox), (parent as GraphicObject));
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

            component.offset.X = parent.Size.X / 100 * component.ioffset.X;
            component.offset.Y = parent.Size.Y / 100 * component.ioffset.Y;

            component.Init = true;
                
            //if (component.isCentered)

            string temp = "";

            Vector2 off = component.offset;

            if (component.Text.StartsWith("@"))
            {
                if (component.Text == "@currentRes")
                    temp = Configuration.Width.ToString() + "x" + Configuration.Height.ToString();
                if (component.Text == "@NumberOfApples")
                    temp = "x" + Convert.ToString(GameHandler.Inventory.NumberOfApples);
                if (component.Text == "@NumberOfChilli")
                    temp = "x" + Convert.ToString(GameHandler.Inventory.NumberOfChilli);
                if (component.Text == "@NumberOfGoldenApples")
                    temp = "x" + Convert.ToString(GameHandler.Inventory.NumberOfGoldenApples);
                if (component.Text == "@NumberOfHoney")
                    temp = "x" + Convert.ToString(GameHandler.Inventory.NumberOfHoney);
                if (component.Text == "@NumberOfSpringWater")
                    temp = "x" + Convert.ToString(GameHandler.Inventory.NumberOfSpringWater);
            }
            else
                temp = component.Text;

            if (component.isCentered)
                off -= Game.Content.Load<SpriteFont>(component.Font).MeasureString(temp) / 2;

            SpriteManager.DrawString(Game.Content.Load<SpriteFont>(component.Font), temp, parent.Position + off, new Color(component.fontColor));
        }

        private void DrawTextBoxComponent(TextBoxObject component, Object2D parent)
        {
            GraphicObject Parent = (GraphicObject)parent;

            component.offset.X = Parent.Size.X / 100 * component.ioffset.X;
            component.offset.Y = Parent.Size.Y / 100 * component.ioffset.Y;

            component.Init = true;

            Vector2 off = component.offset;
            component.BoundingRect = new Rectangle((int)Parent.Position.X + (int)off.X, (int)Parent.Position.Y + (int)off.Y, (int)component.Bounds.X, (int)component.Bounds.Y);

            if (Game.Content.Load<SpriteFont>(component.Font).MeasureString(component.Text).Y > component.Bounds.Y)
            {
                float maxY = (Parent.Position.Y - (Parent.Size.Y / 2)+5);
                float minY = (Parent.Position.Y + component.Bounds.Y - (Parent.Size.Y / 2)) - (Game.Content.Load<SpriteFont>(component.Font).MeasureString(component.Text).Y);
                if (component.currentOffset.Y > maxY)
                    component.currentOffset.Y = maxY;
                else if (component.currentOffset.Y < minY)
                    component.currentOffset.Y = minY;
            }

            Rectangle currentRect = SpriteManager.ScissorRectangle;
            SpriteManager.ScissorRectangle = component.BoundingRect;
            SpriteManager.DrawString(Game.Content.Load<SpriteFont>(component.Font), component.Text, Parent.Position + off + component.currentOffset, new Color(component.fontColor));
            SpriteManager.ScissorRectangle = currentRect;

            DrawGraphicComponent(component.UpScroller, ref parent);
            DrawGraphicComponent(component.DownScroller, ref parent);
        }

        private void DrawListBox(ListBox component, Object2D parent)
        {
            GraphicObject Parent = (GraphicObject)parent;

            component.offset.X = Parent.Size.X / 100 * component.ioffset.X;
            component.offset.Y = Parent.Size.Y / 100 * component.ioffset.Y;

            component.Init = true;

            Vector2 off = component.offset;
            component.BoundingRect = new Rectangle((int)Parent.Position.X + (int)off.X, (int)Parent.Position.Y + (int)off.Y, (int)component.Bounds.X, (int)component.Bounds.Y);

            float textHeight = 0f;
            for (int i = 0; i < (component as ListBox).Items.Length; i++)
                textHeight += Game.Content.Load<SpriteFont>(component.Font).MeasureString((component as ListBox).Items[i].Text).Y;

            if (textHeight > component.Bounds.Y)
            {
                float minY = (Parent.Position.Y - (Parent.Size.Y / 2)) - textHeight;
                float maxY = Parent.Position.Y + (Parent.Size.Y / 2);

                if (component.currentOffset.Y > minY)
                    component.currentOffset.Y = minY;
                if (component.currentOffset.Y > maxY)
                    component.currentOffset.Y = maxY;
            }

            Rectangle currentRect = SpriteManager.ScissorRectangle;
            SpriteManager.ScissorRectangle = component.BoundingRect;
            for (int i = 0; i < component.Items.Length; i++)
            {
                SpriteManager.DrawString(Game.Content.Load<SpriteFont>(component.Font), component.Items[i].Text,
                    (Parent.Position + off + component.currentOffset) + new Vector2(0, (i * Game.Content.Load<SpriteFont>(component.Font).MeasureString(component.Items[i].Text).Y)), new Color(component.fontColor));
            }
            SpriteManager.ScissorRectangle = currentRect;

            DrawGraphicComponent(component.UpScroller, ref parent);
            DrawGraphicComponent(component.DownScroller, ref parent);
        }

        private void DrawGraphicComponent(GraphicObject component, ref Object2D parent)
        {
            // if texture is not yet loaded load it
            if (component.Texture == null)
                if (component.TextureLocation != "")
                {
                    if (component.TextureLocation.StartsWith("@"))
                    {
                        if(component.TextureLocation == "@AppleTexture")
                            component.Texture = Game.Content.Load<Texture2D>("Sprites\\Apple");
                        if(component.TextureLocation == "@GoldenAppleTexture")
                            component.Texture = Game.Content.Load<Texture2D>("Sprites\\GoldenApple");
                        if(component.TextureLocation == "@SpringWaterTexture")
                            component.Texture = Game.Content.Load<Texture2D>("Sprites\\SpringWater");
                        if(component.TextureLocation == "@HoneyTexture")
                            component.Texture = Game.Content.Load<Texture2D>("Sprites\\Honey");
                        if (component.TextureLocation == "@ChilliTexture")
                            component.Texture = Game.Content.Load<Texture2D>("Sprites\\Chilli");
                    }
                    else
                        component.Texture = Game.Content.Load<Texture2D>("Interface/Assets/" + (component as GraphicObject).TextureLocation);
                }
                else
                    component.Texture = new Texture2D(Game.GraphicsDevice, 1, 1);

                if (((component.GetType() == typeof(GraphicObject)) || component.GetType() == typeof(TextBoxObject.Scroller)) && (parent.GetType() == typeof(GraphicObject)))
                {
                    
                    component.Size.X = ((parent as GraphicObject).Size.X / 100 * component.iSize.X);
                    component.Size.Y = ((parent as GraphicObject).Size.Y / 100 * component.iSize.Y);
                    component.Position.X = ((parent as GraphicObject).Size.X / 100 * component.iPosition.X) - (component.Size.X / 2);
                    component.Position.Y = ((parent as GraphicObject).Size.Y / 100 * component.iPosition.Y) - (component.Size.Y / 2);
                    component.Position += (parent as GraphicObject).Position;
                }
                
                /*
                if(component.isCentered)
                    component.offset = new Vector2(component.Size.X, component.Size.Y) / 2;
                */
            

            if (component.fullscreen)
            {
                component.Position.X = 0;
                component.Position.Y = 0;
                component.Size.X = Game.GraphicsDevice.Viewport.Width;
                component.Size.Y = Game.GraphicsDevice.Viewport.Height;

                SpriteManager.Draw(component.Texture, Configuration.Bounds, Color.White);
            }
            else
            {
                SpriteManager.Draw(component.Texture, new Rectangle((int)component.Position.X, (int)component.Position.Y, (int)component.Size.X, (int)component.Size.Y), null, Color.White);
            }
        }

        public Interface(Game game)
            : base(game)
        {
            this.DrawOrder=99;
            currentScreen = Screens.MainMenu;
            lastScreen = Screens.BLANK;
            dm = Game.GraphicsDevice.Adapter.SupportedDisplayModes.ToArray<DisplayMode>();
        }

        private void ClickableComponent(GraphicObject component)
        {
            Rectangle TextureRectangle = new Rectangle((int)component.Position.X, (int)component.Position.Y, (int)component.Size.X, (int)component.Size.Y);

            if (TextureRectangle.Contains(InputHandler.MouseX, InputHandler.MouseY)&&InputHandler.LeftClickPressed)
            {
                #region Main Menu Actions
                if (component.Action.Equals("continue"))
                    currentScreen = Screens.LevelMenu;
                if (component.Action.Equals("Quit"))
                    Game.Exit();
                if (component.Action.Equals("Options"))
                    subMenu = Game.Content.Load<GameLibrary.Interface>("Interface/SubMenuOptions");
                if (component.Action.Equals("DeleteSubMenu"))
                    subMenu = new GameLibrary.Interface();
                if (component.Action.Equals("PlusRes"))
                {
                    DisplayMode tmp = dm[ResID];
                    Configuration.Width = tmp.Width;
                    Configuration.Height = tmp.Height;

                    ResID++;
                    if (ResID >= Game.GraphicsDevice.Adapter.SupportedDisplayModes.Count<DisplayMode>())
                        ResID = 0;
                }
                if (component.Action.Equals("MinusRes"))
                {
                    DisplayMode tmp = dm[ResID];
                    Configuration.Width = tmp.Width;
                    Configuration.Height = tmp.Height;

                    
                    if (ResID <= 0)
                        ResID = (short)(Game.GraphicsDevice.Adapter.SupportedDisplayModes.Count<DisplayMode>());
                    ResID--;
                }
                if (component.Action.Equals("Fullscreen"))
                {
                    Configuration.Toggle();
                }
                #endregion

                #region Lab Menu Actions
                if (component.Action.Equals("Creature"))
                    subMenu = Game.Content.Load<GameLibrary.Interface>("Interface/SubMenuCreatureInfo");
                if (component.Action.Equals("exit"))
                    Interface.currentScreen = Screens.LevelMenu;
                #endregion

                #region Global Dial Actions
                if(component.Action.Equals("Inventory"))
                    subMenu = Game.Content.Load<GameLibrary.Interface>("Interface/SubMenuInventory");
                #endregion

                #region Item Use Actions
                if (component.Action.Equals("UseApple"))
                    GameHandler.Inventory.UseItem((int)Item.ItemName.Apple);
                if (component.Action.Equals("UseGoldenApple"))
                    GameHandler.Inventory.UseItem((int)Item.ItemName.Golden_Apple);
                if (component.Action.Equals("UseChilli"))
                    GameHandler.Inventory.UseItem((int)Item.ItemName.Chilli);
                if (component.Action.Equals("UseHoney"))
                    GameHandler.Inventory.UseItem((int)Item.ItemName.Honey);
                if (component.Action.Equals("UseSpringWater"))
                    GameHandler.Inventory.UseItem((int)Item.ItemName.Spring_Water);
                #endregion

                DebugLog.WriteLine(string.Format("Button Clicked Action =  {0} ", component.Action));
            }
        }


        private void UpdateComponent(List<Object2D> components)
        {
            foreach (Object2D thing in components)
            {
                UpdateComponent(thing);
                if (thing.GetType() == typeof(TextBoxObject) || thing.GetType() == typeof(ListBox))
                {
                    TextBoxObject temp = (TextBoxObject)thing;
                    UpdateTextBoxObject(temp);
                }
            }
        }

        private void UpdateComponent(Object2D component)
        {
            if (component.GetType() == typeof(GraphicObject) || component.GetType() == typeof(TextBoxObject.Scroller))
            {
                UpdateComponent((component as GraphicObject).Children);
                if ((component as GraphicObject).isClickable)
                    ClickableComponent((component as GraphicObject));
            }
        }

        private void UpdateTextBoxObject(TextBoxObject component)
        {
            float textHeight = 0f;
            if(component.GetType() == typeof(TextBoxObject))
                textHeight = Game.Content.Load<SpriteFont>(component.Font).MeasureString(component.Text).Y;
            if (component.GetType() == typeof(ListBox))
                for (int i = 0; i < (component as ListBox).Items.Length; i++)
                    textHeight += Game.Content.Load<SpriteFont>(component.Font).MeasureString((component as ListBox).Items[i].Text).Y;

            if (textHeight > component.Bounds.Y)
            {
                Rectangle UpRect = new Rectangle((int)component.UpScroller.Position.X, (int)component.UpScroller.Position.Y,
                    (int)component.UpScroller.Size.X, (int)component.UpScroller.Size.Y);
                Rectangle DownRect = new Rectangle((int)component.DownScroller.Position.X, (int)component.DownScroller.Position.Y,
                    (int)component.DownScroller.Size.X, (int)component.DownScroller.Size.Y);

                if (UpRect.Contains(InputHandler.MouseX, InputHandler.MouseY) && InputHandler.LeftClickDown)
                    component.Scroll(component.UpScroller.scrollDirection);
                if (DownRect.Contains(InputHandler.MouseX, InputHandler.MouseY) && InputHandler.LeftClickDown)
                    component.Scroll(component.DownScroller.scrollDirection);
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
                    temp = (Game.Content.Load<GameLibrary.Interface>("Interface/MainMenu"));
                }
                else if (currentScreen == Screens.LevelMenu)
                {
                    temp = (Game.Content.Load<GameLibrary.Interface>("Interface/LevelMenu"));
                }
                else if (currentScreen == Screens.Battle)
                {
                    temp = (Game.Content.Load<GameLibrary.Interface>("Interface/BattleScreen"));
                }
                else if (currentScreen == Screens.Lab)
                {
                    temp = (Game.Content.Load<GameLibrary.Interface>("Interface/LabScreen"));
                }
                else
                {
                    temp = new GameLibrary.Interface();
                }
                subMenu = new GameLibrary.Interface();
            }

            // Do any logic required for this type of screen
            lastScreen = currentScreen;
            if (subMenu.content.Count == 0)
                UpdateComponent(temp.content);
            else
                UpdateComponent(subMenu.content);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (temp.Overlay)
                GraphicsDevice.Clear(BackgroundColor);
            SpriteManager.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, _scissorState);
            DrawComponent(temp.content);
            DrawComponent(subMenu.content);
            SpriteManager.End();
            base.Draw(gameTime);
        }
    }
}

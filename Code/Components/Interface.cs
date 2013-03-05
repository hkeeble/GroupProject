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

        #region Draw Functions
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
            else if (component.GetType() == typeof(MinimapObject))
            {
                DrawMinimapObject((component as MinimapObject), ref parent);
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

                // Number Of Items
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

                // -- HOOK UP PLAYER STATISTICS HERE -- \\
                // Player Statistics
                if (component.Text == "@PlayerWeight")
                    temp = "Weight: ";
                if (component.Text == "@PlayerStrength")
                    temp = "Strength: ";
                if (component.Text == "@PlayerDexterity")
                    temp = "Dexterity: ";
                if (component.Text == "@PlayerEndurance")
                    temp = "Endurance: ";
                if (component.Text == "@PlayerHealth")
                    temp = "Health: ";
                if (component.Text == "@PlayerSpeed")
                    temp = "Speed: ";
                
                // Behavioural Traits
                if (component.Text == "@PlayerObedience")
                    temp = "Obedience: ";
                if (component.Text == "@PlayerAggressiveness")
                    temp = "Aggressiveness: ";
                if (component.Text == "@PlayerFocus")
                    temp = "Focus: ";

                // -- HOOK UP SELECTED STATISTICS HERE (USING GAMEHANDLER.INVENTORY.SelectedDNA) -- \\
                // Selected DNA Statistics
                if (component.Text == "@SelectedWeight")
                    temp = "Weight: ";
                if (component.Text == "@SelectedStrength")
                    temp = "Strength: ";
                if (component.Text == "@SelectedDexterity")
                    temp = "Dexterity: ";
                if (component.Text == "@SelectedEndurance")
                    temp = "Endurance: ";
                if (component.Text == "@SelectedHealth")
                    temp = "Health: ";
                if (component.Text == "@SelectedSpeed")
                    temp = "Speed: ";

                // Selected DNA Behavioural Traits
                if (component.Text == "@SelectedObedience")
                    temp = "Obedience: ";
                if (component.Text == "@SelectedAggressiveness")
                    temp = "Aggressiveness: ";
                if (component.Text == "@SelectedFocus")
                    temp = "Focus: ";

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
            component.BoundingRect = new Rectangle((int)Parent.Position.X + (int)off.X, (int)Parent.Position.Y + (int)off.Y, (int)Parent.Size.X, (int)Parent.Size.Y);

            if (Game.Content.Load<SpriteFont>(component.Font).MeasureString(component.Text).Y > component.BoundingRect.Height)
            {
                float maxY = (Parent.Position.Y - (Parent.Size.Y / 2)+5);
                float minY = (Parent.Position.Y + component.BoundingRect.Height - (Parent.Size.Y / 2)) - (Game.Content.Load<SpriteFont>(component.Font).MeasureString(component.Text).Y);
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

            if (component.Init == false)
            {
                string[] itemText = new string[0];
                string[] actions = new string[0];

                // Displays player's attacks
                if (component.ListContentType == "PlayerAttacks")
                {
                    itemText = new string[GameHandler.Player.AvailableAttacks.Count];
                    actions = new string[GameHandler.Player.AvailableAttacks.Count];

                    for (int i = 0; i < itemText.Length; i++)
                    {
                        itemText[i] = GameHandler.Player.AvailableAttacks[i].Name;
                        actions[i] = "PlayerUse" + GameHandler.Player.AvailableAttacks[i].Name;
                    }
                }

                // Displays player's traits
                if (component.ListContentType == "PlayerTraits")
                {
                    // -- DISPLAY TRAITS HERE \\
                }

                // Displays player's owned DNA
                if (component.ListContentType == "PlayerDNAInventory")
                {
                    itemText = new string[GameHandler.Inventory.DNA.Count];
                    actions = new string[GameHandler.Inventory.DNA.Count];

                    for (int i = 0; i < itemText.Length; i++)
                    {
                        itemText[i] = Convert.ToString(GameHandler.Inventory.DNA[i].ID);
                        actions[i] = Convert.ToString(GameHandler.Inventory.DNA[i].ID);
                    }
                }

                // Displays currently selected DNA's traits
                if (component.ListContentType == "SelectedDNATraits")
                {
                    // -- DISPLAY SELECTED CREATURE'S TRAITS HERE -- \\
                }

                // The following will render the text to their own individual textures with bounding rects
                SpriteFont font = Game.Content.Load<SpriteFont>(component.Font);

                component.Items = new ListBox.Item[itemText.Length];

                for (int i = 0; i < component.Items.Length; i++)
                {
                    // Create RenderTarget of correct size
                    RenderTarget2D target = new RenderTarget2D(Configuration.GraphicsDevice, (int)font.MeasureString(itemText[i]).X,
                                                (int)font.MeasureString(itemText[i]).Y, false, Configuration.GraphicsDevice.DisplayMode.Format, DepthFormat.Depth24);
                    component.Items[i] = new ListBox.Item();
                        
                    // Render string to RenderTarget
                    Configuration.GraphicsDevice.SetRenderTarget(target);
                    Configuration.GraphicsDevice.Clear(Color.Transparent);
                    SpriteManager.End();
                    SpriteManager.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
                    SpriteManager.DrawString(font, itemText[i], Vector2.Zero, new Color(component.fontColor));
                    SpriteManager.End();
                    SpriteManager.Begin();
                    Configuration.GraphicsDevice.SetRenderTarget(null);

                    // Move RenderTarget data to Texture2D
                    Color[] data = new Color[target.Width * target.Height];
                    Texture2D temp = new Texture2D(Configuration.GraphicsDevice, target.Width, target.Height);
                    target.GetData<Color>(data);
                    temp.SetData<Color>(data);
                    component.Items[i].Texture = temp;
                    target.Dispose();

                    // Set Inividual element's offset
                    component.Items[i].offset = new Vector2(0, i > 0 ? component.Items[i - 1].Texture.Height*i : 0);

                    // Set component's Action
                    component.Items[i].Action = actions[i];
                }
                    component.currentOffset = Vector2.Zero;
            }

            component.Init = true;

            Vector2 off = component.offset;
            component.BoundingRect = new Rectangle((int)Parent.Position.X + (int)off.X, (int)Parent.Position.Y + (int)off.Y, (int)Parent.Size.X, (int)Parent.Size.Y);

            // Get Height of the text in the list
            float textHeight = 0.0f;
            for (int i = 0; i < component.Items.Length; i++)
                textHeight += component.Items[i].Texture.Height;

            // Modify Current Offset to ensure within the box
            if (textHeight > component.BoundingRect.Height)
            {
                float maxY = (Parent.Position.Y - (Parent.Size.Y / 2));
                float minY = (Parent.Position.Y + component.BoundingRect.Height - (Parent.Size.Y / 2)) - textHeight;
                if (component.currentOffset.Y > maxY)
                    component.currentOffset.Y = maxY;
                else if (component.currentOffset.Y < minY)
                    component.currentOffset.Y = minY;
            }

            // Set Scissor rectangle
            Rectangle currentRect = SpriteManager.ScissorRectangle;
            SpriteManager.ScissorRectangle = component.BoundingRect;

            // Draw List
            for (int i = 0; i < component.Items.Length; i++)
            {
                SpriteManager.Draw(component.Items[i].Texture, Parent.Position + off + component.Items[i].offset + component.currentOffset, new Color(component.fontColor));
                component.Items[i].Update(Parent.Position + off + component.Items[i].offset + component.currentOffset);
            }

            // Revert Scissor Rectangle
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

                if (((component.GetType() == typeof(GraphicObject)) || component.GetType() == typeof(Scroller)) && (parent.GetType() == typeof(GraphicObject)))
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

        // Draw Minimap
        private void DrawMinimapObject(MinimapObject minimap, ref Object2D parent)
        {
            if (minimap.Init == false)
            {
                System.IO.Stream stream = System.IO.File.Open(minimap.TextureLocation + ".png", System.IO.FileMode.Open);
                minimap.Texture = Texture2D.FromStream(Configuration.GraphicsDevice, stream);
                stream.Close();
                minimap.Init = true;
            }

            minimap.Size.X = ((parent as GraphicObject).Size.X / 100 * minimap.iSize.X);
            minimap.Size.Y = ((parent as GraphicObject).Size.Y / 100 * minimap.iSize.Y);
            minimap.DrawRect = new Rectangle(
                              (int)(MathHelper.Clamp(((float)GameHandler.Player.CurrentTile.X * 4) - (minimap.Size.X / 2), 0.0f, minimap.Texture.Width)),
                              (int)(MathHelper.Clamp((GameHandler.Player.CurrentTile.Y * 4) - ((int)minimap.Size.Y / 2), 0.0f, minimap.Texture.Height)),
                              (int)minimap.Size.X,
                              (int)minimap.Size.Y);
            minimap.Position.X = ((parent as GraphicObject).Size.X / 100 * minimap.iPosition.X);
            minimap.Position.Y = ((parent as GraphicObject).Size.Y / 100 * minimap.iPosition.Y);
            minimap.Position += (parent as GraphicObject).Position;

            SpriteManager.Draw(minimap.Texture, new Vector2(minimap.Position.X, minimap.Position.Y), minimap.DrawRect, Color.White);
        }
        #endregion

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
                if (component.Action.Equals("OpenDNAInventory"))
                    subMenu = Game.Content.Load<GameLibrary.Interface>("Interface/SubMenuDNAInventory");
                if (component.Action.Equals("BreedCurrentSelection"))
                    GameHandler.Inventory.UseDNA();
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

        #region Update Functions
        private void UpdateComponent(List<Object2D> components)
        {
            foreach (Object2D thing in components)
            {
                UpdateComponent(thing);
                // Scrollable Objects
                if (thing.GetType() == typeof(TextBoxObject))
                    UpdateTextBox(thing as TextBoxObject);
                if (thing.GetType() == typeof(ListBox))
                    UpdateListBox(thing as ListBox);
            }
        }

        private void UpdateComponent(Object2D component)
        {
            if (component.GetType() == typeof(GraphicObject) || component.GetType() == typeof(Scroller))
            {
                UpdateComponent((component as GraphicObject).Children);
                if ((component as GraphicObject).isClickable)
                    ClickableComponent((component as GraphicObject));
            }
        }

        private void UpdateTextBox(TextBoxObject component)
        {
            float textHeight = textHeight = Game.Content.Load<SpriteFont>(component.Font).MeasureString(component.Text).Y;

            if (textHeight > component.BoundingRect.Height)
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

        private void UpdateListBox(ListBox component)
        {
            float textHeight = 0f;
            for (int i = 0; i < component.Items.Length; i++)
                textHeight += component.Items[i].Texture.Height;

            if (textHeight > component.BoundingRect.Height)
            {
                Rectangle UpRect = new Rectangle((int)component.UpScroller.Position.X, (int)component.UpScroller.Position.Y,
                    (int)component.UpScroller.Size.X, (int)component.UpScroller.Size.Y);
                Rectangle DownRect = new Rectangle((int)component.DownScroller.Position.X, (int)component.DownScroller.Position.Y,
                    (int)component.DownScroller.Size.X, (int)component.DownScroller.Size.Y);

                // Update Scrollers
                if (InputHandler.LeftClickDown)
                {
                    if (UpRect.Contains(InputHandler.MouseX, InputHandler.MouseY))
                        component.Scroll(component.UpScroller.scrollDirection);
                    if (DownRect.Contains(InputHandler.MouseX, InputHandler.MouseY))
                        component.Scroll(component.DownScroller.scrollDirection);
                }
            }


            if (component.isClickable)
            {
                // Update Items
                if (InputHandler.LeftClickPressed)
                {
                    for (int i = 0; i < component.Items.Length; i++)
                    {
                        if (component.Items[i].BoundingRect.Contains(InputHandler.MouseX, InputHandler.MouseY))
                        {
                            #region PlayerDNAInventory Actions
                            if (component.ListContentType == "PlayerDNAInventory")
                                GameHandler.Inventory.SetDNA(i);
                            #endregion
                            
                            // Add if statements for other list actions here
                        }
                    }
                }
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
        #endregion

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

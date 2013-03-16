using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace VOiD.Components
{
    public class LevelEditor : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private enum Mode
        {
            Tile,
            Item,
            Nest,
            Attribute,
            Sign
        }

        TimeSpan timeSinceLastSave = TimeSpan.Zero;

        Texture2D[,] tiles;
        Texture2D tilePointer;
        Point currentTile;
        Color[] selectedTileData;

        Texture2D tileSetRender;
        Rectangle tileSetRenderRect;
        Vector2 tileSetRenderDrawOffset;

        Attributes currentAttribute = Attributes.FlyingAndClimbing;

        List<Point> modifiedTiles;

        Mode currentMode;
        Item.ItemName CurrentItem;
        short currentCreatureID;
        string currentSignText;

        string workingDirectory;

        bool renderTileSet = true;

        public LevelEditor(Game game)
            : base(game)
        {
            
        }

        public override void Initialize()
        {
            workingDirectory = Directory.GetCurrentDirectory().Replace("Build", "Code\\Content\\Maps\\");
            currentMode = Mode.Tile;
            CurrentItem = Item.ItemName.Apple;

            base.Initialize();
        }

        protected override void OnEnabledChanged(object sender, EventArgs args)
        {
            if (this.Enabled == true && Interface.currentScreen == Screens.LevelMenu)
            {
                if (tilePointer == null)
                    tilePointer = Game.Content.Load<Texture2D>("Sprites/tileSelected");

                Console.WriteLine("----- Entering Level Editor -----\n");
                Interface.currentScreen = Screens.BLANK;
                GameHandler.EditMode = true;

                tiles = new Texture2D[GameHandler.TileMap.TileSet.Width / GameHandler.TileMap.TileWidth, GameHandler.TileMap.TileSet.Height / GameHandler.TileMap.TileHeight];

                Color[] currentTileData = new Color[GameHandler.TileMap.TileWidth * GameHandler.TileMap.TileHeight];

                // Get Tileset Data
                Color[] tileSetData = new Color[GameHandler.TileMap.TileSet.Width*GameHandler.TileMap.TileSet.Height];
                tileSetRender = new Texture2D(GraphicsDevice, GameHandler.TileMap.TileSet.Width, GameHandler.TileMap.TileSet.Height);
                GameHandler.TileMap.TileSet.GetData<Color>(tileSetData);
                for (int i = 0; i < tileSetData.Length; i++)
                    if (tileSetData[i].A == 0)
                    {
                        tileSetData[i].A = 255;
                        tileSetData[i].R = 0;
                        tileSetData[i].G = 0;
                        tileSetData[i].B = 0;
                    }
                tileSetRender.SetData<Color>(tileSetData);
                tileSetRenderRect = new Rectangle((Configuration.Width - tileSetRender.Width), (Configuration.Height - tileSetRender.Height), tileSetRender.Width, tileSetRender.Height);
                tileSetRenderDrawOffset = new Vector2(Configuration.Width - tileSetRender.Width, Configuration.Height - tileSetRender.Height);

                // Get TileSet dimensions
                int xTiles = GameHandler.TileMap.TileSet.Width / GameHandler.TileMap.TileWidth;
                int yTiles = GameHandler.TileMap.TileSet.Height / GameHandler.TileMap.TileHeight;

                for (int x = 0; x < xTiles; x++)
                {
                    for (int y = 0; y < yTiles; y++)
                    {
                        tiles[x, y] = new Texture2D(GraphicsDevice, GameHandler.TileMap.TileWidth, GameHandler.TileMap.TileHeight);
                        
                        GameHandler.TileMap.TileSet.GetData<Color>(0, new Rectangle(x * GameHandler.TileMap.TileWidth, y * GameHandler.TileMap.TileHeight, GameHandler.TileMap.TileWidth,
                            GameHandler.TileMap.TileHeight), currentTileData, 0, currentTileData.Length);

                        tiles[x, y].SetData<Color>(currentTileData);
                    }
                }

                // Initialize Current Tile and selectedTileData
                currentTile = new Point(10, 4);
                selectedTileData = new Color[GameHandler.TileMap.TileWidth * GameHandler.TileMap.TileHeight];
                tiles[currentTile.X, currentTile.Y].GetData<Color>(selectedTileData);

                // Default Camera 
                Camera.Move(Vector2.Zero);

                // Initialize modified tiles
                modifiedTiles = new List<Point>();
            }
            else if(Interface.currentScreen == Screens.BLANK)
            {
                Console.WriteLine("----- Exiting Level Editor. -----\n");
                if (!(Interface.currentScreen == Screens.MainMenu))
                {
                    SaveMap();
                    Interface.currentScreen = Screens.LevelMenu;
                }
                GameHandler.EditMode = false;
            }
            base.OnEnabledChanged(sender, args);
        }

        public override void Update(GameTime gameTime)
        {
            // Check Autosave
            timeSinceLastSave += gameTime.ElapsedGameTime;
            if (timeSinceLastSave >= TimeSpan.FromMinutes(5.0))
            {
                SaveMap();
                timeSinceLastSave = TimeSpan.Zero;
                modifiedTiles.Clear();
            }

            if (InputHandler.KeyDown(Keys.F2))
            {
                this.Visible = false;
                this.Enabled = false;
            }

            if (InputHandler.KeyPressed(Keys.D1))
            {
                currentMode = Mode.Tile;
                Console.WriteLine("Tile Mode.\n");
            }

            if (InputHandler.KeyPressed(Keys.D2))
            {
                currentMode = Mode.Item;
                Console.WriteLine("Item Mode.\n");
                Console.WriteLine("Current Item: " + CurrentItem.ToString() + "\n");
            }

            if (InputHandler.KeyPressed(Keys.D3))
            {
                currentMode = Mode.Nest;
                Console.WriteLine("Nest Mode.\n");

                Console.WriteLine("Creature ID for this nest: ");
                currentCreatureID = Convert.ToInt16(Console.ReadLine());
            }

            if (InputHandler.KeyPressed(Keys.D4))
            {
                currentMode = Mode.Attribute;
                Console.WriteLine("Atrribute Mode. \n");
                Console.WriteLine("Current Attribute: " + currentAttribute.ToString());
            }

            if (InputHandler.KeyPressed(Keys.D5))
            {
                currentMode = Mode.Sign;
                Console.WriteLine("Sign Mode.\n");
                Console.Write("Enter a string for this sign (# for newline): ");
                currentSignText = Console.ReadLine().Replace('#','\n');
            }

            if (InputHandler.KeyPressed(Keys.T))
                if (currentMode == Mode.Tile)
                    renderTileSet = !renderTileSet;

            if(InputHandler.KeyPressed(Keys.Z) || InputHandler.KeyPressed(Keys.X))
            {
                switch(currentMode)
                {
                    case Mode.Item:
                        if (InputHandler.KeyPressed(Keys.X) || InputHandler.KeyPressed(Keys.Z))
                        {
                            if (InputHandler.KeyPressed(Keys.X))
                            {
                                if ((int)CurrentItem != Enum.GetValues(typeof(Item.ItemName)).Length)
                                    CurrentItem++;
                                else
                                    CurrentItem = (Item.ItemName)1;
                            }
                            if (InputHandler.KeyPressed(Keys.Z))
                            {
                                if ((int)CurrentItem != 1)
                                    CurrentItem--;
                                else
                                    CurrentItem = (Item.ItemName)Enum.GetValues(typeof(Item.ItemName)).Length;
                            }
                            Console.WriteLine("Current Item: " + CurrentItem.ToString() + "\n");
                        }
                        break;
                    case Mode.Nest:
                        break;
                    case Mode.Attribute:
                        if (InputHandler.KeyPressed(Keys.X) || InputHandler.KeyPressed(Keys.Z))
                        {
                            if (InputHandler.KeyPressed(Keys.X))
                            {
                                if ((int)currentAttribute < 3)
                                    currentAttribute++;
                                else
                                    currentAttribute = (Attributes)1;
                            }
                            if (InputHandler.KeyPressed(Keys.Z))
                            {
                                if ((int)currentAttribute > 1)
                                    currentAttribute--;
                                else
                                    currentAttribute = (Attributes)3;
                            }
                            Console.WriteLine("Current Attribute: " + currentAttribute.ToString());
                        }
                        break;
                }
            }

            if ((InputHandler.LeftClickDown || InputHandler.RightClickPressed) && Configuration.Bounds.Contains(new Point(InputHandler.MouseX, InputHandler.MouseY)))
            {
                Vector2 MousePos = InputHandler.MouseWorldCoords;

                MousePos.X /= GameHandler.TileMap.TileWidth;
                MousePos.Y /= GameHandler.TileMap.TileHeight;

                Vector2 MousePosPixels = new Vector2((int)MousePos.X * GameHandler.TileMap.TileWidth, ((int)MousePos.Y * GameHandler.TileMap.TileHeight));

                switch(currentMode)
                {
                    case Mode.Tile:
                        if (InputHandler.LeftClickDown)
                        {
                            if (tileSetRenderRect.Contains(new Point(InputHandler.MouseX, InputHandler.MouseY)) && renderTileSet)
                            {
                                currentTile = new Point(((InputHandler.MouseX-(int)tileSetRenderDrawOffset.X) / GameHandler.TileMap.TileWidth),
                                                        ((InputHandler.MouseY-(int)tileSetRenderDrawOffset.Y) / GameHandler.TileMap.TileHeight));
                                tiles[currentTile.X, currentTile.Y].GetData<Color>(selectedTileData);
                            }
                            else
                                GameHandler.TileMap.Map.SetData<Color>(0, new Rectangle((int)MousePosPixels.X, (int)MousePosPixels.Y, tiles[currentTile.X, currentTile.Y].Width,
                                    tiles[currentTile.X, currentTile.Y].Height), selectedTileData, 0, selectedTileData.Length);
                                GameHandler.TileMap.SetTile(new Point((int)MousePos.X, (int)MousePos.Y), currentTile);
                        }
                        else if (InputHandler.RightClickPressed)
                            GameHandler.TileMap.TogglePassable(new Point((int)MousePos.X, (int)MousePos.Y));
                        break;
                    case Mode.Item:
                        if (InputHandler.LeftClickPressed)
                        {
                            if(GameHandler.CheckItem(new Point((int)MousePos.X, (int)MousePos.Y)) != null)
                            {
                                ItemEntity i = GameHandler.CheckItem(new Point((int)MousePosPixels.X, (int)MousePosPixels.Y));
                                    GameHandler.RemoveItem(i);
                            }
                            if(GameHandler.CheckItem(new Point((int)MousePos.X, (int)MousePos.Y)) == null)
                                GameHandler.AddItem(new ItemEntity(new Vector2(MousePosPixels.X, MousePosPixels.Y), (int)CurrentItem, Game.Content));
                        }
                        if (InputHandler.RightClickPressed)
                        {
                            ItemEntity i = GameHandler.CheckItem(new Point((int)MousePosPixels.X, (int)MousePosPixels.Y));
                            if (i != null)
                                GameHandler.RemoveItem(i);
                        }
                        break;
                    case Mode.Nest:
                        if (InputHandler.LeftClickPressed)
                            GameHandler.AddNest(new Nest(Game.Content.Load<Texture2D>("Sprites\\Nest"), Game.Content,
                                new Point((int)MousePos.X * GameHandler.TileMap.TileWidth, (int)MousePos.Y * GameHandler.TileMap.TileHeight), currentCreatureID,
                                new Point(GameHandler.TileMap.TileWidth, GameHandler.TileMap.TileHeight), new Point(GameHandler.TileMap.TileWidth, GameHandler.TileMap.TileHeight),
                                new Point((int)GameHandler.TileMap.PlayerSpawn.X, (int)GameHandler.TileMap.PlayerSpawn.Y), GameHandler.TileMap.Passable));
                        if (InputHandler.RightClickPressed)
                        {
                            Nest n = GameHandler.CheckNests(new Point((int)MousePos.X * GameHandler.TileMap.TileWidth, (int)MousePos.Y * GameHandler.TileMap.TileHeight));
                            if(n != null)
                                GameHandler.RemoveNest(n);
                        }
                        break;
                    case Mode.Sign:
                        if (InputHandler.LeftClickPressed)
                            GameHandler.AddSign(new Sign(Game.Content.Load<Texture2D>("Sprites/Sign"),
                                new Vector2((int)MousePos.X * GameHandler.TileMap.TileWidth, (int)MousePos.Y * GameHandler.TileMap.TileHeight), currentSignText));
                        if (InputHandler.RightClickPressed)
                        {
                            Sign s = GameHandler.CheckSign(new Point((int)MousePos.X * GameHandler.TileMap.TileWidth, (int)MousePos.Y * GameHandler.TileMap.TileHeight));
                            if (s != null)
                                GameHandler.RemoveSign(s);
                        }
                        break;
                    case Mode.Attribute:
                        if (InputHandler.LeftClickPressed)
                            GameHandler.TileMap.SetAttribute(new Point((int)MousePos.X, (int)MousePos.Y), currentAttribute);

                    break;
                }
                AddModifiedTile(new Point((int)MousePos.X, (int)MousePos.Y)); // Add the tile to the modified list
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteManager.Begin();
            if (currentMode == Mode.Tile)
            {
                if(!(renderTileSet && tileSetRenderRect.Contains(new Point(InputHandler.MouseX, InputHandler.MouseY))))
                    SpriteManager.Draw(tiles[currentTile.X, currentTile.Y], new Vector2(InputHandler.MouseX, InputHandler.MouseY), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);

                if (renderTileSet) // Render the tileset
                {
                    SpriteManager.Draw(tileSetRender, tileSetRenderDrawOffset, Color.White);
                    SpriteManager.Draw(tilePointer, new Vector2((currentTile.X * GameHandler.TileMap.TileWidth) + (tilePointer.Width / 2),
                        (currentTile.Y * GameHandler.TileMap.TileHeight) + (tilePointer.Height / 2)) + tileSetRenderDrawOffset, Color.White);
                }
            }
            if(currentMode != Mode.Attribute)
                GameHandler.TileMap.DrawCollisionLayer();
            if (currentMode == Mode.Attribute)
                GameHandler.TileMap.DrawAttributeLayer();
            SpriteManager.End();

            base.Draw(gameTime);
        }

        private int TileModified(Point position)
        {
            for (int i = 0; i < modifiedTiles.Count; i++)
                if (modifiedTiles[i] == position)
                    return i;
            return -1;
        }

        private void AddModifiedTile(Point tile)
        {
            int tileLocation = TileModified(tile);

            if (tileLocation == -1)
                modifiedTiles.Add(tile);
            else
                modifiedTiles[tileLocation] = tile;
        }

        private void SaveMap()
        {
            Console.WriteLine("Saving Map...");
            string filePath = workingDirectory + "Level" + GameHandler.CurrentLevel + ".map";
            StreamReader sr = new StreamReader(filePath);
            string tilesetName = sr.ReadLine();
            int arraySize = ((GameHandler.TileMap.Width * 6) * GameHandler.TileMap.Height) + (GameHandler.TileMap.Height - 1);
            char[] tileData = new char[arraySize];
            sr.ReadBlock(tileData, 0, arraySize);

            foreach (Point tile in modifiedTiles)
            {
                int[] currentTile = new int[5];

                for (int i = 0; i < 5; i++)
                    currentTile[i] = UnicodeValueToInt(tileData[(tile.X * 6 + tile.Y * ((GameHandler.TileMap.Width * 6) + 1)) + i]);

                currentTile[0] = GameHandler.TileMap.Tiles[tile.X, tile.Y].X;
                currentTile[1] = GameHandler.TileMap.Tiles[tile.X, tile.Y].Y;
                currentTile[2] = Convert.ToInt32(GameHandler.TileMap.Passable[tile.X, tile.Y]);
                currentTile[3] = GameHandler.TileMap.Attribute[tile.X, tile.Y];

                Item item = GameHandler.CheckItem(new Point(tile.X * GameHandler.TileMap.TileWidth, tile.Y * GameHandler.TileMap.TileHeight));
                if (item != null)
                    currentTile[4] = item.ID;
                else
                    currentTile[4] = 0;

                for (int i = 0; i < 5; i++)
                {
                    char c = Convert.ToChar(currentTile[i] + (int)'0');
                    tileData[(tile.X * 6 + tile.Y * ((GameHandler.TileMap.Width * 6) + 1)) + i] = c;
                }
            }

            string spawnData = ""; // Read spawn data and boss code
            for (int i = 0; i < 5; i++)
                spawnData += sr.ReadLine() + "\n";

            sr.ReadLine();
            string NestData = Convert.ToString(GameHandler.Nests.Count) + "\n";
            for (int i = 0; i < GameHandler.Nests.Count; i++) // Create new nest data
                NestData += (GameHandler.Nests[i].CollisionRect.X / GameHandler.TileMap.TileWidth) + "-" + (GameHandler.Nests[i].CollisionRect.Y / GameHandler.TileMap.TileHeight) + "\n" + GameHandler.Nests[i].ID + "\n";

            // Create new sign data
            string SignData = Convert.ToString(GameHandler.Signs.Count) + "\n";
            for (int i = 0; i < GameHandler.Signs.Count; i++)
                SignData += (GameHandler.Signs[i].CollisionRect.X / GameHandler.TileMap.TileWidth) + "-" + (GameHandler.Signs[i].CollisionRect.Y / GameHandler.TileMap.TileHeight) + "\n" + GameHandler.Signs[i].Text.Replace('\n', '#') + "\n";

            sr.Close();

            File.Delete(filePath);

            // Write out new data
            StreamWriter sw = new StreamWriter(filePath, false);
            sw.WriteLine(tilesetName);
            sw.Write(tileData);
            sw.Write(spawnData);
            sw.Write(NestData);
            sw.Write(SignData);
            sw.Close();
        }

        private int UnicodeValueToInt(int val)
        {
            return (char)val - '0';
        }
    }
}

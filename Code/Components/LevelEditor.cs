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
        #region Struct Definitions
        private struct NewItem
        {
            public Point Position;
            public int ItemID;
            public NewItem(Point position, int itemid)
            {
                Position = position;
                ItemID = itemid;
            }
        }
        public struct ModifiedTile
        {
            public Point Position;
            public Point TileXY;
            public bool Passable;
            public ModifiedTile(Point position, Point tileXY, bool passable)
            {
                Position = position;
                TileXY = tileXY;
                Passable = passable;
            }
        }
        #endregion

        private enum Mode
        {
            Tile,
            Item,
            Nest
        }

        Texture2D[,] tiles;
        Point currentTile;
        Color[] selectedTileData;

        List<ModifiedTile> modifiedTiles;
        List<NewItem> newItems;

        Mode currentMode;
        Item.ItemName CurrentItem;
        short currentCreatureID;

        string workingDirectory;

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
                Console.WriteLine("----- Entering Level Editor -----\n");
                Interface.currentScreen = Screens.BLANK;
                GameHandler.EditMode = true;

                tiles = new Texture2D[GameHandler.TileMap.TileSet.Width / GameHandler.TileMap.TileWidth, GameHandler.TileMap.TileSet.Height / GameHandler.TileMap.TileHeight];

                Color[] currentTileData = new Color[GameHandler.TileMap.TileWidth * GameHandler.TileMap.TileHeight];

                int xTiles = GameHandler.TileMap.TileSet.Width/GameHandler.TileMap.TileWidth;
                int yTiles = GameHandler.TileMap.TileSet.Height/GameHandler.TileMap.TileHeight;

                for (int x = 0; x < xTiles; x++)
                {
                    for (int y = 0; y < yTiles; y++)
                    {
                        tiles[x, y] = new Texture2D(GraphicsDevice, GameHandler.TileMap.TileWidth, GameHandler.TileMap.TileHeight);
                        
                        GameHandler.TileMap.TileSet.GetData<Color>(0, new Rectangle(x * GameHandler.TileMap.TileWidth, y * GameHandler.TileMap.TileHeight, GameHandler.TileMap.TileWidth, GameHandler.TileMap.TileHeight),
                            currentTileData, 0, currentTileData.Length);

                        tiles[x, y].SetData<Color>(currentTileData);
                    }
                }

                currentTile = Point.Zero;
                selectedTileData = new Color[GameHandler.TileMap.TileWidth * GameHandler.TileMap.TileHeight];
                Camera.Move(Vector2.Zero);
                modifiedTiles = new List<ModifiedTile>();
                newItems = new List<NewItem>();
            }
            else
            {
                Console.WriteLine("----- Exiting Level Editor. -----\n");
                if (!(Interface.currentScreen == Screens.MainMenu))
                {
                    ConsoleKeyInfo choice = new ConsoleKeyInfo();

                    while (choice.Key != ConsoleKey.N && choice.Key != ConsoleKey.Y)
                    {
                        Console.WriteLine("Save Changes to Current Level? (Y/N)");
                        choice = Console.ReadKey();
                    }

                    switch (choice.Key)
                    {
                        case ConsoleKey.Y:
                            string filePath = workingDirectory + "Level" + GameHandler.CurrentLevel + ".map";
                            StreamReader sr = new StreamReader(filePath);
                            string tilesetName = sr.ReadLine();
                            char[] levelData = sr.ReadToEnd().ToCharArray();

                            sr.Close();

                            foreach (ModifiedTile tile in modifiedTiles)
                            {
                                int[] currentTile = new int[5];

                                for(int i = 0; i < 5; i++)
                                    currentTile[i] = UnicodeValueToInt(levelData[(tile.Position.X * 6 + tile.Position.Y * ((GameHandler.TileMap.Width*6)+1)) + i]);

                                currentTile[0] = tile.TileXY.X;
                                currentTile[1] = tile.TileXY.Y;
                                currentTile[2] = Convert.ToInt32(tile.Passable);

                                for (int i = 0; i < 5; i++)
                                {
                                    char c = Convert.ToChar(currentTile[i] + (int)'0');
                                    levelData[(tile.Position.X * 6 + tile.Position.Y * ((GameHandler.TileMap.Width * 6) + 1)) + i] = c;
                                }
                            }

                            File.Delete(filePath);
                            StreamWriter sw = new StreamWriter(filePath, false);
                            sw.WriteLine(tilesetName);
                            sw.WriteLine(levelData);
                            sw.Close();
                            break;
                        case ConsoleKey.N:
                            break;
                    }

                    Interface.currentScreen = Screens.LevelMenu;
                }
                GameHandler.EditMode = false;
            }
            base.OnEnabledChanged(sender, args);
        }

        public override void Update(GameTime gameTime)
        {
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

            if(InputHandler.KeyPressed(Keys.Z) || InputHandler.KeyPressed(Keys.X))
            {
                switch(currentMode)
                {
                    case Mode.Tile:
                        if (InputHandler.KeyPressed(Keys.Z))
                            if(currentTile.X != 0)
                                currentTile.X--;
                        if (InputHandler.KeyPressed(Keys.X))
                            currentTile.X++;

                        if (currentTile.X > (GameHandler.TileMap.TileSet.Width / GameHandler.TileMap.TileWidth)-1)
                        {
                            currentTile.X = 0;
                            currentTile.Y++;
                        }

                        if (currentTile.Y > (GameHandler.TileMap.TileSet.Height / GameHandler.TileMap.TileHeight)-1)
                            currentTile.Y = 0;

                        tiles[currentTile.X, currentTile.Y].GetData<Color>(selectedTileData);
                        break;
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
                }
            }

            if (InputHandler.LeftClickDown || InputHandler.RightClickPressed)
            {
                if (InputHandler.MouseX > 0 && InputHandler.MouseX < Configuration.Width && InputHandler.MouseY > 0 && InputHandler.MouseY < Configuration.Height)
                {
                    Vector2 MousePos = InputHandler.MouseWorldCoords;

                    MousePos.X /= GameHandler.TileMap.TileWidth;
                    MousePos.Y /= GameHandler.TileMap.TileHeight;

                    switch(currentMode)
                    {
                        case Mode.Tile:
                            if(InputHandler.LeftClickDown)
                                GameHandler.TileMap.Map.SetData<Color>(0, new Rectangle((int)MousePos.X * GameHandler.TileMap.TileWidth, (int)MousePos.Y * GameHandler.TileMap.TileHeight,
                                    tiles[currentTile.X, currentTile.Y].Width, tiles[currentTile.X, currentTile.Y].Height), selectedTileData, 0, selectedTileData.Length);
                            else if(InputHandler.RightClickPressed)
                                GameHandler.TileMap.TogglePassable(new Point((int)MousePos.X, (int)MousePos.Y));

                            ModifiedTile mTile = new ModifiedTile(new Point((int)MousePos.X, (int)MousePos.Y), new Point(currentTile.X, currentTile.Y), GameHandler.TileMap.Passable[(int)MousePos.X, (int)MousePos.Y]);
                            int tileLocation = TileModified(new Point((int)MousePos.X, (int)MousePos.Y));

                            if (tileLocation == -1)
                                modifiedTiles.Add(mTile);
                            else
                                modifiedTiles[tileLocation] = mTile;
                            break;
                        case Mode.Item:
                            if (InputHandler.LeftClickPressed)
                            {
                                if(GameHandler.CheckItem(new Point((int)MousePos.X, (int)MousePos.Y)) != null)
                                {
                                    ItemEntity i = GameHandler.CheckItem(new Point((int)MousePos.X * GameHandler.TileMap.TileWidth, (int)MousePos.Y * GameHandler.TileMap.TileHeight));
                                     GameHandler.RemoveItem(i);
                                }
                                if(GameHandler.CheckItem(new Point((int)MousePos.X, (int)MousePos.Y)) == null)
                                    GameHandler.AddItem(new ItemEntity(new Vector2((int)MousePos.X * GameHandler.TileMap.TileWidth, (int)MousePos.Y * GameHandler.TileMap.TileHeight), (int)CurrentItem, Game.Content));
                            }
                            if (InputHandler.RightClickPressed)
                            {
                                ItemEntity i = GameHandler.CheckItem(new Point((int)MousePos.X * GameHandler.TileMap.TileWidth, (int)MousePos.Y * GameHandler.TileMap.TileHeight));
                                if (i != null)
                                    GameHandler.RemoveItem(i);
                            }

                            break;
                        case Mode.Nest:
                            if (InputHandler.LeftClickPressed)
                                GameHandler.AddNest(new Nest(Game.Content.Load<Texture2D>("Sprites\\Nest"), Game.Content.Load<Texture2D>("Sprites\\CreatureGeneric"),
                                    new Point((int)MousePos.X * GameHandler.TileMap.TileWidth, (int)MousePos.Y * GameHandler.TileMap.TileHeight), currentCreatureID,
                                    new Point(GameHandler.TileMap.TileWidth, GameHandler.TileMap.TileHeight), new Point(GameHandler.TileMap.TileWidth, GameHandler.TileMap.TileHeight),
                                    new Point((int)GameHandler.TileMap.PlayerSpawn.X, (int)GameHandler.TileMap.PlayerSpawn.Y)));
                            if (InputHandler.RightClickPressed)
                            {
                                Nest n = GameHandler.CheckNests(new Point((int)MousePos.X * GameHandler.TileMap.TileWidth, (int)MousePos.Y * GameHandler.TileMap.TileHeight));
                                if(n != null)
                                    GameHandler.RemoveNest(n);
                            }
                            break;
                    }
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteManager.Begin();
            if(currentMode == Mode.Tile)
                SpriteManager.Draw(tiles[currentTile.X, currentTile.Y], new Vector2(InputHandler.MouseX, InputHandler.MouseY), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            GameHandler.TileMap.DrawCollisionLayer();
            SpriteManager.End();

            base.Draw(gameTime);
        }

        private int TileModified(Point position)
        {
            for (int i = 0; i < modifiedTiles.Count; i++)
                if (modifiedTiles[i].Position == position)
                    return i;
            return -1;
        }

        private int UnicodeValueToInt(int val)
        {
            return (char)val - '0';
        }
    }
}

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
            Nest
        }

        Texture2D[,] tiles;
        Point currentTile;
        Color[] selectedTileData;
        List<Point> modifiedTiles;

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
            workingDirectory = Directory.GetCurrentDirectory() + "\\Content\\Maps\\";
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
                modifiedTiles = new List<Point>();
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
                            StreamWriter sw = new StreamWriter(workingDirectory + "Level" + GameHandler.CurrentLevel + ".map");

                            foreach (Point tile in modifiedTiles)
                            {
                                
                            }

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
                    Vector2 MousePos = new Vector2(InputHandler.MouseX, InputHandler.MouseY) + Camera.Position;

                    MousePos.X /= GameHandler.TileMap.TileWidth;
                    MousePos.Y /= GameHandler.TileMap.TileHeight;

                    if (!modifiedTiles.Contains(new Point((int)MousePos.X, (int)MousePos.Y)))
                        modifiedTiles.Add(new Point((int)MousePos.X, (int)MousePos.Y));

                    switch(currentMode)
                    {
                        case Mode.Tile:
                            if(InputHandler.LeftClickDown)
                                GameHandler.TileMap.Map.SetData<Color>(0, new Rectangle((int)MousePos.X * GameHandler.TileMap.TileWidth, (int)MousePos.Y * GameHandler.TileMap.TileHeight,
                                    tiles[currentTile.X, currentTile.Y].Width, tiles[currentTile.X, currentTile.Y].Height), selectedTileData, 0, selectedTileData.Length);
                            else if(InputHandler.RightClickPressed)
                                GameHandler.TileMap.TogglePassable(new Point((int)MousePos.X, (int)MousePos.Y));
                            break;
                        case Mode.Item:
                            if(InputHandler.LeftClickPressed)
                                GameHandler.AddItem(new ItemEntity(new Vector2((int)MousePos.X * GameHandler.TileMap.TileWidth, (int)MousePos.Y * GameHandler.TileMap.TileHeight), (int)CurrentItem, Game.Content));
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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
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
        Texture2D[,] tiles;
        Point currentTile;
        Color[] selectedTileData;
        List<Point> modifiedTiles;

        public LevelEditor(Game game)
            : base(game)
        {
            
        }

        public override void Initialize()
        {
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

            if(InputHandler.KeyPressed(Keys.Z) || InputHandler.KeyPressed(Keys.X))
            {
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
            }

            if (InputHandler.LeftClickDown || InputHandler.RightClickPressed)
            {
                if (InputHandler.MouseX > 0 && InputHandler.MouseX < Configuration.Width && InputHandler.MouseY > 0 && InputHandler.MouseY < Configuration.Height)
                {
                    Vector2 MousePos = new Vector2(InputHandler.MouseX, InputHandler.MouseY) + Camera.Position;

                    MousePos.X /= GameHandler.TileMap.TileWidth;
                    MousePos.Y /= GameHandler.TileMap.TileHeight;

                    modifiedTiles.Add(new Point((int)MousePos.X, (int)MousePos.Y));

                    if(InputHandler.LeftClickDown)
                        GameHandler.TileMap.Map.SetData<Color>(0, new Rectangle((int)MousePos.X * GameHandler.TileMap.TileWidth, (int)MousePos.Y * GameHandler.TileMap.TileHeight,
                            tiles[currentTile.X, currentTile.Y].Width, tiles[currentTile.X, currentTile.Y].Height), selectedTileData, 0, selectedTileData.Length);
                    else if(InputHandler.RightClickPressed)
                        GameHandler.TileMap.TogglePassable(new Point((int)MousePos.X, (int)MousePos.Y));
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteManager.Begin();
            SpriteManager.Draw(tiles[currentTile.X, currentTile.Y], new Vector2(InputHandler.MouseX, InputHandler.MouseY), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            GameHandler.TileMap.DrawCollisionLayer();
            SpriteManager.End();

            base.Draw(gameTime);
        }
    }
}

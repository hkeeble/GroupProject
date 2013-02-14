using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VOiD.Components;

namespace VOiD
{
    public class TileMap
    {
        public int TileWidth = 32;
        public int TileHeight = 32;

        private int _width, _height, _numberOfNests;
        Texture2D _tileSet;
        private string _tileSetPath;
        private Point[,] _tiles;
        private bool[,] _passable;
        private int[,] _attribute;
        Vector2 _playerSpawn, _bossSpawn, _labPos;
        string _fileName;
        bool isLoaded = false;

        Texture2D _map;

        Texture2D _colTex; // Collision Texture

        public TileMap()
        {//NEEDS WORK BLANK MAP
            _width = 0;
            _height = 0;
        }

        /// <summary>
        /// Creates a new map from a file.
        /// </summary>
        /// <param name="filePath">The name of the level file to be loaded. (File must be in the directory of the executable in a directory named "Levels".)</param>
        /// <param name="graphicsDevice">Constructor requires graphics device to load tilset texture from stream.</param>
        public TileMap(string fileName, GraphicsDevice graphicsDevice, Microsoft.Xna.Framework.Content.ContentManager content)
        {
            _colTex = content.Load<Texture2D>("Sprites\\CollisionTexture");

            try
            {
                _fileName = Directory.GetCurrentDirectory() + "\\Content\\Maps\\" + fileName + ".map";
                StreamReader sr = new StreamReader(_fileName);

                try
                {
                    _tileSet = content.Load<Texture2D>("Tilesets/"+sr.ReadLine());
                    isLoaded = true;//CHECK THIS PLEASE
                }
                catch (FileNotFoundException e)
                {
                    DebugLog.WriteLine("Error loading tileset for level " + fileName + " error message: \n" + e.Message);
                }

                // Get Map Width
                sr.Close();
                sr = new StreamReader(_fileName);
                sr.ReadLine();
                string[] widthString = sr.ReadLine().Split(' ');
                _width = widthString.Length;
                
                // Get Map Height
                sr.Close();
                sr = new StreamReader(_fileName);
                sr.ReadLine();
                int counter = 0;
                while (!sr.ReadLine().Contains('-'))
                    counter++;
                _height = counter;

                sr.Close();
                sr = new StreamReader(_fileName);

                _map = new Texture2D(graphicsDevice, _width*TileWidth, _height*TileHeight, true, SurfaceFormat.Color);
                
                _tileSetPath = sr.ReadLine();

                _tiles = new Point[_width, _height];
                _passable = new bool[_width, _height];
                _attribute = new int[_width, _height];

                try
                {
                    Color[] currentTile = new Color[TileWidth*TileHeight];
                    int xPos;
                    int yPos;

                    for (int y = 0; y < _height; y++)
                    {
                        for (int x = 0; x < _width; x++)
                        {
                            xPos = UnicodeValueToInt(sr.Read());
                            yPos = UnicodeValueToInt(sr.Read());
                            _tileSet.GetData<Color>(0, new Rectangle(xPos*TileWidth,yPos*TileHeight,TileWidth,TileHeight), currentTile, 0, TileWidth*TileHeight);
                            _map.SetData<Color>(0, new Rectangle(x * TileWidth, y * TileHeight, TileWidth, TileHeight), currentTile, 0, TileWidth * TileHeight);
                            _tiles[x, y] = new Point(xPos, yPos);

                            _passable[x, y] = Convert.ToBoolean(UnicodeValueToInt(sr.Read()));
                            _attribute[x, y] = UnicodeValueToInt(sr.Read());

                            int itemID = UnicodeValueToInt(sr.Read());
                            if (itemID != 0)
                            {
                                ItemEntity temp = new ItemEntity(new Vector2(x * TileHeight, y * TileWidth), itemID, content);
                                GameHandler.AddItem(temp);
                            }
                            sr.Read();
                        }
                        sr.ReadLine();
                    }
                }
                catch (Exception e)
                {
                    DebugLog.WriteLine("Error reading tile data from level " + fileName + " error message: \n" + e.Message);
                }
                GenerateMipMap(graphicsDevice, ref _map);

                string pSpawn = sr.ReadLine();
                string[] split = pSpawn.Split('-');
                _playerSpawn.X = Convert.ToInt32(split[0]) * TileWidth;
                _playerSpawn.Y = Convert.ToInt32(split[1]) * TileHeight;

                string bSpawn = sr.ReadLine();
                split = bSpawn.Split('-');
                _bossSpawn.X = Convert.ToInt32(split[0]) * TileWidth;
                _bossSpawn.Y = Convert.ToInt32(split[1]) * TileHeight;

                string lPos = sr.ReadLine();
                split = lPos.Split('-');
                _labPos.X = Convert.ToInt32(split[0]) * TileWidth;
                _labPos.Y = Convert.ToInt32(split[1]) * TileHeight;

                GameHandler.Boss = new Creature(Convert.ToInt16(sr.ReadLine()), content.Load<Texture2D>("Sprites\\CreatureGeneric"),_bossSpawn, 1f);

                _numberOfNests = Convert.ToInt32(sr.ReadLine());

                for (int i = 0; i < _numberOfNests; i++)
                {
                    string cPos = sr.ReadLine();
                    split = cPos.Split('-');
                    GameHandler.AddNest(new Nest(content.Load<Texture2D>("Sprites\\Nest"), content.Load<Texture2D>("Sprites\\CreatureGeneric"),
                        new Point(Convert.ToInt32(split[0])*TileWidth,Convert.ToInt32(split[1])*TileHeight), Convert.ToInt16(sr.ReadLine()), new Point(_width, _height),
                        new Point(TileWidth, TileHeight), new Point((int)_playerSpawn.X, (int)_playerSpawn.Y), _passable));
                }

                sr.Close();
            }
            catch (FileNotFoundException e)
            {
                e.ToString();
                DebugLog.WriteLine("Level " + fileName + " failed to load. File not found.\n");
            }
            catch (Exception e)
            {
                e.ToString();
                DebugLog.WriteLine("Error loading level named " + fileName + " error message: \n" + e.Message);
            }

          
        }

        /// <summary>
        /// Draws the map.
        /// </summary>
        public void Draw()
        {
            if (isLoaded)
                SpriteManager.Draw(_map, Camera.Transform(Vector2.Zero), Color.White);
        }

        public void DrawCollisionLayer()
        {
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    if (_passable[x, y] == false)
                        SpriteManager.Draw(_colTex, Camera.Transform(new Vector2(x * TileWidth, y * TileHeight)), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
        }

        private int UnicodeValueToInt(int val)
        {
            return (char)val - '0';
        }

        private void GenerateMipMap(GraphicsDevice graphicsDevice, ref Texture2D image)
        {
            RenderTarget2D target = new RenderTarget2D(graphicsDevice, image.Width, image.Height, true, SurfaceFormat.Color, DepthFormat.None);
            graphicsDevice.SetRenderTarget(target);
            graphicsDevice.Clear(Color.Black);
            SpriteManager.Begin();
            SpriteManager.Draw(image, Vector2.Zero, Color.White);
            SpriteManager.End();
            graphicsDevice.SetRenderTarget(null);
            image.Dispose();
            image = (Texture2D)target;
        }

        public void TogglePassable(Point tile)
        {
            _passable[tile.X, tile.Y] = !_passable[tile.X, tile.Y];
        }

        /// <summary>
        /// Sets an individual tile.
        /// </summary>
        /// <param name="Position">Position (in tile coordinates) of the tile to set.</param>
        /// <param name="TileXY">Position (in tile coordinates) of the tile in the tileset to use.</param>
        public void SetTile(Point Position, Point TileXY)
        {
            _tiles[Position.X, Position.Y] = TileXY;
        }

        // Public Accessors
        public Vector2 PlayerSpawn { get { return _playerSpawn; } }
        public Vector2 BossSpawn { get { return _bossSpawn; } }
        public Vector2 LabPosition { get { return _labPos; } }
        public int Width { get { return _width; } }
        public int Height { get { return _height; } }
        public Texture2D TileSet { get { return _tileSet; } }
        public bool[,] Passable { get { return _passable; } }
        public int[,] Attribute { get { return _attribute; } }
        public Texture2D Map { get { return _map; } }
        public Point[,] Tiles { get { return _tiles; } }
    }
}
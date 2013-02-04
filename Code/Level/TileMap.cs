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
    class TileMap
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
        // Creature Code Array?
        string _fileName;
        bool isLoaded = false;
        // Apple Array?

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
            try
            {
                _fileName = Directory.GetCurrentDirectory() + "\\Content\\Maps\\" + fileName;
                TextReader tr = new StreamReader(_fileName);

                _width = Convert.ToInt32(tr.ReadLine());
                _height = Convert.ToInt32(tr.ReadLine());
                _tileSetPath = tr.ReadLine();

                try
                {
                    _tileSet = content.Load<Texture2D>("Tilesets/Tileset");
                    isLoaded = true;//CHECK THIS PLEASE
                }
                catch(FileNotFoundException e)
                {
                    DebugLog.WriteLine("Error loading tileset for level " + fileName + " error message: \n" + e.Message);
                }

                _tiles = new Point[_width, _height];
                _passable = new bool[_width, _height];
                _attribute = new int[_width, _height];

                try
                {
                    for (int x = 0; x < _width; x++)
                    {
                        for (int y = 0; y < _height; y++)
                        {
                            _tiles[x, y].X = UnicodeValueToInt(tr.Read());
                            _tiles[x, y].Y = UnicodeValueToInt(tr.Read());
                            _passable[x, y] = Convert.ToBoolean(UnicodeValueToInt(tr.Read()));
                            _attribute[x, y] = UnicodeValueToInt(tr.Read());

                            int itemID = UnicodeValueToInt(tr.Read());
                            if (itemID != 0)
                            {
                                ItemEntity temp = new ItemEntity(new Vector2(y * TileHeight, x * TileWidth), itemID, content);
                                GameHandler.AddItem(temp);
                            }
                            tr.Read();
                        }
                        tr.ReadLine();
                    }
                }
                catch (Exception e)
                {
                    DebugLog.WriteLine("Error reading tile data from level " + fileName + " error message: \n" + e.Message);
                }

                string pSpawn = tr.ReadLine();
                string[] split = pSpawn.Split('-');
                _playerSpawn.X = Convert.ToInt32(split[0]) * TileWidth;
                _playerSpawn.Y = Convert.ToInt32(split[1]) * TileHeight;

                string bSpawn = tr.ReadLine();
                split = bSpawn.Split('-');
                _bossSpawn.X = Convert.ToInt32(split[0]) * TileWidth;
                _bossSpawn.Y = Convert.ToInt32(split[1]) * TileHeight;

                string lPos = tr.ReadLine();
                split = lPos.Split('-');
                _labPos.X = Convert.ToInt32(split[0]) * TileWidth;
                _labPos.Y = Convert.ToInt32(split[1]) * TileHeight;

                GameHandler.Boss = new Creature(Convert.ToInt16(tr.ReadLine()), content.Load<Texture2D>("Sprites\\CreatureGeneric"),_bossSpawn, 1f);

                _numberOfNests = Convert.ToInt32(tr.ReadLine());

                for (int i = 0; i < _numberOfNests; i++)
                {
                    string cPos = tr.ReadLine();
                    split = cPos.Split('-');
                    GameHandler.AddNest(new Nest(content.Load<Texture2D>("Sprites\\Nest"), content.Load<Texture2D>("Sprites\\CreatureGeneric"),
                        new Point(Convert.ToInt32(split[0])*TileWidth,Convert.ToInt32(split[1])*TileHeight), Convert.ToInt16(tr.ReadLine()), new Point(_width, _height),
                        new Point(TileWidth, TileHeight), new Point((int)_playerSpawn.X, (int)_playerSpawn.Y)));
                }

                tr.Close();
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
        /// Draws the level's tiles.
        /// </summary>
        public void Draw()
        {
            if (isLoaded)
                for (int x = 0; x < _width; x++)
                    for (int y = 0; y < _height; y++)
                        SpriteManager.Draw(_tileSet, Camera.Transform(new Vector2(x * TileWidth, y * TileHeight)), new Rectangle(_tiles[y, x].X * TileWidth, _tiles[y, x].Y * TileHeight, TileWidth, TileHeight), Color.White,
                            0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 0f);
        }

        /// <summary>
        /// Returns a block of tiles from the map. Only returns XY data.
        /// </summary>
        /// <param name="x">The X coordinate of the top left corner of the desired block.</param>
        /// <param name="y">The Y coordinate of the top left corner of the desired block.</param>
        /// <param name="width">The width of the block to return.</param>
        /// <param name="height">The height of the block to return</param>
        /// <returns></returns>
        public Point[,] GetTileBlock(int x, int y, int width, int height)
        {
            Point[,] block = new Point[width, height];

            try
            {
                for (int i = 0; i < width; i++)
                    for (int j = 0; j < height; y++)
                        block[i, j] = _tiles[i + x, j + y];
            }
            catch (Exception e)
            {
                DebugLog.WriteLine("Error retrieving tile block of size at XY "  + x + "," + y + " of width and height " + width + "," + height + " from level located at " + _fileName +
                    "\nError Message: " +  e.Message + "\n");
            }

            return block;
        }

        private int UnicodeValueToInt(int val)
        {
            return (char)val - '0';
        }

        // Public Accessors
        public Vector2 PlayerSpawn { get { return _playerSpawn; } }
        public Vector2 BossSpawn { get { return _bossSpawn; } }
        public int Width { get { return _width; } }
        public int Height { get { return _height; } }
        public Texture2D TileSet { get { return _tileSet; } }
        public bool[,] Passable { get { return _passable; } }
        public int[,] Attribute { get { return _attribute; } }

        /// <summary>
        /// Returns the size of the map in pixels (mapSize*tileSize)
        /// </summary>
        public Rectangle PixelSize { get { return new Rectangle(0, 0, _width * TileWidth, _height * TileHeight); } }
    }
}
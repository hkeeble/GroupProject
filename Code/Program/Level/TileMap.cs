using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CreatureGame
{
    class TileMap
    {
        private int _width, _height, _tileWidth, _tileHeight, _numberOfNests, _numberOfApples;
        Texture2D _tileSet;
        private string _tileSetPath;
        private Point[,] _tiles;
        private bool[,] _passable;
        private int[,] _attribute;
        Point _playerSpawn, _bossSpawn, _labPos;
        // Creature Code Array?
        Point[] _applePositions, _nestPositions;
        string _fileName;
        // Apple Array?

        /// <summary>
        /// Creates a new map from a file.
        /// </summary>
        /// <param name="filePath">The name of the level file to be loaded. (File must be in the directory of the executable in a directory named "Levels".)</param>
        /// <param name="graphicsDevice">Constructor requires graphics device to load tilset texture from stream.</param>
        public TileMap(string fileName, GraphicsDevice graphicsDevice)
        {
            try
            {
                _fileName = Directory.GetCurrentDirectory() + "\\Levels\\" + fileName;
                TextReader tr = new StreamReader(_fileName);

                _width = Convert.ToInt32(tr.ReadLine());
                _height = Convert.ToInt32(tr.ReadLine());
                _tileWidth = Convert.ToInt32(tr.ReadLine());
                _tileHeight = Convert.ToInt32(tr.ReadLine());
                _tileSetPath = tr.ReadLine();

                try
                {
                    FileStream tileSetStream = File.OpenRead(Directory.GetCurrentDirectory() + "\\Tilesets\\" + _tileSetPath);
                    _tileSet = Texture2D.FromStream(graphicsDevice, tileSetStream);
                    tileSetStream.Close();
                }
                catch(FileNotFoundException e)
                {
                    DebugLog.WriteLine("Error loading tilset for level " + fileName + " error message: \n" + e.Message);
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
                            tr.Read();
                        }
                        tr.ReadLine();
                    }
                }
                catch (Exception e)
                {
                    DebugLog.WriteLine("Error reading tile data from level " + fileName + " error message: \n" + e.Message);
                }
                //tr.ReadLine();
                //// READ BOSS CODE HERE
                tr.ReadLine();

                string pSpawn = tr.ReadLine();
                string[] split = pSpawn.Split('-');
                _playerSpawn.X = Convert.ToInt32(split[0]);
                _playerSpawn.Y = Convert.ToInt32(split[1]);

                string bSpawn = tr.ReadLine();
                split = bSpawn.Split('-');
                _bossSpawn.X = Convert.ToInt32(split[0]);
                _bossSpawn.Y = Convert.ToInt32(split[1]);

                string lPos = tr.ReadLine();
                split = lPos.Split('-');
                _labPos.X = Convert.ToInt32(split[0]);
                _labPos.Y = Convert.ToInt32(split[1]);

                tr.ReadLine();

                //_numberOfNests = Convert.ToInt32(tr.ReadLine());
                //_nestPositions = new Point[_numberOfNests];
                //for (int i = 0; i < _numberOfNests; i++)
                //{
                //    // READ CREATURE CODE - READLINE
                //    _nestPositions[i].X = Convert.ToInt32(tr.Read());
                //    _nestPositions[i].Y = Convert.ToInt32(tr.Read());
                //    tr.ReadLine(); // Move to next line
                //}

                //tr.ReadLine();

                //_numberOfApples = Convert.ToInt32(tr.ReadLine());
                //_applePositions = new Point[_numberOfApples];
                //for (int i = 0; i < _numberOfApples; i++)
                //{
                //    _applePositions[i].X = Convert.ToInt32(tr.Read());
                //    _applePositions[i].Y = Convert.ToInt32(tr.Read());
                //    tr.ReadLine();
                //}

                tr.Close();
            }
            catch (FileNotFoundException e)
            {
                DebugLog.WriteLine("Level " + fileName + " failed to load. File not found.\n");
            }
            catch (Exception e)
            {
                DebugLog.WriteLine("Error loading level named " + fileName + " error message: \n" + e.Message);
            }

          
        }

        /// <summary>
        /// Draws the level's tiles and the lab.
        /// </summary>
        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
                for (int x = 0; x < _width; x++)
                {
                    for (int y = 0; y < _height; y++)
                    {
                            spriteBatch.Draw(_tileSet, Camera.Transform(new Vector2(x*_tileWidth, y*_tileHeight)), new Rectangle(_tiles[y,x].X*_tileWidth,
                                _tiles[y,x].Y*_tileHeight, _tileWidth, _tileHeight), Color.White);
                    }
                }

                //for (int i = 0; i < _numberOfApples; i++)
                //{
                //    // DRAW APPLES
                //}

                //for (int i = 0; i < _numberOfNests; i++)
                //{
                //    // DRAW NESTS
                //}

                // DRAW LAB
            spriteBatch.End();
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
        public Point[] ApplePositions { get { return _applePositions; } set { _applePositions = value; } }
        public Point[] NestPositions { get { return _nestPositions; } }
        public Point PlayerSpawn { get { return _playerSpawn; } }
        public Point BossSpawn { get { return _bossSpawn; } }
        public int Width { get { return _width; } }
        public int Height { get { return _height; } }
        public int TileWidth { get { return _tileWidth; } }
        public int TileHeight { get { return _tileHeight; } }
        public Texture2D TileSet { get { return _tileSet; } }
        public bool[,] Passable { get { return _passable; } }
        public int[,] Attribute { get { return _attribute; } }
    }
}
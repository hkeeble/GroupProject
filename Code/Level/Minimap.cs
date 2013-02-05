using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VOiD
{
    class Minimap
    {
        const int WIDTH = 20; const int HEIGHT = 20; // Width and Height of MiniMap in number of regions
        const int REGION_WIDTH = 8; const int REGION_HEIGHT = 8; // Width and Height of region tiles in pixels
        int _tileWidth, _tileHeight;
        Point[,] _regionTiles;
        Texture2D _tileSet;
        Texture2D minimap;

        /*
        public void stuff()
        {
            minimap = new Texture2D(GraphicsDevice, WIDTH, HEIGHT,false,SurfaceFormat.Color);



            for (int x = 0; x < WIDTH; x++)
            {
                for (int y = 0; y < HEIGHT; y++)
                {
                    Rectangle tileblock = new Rectangle(_regionTiles[x, y].X * _tileWidth, _regionTiles[x, y].Y * _tileWidth, REGION_WIDTH, REGION_HEIGHT);
                    minimap.SetData<Color>(0,_tileSet.GetData<Color>(0,,TileMap,0,, new Vector2(x * REGION_WIDTH, y * REGION_HEIGHT), Color.White);
                }
                spriteBatch.End();
            }
        }
        */
        public Minimap(TileMap map)
        {
            _tileWidth = map.TileWidth;
            _tileHeight = map.TileHeight;
            _tileSet = map.TileSet;

            int mapHeight = map.Height;
            int mapWidth = map.Width;

            if (mapHeight % 2 > 1)
                mapHeight--;
            if (mapWidth % 2 > 1)
                mapWidth--;

            int blockWidth = mapWidth / WIDTH;
            int blockHeight = mapHeight / HEIGHT;

            for (int x = 0; x < WIDTH; x++)
            {
                for (int y = 0; y < HEIGHT; y++)
                {
                    Point[,] currentBlock = map.GetTileBlock(x, y, blockWidth, blockHeight);

                    SortedList<string, int> occurringTiles = new SortedList<string, int>();

                    // Find all types of tile
                    for (int i = 0; i < blockWidth; i++)
                        for (int j = 0; j < blockWidth; j++)
                        {
                            string currentTile = Convert.ToString(currentBlock[i, j].X) + Convert.ToString(currentBlock[i, j].Y);
                            if (!(occurringTiles.Keys.Contains(currentTile)))
                                occurringTiles.Add(currentTile, 1);
                            else
                                occurringTiles.Values[occurringTiles.IndexOfKey(currentTile)] += 1;
                        }

                    string currentMeanTile = occurringTiles.Keys[0];
                    int occurences = occurringTiles.Values[0];

                    for (int i = 0; i < occurringTiles.Count; i++)
                        if (occurringTiles.Values[i] > occurences)
                        {
                            currentMeanTile = occurringTiles.Keys[i];
                            occurences = occurringTiles.Values[i];
                        }

                    _regionTiles[x, y].X = Convert.ToInt32(currentMeanTile[0]);
                    _regionTiles[x, y].Y = Convert.ToInt32(currentMeanTile[1]);
                }
            }
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
            for (int x = 0; x < WIDTH; x++)
            {
                for (int y = 0; y < HEIGHT; y++)
                {
                    spriteBatch.Draw(_tileSet, new Vector2(x * REGION_WIDTH, y * REGION_HEIGHT), new Rectangle(_regionTiles[x, y].X * _tileWidth, _regionTiles[x, y].Y * _tileWidth,
                        REGION_WIDTH, REGION_HEIGHT), Color.White);
                }
                spriteBatch.End();
            }
        }
    }
}

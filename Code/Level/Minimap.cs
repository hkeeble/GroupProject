using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VOiD.Components;

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
        public Minimap(Texture2D map, GraphicsDevice graphics)
        {
            int width = map.Width / (GameHandler.TileMap.TileWidth/2);
            int height = map.Height / (GameHandler.TileMap.TileHeight/2);

            Color[] mapData = new Color[width * height];

            map.GetData<Color>(2, new Rectangle(0,0,width,height), mapData, 0, mapData.Length);
        }
    }
}
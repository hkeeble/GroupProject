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
        const int MIPMAP_LEVEL = 3;
        Texture2D minimap;

        public Minimap(Texture2D map, Texture2D nestTex, Texture2D labTex, GraphicsDevice graphics)
        {
            int divisor = (int)Math.Pow(2.0, (double)MIPMAP_LEVEL);

            int width = map.Width/divisor;
            int height = map.Height/divisor;

            // Data for MipMaps
            Color[] mapData = new Color[width * height];
            Color[] nestData = new Color[(nestTex.Width / divisor) * (nestTex.Height / divisor)];
            Color[] labData = new Color[(labTex.Width / divisor) * (labTex.Height / divisor)];

            // Get MipMap Data
            map.GetData<Color>(MIPMAP_LEVEL,null, mapData, 0, mapData.Length);
            nestTex.GetData<Color>(MIPMAP_LEVEL, null, nestData, 0, nestData.Length);
            labTex.GetData<Color>(MIPMAP_LEVEL, null, labData, 0, labData.Length);

            // Create Minimap
            minimap = new Texture2D(graphics, width, height);
            minimap.SetData<Color>(mapData);
            
            // Impose Nest Textures
            for (int x = 0; x < map.Width; x+=GameHandler.TileMap.TileWidth)
                for (int y = 0; y < map.Height; y+=GameHandler.TileMap.TileHeight)
                    if (GameHandler.CheckNests(new Rectangle(x, y, GameHandler.TileMap.TileWidth, GameHandler.TileMap.TileHeight)))
                        minimap.SetData<Color>(0, new Rectangle(x/divisor, y/divisor, nestTex.Width / divisor, nestTex.Height / divisor), nestData, 0, nestData.Length);

            // Impose Lab Texture
            minimap.SetData<Color>(0, new Rectangle((int)GameHandler.TileMap.LabPosition.X / divisor, (int)GameHandler.TileMap.LabPosition.Y/ divisor, labTex.Width / divisor, labTex.Height / divisor),
                labData, 0, labData.Length);
        }

        public void Draw()
        {
            SpriteManager.Draw(minimap, new Rectangle(50, Configuration.Height - 128 - 10, 128 + 8, 128), new Rectangle((int)GameHandler.player.Position.X / 8,
                (int)GameHandler.player.Position.Y / 8, 128, 128), Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace CreatureGame
{
    class Level
    {
        private static TileMap _tileMap;
        HandlerEntity handler;
        private List<Entity> entities = new List<Entity>();
        //private Minimap _miniMap;
        // PLAYER
        // CREATURE ARRAY

        public Level(string filePath, GraphicsDevice graphicsDevice, HandlerEntity handler)
        {
            _tileMap = new TileMap(filePath, graphicsDevice);
            Camera.MapRectangle = new Rectangle(0, 0, _tileMap.Width * _tileMap.TileWidth, _tileMap.Height * _tileMap.TileHeight);
            Camera.ViewPortSize = new Vector2(graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);

            this.handler = handler;
            this.handler.Position = new Vector2(_tileMap.PlayerSpawn.X, _tileMap.PlayerSpawn.Y);

            Camera.Position = new Vector2((_tileMap.PlayerSpawn.X + (handler.Texture.Width / 2)) - (graphicsDevice.Viewport.Width / 2),
                (_tileMap.PlayerSpawn.Y + (handler.Texture.Height / 2)) - (graphicsDevice.Viewport.Height / 2));

            //_miniMap = new Minimap(_tileMap);
        }

        public void draw(SpriteBatch spriteBatch)
        {
            //DRAW PLAYER
            // DRAW CREATURES
            _tileMap.draw(spriteBatch);

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
            if(entities.Count > 0)
                foreach (Entity e in entities)
                    e.draw(spriteBatch);
            handler.draw(spriteBatch);
            spriteBatch.End();
            
            //_miniMap.draw(spriteBatch);
        }

        public void update(Rectangle viewPort)
        {
            if(entities.Count > 0)
                foreach (Entity e in entities)
                    e.Update();
            handler.Update();

            Camera.Position = new Vector2((handler.Position.X + (handler.Texture.Width / 2)) - (viewPort.Width / 2),
                                 (handler.Position.Y + (handler.Texture.Height / 2)) - (viewPort.Height / 2));
            // UPDATE PLAYER
            // UPDATE CREATURES
            // UPDATE CAMERA
        }

        public static TileMap Map { get { return _tileMap; } }
    }
}
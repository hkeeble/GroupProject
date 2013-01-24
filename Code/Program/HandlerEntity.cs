using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CreatureGame
{
    class HandlerEntity : DrawableGameComponent
    {
        public static TileMap TileMap;
        private static List<Entity> entities = new List<Entity>();
        //private Minimap _miniMap;
        public static Creature player;
        //public static Inventory inventory;

        public HandlerEntity(Game game):base(game)
        {
            TileMap = new TileMap();
            player = new Creature(00000000, new Texture2D(game.GraphicsDevice, 1, 1), Vector2.Zero, 1f);
        }

        public void LoadSave(Game game, string filePath)
        {
            TileMap = new TileMap(filePath, game.GraphicsDevice);
            Camera.MapRectangle = new Rectangle(0, 0, TileMap.Width * TileMap.TileWidth, TileMap.Height * TileMap.TileHeight);
            Camera.ViewPortSize = new Vector2(game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);


            HandlerEntity.player.Position = new Vector2(TileMap.PlayerSpawn.X, TileMap.PlayerSpawn.Y);

            Camera.Position = new Vector2((TileMap.PlayerSpawn.X + (HandlerEntity.player.Texture.Width / 2)) - (game.GraphicsDevice.Viewport.Width / 2),
                (TileMap.PlayerSpawn.Y + (HandlerEntity.player.Texture.Height / 2)) - (game.GraphicsDevice.Viewport.Height / 2));


            //_miniMap = new Minimap(TileMap);
            player = new Creature(00000000, new Texture2D(game.GraphicsDevice,1,1), Vector2.Zero, 1f);
        }

        public override void Update(GameTime gameTime)
        {
            if (entities.Count > 0)
                foreach (Entity e in entities)
                    e.Update();

            Camera.Position = new Vector2((HandlerEntity.player.Position.X + (HandlerEntity.player.Texture.Width / 2)) - (Viewport.Width / 2),
                                 (HandlerEntity.player.Position.Y + (HandlerEntity.player.Texture.Height / 2)) - (Viewport.Height / 2));

            /*
            if (InputHandler.KeyDown(Microsoft.Xna.Framework.Input.Keys.Down))
                _velocity.Y = 1;
            else if (InputHandler.KeyDown(Microsoft.Xna.Framework.Input.Keys.Up))
                _velocity.Y = -1;
            else if (Position.Y % Level.Map.TileHeight == 1)
                _velocity.Y = 0;
            if (InputHandler.KeyDown(Microsoft.Xna.Framework.Input.Keys.Left))
                _velocity.X = -1;
            else if (InputHandler.KeyDown(Microsoft.Xna.Framework.Input.Keys.Right))
                _velocity.X = 1;
            else if (Position.X % Level.Map.TileWidth == 1)
                _velocity.X = 0;
            */

            player.Update();
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatchComponent.Begin();
            // DRAW PLAYER
            // DRAW CREATURES

            TileMap.Draw();

            if (entities.Count > 0)
                foreach (Entity e in entities)
                    e.Draw();

            //_miniMap.draw(spriteBatch);
            player.Draw();
            SpriteBatchComponent.End();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace VOiD.Components
{
    class GameHandler : DrawableGameComponent
    {
        public static TileMap TileMap;
        private static Creature boss;
        private static List<Nest> nests = new List<Nest>();
        private static List<ItemEntity> items = new List<ItemEntity>();
        //private Minimap _miniMap;
        public static Creature player;
        public static Inventory inventory;

        const int NUMBER_OF_ITEM_TYPES = 5;

        public GameHandler(Game game)
            : base(game)
        {
            TileMap = new TileMap("TestMap.txt", game.GraphicsDevice, game.Content);
            player = new Creature(2345, game.Content.Load<Texture2D>("Sprites\\handler"), GameHandler.TileMap.PlayerSpawn, 1f);
            inventory = new Inventory(NUMBER_OF_ITEM_TYPES, game.Content);
        }

        public static Creature Boss { get { return boss; } set { boss = value; } }

        public static void AddItem(ItemEntity item)
        {
            items.Add(item);
        }

        public static void AddNest(Nest nest)
        {
            nests.Add(nest);
        }

        public void LoadSave(Game game, string filePath)
        {
            TileMap = new TileMap(filePath, game.GraphicsDevice, game.Content);
            Camera.MapRectangle = new Rectangle(0, 0, TileMap.Width * TileMap.TileWidth, TileMap.Height * TileMap.TileHeight);

            GameHandler.player.Position = new Vector2(TileMap.PlayerSpawn.X, TileMap.PlayerSpawn.Y);

            Camera.Position = new Vector2((TileMap.PlayerSpawn.X + (GameHandler.player.Texture.Width / 2)) - (Configuration.Width / 2),
                (TileMap.PlayerSpawn.Y + (GameHandler.player.Texture.Height / 2)) - (Configuration.Height / 2));

            //_miniMap = new Minimap(TileMap);
            player = new Creature(00000000, new Texture2D(game.GraphicsDevice,1,1), Vector2.Zero, 1f);
        }

        public override void Update(GameTime gameTime)
        {
            if (nests.Count > 0)
                foreach (Nest n in nests)
                    n.Update();

            Camera.Position = new Vector2((GameHandler.player.Position.X + (GameHandler.player.Texture.Width / 2)) - (Configuration.Width / 2),
                                 (GameHandler.player.Position.Y + (GameHandler.player.Texture.Height / 2)) - (Configuration.Height / 2));

            HandlePlayerMovement();

            for(int i = 0; i < items.Count; i++) // NOT VERY EFFICIENT - MAY NEED REPLACING
            {
                if (player.CollisionRect.Intersects(items[i].CollisionRect))
                {
                    inventory.AddItem(items[i]);
                    items.Remove(items[i]);
                }
            }

            for (int i = 0; i < nests.Count; i++)
            {
                for (int j = 0; j < nests[i].Creatures.Count; j++)
                {
                    if (nests[i].Creatures[j].CollisionRect.Intersects(player.CollisionRect))
                    {
                        // INVOKE BATTLE HERE
                    }
                }
            }

            player.Update();
            base.Update(gameTime);
        }

        private void HandlePlayerMovement()
        {
            if (player.Direction.Y == 0 && player.Position.X % TileMap.TileWidth == 0)
            {
                if (InputHandler.KeyDown(Keys.Down))
                    player.Direction.Y = 1;
                else if (InputHandler.KeyDown(Keys.Up))
                    player.Direction.Y = -1;
            }
            else if (player.Position.Y % TileMap.TileHeight == 0)
            {
                if (player.Direction.Y == 1 && !InputHandler.KeyDown(Keys.Down))
                    player.Direction.Y = 0;
                if (player.Direction.Y == -1 && !InputHandler.KeyDown(Keys.Up))
                    player.Direction.Y = 0;
            }

            if (player.Direction.X == 0 && player.Position.Y % TileMap.TileHeight == 0)
            {
                if (InputHandler.KeyDown(Keys.Left))
                    player.Direction.X = -1;
                else if (InputHandler.KeyDown(Keys.Right))
                    player.Direction.X = 1;
            }
            else if (player.Position.X % TileMap.TileWidth == 0)
            {
                if (player.Direction.X == 1 && !InputHandler.KeyDown(Keys.Right))
                    player.Direction.X = 0;
                if (player.Direction.X == -1 && !InputHandler.KeyDown(Keys.Left))
                    player.Direction.X = 0;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteManager.Begin(SpriteSortMode.FrontToBack,BlendState.AlphaBlend);
            // DRAW PLAYER
            // DRAW CREATURES

            player.Draw();
            TileMap.Draw();

            if (nests.Count > 0)
                foreach (Nest n in nests)
                    n.Draw();

            if (items.Count > 0)
                foreach (ItemEntity i in items)
                    i.Draw();

            //_miniMap.draw(spriteBatch);
            SpriteManager.End();
        }
    }
}
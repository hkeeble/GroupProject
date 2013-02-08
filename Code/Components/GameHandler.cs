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
        public static int CurrentLevel;
        public static TileMap TileMap;
        public static Creature Boss;
        public static Entity Lab;
        public static Minimap Minimap;
        public static Creature Player;
        public static Inventory Inventory;
        public static bool Enabled = true;

        private static List<Nest> Nests = new List<Nest>();
        private static List<ItemEntity> Items = new List<ItemEntity>();

        const int NUMBER_OF_ITEM_TYPES = 5;

        public GameHandler(Game game)
            : base(game)
        {
            Inventory = new Inventory(NUMBER_OF_ITEM_TYPES, game.Content);
            SaveHandler.LoadSave(game.GraphicsDevice, Game.Content);
        }

        public static void AddItem(ItemEntity item)
        {
            Items.Add(item);
        }

        public static void AddNest(Nest nest)
        {
            Nests.Add(nest);
        }

        public override void Update(GameTime gameTime)
        {
            if (Enabled)
            {
                if (Nests.Count > 0)
                    foreach (Nest n in Nests)
                        n.Update();
                
                Camera.Position = new Vector2((GameHandler.Player.Position.X + (GameHandler.Player.Texture.Width / 2)) - (Configuration.Width / 2),
                                     (GameHandler.Player.Position.Y + (GameHandler.Player.Texture.Height / 2)) - (Configuration.Height / 2));

                if (Interface.currentScreen == Screens.LevelMenu)
                {
                    HandlePlayerMovement();
                    HandleMouse();
                }

                for (int i = 0; i < Items.Count; i++) // NOT VERY EFFICIENT - MAY NEED REPLACING
                {
                    if (Player.CollisionRect.Intersects(Items[i].CollisionRect))
                    {
                        Inventory.AddItem(Items[i]);
                        Items.Remove(Items[i]);
                    }
                }

                for (int i = 0; i < Nests.Count; i++)
                {
                    for (int j = 0; j < Nests[i].Creatures.Count; j++)
                    {
                        if (Nests[i].Creatures[j].CollisionRect.Intersects(Player.CollisionRect))
                        {
                            // INVOKE BATTLE HERE
                            BattleHandler.InitiateBattle(Nests[i].Creatures[j], Player);
                        }
                    }
                }

                // Check Boss Collision
                if (Player.CurrentTile.X == Boss.CurrentTile.X - 1 || Player.CurrentTile.X == Boss.CurrentTile.X + 1 ||
                    Player.CurrentTile.Y == Boss.CurrentTile.Y - 1 || Player.CurrentTile.Y == Boss.CurrentTile.Y + 1)
                    if (Player.CollisionRect.Intersects(Boss.CollisionRect))
                    {
                        // INVOKE BOSS BATTLE HERE
                        LoadLevel(CurrentLevel + 1); // Move to next level
                    }

                // Check lab collision
                if(Player.CurrentTile.X == Lab.CurrentTile.X - 1 || Player.CurrentTile.X == Lab.CurrentTile.X + 1 ||
                    Player.CurrentTile.Y == Lab.CurrentTile.Y - 1 || Player.CurrentTile.Y == Lab.CurrentTile.Y + 1)
                        if(Player.CollisionRect.Intersects(Lab.CollisionRect))
                            Interface.currentScreen = Screens.Lab;

                Player.Update();
                Boss.Update();
                base.Update(gameTime);
            }
        }

        private void HandleMouse()
        {
            Vector2 mousePos = Camera.Transform(new Vector2(InputHandler.MouseX, InputHandler.MouseY));
            Rectangle mouseRect = new Rectangle((int)mousePos.X, (int)mousePos.Y, 10, 10);

            foreach(Nest n in Nests)
                foreach (Creature c in n.Creatures)
                {
                    if (c.CollisionRect.Intersects(mouseRect))
                        Console.Out.WriteLine("Collision");
                }
        }

        private void HandlePlayerMovement()
        {
            if (Player.Direction.Y == 0 && Player.Position.X % TileMap.TileWidth == 0)
            {
                if (InputHandler.KeyDown(Keys.Down))
                    Player.Direction.Y = 1;
                else if (InputHandler.KeyDown(Keys.Up))
                    Player.Direction.Y = -1;
            }
            else if (Player.Position.Y % TileMap.TileHeight == 0)
            {
                if (Player.Direction.Y == 1 && !InputHandler.KeyDown(Keys.Down))
                    Player.Direction.Y = 0;
                if (Player.Direction.Y == -1 && !InputHandler.KeyDown(Keys.Up))
                    Player.Direction.Y = 0;
            }

            if (Player.Direction.X == 0 && Player.Position.Y % TileMap.TileHeight == 0)
            {
                if (InputHandler.KeyDown(Keys.Left))
                    Player.Direction.X = -1;
                else if (InputHandler.KeyDown(Keys.Right))
                    Player.Direction.X = 1;
            }
            else if (Player.Position.X % TileMap.TileWidth == 0)
            {
                if (Player.Direction.X == 1 && !InputHandler.KeyDown(Keys.Right))
                    Player.Direction.X = 0;
                if (Player.Direction.X == -1 && !InputHandler.KeyDown(Keys.Left))
                    Player.Direction.X = 0;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (Enabled)
            {
                SpriteManager.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

                Minimap.Draw();
                Player.Draw();
                TileMap.Draw();
                Lab.Draw();
                Boss.Draw();

                if (Nests.Count > 0)
                    foreach (Nest n in Nests)
                        n.Draw();

                if (Items.Count > 0)
                    foreach (ItemEntity i in Items)
                        i.Draw();

                SpriteManager.End();
            }
        }

        public static bool CheckNests(Rectangle area)
        {
            foreach(Nest n in Nests)
                if(n.CollisionRect.Intersects(area))
                    return true;
            return false;
        }

        private void LoadLevel(int levelNumber)
        {
            // Clear Current Data
            if (Nests.Count > 0)
            {
                foreach (Nest n in Nests)
                    if(n.Creatures.Count > 0)
                        n.Creatures.Clear();
                Nests.Clear();
            }
            if(Items.Count > 0)
                Items.Clear();

            TileMap = new TileMap("Level" + levelNumber, GraphicsDevice, Game.Content);
            Minimap = new Minimap(TileMap.Map, Game.Content.Load<Texture2D>("Sprites\\Nest"), Game.Content.Load<Texture2D>("Sprites\\Lab"), GraphicsDevice);
            GameHandler.Player.Position = GameHandler.TileMap.PlayerSpawn;
            GameHandler.Lab.Position = GameHandler.TileMap.LabPosition;
            GameHandler.Minimap = new Minimap(GameHandler.TileMap.Map, Game.Content.Load<Texture2D>("Sprites\\Nest"), Game.Content.Load<Texture2D>("Sprites\\Lab"), Game.GraphicsDevice);

            CurrentLevel = levelNumber;
            SaveHandler.SaveGame();
        }
    }
}
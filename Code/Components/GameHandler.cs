#define DEVBUILD

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace VOiD.Components
{
    class GameHandler : DrawableGameComponent
    {
        #region Declarations
        public static int CurrentLevel;
        public static TileMap TileMap;
        public static Creature Boss;
        public static Entity Lab;
        public static Minimap Minimap;
        public static Creature Player;
        public static Inventory Inventory;
        public static new bool Enabled = true;
        public static new bool Visible = true;
        public static bool EditMode = false;
       
        private static string _currentMessageBoxText;
        
        private static List<Nest> nests = new List<Nest>();
        private static List<ItemEntity> Items = new List<ItemEntity>();
        private static List<Sign> signs = new List<Sign>();

        private static Attributes _currentAttributeInUse = Attributes.None;

        private Texture2D exclamationSprite;

        const int NUMBER_OF_ITEM_TYPES = 5;
        #endregion

        public GameHandler(Game game)
            : base(game)
        {
            Inventory = new Inventory(NUMBER_OF_ITEM_TYPES, game.Content);
            exclamationSprite = game.Content.Load<Texture2D>("Sprites/exclamation");
            Enabled = false;
            Visible = false;
        }

        #region Add/Remove Entities
        public static void AddItem(ItemEntity item)
        {
            Items.Add(item);
        }

        public static void RemoveItem(ItemEntity item)
        {
            Items.Remove(item);
        }

        public static void AddNest(Nest nest)
        {
            nests.Add(nest);
        }

        public static void RemoveNest(Nest nest)
        {
            nests.Remove(nest);
        }

        public static void AddSign(Sign sign)
        {
            signs.Add(sign);
        }

        public static void RemoveSign(Sign sign)
        {
            signs.Remove(sign);
        }
        #endregion

        public override void Update(GameTime gameTime)
        {
            if (Enabled && !EditMode)
            {
                // DEV CONSOLE
#if DEVBUILD
                if (InputHandler.KeyPressed(Keys.F3))
                    DevConsole.Open(Game.Content, Game.GraphicsDevice);
#endif

                for (int i = 0; i < nests.Count; i++)
                    nests[i].Update(gameTime);

                Camera.Position = new Vector2((GameHandler.Player.Position.X + (GameHandler.Player.Texture.Width / 2)) - (Configuration.Width / 2),
                                     (GameHandler.Player.Position.Y + (GameHandler.Player.Texture.Height / 2)) - (Configuration.Height / 2));

                #region Player Input
                if (Interface.currentScreen == Screens.LevelMenu)
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

                    if (InputHandler.KeyPressed(Keys.Enter))
                    {
                        Sign s = CheckSign(new Point((int)Player.Position.X + ((int)Player.PreviousDirection.X * TileMap.TileWidth),
                                                     (int)Player.Position.Y + ((int)Player.PreviousDirection.Y * TileMap.TileHeight)));
                        Item i = CheckItem(new Point((int)Player.Position.X, (int)Player.Position.Y));
                        if (s != null)
                        {
                            Interface.ShowMessageBox();
                            _currentMessageBoxText = s.Text;
                            Console.WriteLine("Read Sign at " + s.Position.X + " - " + s.Position.Y + "\n");
                        }
                        if (i != null)
                        {
                            if (Inventory.Items[i.ID].Amount == 99)
                            {
                                _currentMessageBoxText = "You cannot carry anymore " + i.Name + "s!";
                                Interface.ShowMessageBox();
                            }
                            else
                            {
                                Inventory.AddItem(new Item(i.ID, Game.Content));
                                Items.Remove(Items[i.ID]);
                                _currentMessageBoxText = "You found: " + i.Name + "!";
                                Interface.ShowMessageBox();
                            }
                        }

                        // Check lab
                        Rectangle pRect = new Rectangle(Player.CollisionRect.X + (int)(Player.PreviousDirection.X * Player.CollisionRect.Width),
                                                         Player.CollisionRect.Y + (int)(Player.PreviousDirection.Y * Player.CollisionRect.Height), Player.CollisionRect.Width, Player.CollisionRect.Height);
                        if (pRect.Intersects(Lab.CollisionRect))
                        {
                            Interface.currentScreen = Screens.Lab;
                            Player.Health = Player.Dominant.Health.Level;
                        }
                    }
                }
                #endregion

                for (int i = 0; i < nests.Count; i++)
                {
                    if (Player.CollisionRect.Intersects(nests[i].MoveArea))
                    {
                        for (int j = 0; j < nests[i].Creatures.Count; j++)
                        {
                            if (nests[i].Creatures[j].CollisionRect.Intersects(Player.CollisionRect))
                                BattleHandler.InitiateBattle(nests[i].Creatures[j]);
                        }
                    }
                }

                // Check Boss Collision
                if (Player.CurrentTile.X == Boss.CurrentTile.X - 1 || Player.CurrentTile.X == Boss.CurrentTile.X + 1 ||
                    Player.CurrentTile.Y == Boss.CurrentTile.Y - 1 || Player.CurrentTile.Y == Boss.CurrentTile.Y + 1)
                    if (Player.CollisionRect.Intersects(Boss.CollisionRect))
                    {
                        // INVOKE BOSS BATTLE HERE
                        LoadLevel(CurrentLevel + 1, Game.Content, Game.GraphicsDevice); // Move to next level
                        SaveHandler.SaveGame();
                    }

                // Update currently used attribute
                if (TileMap.Attribute[Player.CurrentTile.X, Player.CurrentTile.Y] == (int)Attributes.Flying)
                    _currentAttributeInUse = Attributes.Flying;
                else if (TileMap.Attribute[Player.CurrentTile.X, Player.CurrentTile.Y] == (int)Attributes.FlyingAndClimbing)
                    _currentAttributeInUse = Attributes.FlyingAndClimbing;
                else if (TileMap.Attribute[Player.CurrentTile.X, Player.CurrentTile.Y] == (int)Attributes.FlyingAndSwimming)
                    _currentAttributeInUse = Attributes.FlyingAndSwimming;
                else
                    _currentAttributeInUse = Attributes.None;

                #if DEVBUILD
                #region Level Edit
                if (InputHandler.KeyPressed(Keys.F1))
                {
                    Game1.LevelEditor.Enabled = true;
                    Game1.LevelEditor.Visible = true;
                    EditMode = true;
                }
                #endregion
                #endif

                Player.Update(gameTime);
                Boss.Update(gameTime);

                base.Update(gameTime);
            }
            else if (EditMode)
            {
                if (InputHandler.KeyDown(Keys.Down))
                    Camera.Move(new Vector2(0, 3));
                if (InputHandler.KeyDown(Keys.Up))
                    Camera.Move(new Vector2(0, -3));
                if (InputHandler.KeyDown(Keys.Left))
                    Camera.Move(new Vector2(-3, 0));
                if (InputHandler.KeyDown(Keys.Right))
                    Camera.Move(new Vector2(3, 0));
            }
        }

         public override void Draw(GameTime gameTime)
        {
            if (Visible)
            {
                if (Interface.currentScreen == Screens.LevelMenu || Interface.currentScreen == Screens.BLANK)
                {
                    #region Draw Map and Entities
                    if (Enabled || EditMode)
                    {
                        SpriteManager.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

                        // Check for adjacent sign, show exclamation if so
                        Sign sign = CheckSign(new Point((int)Player.Position.X + ((int)Player.PreviousDirection.X * TileMap.TileWidth),
                                 (int)Player.Position.Y + ((int)Player.PreviousDirection.Y * TileMap.TileHeight)));
                        if (sign != null)
                            SpriteManager.Draw(exclamationSprite, Camera.Transform(GameHandler.Player.Position - new Vector2(0, exclamationSprite.Height)), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);

                        // Check if on an item, show exclamation if so
                        Item item = CheckItem(new Point((int)Player.Position.X, (int)Player.Position.Y));
                        if (item != null)
                            SpriteManager.Draw(exclamationSprite, Camera.Transform(GameHandler.Player.Position - new Vector2(0, exclamationSprite.Height)), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);

                        // Check if facing lab, if so show exclamation mark
                        Rectangle pRect = new Rectangle(Player.CollisionRect.X + (int)(Player.PreviousDirection.X * Player.CollisionRect.Width),
                            Player.CollisionRect.Y + (int)(Player.PreviousDirection.Y * Player.CollisionRect.Height), Player.CollisionRect.Width, Player.CollisionRect.Height);
                        if (pRect.Intersects(Lab.CollisionRect))
                            SpriteManager.Draw(exclamationSprite, Camera.Transform(GameHandler.Player.Position - new Vector2(0, exclamationSprite.Height)), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);

                        if (!EditMode)
                            Player.Draw();

                        TileMap.Draw();
                        Lab.Draw();
                        Boss.Draw();

                        for(int i = 0; i < signs.Count; i++)
                                signs[i].Draw();

                        for(int i = 0; i < nests.Count; i++)
                                nests[i].Draw();

                        for(int i = 0; i < Items.Count; i++)
                                Items[i].Draw();
                        
                        SpriteManager.End();
                    }
                    #endregion
                }
                base.Draw(gameTime);
            }
        }

        #region Check Entity Locations
         public static bool CheckNests(Rectangle area)
        {
            foreach(Nest n in nests)
                if(n.CollisionRect.Intersects(area))
                    return true;
            return false;
        }

        public static Nest CheckNests(Point position)
        {
            foreach (Nest n in nests)
                if (new Point(n.CollisionRect.X, n.CollisionRect.Y) == position)
                    return n;
            return null;
        }

        public static ItemEntity CheckItem(Point position)
        {
            foreach (ItemEntity i in Items)
                if (new Point(i.CollisionRect.X, i.CollisionRect.Y) == position)
                    return i;
            return null;
        }

        public static Sign CheckSign(Point position)
        {
            foreach (Sign s in signs)
                if (new Point(s.CollisionRect.X, s.CollisionRect.Y) == position)
                    return s;
            return null;
        }
        #endregion

        #region Load Level
        public static void LoadLevel(int levelNumber, ContentManager content, GraphicsDevice graphics)
        {
            // Clear Current Data
            if (nests.Count > 0)
            {
                foreach (Nest n in nests)
                    if(n.Creatures.Count > 0)
                        n.Creatures.Clear();
                nests.Clear();
            }
            if(Items.Count > 0)
                Items.Clear();
            if (signs.Count > 0)
                signs.Clear();

            TileMap = new TileMap("Level" + levelNumber, graphics, content);
            Minimap = new Minimap(TileMap.Map, content.Load<Texture2D>("Sprites\\Nest"), content.Load<Texture2D>("Sprites\\Lab"), graphics);
            Player.Position = GameHandler.TileMap.PlayerSpawn;
            Lab.Position = GameHandler.TileMap.LabPosition;
            Interface.UpdateMiniMap();
            CurrentLevel = levelNumber;
            SaveHandler.SaveGame();
        }
        #endregion

        public static List<Nest> Nests { get { return nests; } }
        public static List<Sign> Signs { get { return signs; } }
        public static string CurrentSignText { get { return _currentMessageBoxText; } }
        public static Attributes CurrentAttributeInUse { get { return _currentAttributeInUse; } }
    }
}
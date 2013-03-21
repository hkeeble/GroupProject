using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace VOiD.Components
{
    class BattleHandler : DrawableGameComponent
    {
        private static Random random = new Random();
        private static bool InSession;
        private static Creature B;
        private static bool Win=false;
        public static int AttackSelection = 0;
        public static bool ActionSelected = false;

        private static string _lastPlayerAction;
        private static string _lastEnemyAction;

        private Texture2D background;

        private static Texture2D FightCloud;
        private static Animation attackAnim;
        private static TimeSpan turnTimer;
        private static bool canSelectAction;

        public BattleHandler(Game game)
            : base(game)
        {
            background = game.Content.Load<Texture2D>("Interface/Assets/Battle/background");

            FightCloud = game.Content.Load<Texture2D>("Sprites/Fight_Cloud");
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public static void InitiateBattle(Creature b)
        {
            Interface.currentScreen = Screens.Battle;
            B = b;
            InSession = true;
            canSelectAction = true;
            turnTimer = TimeSpan.Zero;

            attackAnim = new Animation(FightCloud, new Rectangle(Configuration.Width / 2, Configuration.Height / 2, Configuration.Width - 350, Configuration.Height - 100), 100, 400, 550);
            attackAnim.DrawRectangle.X = (Configuration.Width / 2) - (attackAnim.DrawRectangle.Width / 2);
            attackAnim.DrawRectangle.Y = (Configuration.Height / 2) - (attackAnim.DrawRectangle.Height / 2);
        }

        public override void Update(GameTime gameTime)
        {
            if (InSession)
            {
                if (GameHandler.Player.Health > 0 && B.Health > 0)
                {
                    //Select Attack
                    if (ActionSelected)
                    {
                        ActionSelected = false;
                        canSelectAction = false;
                    }
                }

                if (canSelectAction == false)
                {
                    turnTimer += gameTime.ElapsedGameTime;

                    attackAnim.Update(gameTime);

                    if (turnTimer >= TimeSpan.FromSeconds(3))
                    {
                        canSelectAction = true;
                        turnTimer = TimeSpan.Zero;

                        // Player Attack
                        if (AttackSelection != null)
                        {
                            _lastPlayerAction = "You attack with a " + GameHandler.Player.AvailableAttacks[AttackSelection].Name
                                + "\ndealing " + GameHandler.Player.AvailableAttacks[AttackSelection].Damage + " points of damage!";
                        }

                        // Enemy Attack
                        int AttackPatternSigma = random.Next(B.AvailableAttacks.Count);
                        _lastEnemyAction = "Enemy attacks with a " + B.AvailableAttacks[AttackPatternSigma].Name + "\ndealing " + B.AvailableAttacks[AttackPatternSigma].Damage + " points of damage!";

                        // Distribute Damage (Based on speed)
                        bool playerFirst = false;
                        if (B.Speed > GameHandler.Player.Speed)
                            playerFirst = true;

                        if (B.Speed == GameHandler.Player.Speed) // If equal, randomize
                            if (random.Next(1) == 1)
                                playerFirst = true;

                        if (!playerFirst)
                        {
                            GameHandler.Player.Health -= (int)B.AvailableAttacks[AttackPatternSigma].Damage;
                            if (GameHandler.Player.Health < 0)
                                GameHandler.Player.Health = 0;
                            if (GameHandler.Player.Health > 0)
                            {
                                B.Health -= (int)GameHandler.Player.AvailableAttacks[AttackSelection].Damage;
                                if (B.Health < 0)
                                    B.Health = 0;
                            }
                            else
                                _lastPlayerAction = "Player was too slow and is knocked out!";
                        }
                        else
                        {
                            B.Health -= (int)GameHandler.Player.AvailableAttacks[AttackSelection].Damage;
                            if (B.Health < 0)
                                B.Health = 0;
                            if (B.Health > 0)
                            {
                                GameHandler.Player.Health -= (int)B.AvailableAttacks[AttackPatternSigma].Damage;
                                if (GameHandler.Player.Health < 0)
                                    GameHandler.Player.Health = 0;
                            }
                            else
                                _lastEnemyAction = "Enemy was too slow and is knocked out!";
                        }
                    }
                }

                if (B.Health <= 0 || GameHandler.Player.Health <= 0)
                {
                    GameHandler.Player.Position = GameHandler.Lab.Position + new Vector2(GameHandler.TileMap.TileWidth, GameHandler.TileMap.TileHeight * 3);
                    Interface.currentScreen = Screens.LevelMenu;
                    InSession = false;
                    _lastEnemyAction = " ";
                    _lastPlayerAction = " ";
                    if (B.Health <= 0 && GameHandler.Player.Health > 0)
                    {
                        Win = true;
                        Interface.currentScreen = Screens.Victory;
                    }
                    else
                    {
                        Win = false;
                        Interface.currentScreen = Screens.GameOver;
                    }

                    GameHandler.Enabled = true;
                }
                else
                {
                    GameHandler.Enabled = false;
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (InSession)
            {
                SpriteManager.Begin();
                SpriteManager.Draw(background, Configuration.Bounds, Color.White);
                if (!canSelectAction)
                    attackAnim.Draw();
                SpriteManager.End();
                if (canSelectAction)
                {
                    GameHandler.Player.Draw(Game.GraphicsDevice, Matrix.CreateRotationY(MathHelper.ToRadians(45.0f)) * Matrix.CreateTranslation(new Vector3(1, 0, -4)));
                    B.Draw(Game.GraphicsDevice, Matrix.CreateRotationY(MathHelper.ToRadians(-45.0f)) * Matrix.CreateTranslation(new Vector3(-1, 0, -4)));
                }
            }
            base.Draw(gameTime);
        }

        public static string LastPlayerAction { get { return _lastPlayerAction; } set { _lastPlayerAction = value; } }
        public static string LastEnemyAction { get { return _lastEnemyAction; } set { _lastEnemyAction = value; } }
        public static bool CanSelectAction { get { return canSelectAction; } }
        public static bool IsInSession { get { return InSession; } }
        public static Creature Enemy { get { return B; } }
    }
}

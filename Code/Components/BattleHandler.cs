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
        private static bool PlayerMove;
        private static Creature B;
        private static bool Win=false;
        public static int AttackSelection = 0;
        public static bool AttackSelected = false;

        private static string _lastPlayerMove;
        private static string _lastEnemyMove;

        private Texture2D background;

        public BattleHandler(Game game)
            : base(game)
        {
            background = game.Content.Load<Texture2D>("Interface/Assets/Battle/background");
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public static void InitiateBattle(Creature b)
        {
            Interface.currentScreen=Screens.Battle;
            B = b;
            InSession = true;
            PlayerMove = false;
        }

        public override void Update(GameTime gameTime)
        {
            if (InSession)
            {
                if (PlayerMove && B.Health > 0)
                {
                    //Select Attack
                    if (AttackSelected)
                    {
                        B.Health -= (int)GameHandler.Player.AvailableAttacks[AttackSelection].Damage;
                        _lastPlayerMove = "You attack with a " + GameHandler.Player.AvailableAttacks[AttackSelection].Name
                            + "\ndealing " + GameHandler.Player.AvailableAttacks[AttackSelection].Damage + " points of damage!";
                        Console.WriteLine("AI Health - " + B.Health);
                        PlayerMove = false;
                        AttackSelected = false;
                    }
                }
                else if (GameHandler.Player.Health > 0)
                {
                    int AttackPatternSigma = random.Next(B.AvailableAttacks.Count);
                    _lastEnemyMove = "Enemy attacks with a " + B.AvailableAttacks[AttackPatternSigma].Name + "\ndealing " + B.AvailableAttacks[AttackPatternSigma].Damage + " points of damage!";
                    GameHandler.Player.Health -= (int)B.AvailableAttacks[AttackPatternSigma].Damage;
                    Console.WriteLine("Player Health - " + GameHandler.Player.Health);
                    PlayerMove = true;
                }


                if (B.Health <= 0 || GameHandler.Player.Health <= 0)
                {
                    GameHandler.Player.Position = GameHandler.Lab.Position + new Vector2(GameHandler.TileMap.TileWidth, GameHandler.TileMap.TileHeight * 3);
                    Interface.currentScreen = Screens.LevelMenu;
                    InSession = false;
                    if (B.Health <= 0 && GameHandler.Player.Health > 0)
                        Win = true;
                    else
                        Win = false;

                    Console.WriteLine("Did you Win - " + Win);
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
                SpriteManager.End();
                GameHandler.Player.Draw(Game.GraphicsDevice, Matrix.CreateRotationY(MathHelper.ToRadians(45.0f)) * Matrix.CreateTranslation(new Vector3(1, 0, -4)));
                B.Draw(Game.GraphicsDevice, Matrix.CreateRotationY(MathHelper.ToRadians(-45.0f)) * Matrix.CreateTranslation(new Vector3(-1, 0, -4)));
            }
            base.Draw(gameTime);
        }

        public static string LastPlayerMove { get {return _lastPlayerMove; } }
        public static string LastEnemyMove { get { return _lastEnemyMove; } }
        public static Creature Enemy { get { return B; } }
    }
}

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

        public BattleHandler(Game game)
            : base(game)
        {
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
                    B.Health -= (int)GameHandler.Player.AvailableAttacks[0].Damage;
                    Console.WriteLine("Player Used - " + GameHandler.Player.AvailableAttacks[0].Name);
                    Console.WriteLine("AI Health - " + B.Health);
                    PlayerMove = false;
                }
                else if (GameHandler.Player.Health > 0)
                {
                    int AttackPatternSigma = random.Next(B.AvailableAttacks.Count);
                    GameHandler.Player.Health -= (int)B.AvailableAttacks[AttackPatternSigma].Damage;
                    Console.WriteLine("AI Used - " + B.AvailableAttacks[AttackPatternSigma].Name);
                    Console.WriteLine("Player Health - " + GameHandler.Player.Health);
                    PlayerMove = true;
                }


                if (B.Health <= 0 || GameHandler.Player.Health <= 0)
                {
                    GameHandler.Player.Position = Vector2.Zero;
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
                GameHandler.Player.Draw(Game.GraphicsDevice, Matrix.CreateRotationY(MathHelper.ToRadians(45.0f)) * Matrix.CreateTranslation(new Vector3(1, 0, -4)));
                B.Draw(Game.GraphicsDevice, Matrix.CreateRotationY(MathHelper.ToRadians(-45.0f)) * Matrix.CreateTranslation(new Vector3(-1, 0, -4)));
            }
            base.Draw(gameTime);
        }
    }
}

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
    public class BattleHandler : DrawableGameComponent
    {
        private static bool InSession;
        private static Creature A;
        private static Creature B;

        public BattleHandler(Game game)
            : base(game)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public static void InitiateBattle(Creature a,Creature b)
        {
            Interface.currentScreen=Screens.Battle;
            A = a;
            B = b;
            InSession = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (InSession)
            {
                //do battle logic here
                GameHandler.Enabled = false;
                bool BattleEnd = false;
                if (BattleEnd)
                {
                    InSession = false;
                    Interface.currentScreen = Screens.LevelMenu;
                    GameHandler.Enabled = true;
                }
            }


            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (InSession)
            {
                A.Draw(Game.GraphicsDevice, Matrix.CreateRotationY(MathHelper.ToRadians(45.0f)) * Matrix.CreateTranslation(new Vector3(2, 0, -4)));
                B.Draw(Game.GraphicsDevice, Matrix.CreateRotationY(MathHelper.ToRadians(-45.0f)) * Matrix.CreateTranslation(new Vector3(-2, 0, -4)));
            }
            base.Draw(gameTime);
        }
    }
}

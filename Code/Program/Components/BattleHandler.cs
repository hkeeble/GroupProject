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
    public class BattleHandler : Microsoft.Xna.Framework.GameComponent
    {
        private static bool InSession;

        public BattleHandler(Game game)
            : base(game)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public static void InitiateBattle(Creature A,Creature B)
        {
            Interface.currentScreen=Screens.Battle;
            InSession = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (InSession)
            {
                //do battle logic here

                bool BattleEnd = true;
                if (BattleEnd)
                    InSession = false;
            }


            base.Update(gameTime);
        }
    }
}

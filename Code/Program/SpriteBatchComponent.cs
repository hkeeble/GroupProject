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
using System.IO;

namespace CreatureGame
{
    class SpriteBatchComponent : GameComponent
    {
        public static SpriteBatch spriteBatch;

        public SpriteBatchComponent(Game game)
            : base(game)
        {
            spriteBatch = new SpriteBatch(game.GraphicsDevice);
        }

        public static void Begin()
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
        }

        public static void Draw(Texture2D texture, Vector2 position, Color color)
        {
            spriteBatch.Draw(texture, position, color);
        }

        public static void Draw(Texture2D texture, Vector2 position, Rectangle rectangle, Color color)
        {
            spriteBatch.Draw(texture, position, rectangle, color);
        }

        public static void End()
        {
            spriteBatch.End();
        }
    }
}

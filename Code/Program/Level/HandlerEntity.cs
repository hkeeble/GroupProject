using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CreatureGame
{
    class HandlerEntity : Entity
    {
        const float MOVE_SPEED = 1f;

        public HandlerEntity(Texture2D texture, Vector2 position)
            : base(texture, position, MOVE_SPEED)
        {
        }

        public override void Update()
        {
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

            base.Update();
        }
    }
}
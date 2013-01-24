using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CreatureGame
{
    class ItemEntity : Item
    {
        private Vector2 _mapPosition;

        public ItemEntity(Texture2D texture, Vector2 position, int id)
            : base(id)
        {
            _mapPosition = position;
        }

        public Vector2 ScreenPosition { get { return Camera.Transform(_mapPosition); } }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, ScreenPosition, Color.White);
        }
    }
}

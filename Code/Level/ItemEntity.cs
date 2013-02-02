using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VOiD.Components;

namespace VOiD
{
    class ItemEntity : Item
    {
        private Vector2 _mapPosition;
        private Rectangle _collisionRect;

        public ItemEntity(Vector2 position, int id, Microsoft.Xna.Framework.Content.ContentManager content)
            : base(id, content)
        {
            _mapPosition = position;
            _collisionRect = new Rectangle((int)_mapPosition.X, (int)_mapPosition.Y, Texture.Width, Texture.Height);
        }

        public Vector2 ScreenPosition { get { return Camera.Transform(_mapPosition); } }
        public Rectangle CollisionRect { get { return _collisionRect; } }

        public void Draw()
        {
            SpriteManager.Draw(Texture, ScreenPosition, null, Color.White, 0f, Vector2.Zero, new Vector2(1,1), SpriteEffects.None, 0.5f);
        }
    }
}

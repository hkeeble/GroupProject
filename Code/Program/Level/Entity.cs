using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CreatureGame
{
    class Entity
    {
        private Texture2D _texture;
        private Vector2 _position;
        protected Point _currentTile;
        private float _moveSpeed = 0.0f;
        public Vector2 Direction = Vector2.Zero;

        public Entity()
        {
            _texture = null;
            _position = Vector2.Zero;
            _moveSpeed = 1f;
            _currentTile = Point.Zero;
        }

        public Entity(Texture2D texture, Vector2 position, float moveSpeed)
        {
            _texture = texture;
            _position = position;
            _moveSpeed = moveSpeed;
            _currentTile = Point.Zero;
        }

        public void Draw()
        {
            if (_texture != null)
                SpriteBatchComponent.Draw(_texture, Camera.Transform(_position), Color.White);
        }

        public virtual void Update()
        {
            //if (_position.X % HandlerEntity.TileMap.TileWidth != 1 || _position.Y % HandlerEntity.TileMap.TileHeight != 1)
            //{
            //    _currentTile = new Point((int)(_position.X / HandlerEntity.TileMap.TileWidth), (int)(_position.Y / HandlerEntity.TileMap.TileHeight));
            //    if (Direction.X != 0 || Direction.Y != 0)
            //    {
            //        if (HandlerEntity.TileMap.Passable[_currentTile.Y, _currentTile.X + (int)Direction.X] == false)
            //            Direction.X = 0;
            //        if (HandlerEntity.TileMap.Passable[_currentTile.Y + (int)Direction.Y, _currentTile.X] == false)
            //            Direction.Y = 0;
            //    }
            //}
            _position += (Direction * _moveSpeed);
        }

        public Texture2D Texture { get { return _texture; } }
        public Vector2 Position { get { return _position; } set { _position = value; } }
    }
}

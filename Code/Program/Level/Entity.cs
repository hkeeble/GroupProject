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
        protected Vector2 _velocity = Vector2.Zero;

        public Entity(Texture2D texture, Vector2 position, float moveSpeed)
        {
            _texture = texture;
            _position = position;
            _moveSpeed = moveSpeed;
            _currentTile = Point.Zero;
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Camera.Transform(_position), Color.White);
        }

        public virtual void Update()
        {
            if (_position.X % Level.Map.TileWidth != 1 || _position.Y % Level.Map.TileHeight != 1)
            {
                _currentTile = new Point((int)(_position.X / Level.Map.TileWidth), (int)(_position.Y / Level.Map.TileHeight));
                if (_velocity.X != 0 || _velocity.Y != 0)
                {
                    if (Level.Map.Passable[_currentTile.Y, _currentTile.X + (int)_velocity.X] == false)
                        _velocity.X = 0;
                    if (Level.Map.Passable[_currentTile.Y + (int)_velocity.Y, _currentTile.X] == false)
                        _velocity.Y = 0;
                }
            }
            _position += (_velocity*_moveSpeed);
        }

        public Texture2D Texture { get { return _texture; } }
        public Vector2 Position { get { return _position; } set { _position = value; } }
    }
}

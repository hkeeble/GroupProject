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
        private Point _newLoc;
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
                SpriteBatchComponent.Draw(_texture, Camera.Transform(_position), Color.White, 1f);
        }

        public virtual void Update()
        {
            if (_position.X % HandlerEntity.TileMap.TileWidth == 0 && _position.Y % HandlerEntity.TileMap.TileHeight == 0)
                _newLoc = NewLocation;
            
            Console.Out.WriteLine(_newLoc.X + " + " + _newLoc.Y);
            
            if(HandlerEntity.TileMap.Passable[_newLoc.Y, _newLoc.X] == false)
            {
                if(Direction.X != 0)
                    Direction.X = 0;
                if(Direction.Y != 0)
                    Direction.Y = 0;
            }

            _position += (Direction * _moveSpeed);

            // Keep in map bounds
            if (_position.X < 0)
                _position.X = 0;
            if (_position.Y < 0)
                _position.Y = 0;
            if (_position.X > HandlerEntity.TileMap.PixelSize.Width)
                _position.X = HandlerEntity.TileMap.PixelSize.Width;
            if (_position.X > HandlerEntity.TileMap.PixelSize.Height)
                _position.X = HandlerEntity.TileMap.PixelSize.Height;
        }

        public Texture2D Texture { get { return _texture; } }
        public Vector2 Position { get { return _position; } set { _position = value; } }

        private Point NewLocation
        {
            get
            {
                Point location = new Point((int)(_position.X / HandlerEntity.TileMap.TileWidth) + (int)Direction.X, (int)(_position.Y / HandlerEntity.TileMap.TileHeight) + (int)Direction.Y);
                if (location.X < 0)
                    location.X = 0;
                if (location.Y < 0)
                    location.Y = 0;
                if (location.X > HandlerEntity.TileMap.Width)
                    location.X = HandlerEntity.TileMap.Width;
                if (location.Y > HandlerEntity.TileMap.Height)
                    location.Y = HandlerEntity.TileMap.Height;
                return location;
            }
        }
    }
}

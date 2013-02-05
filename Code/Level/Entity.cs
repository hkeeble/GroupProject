using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VOiD.Components;

namespace VOiD
{
    public class Entity
    {
        private Texture2D _texture;
        private Vector2 _position;
        protected Point _currentTile;
        private Point _newLoc;
        private float _moveSpeed = 0.0f;
        public Vector2 Direction = Vector2.Zero;
        private Rectangle _collisionRect;

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
            _collisionRect = new Rectangle((int)_position.X, (int)_position.Y, _texture.Width, _texture.Height);
        }

        public void Draw()
        {
            if (_texture != null)
            {
                SpriteManager.Draw(_texture, Camera.Transform(_position), null, Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 1f);
            }
        }

        public virtual void Update()
        {
            if (_position.X % GameHandler.TileMap.TileWidth == 0 && _position.Y % GameHandler.TileMap.TileHeight == 0)
            {
                _newLoc = NewLocation;
                _currentTile = CurrentTile;
            }

            if(GameHandler.TileMap.Passable[_newLoc.X, _newLoc.Y] == false)
            {
                if(Direction.X != 0)
                    Direction.X = 0;
                if(Direction.Y != 0)
                    Direction.Y = 0;
            }
            else if (Direction.X != 0 && Direction.Y != 0)
            {
                if(GameHandler.TileMap.Passable[_currentTile.X, _newLoc.Y] == false)
                    Direction.Y = 0;
                if(GameHandler.TileMap.Passable[_newLoc.X, _currentTile.Y] == false)
                    Direction.X = 0;
            }

            _position += (Direction * _moveSpeed);

            // Keep in map bounds
            if (_position.X < 0)
                _position.X = 0;
            if (_position.Y < 0)
                _position.Y = 0;
            if (_position.X > GameHandler.TileMap.Map.Width-GameHandler.TileMap.TileWidth)
                _position.X = GameHandler.TileMap.Map.Width - GameHandler.TileMap.TileWidth;
            if (_position.Y > GameHandler.TileMap.Map.Height - GameHandler.TileMap.TileHeight)
                _position.Y = GameHandler.TileMap.Map.Width - GameHandler.TileMap.TileWidth;

            _collisionRect = new Rectangle((int)_position.X, (int)_position.Y, _texture.Width, _texture.Height);
        }

        public Texture2D Texture { get { return _texture; } }
        public Vector2 Position { get { return _position; } set { _position = value; } }
        public Rectangle CollisionRect { get { return _collisionRect; } }

        private Point NewLocation
        {
            get
            {
                Point location = new Point((int)(_position.X / GameHandler.TileMap.TileWidth) + (int)Direction.X, (int)(_position.Y / GameHandler.TileMap.TileHeight) + (int)Direction.Y);
                if (location.X < 0)
                    location.X = 0;
                if (location.Y < 0)
                    location.Y = 0;
                if (location.X > GameHandler.TileMap.Width-1)
                    location.X = GameHandler.TileMap.Width-1;
                if (location.Y > GameHandler.TileMap.Height-1)
                    location.Y = GameHandler.TileMap.Height-1;
                return location;
            }
        }

        public Point CurrentTile
        {
            get
            {
                 return new Point((int)(_position.X / GameHandler.TileMap.TileWidth), (int)(_position.Y / GameHandler.TileMap.TileHeight));
            }
        }
    }
}

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
        private enum AnimDirection
        {
            Down,
            Left,
            Up,
            Right
        }

        private Texture2D _texture;
        private Vector2 _position;
        protected Point _currentTile;
        private Point _newLoc;
        private float _moveSpeed = 0.0f;
        public Vector2 Direction = Vector2.Zero;
        private Rectangle _collisionRect;

        #region Animation Declarations
        bool _animated;
        private TimeSpan _timeToNextFrame;
        private int _millisecondsBetweenFrame;
        private Point _currentFrame;

        private Rectangle _frameRect;
        private int _frameWidth, _frameHeight;
        private int _sheetFrameWidth, _sheetFrameHeight;
        #endregion

        public Entity()
        {
            _texture = null;
            _position = Vector2.Zero;
            _moveSpeed = 1f;
            _currentTile = Point.Zero;
        }

        /// <summary>
        /// Creates a new visible, non-animated entity.
        /// </summary>
        public Entity(Texture2D texture, Vector2 position, float moveSpeed)
        {
            _texture = texture;
            _position = position;
            _moveSpeed = moveSpeed;
            _currentTile = Point.Zero;
            _collisionRect = new Rectangle((int)_position.X, (int)_position.Y, _texture.Width, _texture.Height);
            _animated = false;
        }

        /// <summary>
        /// Overloaded constructor. Creates an animated world entity.
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="position"></param>
        /// <param name="moveSpeed"></param>
        public Entity(Texture2D texture, Vector2 position, float moveSpeed, int frameWidth, int frameHeight, int millisecondsBetweenFrame)
            : this(texture, position, moveSpeed)
        {
            _animated = true;

            _frameWidth = frameWidth;
            _frameHeight = frameHeight;
            _currentFrame = Point.Zero;
            _frameRect = new Rectangle(_currentFrame.X * _frameWidth, _currentFrame.Y * _frameHeight, _frameWidth, _frameHeight);
            _collisionRect = new Rectangle(0, 0, _frameWidth, _frameHeight);
            _millisecondsBetweenFrame = millisecondsBetweenFrame;

            _sheetFrameWidth = _texture.Width / frameWidth;
            _sheetFrameHeight = _texture.Height / frameHeight;
        }

        public void Draw()
        {
            if (_texture != null)
            {
                if(!_animated)
                    SpriteManager.Draw(_texture, Camera.Transform(_position), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                else
                    SpriteManager.Draw(_texture, Camera.Transform(_position), _frameRect, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            if (_position.X % GameHandler.TileMap.TileWidth == 0 && _position.Y % GameHandler.TileMap.TileHeight == 0)
            {
                _newLoc = NewLocation;
                _currentTile = CurrentTile;
            }

            if(GameHandler.TileMap.Passable[_newLoc.X, _newLoc.Y] == false)
            {
                if (GameHandler.TileMap.Attribute[_newLoc.X, _newLoc.Y] != 0)
                {
                    if (this.GetType() == typeof(Creature))
                    {
                        bool canMove = false;

                        if (GameHandler.TileMap.Attribute[_newLoc.X, _newLoc.Y] == (int)Attributes.Flying)
                            if (this.GetType() == typeof(Creature))
                                if ((this as Creature).canFly)
                                    canMove = true;
                        if (GameHandler.TileMap.Attribute[_newLoc.X, _newLoc.Y] == (int)Attributes.Climbing)
                            if (this.GetType() == typeof(Creature))
                                if ((this as Creature).canClimb)
                                    canMove = true;
                        if (GameHandler.TileMap.Attribute[_newLoc.X, _newLoc.Y] == (int)Attributes.Swimming)
                            if (this.GetType() == typeof(Creature))
                                if ((this as Creature).canSwim)
                                    canMove = true;
                        if (GameHandler.TileMap.Attribute[_newLoc.X, _newLoc.Y] == (int)Attributes.FlyingAndSwimming)
                            if (this.GetType() == typeof(Creature))
                                if ((this as Creature).canSwim || (this as Creature).canFly)
                                    canMove = true;
                        if (GameHandler.TileMap.Attribute[_newLoc.X, _newLoc.Y] == (int)Attributes.FlyingAndClimbing)
                            if (this.GetType() == typeof(Creature))
                                if ((this as Creature).canClimb || (this as Creature).canFly)
                                    canMove = true;

                        if (!canMove)
                        {
                            if (Direction.X != 0)
                                Direction.X = 0;
                            if (Direction.Y != 0)
                                Direction.Y = 0;
                        }
                    }
                }
                else
                {
                    if (Direction.X != 0)
                        Direction.X = 0;
                    if (Direction.Y != 0)
                        Direction.Y = 0;
                }
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
            if (_position.X > GameHandler.TileMap.Map.Width - GameHandler.TileMap.TileWidth)
                _position.X = GameHandler.TileMap.Map.Width - GameHandler.TileMap.TileWidth;
            if (_position.Y > GameHandler.TileMap.Map.Height - GameHandler.TileMap.TileHeight)
                _position.Y = GameHandler.TileMap.Map.Height - GameHandler.TileMap.TileHeight;

            #region Update Animation
            if (_animated)
            {
                if (Direction.X == 1)
                    _currentFrame.Y = (int)AnimDirection.Right;
                if (Direction.X == -1)
                    _currentFrame.Y = (int)AnimDirection.Left;
                if (Direction.Y == 1)
                    _currentFrame.Y = (int)AnimDirection.Down;
                if (Direction.Y == -1)
                    _currentFrame.Y = (int)AnimDirection.Up;

                if (Direction != Vector2.Zero)
                {
                    _timeToNextFrame += gameTime.ElapsedGameTime;

                    if (_timeToNextFrame >= TimeSpan.FromMilliseconds(_millisecondsBetweenFrame))
                    {
                        _timeToNextFrame = TimeSpan.Zero;
                        _currentFrame.X++;
                        if (_currentFrame.X > _sheetFrameWidth-1)
                            _currentFrame.X = 0;
                    }
                }
                else
                    _currentFrame.X = 0;

                _frameRect = new Rectangle(_currentFrame.X * _frameWidth, _currentFrame.Y * _frameHeight, _frameWidth, _frameHeight);
                _collisionRect = new Rectangle((int)_position.X, (int)_position.Y, _frameWidth, _frameHeight);
            }
            else
                _collisionRect = new Rectangle((int)_position.X, (int)_position.Y, _texture.Width, _texture.Height);
            #endregion
        }

        public Texture2D Texture { get { return _texture; } }
        public Vector2 Position { get { return _position; } set { _position = value; } }
        public Rectangle CollisionRect { get { return _collisionRect; } }
        public float MoveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } }

        /// <summary>
        /// Sets the creature's texture. If animated, the new texture must have the same dimensions as the previous texture.
        /// </summary>
        public void SetTexture(Texture2D texture)
        {
            _texture = texture;
        }

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

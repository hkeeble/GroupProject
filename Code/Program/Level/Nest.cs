using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VOiD.Components;

namespace VOiD
{
    class Nest
    {
        private Vector2 _position;
        private Texture2D _texture;
        private List<Creature> creatures = new List<Creature>();
        private Rectangle _moveArea;

        private Random rand;

        const int MAX_CREATURES = 4;
        const int MOVE_AREA_SIZE = 10;

        public Nest(Texture2D texture, Texture2D creatureTexture, Point position, short ID, Point mapDimensions, Point tileDimensions)
        {
            rand = new Random(DateTime.Now.Millisecond);
            _texture = texture;
            _position = new Vector2(position.X, position.Y);

            _moveArea = new Rectangle((int)(_position.X / tileDimensions.X) - (MOVE_AREA_SIZE / 2), (int)(_position.Y / tileDimensions.Y) - (MOVE_AREA_SIZE / 2), MOVE_AREA_SIZE, MOVE_AREA_SIZE);
            if(_moveArea.X < 0)
                _moveArea.X = 0;
            if(_moveArea.Y < 0)
                _moveArea.Y = 0;
            if (_moveArea.X + MOVE_AREA_SIZE > mapDimensions.X)
                _moveArea.Width = mapDimensions.Y;
            if (_moveArea.Y + MOVE_AREA_SIZE > mapDimensions.Y)
                _moveArea.Height = mapDimensions.Y;

            for (int i = 0; i < MAX_CREATURES; i++)
            {
                Point Position = new Point(rand.Next(_moveArea.X, _moveArea.X + _moveArea.Width),
                    rand.Next(_moveArea.Y, _moveArea.Y + _moveArea.Height));

                creatures.Add(new Creature(ID, creatureTexture, new Vector2(Position.X * tileDimensions.X, Position.Y * tileDimensions.Y), 0.5f));
            }
        }

        public void Update()
        {
            Console.Out.WriteLine(GameHandler.player.CurrentTile.X + " " + GameHandler.player.CurrentTile.Y);
            Vector2 direction;
            rand = new Random(DateTime.Now.Millisecond);

            if(creatures.Count > 0)
                foreach (Creature c in creatures)
                {
                    if (c.Position.X % GameHandler.TileMap.TileWidth == 0 && c.Position.Y % GameHandler.TileMap.TileHeight == 0)
                    {
                        direction = new Vector2(rand.Next(0, 2), rand.Next(0, 2));
                        direction.X = (direction.X == 0 ? -1 : direction.X);
                        direction.Y = (direction.Y == 0 ? -1 : direction.Y);

                        if (c.CurrentTile.X + direction.X < _moveArea.X || c.CurrentTile.X + direction.X > (_moveArea.X + _moveArea.Width))
                            direction.X = 0;
                        if (c.CurrentTile.Y + direction.Y < _moveArea.Y || c.CurrentTile.Y + direction.Y > (_moveArea.Y + _moveArea.Height))
                            direction.Y = 0;

                        c.Direction = direction;
                    }
                    c.Update();
                }
        }

        public void Draw()
        {
            SpriteManager.Draw(_texture, Camera.Transform(_position), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);

            foreach (Creature e in creatures)
                e.Draw();
        }

        public List<Creature> Creatures { get { return creatures; } }
    }
}

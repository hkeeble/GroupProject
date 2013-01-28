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
        const int MOVE_AREA_SIZE = 5;

        public Nest(Texture2D texture, Texture2D creatureTexture, Point position, short ID, Point mapDimensions, Point tileDimensions)
        {
            rand = new Random(DateTime.Now.Millisecond);
            _texture = texture;
            _position = new Vector2(position.X, position.Y);

            _moveArea = new Rectangle((int)_position.X - MOVE_AREA_SIZE, (int)_position.Y - MOVE_AREA_SIZE, MOVE_AREA_SIZE, MOVE_AREA_SIZE);
            if(_moveArea.X < 0)
                _moveArea.X = 0;
            if(_moveArea.Y < 0)
                _moveArea.Y = 0;
            //if (_moveArea.X + MOVE_AREA_SIZE > mapDimensions.X)
            //    _moveArea.Width = mapDimensions.Y;
            //if (_moveArea.Y + MOVE_AREA_SIZE > mapDimensions.Y)
            //    _moveArea.Height = mapDimensions.Y;

            for(int i = 0; i < MAX_CREATURES; i++)
            {
                Vector2 Position = new Vector2(rand.Next(_moveArea.X, (_moveArea.X + _moveArea.Width)),
                    rand.Next(_moveArea.Y / tileDimensions.Y, (_moveArea.Y + _moveArea.Height) / tileDimensions.Y));

                DebugLog.WriteLine(Convert.ToString(Position.X) + " " + Convert.ToString(Position.Y));

                creatures.Add(new Creature(ID, creatureTexture, new Vector2(Position.X, Position.Y), 1f));
            }
        }

        public void Update()
        {
            rand = new Random(DateTime.Now.Millisecond);
        }

        public void Draw()
        {
            SpriteManager.Draw(_texture, Camera.Transform(_position), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);

            foreach (Creature e in creatures)
                e.Draw();
        }
    }
}

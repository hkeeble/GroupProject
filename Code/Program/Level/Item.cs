using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VOiD
{
    class Item
    {
        private enum itemName
        {
            APPLE = 1,
            GOLDEN_APPLE = 2,
            SPRING_WATER = 3,
            HONEY = 4,
            CHILLI = 5
        }

        private Texture2D _texture;
        private string _name;
        private int _ID;
        private int _amount;

        public Item(int id, Microsoft.Xna.Framework.Content.ContentManager content)
        {
            _ID = id;
            if (_ID == (int)itemName.APPLE)
            {
                _name = "Apple";
                _texture = content.Load<Texture2D>("Sprites\\Apple");
            }
            if (_ID == (int)itemName.GOLDEN_APPLE)
            {
                _name = "Golden Apple";
                _texture = content.Load<Texture2D>("Sprites\\GoldenApple");
            }
            if (_ID == (int)itemName.SPRING_WATER)
            {
                _name = "Spring Water";
                _texture = content.Load<Texture2D>("Sprites\\SpringWater");
            }
            if (_ID == (int)itemName.HONEY)
            {
                _name = "Honey";
                _texture = content.Load<Texture2D>("Sprites\\Honey");
            }
            if (_ID == (int)itemName.CHILLI)
            {
                _name = "Chilli";
                _texture = content.Load<Texture2D>("Sprites\\Chilli");
            }
        }

        public string Name { get { return _name; } }
        public int ID { get { return _ID; } }
        public Texture2D Texture { get { return _texture; } }
        public int Amount { get { return _amount; } set { if (_amount + value < 99) _amount += value; } }
    }
}

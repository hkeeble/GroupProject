using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CreatureGame
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
        public int Amount;

        public Item(int id)
        {
            _ID = id;
            if (_ID == (int)itemName.APPLE)
                _name = "Apple";
            if (_ID == (int)itemName.GOLDEN_APPLE)
                _name = "Golden Apple";
            if (_ID == (int)itemName.SPRING_WATER)
                _name = "Spring Water";
            if (_ID == (int)itemName.HONEY)
                _name = "Honey";
            if (_ID == (int)itemName.CHILLI)
                _name = "Chilli";
        }

        public string Name { get { return _name; } }
        public int ID { get { return _ID; } }
        public Texture2D Texture { get { return _texture; } }
    }
}

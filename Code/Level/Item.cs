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
        public enum ItemName
        {
            Apple = 1,
            Golden_Apple = 2,
            Spring_Water = 3,
            Honey = 4,
            Chilli = 5
        }

        private Texture2D _texture;
        private int _ID;
        private int _amount;

        public Item(int id, Microsoft.Xna.Framework.Content.ContentManager content)
        {
            _ID = id;
            if (_ID == (int)ItemName.Apple)
                _texture = content.Load<Texture2D>("Sprites\\Apple");
            if (_ID == (int)ItemName.Golden_Apple)
                _texture = content.Load<Texture2D>("Sprites\\GoldenApple");
            if (_ID == (int)ItemName.Spring_Water)
                _texture = content.Load<Texture2D>("Sprites\\SpringWater");
            if (_ID == (int)ItemName.Honey)
                _texture = content.Load<Texture2D>("Sprites\\Honey");
            if (_ID == (int)ItemName.Chilli)
                _texture = content.Load<Texture2D>("Sprites\\Chilli");
        }

        public string Name
        {
            get
            {
                if (_ID != 0)
                    return ((ItemName)_ID).ToString().Replace('_', ' ');
                else
                    return "BLANK";
            }
        }


        public int ID { get { return _ID; } }
        public Texture2D Texture { get { return _texture; } }
        public int Amount { get { return _amount; } set { if (_amount + value < 99) _amount += value; } }
    }
}

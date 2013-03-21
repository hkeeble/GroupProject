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
        private string _description;

        public Item(int id, Microsoft.Xna.Framework.Content.ContentManager content)
        {
            _ID = id;
            if (_ID == (int)ItemName.Apple)
            {
                _texture = content.Load<Texture2D>("Sprites\\Apple");
                _description = "Restores a small amount of health when eaten.";
            }
            if (_ID == (int)ItemName.Golden_Apple)
            {
                _texture = content.Load<Texture2D>("Sprites\\GoldenApple");
                _description = "Fully restores a creature when eaten.";
            }
            if (_ID == (int)ItemName.Spring_Water)
            {
                _texture = content.Load<Texture2D>("Sprites\\SpringWater");
                _description = "Fresh from the springs. Fully restores stamina.";
            }
            if (_ID == (int)ItemName.Honey)
            {
                _texture = content.Load<Texture2D>("Sprites\\Honey");
                _description = "Sugary goodness. Increases a creature's obedience.";
            }
            if (_ID == (int)ItemName.Chilli)
            {
                _texture = content.Load<Texture2D>("Sprites\\Chilli");
                _description = "Makes a creature more aggressive when eaten.";
            }
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

        public string Description { get { return _description; } }

        public void Use()
        {
            _amount--;
        }

        public void Add(int amount)
        {
            _amount += amount;
            if (_amount > 99)
                _amount = 99;
        }

        public void Remove(int amount)
        {
            _amount -= amount;
            if (_amount < 0)
                _amount = 0;
        }

        public int ID { get { return _ID; } }
        public Texture2D Texture { get { return _texture; } }
        public int Amount { get { return _amount; } }
    }
}

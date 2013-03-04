using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VOiD.Components;

namespace VOiD
{
    class Inventory
    {
        private List<Item> items;

        public List<Item> Items { get { return items; } }

        public Inventory(int numberOfItemTypes, Microsoft.Xna.Framework.Content.ContentManager content)
        {
            items = new List<Item>(numberOfItemTypes);

            for(int i = 0; i < numberOfItemTypes; i++)
                items.Add(new Item(i, content));
        }

        public void AddItem(Item item, int amount = 1)
        {
            items[item.ID - 1].Amount += amount;
            DebugLog.WriteLine("Player collected item ID " + Convert.ToString(item.ID));
        }

        public void UseItem(int ID)
        {
            if (items[ID - 1].Amount > 0)
                items[ID - 1].Use();
        }

        public int NumberOfApples { get { return items[(int)Item.ItemName.Apple-1].Amount; } }
        public int NumberOfGoldenApples { get { return items[(int)Item.ItemName.Golden_Apple-1].Amount; } }
        public int NumberOfChilli{ get { return items[(int)Item.ItemName.Chilli-1].Amount; } }
        public int NumberOfHoney { get { return items[(int)Item.ItemName.Honey-1].Amount; } }
        public int NumberOfSpringWater { get { return items[(int)Item.ItemName.Spring_Water-1].Amount; } }
    }
}
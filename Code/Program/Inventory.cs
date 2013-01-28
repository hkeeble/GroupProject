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

        public Inventory(int numberOfItemTypes)
        {
            items = new List<Item>(numberOfItemTypes);

            for(int i = 0; i < numberOfItemTypes; i++)
                items.Add(new Item(i));
        }

        public void AddItem(Item item, int amount = 1)
        {
            items[item.ID-1].Amount += amount;
            DebugLog.WriteLine("Player collected item ID " + Convert.ToString(item.ID));
        }
    }
}

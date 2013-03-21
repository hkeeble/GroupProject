using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VOiD.Components;

namespace VOiD
{
    class Inventory
    {
        private Creature selectedDNA; // Currently selected DNA for use in Menus
        private Item selectedItem; // Currently selected item for us in Menus
        private List<Item> items;
        private List<Creature> dna;

        public List<Item> Items { get { return items; } }
        public List<Creature> DNA { get { return dna; } }
        public Creature SelectedDNA { get { return selectedDNA; } }
        public Item SelectedItem { get { return selectedItem; } }

        public Inventory(int numberOfItemTypes, Microsoft.Xna.Framework.Content.ContentManager content)
        {
            items = new List<Item>(numberOfItemTypes);
            dna = new List<Creature>();

            for(int i = 0; i < numberOfItemTypes; i++)
                items.Add(new Item(i, content));
        }

        /// <summary>
        /// Adds an item to the inventory.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <param name="amount">Amount of the item to add (defaults to 1)</param>
        public void AddItem(Item item, int amount = 1)
        {
            items[item.ID - 1].Add(amount);
            DebugLog.WriteLine("Player collected item ID " + Convert.ToString(item.ID));
        }

        public void RemoveItem(Item item, int amount = 1)
        {
            items[item.ID - 1].Remove(amount);
            DebugLog.WriteLine("Player lost item ID " + Convert.ToString(item.ID));
        }

        /// <summary>
        /// Uses an item with a specified ID, calling the item's own use function and reducing it's amount.
        /// </summary>
        /// <param name="ID">The ID of the item to use (you can use the enum Item.itemName and cast to integer for clarity)</param>
        public void UseItem(int ID)
        {
            // If none left and item is selected, clear selection
            if (selectedItem.ID == items[ID - 1].ID)
                if (items[ID - 1].Amount == 0)
                    selectedItem = null;
            if(!(BattleHandler.IsInSession && !BattleHandler.CanSelectAction))
            {
                if (items[ID - 1].Amount > 0)
                {
                    string itemUsed = "";
                    string effect = "";
                    if (ID == (int)Item.ItemName.Apple)
                    {
                        if (GameHandler.Player.Health < GameHandler.Player.Dominant.Health.Level - 1)
                        {
                            int restoreValue = (int)(GameHandler.Player.Dominant.Health.Level * 0.1f);
                            GameHandler.Player.Health += restoreValue;
                            itemUsed = "Apple";
                            if (GameHandler.Player.Health >= GameHandler.Player.Dominant.Health.Level)
                            {
                                GameHandler.Player.Health = GameHandler.Player.Dominant.Health.Level - 1;
                                effect = "Creature restored to full health!";
                            }
                            else
                                effect = "\nRestored " + restoreValue + " health points!";
                        }
                        else
                        {
                            GameHandler.CurrentMessageBoxText = "You're already at max health!";
                            Interface.ShowMessageBox();
                            return;
                        }
                    }

                    if (ID == (int)Item.ItemName.Golden_Apple)
                    {
                        itemUsed = "Golden Apple";
                        if (GameHandler.Player.Health < GameHandler.Player.Dominant.Health.Level - 1 || GameHandler.Player.Dominant.Health.Level < GameHandler.Player.Dominant.Health.Maximum)
                        {
                            if (GameHandler.Player.Health < GameHandler.Player.Dominant.Health.Level - 1)
                            {
                                int restoreValue = (int)(GameHandler.Player.Dominant.Health.Level * 0.5f);
                                GameHandler.Player.Health += restoreValue;
                                if (GameHandler.Player.Health >= GameHandler.Player.Dominant.Health.Level)
                                    GameHandler.Player.Health = GameHandler.Player.Dominant.Health.Level - 1;
                                effect = "\nRestored " + restoreValue + " health points!";
                            }

                            if (GameHandler.Player.Dominant.Health.Level < GameHandler.Player.Dominant.Health.Maximum)
                            {
                                ushort healthAdded = (ushort)((GameHandler.Player.Dominant.Health.Maximum - GameHandler.Player.Dominant.Health.Level) * 0.25);
                                GameHandler.Player.Dominant.Health.Level += healthAdded;
                                if (GameHandler.Player.Dominant.Health.Level > GameHandler.Player.Dominant.Health.Maximum)
                                    GameHandler.Player.Dominant.Health.Level = GameHandler.Player.Dominant.Health.Maximum;
                                effect += "\nMaximum health increased by " + healthAdded + "!";
                            }
                        }
                        else
                        {
                            GameHandler.CurrentMessageBoxText = "That would have no effect!";
                            Interface.ShowMessageBox();
                            return;
                        }
                    }

                    if (ID == (int)Item.ItemName.Spring_Water)
                    {
                        // What can this do?
                    }

                    if (ID == (int)Item.ItemName.Honey)
                    {
                        if (GameHandler.Player.Dominant.Obedience.Level < GameHandler.Player.Dominant.Obedience.Maximum - 1)
                        {
                            itemUsed = "Honey";
                            ushort increase = (ushort)((GameHandler.Player.Dominant.Obedience.Maximum - GameHandler.Player.Dominant.Obedience.Level) * 0.15);
                            GameHandler.Player.Dominant.Obedience.Level += increase;
                            effect = "\nYour creature is pleased, it's obedience increased by " + increase + "!";
                        }
                        else
                        {
                            GameHandler.CurrentMessageBoxText = "That would have no effect!\nYou're creature's obedience cannot improve anymore.";
                            Interface.ShowMessageBox();
                            return;
                        }
                    }

                    if (ID == (int)Item.ItemName.Chilli)
                    {
                        if (GameHandler.Player.Dominant.Agressiveness.Level < GameHandler.Player.Dominant.Agressiveness.Maximum - 1)
                        {
                            itemUsed = "Chilli";
                            ushort increase = (ushort)((GameHandler.Player.Dominant.Agressiveness.Maximum - GameHandler.Player.Dominant.Agressiveness.Level) * 0.15);
                            GameHandler.Player.Dominant.Agressiveness.Level += increase;
                            effect = "\nSpicy! You're creature's aggressiveness increased by " + increase + "!";
                        }
                        else
                        {
                            GameHandler.CurrentMessageBoxText = "That would have no effect!\nYou're creature's aggressiveness cannot improve anymore.";
                            Interface.ShowMessageBox();
                            return;
                        }
                    }

                    items[ID - 1].Use();

                    if (!BattleHandler.IsInSession)
                    {
                        GameHandler.CurrentMessageBoxText = "You fed your creature the " + itemUsed + "!" + effect;
                        Interface.ShowMessageBox();
                    }
                    else if (BattleHandler.IsInSession && BattleHandler.CanSelectAction)
                    {
                        BattleHandler.LastPlayerAction = "You fed your creature the " + itemUsed + "!" + effect;
                        BattleHandler.ActionSelected = true;
                        BattleHandler.AttackSelection = -1;
                        Interface.ExitSubMenu();
                    }
                }
            }
            else
            {
                GameHandler.CurrentMessageBoxText = "You can't use that right now!";
                Interface.ShowMessageBox();
            }
        }
        
        /// <summary>
        /// Sets the currently selected item.
        /// </summary>
        /// <param name="ID">ID of the item selected.</param>
        public void SetItem(int ID, Microsoft.Xna.Framework.Content.ContentManager content)
        {
            selectedItem = new Item(ID, content);
        }

        /// <summary>
        /// Sets the selected DNA to an element in the list of owned DNA. (Use UseDNA() to use the selected DNA)
        /// </summary>
        /// <param name="indexInList">The index in the list in which the DNA is located.</param>
        public void SetDNA(int indexInList)
        {
            selectedDNA = dna[indexInList];
        }

        /// <summary>
        /// Adds DNA to the owned DNA.
        /// </summary>
        /// <param name="newDNA">The new DNA data.</param>
        public void AddDNA(Creature newDNA)
        {
            dna.Add(newDNA);
        }

        /// <summary>
        /// Use the currently selected DNA. If selectedDNA is null, this function does nothing. (Use SetDNA(int indexInList) to set the selected DNA).
        /// </summary>
        public void UseDNA()
        {
            if (selectedDNA != null)
            {
                dna.Remove(selectedDNA);
                GameHandler.Player = new Creature(GameHandler.Player, selectedDNA, GameHandler.Player.Texture, GameHandler.Player.Position, GameHandler.Player.MoveSpeed, 32, 32, 100);
                selectedDNA = null;
            }
        }

        /// <summary>
        /// Clears any selections made in a menu
        /// </summary>
        public void ClearSelections()
        {
            if (selectedDNA != null)
                selectedDNA = null;
            if (selectedItem != null)
                selectedItem = null;
        }

        public int NumberOfApples { get { return items[(int)Item.ItemName.Apple-1].Amount; } }
        public int NumberOfGoldenApples { get { return items[(int)Item.ItemName.Golden_Apple-1].Amount; } }
        public int NumberOfChilli{ get { return items[(int)Item.ItemName.Chilli-1].Amount; } }
        public int NumberOfHoney { get { return items[(int)Item.ItemName.Honey-1].Amount; } }
        public int NumberOfSpringWater { get { return items[(int)Item.ItemName.Spring_Water-1].Amount; } }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using VOiD.Components;

namespace VOiD
{
    static class DevConsole
    {
        private static bool hasOpened = false;

        public static void Open(ContentManager content)
        {
            if (!hasOpened)
            {
                Console.WriteLine("---- Adventures of Dr. Lazarus Developer Console ----");
                hasOpened = false;
            }

            Console.WriteLine("Command: ");
            ProcessStringInput(Console.ReadLine().ToLower(), content);
        }

        private static void ProcessStringInput(string input, ContentManager content)
        {
            try
            {
                string[] args = input.Split(' ');

                if (args[0] == "player")
                {
                    if (args[1] == "creature")
                    {
                        if (args[2] == "id")
                        {
                            Console.WriteLine("Generating new player creature with ID " + args[3] + "...");
                            GameHandler.Player = new Creature(Convert.ToInt32(args[3]), GameHandler.Player.Texture, GameHandler.Player.Position, GameHandler.Player.MoveSpeed, 32, 32, 100);
                        }
                        if (args[2] == "breed")
                        {
                            Console.WriteLine("Breeding player creature with ID " + args[3] + "...");
                            GameHandler.Player = new Creature(GameHandler.Player, new Creature(Convert.ToInt16(args[3])), GameHandler.Player.Texture, Vector2.Zero, 1f, 32, 32, 100);
                        }
                    }

                    if (args[1] == "item")
                    {
                        if (args[2] == "add")
                        {
                            GameHandler.Inventory.AddItem(new Item(Convert.ToInt32(args[3]), content), (args.Length > 4 ? Convert.ToInt32(args[4]) : 1));
                        }
                    }
                    if (args[1] == "position")
                    {

                    }
                    if (args[1] == "boss")
                    {

                    }
                    if (args[1] == "level")
                    {

                    }
                }
            }
            catch
            {
                Console.WriteLine("Unrecognized command.\n");
            }
        }
    }
}

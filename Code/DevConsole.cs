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
        public static void Open(ContentManager content, GraphicsDevice graphics)
        {
            Console.WriteLine("---- Adventures of Dr. Lazarus Developer Console ----");

            Console.WriteLine("Command: ");
            ProcessStringInput(Console.ReadLine().ToLower(), content, graphics);
        }

        private static void ProcessStringInput(string input, ContentManager content, GraphicsDevice graphics)
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
                            GameHandler.Player = new Creature(GameHandler.Player, new Creature(Convert.ToInt16(args[3])), GameHandler.Player.Texture, Vector2.Zero, 2f, 32, 32, 100);
                        }
                    }
                    if (args[1] == "getid")
                        Console.WriteLine(GameHandler.Player.ID);
                    if (args[1] == "item")
                    {
                        if (args[2] == "add")
                            GameHandler.Inventory.AddItem(new Item(Convert.ToInt32(args[3]), content), (args.Length > 4 ? Convert.ToInt32(args[4]) : 1));
                        if (args[2] == "remove")
                            GameHandler.Inventory.RemoveItem(new Item(Convert.ToInt32(args[3]), content), (args.Length > 4 ? Convert.ToInt32(args[4]) : 1));
                    }
                    if (args[1] == "position")
                    {
                        string[] split = args[2].Split('-');
                        if(GameHandler.TileMap.Contains(new Point(Convert.ToInt32(split[0]), Convert.ToInt32(split[1]))))
                            GameHandler.Player.Position = new Vector2(Convert.ToInt32(split[0]) * GameHandler.TileMap.TileWidth, Convert.ToInt32(split[1]) * GameHandler.TileMap.TileHeight);
                        else
                            Console.WriteLine("Map does not contain that tile!\n");
                    }
                }
                if (args[0] == "loadlevel")
                {
                    GameHandler.CurrentLevel = Convert.ToInt32(args[1]);
                    Console.WriteLine("Loading level...");
                    GameHandler.LoadLevel(GameHandler.CurrentLevel, content, graphics);
                }
                if (args[0] == "boss")
                {
                    if (args[1] == "getid")
                        Console.WriteLine(Convert.ToString(GameHandler.Boss.ID));
                    if (args[1] == "id")
                    {
                        Console.WriteLine("Generating new boss creature with ID " + args[2] + "...");
                        GameHandler.Boss = new Creature(Convert.ToInt32(args[2]), GameHandler.Boss.Texture, GameHandler.Boss.Position, GameHandler.Player.MoveSpeed, 47, 48, 100);
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

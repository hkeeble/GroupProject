using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using VOiD.Components;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace VOiD
{
    public static class SaveHandler
    {
        private static string SAVE_FILE = "data.sav";
       
        /// <summary>
        /// Loads the current save game and returns true. If none exists, creates on and returns false.
        /// </summary>
        public static bool LoadSave(GraphicsDevice graphics, ContentManager content)
        {
            Interface.currentScreen = Screens.Loading;

            string cDir = Directory.GetCurrentDirectory();
            string[] files = Directory.GetFiles(cDir);

            bool saveExists = false;
            for (int i = 0; i < files.Length; i++)
                if (files[i] == cDir + "\\" + SAVE_FILE)
                    saveExists = true;

            if (!saveExists)
            {
                StreamWriter sw = new StreamWriter(SAVE_FILE);
                sw.WriteLine(1);
                sw.WriteLine(2345);
                sw.WriteLine(0);
                sw.Close();
            }

            StreamReader sr = new StreamReader(SAVE_FILE);
            GameHandler.CurrentLevel = Convert.ToInt32(sr.ReadLine());
            GameHandler.TileMap = new TileMap("Level" + GameHandler.CurrentLevel, graphics, content);
            GameHandler.Player = new Creature(Convert.ToInt16(sr.ReadLine()), content.Load<Texture2D>("Sprites\\handler"), GameHandler.TileMap.PlayerSpawn, 2f);
            GameHandler.Lab = new Entity(content.Load<Texture2D>("Sprites\\Lab"), GameHandler.TileMap.LabPosition, 0f, 32, 32, 100);
            GameHandler.Minimap = new Minimap(GameHandler.TileMap.Map, content.Load<Texture2D>("Sprites\\Nest"), content.Load<Texture2D>("Sprites\\Lab"), graphics);

            int currentID = Convert.ToInt32(sr.ReadLine());

            while (currentID != 0)
            {
                GameHandler.Inventory.AddItem(new Item(currentID, content), Convert.ToInt32(sr.ReadLine()));
                currentID = Convert.ToInt32(sr.ReadLine());
            }

            sr.Close();

            return saveExists;
        }

        public static void SaveGame()
        {
            StreamWriter sw = new StreamWriter(SAVE_FILE);
            sw.WriteLine(GameHandler.CurrentLevel);
            sw.WriteLine(GameHandler.Player.ID);

            foreach (Item i in GameHandler.Inventory.Items)
            {
                sw.WriteLine(i.ID);
                sw.WriteLine(i.Amount);
            }
            sw.WriteLine(0);

            sw.Close();
        }
    }
}

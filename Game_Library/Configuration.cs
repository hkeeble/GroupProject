using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Game_Library
{
    public class Configuration
    {
        public int Width;
        public int Height;
        public bool Fullscreen;

        public Configuration()
        {
            Width = 640;
            Height = 480;
            Fullscreen = false;
        }
    }
}

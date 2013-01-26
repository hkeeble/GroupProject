using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameLibrary
{
    public class Interface
    {
        public List<Object2D> content;
        public Color backgroundColor;
    }

    public class Object2D
    {
        public Vector2 Size;
        public string Location;
        public bool isCentered;
        public List<Object2D> Children;
        public Object2D()
        {
            Children = new List<Object2D>();
            isCentered = false;
            Init = false;
        }

        
        [ContentSerializerIgnore]
        public bool Init;
        [ContentSerializerIgnore]
        public Vector2 Position;
    }

    public class GraphicObject : Object2D
    {
        public string TextureLocation;
        public bool fullscreen;
        //public string Action;
        [ContentSerializerIgnore]
        public Texture2D Texture = null;
    }

    public class TextObject : Object2D
    {
        public string Text;
        public string Font;
        public byte fontSize;
        public Color fontColor;
    }


}

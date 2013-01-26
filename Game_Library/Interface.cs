﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameLibrary
{
    public class Interface
    {
        public List<Object2D> content;
        public Color backgroundColor=Color.Black;
        public bool Overlay;
        public Interface()
        {
            content = new List<Object2D>();
        }
    }

    public class Object2D
    {
        [ContentSerializerIgnore]
        public bool Init=false;
        
    }

    public class GraphicObject : Object2D
    {
        public Vector2 Size;
        public Vector2 Position;
        public bool isClickable;
        public string TextureLocation;
        public bool fullscreen;
        public string Action;
        [ContentSerializerIgnore]
        public Texture2D Texture = null;
        public List<Object2D> Children;
        public GraphicObject()
        {
            Children = new List<Object2D>();
        }
    }

    public class TextObject : Object2D
    {
        public string Text;
        public Color fontColor;
        public string Location;
        //[ContentSerializerIgnore]
        public Vector2 offset;
        public TextObject()
        {
            offset = Vector2.Zero;
        }
    }


}

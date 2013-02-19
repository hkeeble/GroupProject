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
        public Vector2 iSize;
        public Vector2 iPosition;
        [ContentSerializerIgnore]
        public Vector2 Size=Vector2.Zero;
        [ContentSerializerIgnore]
        public Vector2 Position=Vector2.Zero;
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
        public bool isCentered = false;
        public Vector2 ioffset;
        [ContentSerializerIgnore]
        public Vector2 offset=Vector2.Zero;
    }

    public class TextBoxObject : TextObject
    {
        public class Scroller : GraphicObject
        {
            public Vector2 scrollDirection;
        }

        public Vector2 Bounds;
        public Scroller UpScroller, DownScroller;
        [ContentSerializerIgnore]
        public Rectangle BoundingRect;
        [ContentSerializerIgnore]
        public Vector2 currentOffset;

        public void Scroll(Vector2 offset)
        {
            currentOffset += offset;
        }
    }
}

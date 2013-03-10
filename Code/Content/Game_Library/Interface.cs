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
        public bool isClickable;
        public string TextureLocation;
        public bool fullscreen;
        public string Action;
        public List<Object2D> Children;

        [ContentSerializerIgnore] public Vector2 Size = Vector2.Zero;
        [ContentSerializerIgnore] public Vector2 Position = Vector2.Zero;
        [ContentSerializerIgnore] public Texture2D Texture = null;

        public GraphicObject()
        {
            Children = new List<Object2D>();
        }
    }

    public class TextObject : Object2D
    {
        public string Text;
        public string Font;
        public Vector3 fontColor;
        public bool isCentered = false;
        public Vector2 ioffset;
        [ContentSerializerIgnore]
        public Vector2 offset=Vector2.Zero;
    }

    public class Scroller : GraphicObject
    {
        public Vector2 scrollDirection;
    }

    public class TextBoxObject : TextObject
    {
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

    public class ListBox : Object2D
    {
        public string ListContentType;
        public string Font;
        public Vector3 fontColor;
        public Scroller UpScroller, DownScroller;
        public Vector2 ioffset;
        public bool isClickable;

        public class Item
        {
            public Texture2D Texture;
            public Vector2 offset;
            public Rectangle BoundingRect;
            public string Action;
            public void Update(Vector2 screenPos)
            {
                BoundingRect = new Rectangle((int)screenPos.X, (int)screenPos.Y, Texture.Width, Texture.Height);
            }
        }

        [ContentSerializerIgnore] public Item[] Items = new Item[0];
        [ContentSerializerIgnore] public Rectangle BoundingRect;
        [ContentSerializerIgnore] public Vector2 currentOffset;
        [ContentSerializerIgnore] public Vector2 offset;

        public void Scroll(Vector2 offset)
        {
            currentOffset += offset;
        }
    }

    public class MinimapObject : GraphicObject
    {
        [ContentSerializerIgnore] public Rectangle DrawRect;
    }
}

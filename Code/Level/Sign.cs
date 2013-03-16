using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VOiD
{
    class Sign : Entity
    {
        private string _text;

        public Sign(Texture2D texture, Vector2 position, string text)
            : base(texture, position, 0f)
        {
            _text = text;
        }

        public string Text { get { return _text; } }
    }
}

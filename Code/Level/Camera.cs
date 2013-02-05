using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VOiD.Components;

namespace VOiD
{
    static class Camera
    {
        private static Vector2 _position = Vector2.Zero;
        private static Rectangle _mapRect = new Rectangle(0, 0, 0, 0);

        /// <summary>
        /// Returns the position of the camera, the top left pixel being displayed. Can also be used to set camera position.
        /// </summary>
        public static Vector2 Position
        {
            get { return _position; }
            set { _position = new Vector2(MathHelper.Clamp(value.X, 0, GameHandler.TileMap.Map.Width-Configuration.Bounds.Width),
                    MathHelper.Clamp(value.Y, -GameHandler.TileMap.TileHeight, GameHandler.TileMap.Map.Height-Configuration.Bounds.Height)); }
        }

        /// <summary>
        /// Rectangle representing entire map space.
        /// </summary>
        public static Rectangle MapRectangle
        {
            get { return _mapRect; }
            set { _mapRect = value; }
        }

        /// <summary>
        /// Determines if an object is visible on-screen by using it's bounding rectangle.
        /// </summary>
        /// <param name="bounds">The object's bounding rectangle.</param>
        public static bool ObjectVisible(Rectangle bounds)
        {
            return _mapRect.Intersects(bounds);
        }

        /// <summary>
        /// Moves the camera by a given offset.
        /// </summary>
        /// <param name="offset">Vector to apply to camera position.</param>
        public static void Move(Vector2 offset)
        {
            _position += offset;
        }

        /// <summary>
        /// Takes a world location and transforms it into screen coordinates.
        /// </summary>
        /// <param name="mapLocation">Location of object on map.</param>
        public static Vector2 Transform(Vector2 mapLocation)
        {
            return new Vector2((int)(mapLocation.X - _position.X), (int)(mapLocation.Y - _position.Y));
        }
    }
}

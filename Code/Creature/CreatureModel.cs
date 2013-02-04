using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VOiD
{
    class CreatureModel
    {
        public GeometricPrimitive model;
        public Vector3 Position;
        public Vector3 Rotation;
        public List<CreatureModel> children;
        Matrix wtf;

        public CreatureModel(GraphicsDevice graphicsDevice)
        {
            model = new CubePrimitive(graphicsDevice);
            Position = Vector3.Zero;
            Rotation = Vector3.Zero;
            wtf = new Matrix();
            children = new List<CreatureModel>();
        }

        public CreatureModel(GeometricPrimitive inPrim, Vector3 Pos, Vector3 Rot)
        {
            model = inPrim;
            Position = Pos;
            Rotation = Rot;
            wtf = new Matrix();
            children = new List<CreatureModel>();
        }

        public void Draw(Matrix Parent)
        {
            wtf = Parent * Matrix.CreateTranslation(Position) * Matrix.CreateFromYawPitchRoll(Rotation.X, Rotation.Y, Rotation.Z);
            model.Draw(wtf, Matrix.CreateTranslation(Vector3.Down*1.5f), Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f),VOiD.Components.Configuration.AspectRatio,0.1f,100f), Color.White);

            foreach (CreatureModel child in children)
            {
                child.Draw(wtf);
            }
        }
    }
}

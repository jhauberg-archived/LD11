using System;

using Microsoft.Xna.Framework;

using Mantra.Framework;

namespace Mantra.XNA
{
    public class Transform : Behavior
    {
        public Vector3 Position = Vector3.Zero;
        public Vector3 Scale = Vector3.One;
        public Quaternion Rotation = Quaternion.Identity;

        public Matrix World
        {
            get
            {
                return
                    Matrix.CreateScale(Scale) *
                    Matrix.CreateFromQuaternion(Rotation) *
                    Matrix.CreateTranslation(Position);
            }
        }
    }
}

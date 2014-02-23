using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Mantra.Framework;

namespace Mantra.XNA.Graphics
{
    public class Camera : Drawable
    {
        GraphicsDevice device;

        [Dependency]
        Transform transform = null;

        float near = 1;
        float far = 100;

        public Camera(GraphicsDevice device)
        {
            this.device = device;

            DrawOrder = -1;
        }

        public override void Draw()
        {
            device.Clear(ClearColor);
        }

        public Color ClearColor { get; set; }

        public virtual Matrix View
        {
            get
            {
                return Matrix.CreateLookAt(
                    transform.Position, 
                    Vector3.Zero, 
                    Vector3.Up);
            }
        }

        public virtual Matrix Projection
        {
            get
            {
                return Matrix.CreatePerspectiveFieldOfView(
                    MathHelper.PiOver4, 
                    (float)device.Viewport.Width / device.Viewport.Height, 
                    near, far);
            }
        }
    }
}

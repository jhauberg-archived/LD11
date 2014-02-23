using System;
using System.Collections.Generic;
using System.Linq;

using Mantra.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mantra.XNA;
using Mantra.XNA.Graphics;


namespace LD11.Logicals
{
    class ShakeEventArgs : EventArgs
    {
        Vector3 direction, force;

        public ShakeEventArgs(Vector3 direction, Vector3 force)
        {
            this.direction = direction;
            this.force = force;
        }

        public Vector3 Direction
        {
            get
            {
                return direction;
            }
        }

        // with just force we can just normalize it and have direction.. so both are kinda unecessarry

        public Vector3 Force
        {
            get
            {
                return force;
            }
        }
    }

    class StatueMouseController : Behavior
    {
        [Dependency(Group = Groups.Camera)]
        DefaultCamera camera = null;

        [Dependency]
        Transform transform = null;

        MouseState msLast;

        Matrix tmp_view, tmp_proj;

        float minMouseMove = 0.07f;

        float friction = 0.8f;
        float power = 30;

        ArcBall ball;

        //float left, up;

        public event EventHandler<ShakeEventArgs> Shaken;

        void OnShake(ShakeEventArgs args)
        {
            if (Shaken != null) {
                Shaken(this, args);
            }
        }

        public override void Initialize()
        {
            base.Initialize();

            ball = new ArcBall();

            tmp_proj = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GameContainer.Graphics.GraphicsDevice.Viewport.AspectRatio, 2, 3);
            tmp_view = Matrix.CreateLookAt(new Vector3(0, 0, 1), Vector3.Zero, Vector3.Up);

            msLast = Mouse.GetState();
        }

        public override void Update(TimeSpan elapsed)
        {
            MouseState ms = Mouse.GetState();

            GraphicsDevice device = GameContainer.Graphics.GraphicsDevice;

            if (ms.LeftButton == ButtonState.Pressed) {
                if (ms.X != msLast.X || ms.Y != msLast.Y) {
                    Vector3 objSpace = device.Viewport.Unproject(new Vector3(ms.X, ms.Y, 0), tmp_proj, tmp_view, Matrix.Identity);
                    Vector3 objSpaceLast = device.Viewport.Unproject(new Vector3(msLast.X, msLast.Y, 0), tmp_proj, tmp_view, Matrix.Identity);

                    objSpace.Z = 0;
                    objSpaceLast.Z = 0;

                    // crazy way of making sure we only continue on mousemoves that were not just pixel to pixel
                    float distance = Vector3.Distance(objSpaceLast, objSpace);

                    if (distance > minMouseMove) {
                        Vector3 moveDirection = Vector3.Normalize(objSpace - objSpaceLast);
                        Vector3 force = (moveDirection * power) * (float)elapsed.TotalSeconds;

                        transform.Position += force;

                        OnShake(new ShakeEventArgs(moveDirection, force));
                    }
                }
            }

            transform.Position *= friction;
            
            ball.Update(camera.View, camera.Projection, Matrix.Identity, device);

            transform.Rotation = ball.CurrentQuaternion;
            /*
            if (Keyboard.GetState().IsKeyDown(Keys.Left)) {
                left += 2 * (float)elapsed.TotalSeconds;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right)) {
                left -= 2 * (float)elapsed.TotalSeconds;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up)) {
                up -= 2 * (float)elapsed.TotalSeconds;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down)) {
                up += 2 * (float)elapsed.TotalSeconds;
            }

            transform.Rotation = Quaternion.CreateFromRotationMatrix(Matrix.CreateRotationZ(left) * Matrix.CreateRotationX(up));
            */

            msLast = ms;
        }
    }
}

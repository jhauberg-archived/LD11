using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mantra.Framework;
using Mantra.XNA;
using Microsoft.Xna.Framework;
using LD11.Visuals;

namespace LD11.Logicals
{
    class StatuePreviewSpinner : Behavior
    {
        [Dependency(Group = Groups.Camera)]
        Transform cameraTransform = null;

        [Dependency]
        StatueInformation statueInfo = null;
        [Dependency]
        Transform transform = null;

        float a = 0;

        public override void Initialize()
        {
            base.Initialize();

            transform.Scale = new Vector3(0.35f);
            transform.Position = new Vector3(0, 0, 0);
        }

        public override void Update(TimeSpan elapsed)
        {
            float distanceToCenter = statueInfo.BoundingSphere.Radius / (float)Math.Sin(MathHelper.PiOver4 / 2);

            Vector3 back = Vector3.Backward;

            cameraTransform.Position = (back * distanceToCenter);
            cameraTransform.Position.Y = statueInfo.BoundingSphere.Radius;

            a += 2 * (float)elapsed.TotalSeconds;

            transform.Rotation = Quaternion.CreateFromRotationMatrix(Matrix.CreateRotationY(a));
        }
    }
}

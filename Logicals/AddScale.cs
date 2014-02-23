using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mantra.XNA;
using Mantra.Framework;
using Microsoft.Xna.Framework;

namespace LD11.Logicals
{
    public class AddScale : Behavior
    {
        [Dependency]
        Transform transform = null;

        public override void Update(TimeSpan elapsed)
        {
            transform.Scale = Vector3.Clamp(
                transform.Scale += Amount * (float)elapsed.TotalSeconds,
                Minimum,
                Maximum);
        }

        public Vector3 Maximum { get; set; }
        public Vector3 Minimum { get; set; }
        public Vector3 Amount { get; set; }
    }
}

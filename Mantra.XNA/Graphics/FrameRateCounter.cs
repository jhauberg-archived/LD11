using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mantra.XNA.Graphics
{
    public class FrameRateCounter : Drawable
    {
        int frameCounter = 0;

        TimeSpan elapsedTime = TimeSpan.Zero;

        public override void Update(TimeSpan elapsed)
        {
            elapsedTime += elapsed;

            if (elapsedTime > TimeSpan.FromSeconds(1)) {
                elapsedTime -= TimeSpan.FromSeconds(1);
                FrameRate = frameCounter;
                frameCounter = 0;
            }
        }

        public override void Draw()
        {
            frameCounter++;
        }

        public int FrameRate { get; set; }
    }
}

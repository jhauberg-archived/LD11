using System;
using System.Collections.Generic;
using Mantra.Framework;

namespace Mantra.XNA.Graphics
{
    internal class DrawOrderComparer : IComparer<Drawable>
    {
        public static readonly DrawOrderComparer Default = new DrawOrderComparer();

        public int Compare(Drawable x, Drawable y)
        {
            if ((x == null) && (y == null)) {
                return 0;
            }

            if (x != null) {
                if (y == null) {
                    return -1;
                }
                if (x.Equals(y)) {
                    return 0;
                }
                if (x.DrawOrder < y.DrawOrder) {
                    return -1;
                }
            }

            return 1;
        }
    }

    internal class UpdateOrderComparer : IComparer<Behavior>
    {
        public static readonly UpdateOrderComparer Default = new UpdateOrderComparer();

        public int Compare(Behavior x, Behavior y)
        {
            if ((x == null) && (y == null)) {
                return 0;
            }

            if (x != null) {
                if (y == null) {
                    return -1;
                }
                if (x.Equals(y)) {
                    return 0;
                }
                if (x.UpdateOrder < y.UpdateOrder) {
                    return -1;
                }
            }

            return 1;
        }
    }
}

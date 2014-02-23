using System;

using Mantra.Framework;

namespace Mantra.XNA.Graphics
{
    /// <summary>
    /// Represents a drawable behavior.
    /// </summary>
    public abstract class Drawable : Behavior
    {
        bool visible = true;

        int drawOrder = 1;

        public event EventHandler DrawOrderChanged;

        void OnDrawOrderChanged(object sender, EventArgs args)
        {
            if (DrawOrderChanged != null) {
                DrawOrderChanged(sender, args);
            }
        }

        /// <summary>
        /// Handles drawing specific functionality.
        /// </summary>
        public virtual void Draw() { }

        /// <summary>
        /// Gets the order of which to draw in.
        /// </summary>
        public int DrawOrder
        {
            get
            {
                return drawOrder;
            }
            set
            {
                if (value != drawOrder) {
                    drawOrder = value;

                    OnDrawOrderChanged(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets or sets whether this behavior is to be drawn.
        /// </summary>
        public bool Visible
        {
            get
            {
                return visible;
            }
            set
            {
                visible = value;
            }
        }
    }
}

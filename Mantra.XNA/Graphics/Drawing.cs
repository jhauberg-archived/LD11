using System;
using System.Collections.Generic;

using Mantra.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mantra.XNA.Graphics
{
    /*
    public sealed class SpriteDrawing : Module<Drawable>
    {
        SpriteBatch batch;

        public SpriteDrawing(GraphicsDevice device)
            : base() 
        {
            batch = new SpriteBatch(device);
        }

        public SpriteDrawing(GraphicsDevice device, Delegater delegater)
            : base(delegater) { batch = new SpriteBatch(device); } // boo!

        public SpriteDrawing(GraphicsDevice device, Delegater delegater, bool traverseNow)
            : base(delegater, traverseNow) { batch = new SpriteBatch(device); } // boo!
        
        protected override void OnItemRemoved(Drawable item)
        {
            Sort();
        }

        void item_DrawOrderChanged(object sender, EventArgs e)
        {
            Sort();
        }

        void Sort()
        {
            items.Sort(DrawOrderComparer.Default);
        }

        /// <summary>
        /// Begins drawing of caught behaviors.
        /// </summary>
        /// <param name="elapsed"></param>
        public override void Update()
        {
            Draw();
        }

        /// <summary>
        /// Draws caught behaviors.
        /// </summary>
        void Draw()
        {
            for (int i = 0; i < items.Count; i++) {
                // TODO: replace with .Visible
                if (items[i].Enabled) {
                    items[i].Draw();
                }
            }
        }
    }
    */
    /// <summary>
    /// Represents a system for drawing drawable behaviors.
    /// </summary>
    // ThreadedDrawing, something.. just remove the *Module
    public sealed class Drawing : Module<Drawable>
    {
        public Drawing()
            : base() { }

        public Drawing(Delegater delegater)
            : base(delegater) { }

        public Drawing(Delegater delegater, bool traverseNow)
            : base(delegater, traverseNow) { }

        protected override void OnItemAdded(Drawable item)
        {
            Sort();
            
            item.DrawOrderChanged += item_DrawOrderChanged;
        }

        protected override void OnItemRemoved(Drawable item)
        {
            Sort();
        }

        void item_DrawOrderChanged(object sender, EventArgs e)
        {
            Sort();
        }

        void Sort()
        {
            items.Sort(DrawOrderComparer.Default);
        }

        /// <summary>
        /// Begins drawing of caught behaviors.
        /// </summary>
        /// <param name="elapsed"></param>
        public override void Update()
        {
            Draw();
        }

        /// <summary>
        /// Draws caught behaviors.
        /// </summary>
        void Draw()
        {
            for (int i = 0; i < items.Count; i++) {
                if (items[i].Visible) {
                    items[i].Draw();
                }
            }
        }
    }
}

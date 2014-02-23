using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Mantra.Framework;

using Mantra.XNA.Graphics;

namespace Mantra.XNA
{
    /// <summary>
    /// Represents a system for updating behaviors.
    /// </summary>
    public sealed class Timing : Module<Behavior>
    {
        public Timing()
            : base() { }

        public Timing(Delegater delegater)
            : base(delegater) { }

        protected override void OnItemAdded(Behavior item)
        {
            Sort();

            item.UpdateOrderChanged += item_UpdateOrderChanged;
        }

        protected override void OnItemRemoved(Behavior item)
        {
            Sort();
        }

        void item_UpdateOrderChanged(object sender, EventArgs e)
        {
            Sort();
        }

        void Sort()
        {
            items.Sort(UpdateOrderComparer.Default);
        }

        public override void Update()
        {
            for (int i = 0; i < items.Count; i++) {
                if (items[i].Enabled) {
                    items[i].Update(Elapsed);
                }
            }
        }

        // taking the cheap solution atm
        public TimeSpan Elapsed { get; set; }
    }
}

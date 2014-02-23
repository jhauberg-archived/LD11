using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mantra.Framework
{
    // uncertain about this coordinator class. Repository dont REALLY need it.. Module could be without it
    // as it's uncertain what modules actually do

    /// <summary>
    /// Represents a pluggable repository module.
    /// </summary>
    /// <typeparam name="T">The type of behaviors to catch.</typeparam>
    public abstract class Module<T> : Coordinator<T>, IUpdateable where T : Behavior
    {
        bool enabled = true;

        protected Module()
        {

        }

        protected Module(Delegater delegater)
        {
            Subscribe(delegater);
        }

        protected Module(Delegater delegater, bool traverseNow)
            : this(delegater)
        {
            Repository repo = delegater.Repository;

            if (traverseNow) {
                for (int i = 0; i < repo.Behaviors.Count; i++) {
                    Behavior behavior = repo.Behaviors[i];

                    delegater_Bound(this, 
                        new BindingEventArgs(behavior.Group, behavior));
                }
            }
        }

        public void Subscribe(Delegater delegater)
        {
            delegater.Bound += new EventHandler<BindingEventArgs>(delegater_Bound);
            delegater.Unbound += new EventHandler<BindingEventArgs>(delegater_Unbound);
        }

        void delegater_Unbound(object sender, BindingEventArgs e)
        {
            if (e.Behavior is T) {
                if (items.Contains((T)e.Behavior)) {
                    Remove((T)e.Behavior);
                }
            }
        }

        void delegater_Bound(object sender, BindingEventArgs e)
        {
            Type itemType = e.Behavior.GetType();

            if (itemType.IsSubclassOf(TypeToCatch) || itemType.Equals(TypeToCatch)) {
                Add((T)e.Behavior);
            }
        }

        /// <summary>
        /// Handles module specific logic.
        /// </summary>
        public virtual void Update() { }

        /// <summary>
        /// Gets the type that the module is catching.
        /// </summary>
        Type TypeToCatch
        {
            get
            {
                return typeof(T);
            }
        }

        public bool Enabled
        {
            get
            {
                return enabled;
            }
            set
            {
                enabled = value;
            }
        }
    }
}

using System;

namespace Mantra.Framework
{
    /// <summary>
    /// Provides data for behavior binding related events.
    /// </summary>
    public sealed class BindingEventArgs : EventArgs
    {
        string group;
        Behavior behavior;

        public BindingEventArgs(string group, Behavior behavior)
        {
            this.group = group;
            this.behavior = behavior;
        }

        /// <summary>
        /// Gets the group that the behavior was/is bound to.
        /// </summary>
        public string Group
        {
            get
            {
                return group;
            }
        }

        /// <summary>
        /// Gets the behavior that was bound/unbound.
        /// </summary>
        public Behavior Behavior
        {
            get
            {
                return behavior;
            }
        }
    }
}

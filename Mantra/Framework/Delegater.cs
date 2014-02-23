using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mantra.Framework
{
    /// <summary>
    /// Provides methods for binding/unbinding behaviors to groups.
    /// </summary>
    public sealed class Delegater
    {
        internal Repository Repository
        {
            get;
            set;
        }

        /// <summary>
        /// Fired when a behavior has been bound to a group, and has been initialized.
        /// </summary>
        public event EventHandler<BindingEventArgs> Bound;
        /// <summary>
        /// Fired when a behavior has been un-bound from a group.
        /// </summary>
        public event EventHandler<BindingEventArgs> Unbound;

        public Delegater(Repository system)
        {
            this.Repository = system;

            // meh
            Repository.Delegater = this;
        }

        /// <summary>
        /// Binds a behavior to a group.
        /// </summary>
        /// <typeparam name="T">The type of the behavior.</typeparam>
        /// <param name="group">The name of the group.</param>
        /// <param name="behavior">The behavior to add.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.ArgumentException"></exception>
        public void Bind<T>(string group, T behavior) where T : Behavior
        {
            if (behavior == null) {
                throw new ArgumentNullException("behavior", "The delegater failed to bind the behavior because it was 'null'");
            }

            if (behavior.Group != null && behavior.Group.Length > 0) {
                if (behavior.Group == group) {
                    throw new ArgumentException("The delegater failed to bind the behavior because it was already bound to the same group.", "behavior");
                } else {
                    Unbind(behavior.Group, behavior);
                }
            }

            behavior.Group = group;
            behavior.Repository = Repository;

            behavior.Initialize();

            Repository.Add(behavior);

            OnBound(group, behavior);
        }

        /// <summary>
        /// Un-binds a behavior from a group.
        /// </summary>
        /// <typeparam name="T">The type of the behavior</typeparam>
        /// <param name="group">The name of the group.</param>
        /// <param name="behavior">The behavior.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.ArgumentException"></exception>
        public void Unbind<T>(string group, T behavior) where T : Behavior
        {
            if (behavior == null) {
                throw new ArgumentNullException("behavior", "The delegater failed to unbind the behavior because it was 'null'");
            }

            if (!(behavior.Group != null && behavior.Group.Length > 0)) {
                throw new ArgumentException("The delegater failed to unbind the behavior because it was not bound to any group.", "behavior");
            }

            if (!Repository.Behaviors.Contains(behavior)) {
                throw new ArgumentException("The delegater failed to unbind the behavior because it was not registered.", "behavior");
            }

            Repository.Remove(behavior);

            behavior.Group = null;
            behavior.Repository = null;

            OnUnbound(group, behavior);
        }

        /// <summary>
        /// Un-binds a behavior from a group.
        /// </summary>
        /// <typeparam name="T">The type of the behavior</typeparam>
        /// <param name="group">The name of the group.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.ArgumentException"></exception>
        public void Unbind<T>(string group) where T : Behavior
        {
            Unbind(group, Repository.Get<T>(group));
        }

        private void OnBound(string group, Behavior behavior)
        {
            if (Bound != null) {
                Bound(this, new BindingEventArgs(group, behavior));
            }
        }

        private void OnUnbound(string group, Behavior behavior)
        {
            if (Unbound != null) {
                Unbound(this, new BindingEventArgs(group, behavior));
            }
        }
    }
}

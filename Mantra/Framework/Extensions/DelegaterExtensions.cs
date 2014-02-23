using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mantra.Framework.Extensions
{
    /// <summary>
    /// Provides extension methods that deals with behavior bindings.
    /// </summary>
    public static class DelegaterExtensions
    {
        /// <summary>
        /// Re-names a group, and binds all bound behaviors to the new group.
        /// </summary>
        /// <param name="delegater">The BehaviorDelegater that maintains the behaviors.</param>
        /// <param name="oldGroup">The name of the group to re-name.</param>
        /// <param name="newGroup">The new name of the group.</param>
        public static void Rename(this Delegater delegater, string oldGroup, string newGroup)
        {
            var boundBehaviors = delegater.Repository.Behaviors.Where
                (
                    behavior => behavior.Group == oldGroup
                );

            for (int i = boundBehaviors.Count() - 1; i >= 0; i--) {
                delegater.Bind(newGroup, boundBehaviors.ElementAt(i));
            }
        }

        /// <summary>
        /// Un-binds all behaviors from a group.
        /// </summary>
        /// <param name="delegater"></param>
        /// <param name="group"></param>
        public static void UnbindAll(this Delegater delegater, string group)
        {
            var boundBehaviors = delegater.Repository.GetAllBoundBehaviors(group);

            foreach (Behavior behavior in boundBehaviors) {
                delegater.Unbind(group, behavior);
            }
        }
    }
}

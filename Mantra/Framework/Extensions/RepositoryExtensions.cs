using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Mantra.Framework.Extensions
{
    /// <summary>
    /// Provides extention methods for the behavior database.
    /// </summary>
    public static class RepositoryExtensions
    {
        /// <summary>
        /// Returns a read-only list of all groups that are currently in use.
        /// </summary>
        /// <returns></returns>
        public static ReadOnlyCollection<string> GetAllGroups(this Repository repository)
        {
            List<string> groups = new List<string>();

            for (int i = 0; i < repository.Behaviors.Count; i++) {
                if (!groups.Contains(repository.Behaviors[i].Group)) {
                    groups.Add(repository.Behaviors[i].Group);
                }
            }

            return new ReadOnlyCollection<string>(groups);
        }

        /// <summary>
        /// Returns a read-only list of all behaviors that are currently bound to a group.
        /// </summary>
        /// <param name="group">The name of the group.</param>
        /// <returns></returns>
        public static ReadOnlyCollection<Behavior> GetAllBoundBehaviors(this Repository repository, string group)
        {
            // could be optimized to run faster if implemented directly in Repository, as it then has access to the dictionary
            var results = repository.Behaviors.Where
                (
                    x => x.Group == group
                );
            
            return new ReadOnlyCollection<Behavior>(results.ToList());
        }

        /// <summary>
        /// Returns a read-only list of all behaviors in the repository that are of a given type.
        /// </summary>
        /// <typeparam name="T">The type of the behavior.</typeparam>
        /// <returns></returns>
        public static ReadOnlyCollection<T> GetAllBoundBehaviors<T>(this Repository repository) where T : Behavior
        {
            var results = repository.Behaviors.Where
                (
                    x => x.GetType() == typeof(T)
                );

            List<T> beh = new List<T>();

            foreach (Behavior b in results) {
                beh.Add(b as T);
            }

            return new ReadOnlyCollection<T>(beh);
        }

        // same for OfType (for derived behaviors)
    }
}

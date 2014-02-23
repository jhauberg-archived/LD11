using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Mantra.Framework
{
    /// <summary>
    /// Represents a database of behaviors.
    /// </summary>
    public sealed class Repository : Coordinator<Behavior>
    {
        Dictionary<string, Dictionary<Type, Behavior>> dict;

        ReadOnlyCollection<Behavior> readOnlyItems;

        public Repository()
            : this(2) { }

        public Repository(int initialCapacity)
            : base(initialCapacity)
        {
            Delegater = new Delegater(this);

            dict = new Dictionary<string, Dictionary<Type, Behavior>>(initialCapacity);

            readOnlyItems = new ReadOnlyCollection<Behavior>(items);
        }

        protected override void OnItemAdded(Behavior item)
        {
            Type itemType = item.GetType();

            if (dict.ContainsKey(item.Group)) {
                try {
                    dict[item.Group].Add(
                        itemType, item);
                } catch (ArgumentException) {
                    throw new ArgumentException("The coordinator failed to bind the behavior because a behavior of the same type was already bound to this group.", "behavior");
                }
            } else {
                dict.Add(
                    item.Group,
                    new Dictionary<Type, Behavior>() { 
                        { itemType, item } });
            }
        }

        protected override void OnItemRemoved(Behavior item)
        {
            if (dict.ContainsKey(item.Group)) {
                if (dict[item.Group].ContainsValue(item)) {
                    dict[item.Group].Remove(item.GetType());
                }
            }
        }

        /// <summary>
        /// Attempts to retrieve a reference to a behavior of a specific type that is bound to a group.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="group"></param>
        /// <returns></returns>
        public T Get<T>(string group) where T : Behavior
        {
            T behavior = null;

            return Get(group, behavior);
        }

        /// <summary>
        /// Attempts to retrieve a reference to a behavior of a specific type that is bound to a group.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="group"></param>
        /// <param name="behavior"></param>
        /// <returns></returns>
        public T Get<T>(string group, T behavior) where T : Behavior
        {
            try {
                behavior = dict[group][typeof(T)] as T;
            } catch (KeyNotFoundException) {
                throw new ArgumentException("The coordinator failed to retrieve a reference to a behavior of T because it has not been bound to this group.", "behavior");
            }

            return behavior;
        }

        /// <summary>
        /// Attempts to retrieve a reference to a behavior of a type that is bound to a group.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="group"></param>
        /// <returns></returns>
        public T GetOfType<T>(string group) where T : Behavior
        {
            T behavior = null;

            return GetOfType(group, behavior);
        }

        /// <summary>
        /// Attempts to retrieve a reference to a behavior of a type that is bound to a group.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="group"></param>
        /// <param name="behavior"></param>
        /// <returns></returns>
        public T GetOfType<T>(string group, T behavior) where T : Behavior
        {
            var results = dict[group].Values.Where
                (
                    x => x is T
                );

            if (results.Count() > 0) {
                behavior = results.First() as T;
            } else {
                throw new ArgumentException("The coordinator failed to retrieve a reference to a behavior of T because it has not been bound to this group.", "behavior");
            }

            return behavior;
        }

        /// <summary>
        /// Determines if a behavior of a type is bound to a group.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="group"></param>
        /// <returns></returns>
        public bool IsBoundOfType<T>(string group) where T : Behavior
        {
            T behavior = null;

            return IsBoundOfType(group, behavior);
        }

        /// <summary>
        /// Determines if a behavior of a type is bound to a group.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="group"></param>
        /// <param name="behavior"></param>
        /// <returns></returns>
        public bool IsBoundOfType<T>(string group, T behavior) where T : Behavior
        {
            try {
                GetOfType(group, behavior);
            } catch (ArgumentException) {
                // ..
            }

            return behavior != null;
        }

        /// <summary>
        /// Determines if a behavior of a specific type is bound to a group.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="group"></param>
        /// <returns></returns>
        public bool IsBound<T>(string group) where T : Behavior
        {
            T behavior = null;

            return IsBound(group, behavior);
        }

        /// <summary>
        /// Determines if a behavior of a specific type is bound to a group.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="group"></param>
        /// <param name="behavior"></param>
        /// <returns></returns>
        public bool IsBound<T>(string group, T behavior) where T : Behavior
        {
            try {
                Get(group, behavior);
            } catch (ArgumentException) {
                // ..
            }

            return behavior != null;
        }

        /// <summary>
        /// Gets the delegater assigned to the database.
        /// </summary>
        public Delegater Delegater
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets a read-only list of all behaviors kept in the database.
        /// </summary>
        public ReadOnlyCollection<Behavior> Behaviors
        {
            get
            {
                return readOnlyItems;
            }
        }
    }
}

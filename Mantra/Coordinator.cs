using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Mantra
{
    /// <summary>
    /// Represents the base of classes wishing to maintain a list of a type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Coordinator<T>
    {
        protected List<T> items;

        protected Coordinator()
            : this(2) { }

        protected Coordinator(int initialCapacity)
        {
            items = new List<T>(initialCapacity);
        }
        
        internal void Add(T item)
        {
            items.Add(item);

            OnItemAdded(item);
        }

        internal void Remove(T item)
        {
            items.Remove(item);

            OnItemRemoved(item);
        }

        protected virtual void OnItemAdded(T item) { }
        protected virtual void OnItemRemoved(T item) { }
    }
}

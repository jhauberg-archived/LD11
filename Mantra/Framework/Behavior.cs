using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections.ObjectModel;

namespace Mantra.Framework
{
    /// <summary>
    /// Represents a modular behavior.
    /// </summary>
    public abstract class Behavior
    {
        bool enabled = true;

        int updateOrder = 1;

        public event EventHandler UpdateOrderChanged;

        void OnUpdateOrderChanged(object sender, EventArgs args)
        {
            if (UpdateOrderChanged != null) {
                UpdateOrderChanged(sender, args);
            }
        }

        Dictionary<FieldInfo, Dependency> dependencies = new Dictionary<FieldInfo, Dependency>();

        protected Behavior()
        {
            // scour fields and find those marked as dependencies
            // the base constructor is always called first, even if not explicitly called through : base()
            FieldInfo[] fields = GetType().GetFields(
                BindingFlags.Instance |
                BindingFlags.Public |
                BindingFlags.NonPublic);

            foreach (FieldInfo field in fields) {
                foreach (Dependency dependency in field.GetCustomAttributes(typeof(Dependency), false)) {
                    if (!field.FieldType.IsSubclassOf(typeof(Behavior))) {
                        throw new ArgumentException(
                            "This field can not be marked as a dependency.", field.DeclaringType.ToString() + "." + field.Name);
                    }

                    try {
                        dependencies.Add(field, dependency);
                    } catch (ArgumentNullException ane) {
                        Console.Error.WriteLine(ane.ToString());
                    } catch (ArgumentException ae) {
                        Console.Error.WriteLine(ae.ToString());
                    }
                }
            }
        }

        /// <summary>
        /// Initializes behavior specific variables, and injects local dependencies.
        /// </summary>
        public virtual void Initialize()
        {
            foreach (FieldInfo field in dependencies.Keys) {
                Dependency dependency = dependencies[field];

                string grp =
                    (dependency.Group != null && dependency.Group.Length > 0) ?
                        dependency.Group :  // other
                        Group;              // local

                InjectField(grp, field);
            }
        }
        
        /// <summary>
        /// Handles behavior specific logic.
        /// </summary>
        /// <param name="elapsed">The amount of time spent since previous frame.</param>
        public virtual void Update(TimeSpan elapsed) { }

        void InjectField(string toGroup, FieldInfo field)
        {
            Behavior behavior = BehaviorHelper.GetByType(Repository, toGroup, field.FieldType);

            if (behavior != null) {
                field.SetValue(this, behavior);

                return;
            }

            behavior = BehaviorHelper.Create(field.FieldType);

            if (behavior != null) {
                Repository.Delegater.Bind(toGroup, behavior);

                field.SetValue(this, behavior);
            }
        }

        /// <summary>
        /// Gets or sets whether this behavior is to be updated.
        /// </summary>
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

        public int UpdateOrder
        {
            get
            {
                return updateOrder;
            }
            set
            {
                if (updateOrder != value) {
                    updateOrder = value;

                    OnUpdateOrderChanged(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets the group that this behavior is bound to.
        /// </summary>
        public string Group
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the repository that this behavior is contained in.
        /// </summary>
        public Repository Repository
        {
            get;
            internal set;
        }
    }
}

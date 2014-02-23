using System;

namespace Mantra.Framework
{
    /// <summary>
    /// Indicates that the field is a dependency.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class Dependency : Attribute
    {
        /// <summary>
        /// Gets or sets the name of the group the dependency reference should be retrieved from.
        /// </summary>
        /// <remarks>
        /// The group can be the local group, but if specified, then injection is delayed until Start. (i.e. not during Initialize)
        /// </remarks>
        public string Group
        {
            get;
            set;
        }

        // add property called Wait,
        // waits with binding/injecting dependency until it has been caught, this way we can get rid of the Start() idea all together
        // since the behavior will be injected as soon as its registered as bound to specified group.. this leads to bunch of other probs though
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mantra.Framework
{
    public static class BehaviorHelper
    {
        public static Behavior Create(Type behaviorType)
        {
            Behavior result = null;

            try {
                result = Activator.CreateInstance(behaviorType) as Behavior;
            } catch (MissingMethodException) {
                throw new MissingMethodException(String.Format(
                    "The behavior type '{0}' does not implement an empty constructor.", behaviorType.ToString()));
            }

            return result;
        }

        public static Behavior GetByType(Repository repository, string group, Type behaviorType)
        {
            var found = repository.Behaviors.Where
                (
                    x => x.GetType().Equals(behaviorType) && x.Group == group
                );

            Behavior result = null;

            if (found.Count() > 0) {
                result = found.First();
            }

            return result;
        }
    }
}

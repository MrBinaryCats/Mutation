using UnityEngine;
using UnityEngine.Serialization;

namespace Mutations.Mutations.Core
{
    /// <summary>
    ///     Standard mutation type.
    ///     Allows for a default mutation state, and changing of that state
    /// </summary>
    /// <typeparam name="T">The type of the data the mutation will modify</typeparam>
    /// <typeparam name="T1">The type of object which will be affected by the</typeparam>
    public abstract class Mutation<T, T1> : MutationBase where T1 : Object
    {
        /// <summary>
        ///     The default value the mutation state should have
        /// </summary>
        [SerializeField] protected T defaultValue;

        public T DefaultValue => defaultValue;

        /// <summary>
        ///     Applies a new mutation value to the object
        /// </summary>
        /// <param name="instance">the instance of the object which will get affected by the mutation</param>
        /// <param name="value">the value that should be set</param>
        public abstract void Apply(T1 instance, in T value);

        /// <summary>
        ///     Resets the mutation state of the instance to the mutations default value
        /// </summary>
        /// <param name="instance">the instance of the object which will get affected by the mutation</param>
        /// <returns>The new which was applied</returns>
        public virtual T ResetToDefault(T1 instance)
        {
            Apply(instance, defaultValue);
            return defaultValue;
        }
    }
}
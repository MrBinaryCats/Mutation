using DefaultNamespace;

namespace Mutations.Mutations.Core
{
    /// <summary>
    /// Standard mutation type.
    /// Allows for a default mutation state, and changing of that state
    /// </summary>
    /// <typeparam name="T">The type of the data the mutation will modify</typeparam>
    /// <typeparam name="T1">The type of object which will be affected by the</typeparam>
    public abstract class Mutation<T, T1> : MutationBase where T1 : UnityEngine.Object
    {
        /// <summary>
        /// The default value the mutation state should have
        /// </summary>
        public T DefaultValue;

        /// <summary>
        /// Applies a new mutation value to the object
        /// </summary>
        /// <param name="instance">the instance of the object which will get affected by the mutation</param>
        /// <param name="value">the value that should be set</param>
        public abstract void Apply(T1 instance,in T value);

        /// <summary>
        /// Resets the mutation state of the instance to the mutations default value
        /// </summary>
        /// <param name="instance">the instance of the object which will get affected by the mutation</param>
        public virtual void ResetToDefault(T1 instance)
        {
            Apply(instance, DefaultValue);
        }
    }

}
using UnityEngine;

namespace DefaultNamespace
{
    /// <summary>
    /// Mutation which can have multiple possible states.
    /// Each possible state is a set distance (step) away from the previous state 
    /// </summary>
    /// <typeparam name="T"><inheritdoc/></typeparam>
    /// <typeparam name="T1"><inheritdoc/></typeparam>
    public abstract class MutationStep<T, T1> : Mutation<T, T1> where T1: UnityEngine.Object
    {
        /// <summary>
        /// The amount to increment/decrement the current mutation state by
        /// </summary>
        public T Step;

        /// <summary>
        /// Increments the current current value, by the step, and applies it to the instance
        /// </summary>
        /// <param name="instance">The instance of the object which will get affected by the mutation</param>
        /// <param name="currentValue">The current value of the mutation</param>
        /// <returns>the new value which was applied to the instance</returns>
        public abstract T ApplyNext(T1 instance, T currentValue);
        /// <summary>
        /// Decrements the current current value, by the step, and applies it to the instance
        /// </summary>
        /// <param name="instance">The instance of the object which will get affected by the mutation</param>
        /// <param name="currentValue">The current value of the mutation</param>
        /// <returns>the new value which was applied to the instance</returns>
        public abstract T ApplyPrevious(T1 instance, T currentValue);

    }

}
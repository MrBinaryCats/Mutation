using UnityEngine;

namespace Mutations.Mutations.Core
{
    /// <summary>
    /// Mutation which can have multiple possible states.
    /// Each possible state is stored in an array, the elements can be applied to the object
    /// </summary>
    /// <typeparam name="T"><inheritdoc/></typeparam>
    /// <typeparam name="T1"><inheritdoc/></typeparam>
    public abstract class Mutations<T, T1> : Mutation<T, T1> where T1 : UnityEngine.Object
    {
        /// <summary>
        /// The Default index of the values array which should be used for the default mutation state
        /// </summary>
        public int DefaultIndex;
        /// <summary>
        /// List of possible mutation states that can be applied to the object
        /// </summary>
        public T[] Values;
        /// <summary>
        /// How to iterate though the mutation state when <see cref="ApplyNext"/>/<see cref="ApplyNext"/>is used
        /// </summary>
        public WrapMode Wrap;

        /// <summary>
        /// Iterates through the mutation states and applies the next mutation state to the given instance
        /// </summary>
        /// <param name="instance">The instance of the object which will get affected by the mutation</param>
        /// <param name="currentIndex">The current mutation state index</param>
        /// <returns>The new current mutation state index</returns>
        public int ApplyNext(T1 instance, int currentIndex)
        {
            var newIndex = currentIndex;
            switch (Wrap)
            {
                case WrapMode.Once:
                    if (currentIndex == int.MinValue)
                    {
                        Debug.LogError("Already cycled through the array");
                        return int.MinValue;
                    }

                    newIndex = ++currentIndex;
                    if (currentIndex >= Values.Length)
                        return int.MinValue;
                    break;
                case WrapMode.Loop:
                    if (currentIndex >= Values.Length - 1)
                        currentIndex = 0;
                    else
                        currentIndex++;
                    newIndex = currentIndex;
                    break;
                case WrapMode.PingPong:
                    currentIndex++;
                    newIndex = Mathf.RoundToInt(Mathf.PingPong(currentIndex, Values.Length - 1));
                    break;
                case WrapMode.Default:
                case WrapMode.ClampForever:
                    newIndex = currentIndex = Mathf.Min(currentIndex + 1, Values.Length - 1);
                    break;
            }

            Apply(instance, Values[newIndex]);
            return currentIndex;
        }
        /// <summary>
        /// Iterates through the mutation states and applies the previously mutation state to the given instance
        /// </summary>
        /// <param name="instance">The instance of the object which will get affected by the mutation</param>
        /// <param name="currentIndex">The current mutation state index</param>
        /// <returns>The new current mutation state index</returns>
        public int ApplyPrevious(T1 instance, int currentIndex)
        {
            var newIndex = currentIndex;
            switch (Wrap)
            {
                case WrapMode.Once:
                    if (currentIndex == int.MinValue)
                    {
                        Debug.LogError("Already cycled through the array");
                        return int.MinValue;
                    }

                    newIndex = --currentIndex;
                    if (currentIndex < 0)
                        return int.MinValue;
                    break;
                case WrapMode.Loop:
                    if (currentIndex == 0)
                        currentIndex = Values.Length - 1;
                    else
                        currentIndex--;
                    newIndex = currentIndex;
                    break;
                case WrapMode.PingPong:
                    currentIndex--;
                    newIndex = Mathf.RoundToInt(Mathf.PingPong(currentIndex - 1, Values.Length - 1));
                    break;
                case WrapMode.Default:
                case WrapMode.ClampForever:
                    newIndex = currentIndex = Mathf.Max(currentIndex - 1, 0);
                    break;
            }

            Apply(instance, Values[newIndex]);
            return currentIndex;
        }

        /// <inheritdoc/>
        public override void ResetToDefault(T1 instance)
        {
            if (Values.Length == 0)
            {
                Debug.LogWarning($"{instance.name} - Mutation, {name}, has no list of values");
                return;
            }

            Apply(instance, Values[DefaultIndex]);
        }
    }
}
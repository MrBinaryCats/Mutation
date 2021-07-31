using UnityEngine;
using UnityEngine.Serialization;

namespace Mutations.Mutations.Core
{
    /// <summary>
    ///     Mutation which can have multiple possible states.
    ///     Each possible state is stored in an array, the elements can be applied to the object
    /// </summary>
    /// <typeparam name="T">
    ///     <inheritdoc />
    /// </typeparam>
    /// <typeparam name="T1">
    ///     <inheritdoc />
    /// </typeparam>
    public abstract class Mutations<T, T1> : Mutation<T, T1> where T1 : Object
    {
        /// <summary>
        ///     The Default index of the values array which should be used for the default mutation state
        /// </summary>
        [SerializeField] protected int defaultIndex;

        /// <summary>
        ///     List of possible mutation states that can be applied to the object
        /// </summary>
        [SerializeField] protected T[] values;

        /// <summary>
        ///     How to iterate though the mutation state when <see cref="ApplyNext" />/<see cref="ApplyNext" />is used
        /// </summary>
        [SerializeField] protected WrapMode wrap;
        /// <summary>
        ///     How to iterate though the mutation state when <see cref="ApplyNext" />/<see cref="ApplyNext" />is used
        /// </summary>
        public WrapMode Wrap => wrap;
        /// <summary>
        ///     Iterates through the mutation states and applies the next mutation state to the given instance
        /// </summary>
        /// <param name="instance">The instance of the object which will get affected by the mutation</param>
        /// <param name="currentIndex">The current mutation state index</param>
        /// <returns>The new current mutation state index</returns>
        public int ApplyNext(T1 instance, int currentIndex)
        {
            var newIndex = currentIndex;
            switch (wrap)
            {
                case WrapMode.Once:
                    if (currentIndex == int.MinValue)
                    {
                        Debug.LogError("Already cycled through the array");
                        return int.MinValue;
                    }

                    newIndex = ++currentIndex;
                    if (currentIndex >= values.Length)
                        return int.MinValue;
                    break;
                case WrapMode.Loop:
                    if (currentIndex >= values.Length - 1)
                        currentIndex = 0;
                    else
                        currentIndex++;
                    newIndex = currentIndex;
                    break;
                case WrapMode.PingPong:
                    currentIndex++;
                    newIndex = Mathf.RoundToInt(Mathf.PingPong(currentIndex, values.Length - 1));
                    break;
                case WrapMode.Default:
                case WrapMode.ClampForever:
                    newIndex = currentIndex = Mathf.Min(currentIndex + 1, values.Length - 1);
                    break;
            }

            if (newIndex < 0 || newIndex >= values.Length)
                Apply(instance, defaultValue);
            else
                Apply(instance, values[newIndex]);
            return currentIndex;
        }

        /// <summary>
        ///     Iterates through the mutation states and applies the previously mutation state to the given instance
        /// </summary>
        /// <param name="instance">The instance of the object which will get affected by the mutation</param>
        /// <param name="currentIndex">The current mutation state index</param>
        /// <returns>The new current mutation state index</returns>
        public int ApplyPrevious(T1 instance, int currentIndex)
        {
            var newIndex = currentIndex;
            switch (wrap)
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
                        currentIndex = values.Length - 1;
                    else
                        currentIndex--;
                    newIndex = currentIndex;
                    break;
                case WrapMode.PingPong:
                    currentIndex--;
                    newIndex = Mathf.RoundToInt(Mathf.PingPong(currentIndex - 1, values.Length - 1));
                    break;
                case WrapMode.Default:
                case WrapMode.ClampForever:
                    newIndex = currentIndex = Mathf.Max(currentIndex - 1, 0);
                    break;
            }

            if (newIndex < 0 || newIndex >= values.Length)
                Apply(instance, defaultValue);
            else
                Apply(instance, values[newIndex]);
            return currentIndex;
        }

        /// <inheritdoc />
        public override T ResetToDefault(T1 instance)
        {
            var index = ResetToDefaultIndex(instance);
            return GetValueAtIndex(index);
        }
        /// <summary>
        /// Reset the value to the value of the array element at the default index 
        /// </summary>
        /// <param name="instance">The instance of the object which will get affected by the mutation</param>
        /// <returns>If the array is not empty, it will return the default index, else it will apply -1</returns>
        public int ResetToDefaultIndex(T1 instance)
        {
            var value = values.Length == 0 ? defaultValue : values[defaultIndex];
            Apply(instance, value);
            return values.Length == 0 ? -1 : defaultIndex;
        }

        /// <summary>
        /// Gets the value at the given index
        /// </summary>
        /// <param name="index">index of which you want to get the value of</param>
        /// <returns>the value of the the mutation state with given index</returns>
        public T GetValueAtIndex(int index)
        {
            return index == -1? defaultValue : values[index];
        }

        /// <summary>
        /// Gets the number of mutation states for this mutation
        /// </summary>
        /// <returns>The length of the values array</returns>
        public int GetValuesCount()
        {
            return values.Length;
        }
    }
}
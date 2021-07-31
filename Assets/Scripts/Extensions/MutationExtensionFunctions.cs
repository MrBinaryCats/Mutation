using System;
using System.Collections.Generic;
using Mutations.Mutations.Core;

namespace Mutations.Extensions
{
    public static class MutationExtensionFunctions
    {
        /// <summary>
        ///     Gets the mutation for a given index
        /// </summary>
        /// <param name="index">Index of the mutation to retrieve</param>
        /// <param name="mutations">The mutation array</param>
        /// <returns>The mutation for the given index</returns>
        public static MutationBase GetMutationAtIndex(this MutationBase[] mutations, int index)
        {
            return mutations[index];
        }

        /// <summary>
        ///     Gets the number of mutations for the EntityData
        /// </summary>
        /// <returns>The number of mutations</returns>
        public static int GetMutationCount(this MutationBase[] mutations)
        {
            return mutations.Length;
        }

        /// <summary>
        ///     Tries to get the mutation of a given type from the entity
        /// </summary>
        ///         /// <param name="mutations">The mutation array</param>
        /// <param name="mutation">Output: The returned mutation</param>
        /// <typeparam name="T">The type of mutation to retrieve</typeparam>
        /// <returns>If the mutation exists for the entity</returns>
        public static bool TryGetMutation<T>(this IEnumerable<MutationBase> mutations, out T mutation) where T : MutationBase
        {
            foreach (var mutationBase in mutations)
                if (mutationBase is T mutant)
                {
                    mutation = mutant;
                    return true;
                }

            mutation = default;
            return false;
        }

        /// <summary>
        ///     Gets if the Entity has a given type of mutation
        /// </summary>
        /// <param name="mutations">The mutation array</param>
        /// <param name="type">The type of the mutation to check</param>
        /// <returns>If the entity has the mutation</returns>
        public static bool HasMutationType(this IEnumerable<MutationBase> mutations, Type type)
        {
            foreach (var mutationBase in mutations)
                if (mutationBase.GetType() == type)
                    return true;
            return false;
        }
    }
}
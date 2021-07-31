using System;
using Mutations.Mutations.Core;
using UnityEngine;

namespace Mutations.Entity
{
    [CreateAssetMenu(fileName = "FILENAME", menuName = "Entity", order = 0)]
    public class EntityData : ScriptableObject
    {
        [HideInInspector] [SerializeField] protected MutationBase[] mutations;

        [SerializeField] protected string entityName;
        [SerializeField] protected int hitPoints;
        [SerializeField] protected int strength;

        //public, readonly, accessors for the data
        public string EntityName => entityName;
        public int HitPoints => hitPoints;
        public int Strength => strength;

        /// <summary>
        ///     Gets the mutation for a given index
        /// </summary>
        /// <param name="index">Index of the mutation to retrieve</param>
        /// <returns>The mutation for the given index</returns>
        public MutationBase GetMutationAtIndex(int index)
        {
            return mutations[index];
        }

        /// <summary>
        ///     Gets the number of mutations for the EntityData
        /// </summary>
        /// <returns>The number of mutations</returns>
        public int GetMutationCount()
        {
            return mutations.Length;
        }

        /// <summary>
        ///     Tries to get the mutation of a given type from the entity
        /// </summary>
        /// <param name="mutation">Output: The returned mutation</param>
        /// <typeparam name="T">The type of mutation to retrieve</typeparam>
        /// <returns>If the mutation exists for the entity</returns>
        public bool TryGetMutation<T>(out T mutation) where T : MutationBase
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
        /// <param name="type">The type of the mutation to check</param>
        /// <returns>If the entity has the mutation</returns>
        public bool HasMutationType(Type type)
        {
            foreach (var mutationBase in mutations)
                if (mutationBase.GetType() == type)
                    return true;
            return false;
        }
    }
}
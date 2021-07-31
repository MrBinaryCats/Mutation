using System;
using Mutations.Mutations.Core;
using UnityEngine;
using UnityEngine.Serialization;

namespace Mutations.Entity
{
    [CreateAssetMenu(fileName = "FILENAME", menuName = "Entity", order = 0)]
    public class EntityData : ScriptableObject
    {
        [HideInInspector] [SerializeField] protected MutationBase[] mutations;

        public string EntityName;
        public int HitPoints;
        public int strength;

        /// <summary>
        /// Gets the mutation for a given index
        /// </summary>
        /// <param name="index">Index of the mutation to retrieve</param>
        /// <returns>The mutation for the given index</returns>
        public MutationBase GetMutationAtIndex(int index) => mutations[index];

        /// <summary>
        /// Gets the number of mutations for the EntityData
        /// </summary>
        /// <returns>The number of mutations</returns>
        public int GetMutationCount() => mutations.Length;

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

        public bool HasMutationType(Type type)
        {
            foreach (var mutationBase in mutations)
                if (mutationBase.GetType() == type)
                    return true;
            return false;
        }
    }
}
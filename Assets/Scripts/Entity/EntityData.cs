using System;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "FILENAME", menuName = "Entity", order = 0)]
    public class EntityData : ScriptableObject
    {
        [HideInInspector]
        public MutationBase[] Mutations;

        public string EntityName;
        public int HitPoints;
        public int strength;
        public bool TryGetMutation<T>(out T mutation) where T : MutationBase
        {
            foreach (var mutationBase in Mutations)
            {
                if (mutationBase is T mutant)
                {
                    mutation = mutant;
                    return true;
                }
            }

            mutation = default;
            return false;
        }
        public bool HasMutationType(Type type) 
        {
            foreach (var mutationBase in Mutations)
            {
                if (mutationBase.GetType() == type)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
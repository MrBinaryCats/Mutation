using System;
using System.Collections.Generic;
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
        public MutationBase[] Mutations => mutations;
        public string EntityName => entityName;
        public int HitPoints => hitPoints;
        public int Strength => strength;
    }
}
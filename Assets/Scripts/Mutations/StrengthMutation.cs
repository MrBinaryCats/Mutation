using System;
using System.Threading;
using UnityEngine;

namespace DefaultNamespace
{
    /// <summary>
    /// Color mutation, applies a given colour onto the meshrenders material
    /// </summary>
    public class StrengthMutation :  Mutations<StrengthMutation.Weapons,EntityController>
    {
        
        public enum Weapons {Fist, Rock, Staff, Sword}
        
        public override void Apply(EntityController instance, in Weapons value)
        {
            instance.IncreaseStrength((int) value);
        }
    }
}
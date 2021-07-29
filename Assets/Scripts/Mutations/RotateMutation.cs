using System;
using UnityEditor;
using UnityEngine;

namespace DefaultNamespace
{
    /// <summary>
    /// Mutation which tells the Rotator if it should be rotating and how fast it should rotate
    /// </summary>
    public class RotateMutation : Mutation<bool, Rotator>
    {
        public float Speed;
        public override void Apply(Rotator instance, in bool value)
        {
            instance.SetRotateState(value, Speed);
        }
        
    }
}
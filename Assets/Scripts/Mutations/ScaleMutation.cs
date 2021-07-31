using Mutations.Mutations.Core;
using UnityEngine;

namespace Mutations.Mutations
{
    /// <summary>
    ///     Mutation which applies a scale to the gameobject
    ///     <inheritdoc />
    /// </summary>
    public class ScaleMutation : MutationStep<Vector3, Transform>
    {
        /// <inheritdoc />
        public override void Apply(Transform instance, in Vector3 value)
        {
            instance.localScale = value;
        }
        
        /// <inheritdoc />
        public override Vector3 ApplyNext(Transform instance, Vector3 value)
        {
            var next = value + step;
            Apply(instance, next);
            return next;
        }

        /// <inheritdoc />
        public override Vector3 ApplyPrevious(Transform instance, Vector3 value)
        {
            var next = value - step;
            Apply(instance, next);
            return next;
        }
    }
}
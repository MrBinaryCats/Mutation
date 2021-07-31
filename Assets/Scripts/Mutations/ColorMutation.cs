using Mutations.Mutations.Core;
using UnityEngine;

namespace Mutations.Mutations
{
    /// <summary>
    ///     Color mutation, applies a given colour onto the meshrenders material
    /// </summary>
    public class ColorMutation : Mutations<Color, MeshRenderer>
    {
        [SerializeField] private bool UsePropertyBlock;

        [WarnIfEmpty] [Delayed] [SerializeField]
        private string ColorPropName;

        [SerializeField] private int ColorPropID;

        private MaterialPropertyBlock block;

        public void OnEnable()
        {
            if (UsePropertyBlock)
                block = new MaterialPropertyBlock();
        }

        public override void Apply(MeshRenderer instance, in Color value)
        {
            if (UsePropertyBlock)
            {
                block.Clear();
                if (instance.HasPropertyBlock())
                    instance.GetPropertyBlock(block);
                block.SetColor(ColorPropName, value);
                instance.SetPropertyBlock(block);
            }
            else
            {
                instance.material.SetColor(ColorPropID, value);
            }
        }
    }
}
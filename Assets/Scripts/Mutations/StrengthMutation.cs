using Mutations.Entity;
using Mutations.Mutations.Core;

namespace Mutations.Mutations
{
    /// <summary>
    ///     Color mutation, applies a given colour onto the meshrenders material
    /// </summary>
    public class StrengthMutation : Mutations<StrengthMutation.Weapons, EntityController>
    {
        public enum Weapons
        {
            Fist,
            Rock,
            Staff,
            Sword
        }

        public override void Apply(EntityController instance, in Weapons value)
        {
            instance.IncreaseStrength((int) value);
        }
    }
}
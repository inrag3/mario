using UnityEngine;

namespace Collectables.Buffs
{
    public abstract class BuffData : ScriptableObject, IBuff
    {
        public abstract void Apply(IBuffable buff);
    }
}
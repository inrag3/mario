using System.Collections.Generic;
using UnityEngine;

namespace Collectables.Buffs
{
    public class Buff : Collectable, IBuff
    {
        [SerializeField] private List<BuffData> _buffs;

        public void Apply(IBuffable buff)
        {
            _buffs.ForEach(x => x.Apply(buff));
        }
        public override void Collect()
        {
            Destroy(gameObject);
        }
    }

    public interface IBuffable
    {
        public void ActivateStarPower(float duration);
    }

    public interface IBuff
    {
        public void Apply(IBuffable buff);
    }
}
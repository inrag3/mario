using UnityEngine;

namespace Collectables.Buffs
{
    [CreateAssetMenu(fileName = "StarPowerBuff", menuName = "Collectables/StarPowerBuff")]
    public class StarPowerBuff : BuffData
    {
        [SerializeField] private float _duration;
        public override void Apply(IBuffable buffable)
        {
            buffable.ActivateStarPower(_duration);
        }
    }
}
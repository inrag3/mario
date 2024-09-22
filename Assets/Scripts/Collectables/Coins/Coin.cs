using UnityEngine;

namespace Collectables.Coins
{
    public class Coin : Collectable
    {
        [field: SerializeField] public int Value { get; private set; }
        public override void Collect()
        {
            Destroy(gameObject);
        }
    }
}
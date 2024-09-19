using DG.Tweening;
using UnityEngine;

namespace Interactables
{
    public class MisteryBox : Tile
    {
        [SerializeField] private Coin _prefab;
        protected override void DoActionBetweenAnimations()
        {
            var coin = Instantiate(_prefab, transform.position, Quaternion.identity);
            coin.transform.localScale = Vector3.zero;
            coin.transform
                .DOScale(Vector3.one, 0.15f);
            coin.transform
                .DOMoveY(coin.transform.position.y + 1, 0.2f);
        }
    }
}
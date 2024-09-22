using Collectables;
using DG.Tweening;
using UnityEngine;

namespace Interactables
{
    public class MisteryBox : Tile
    {
        [SerializeField] private Collectable _prefab;
        protected override void DoActionBetweenAnimations()
        {
            var buff = Instantiate(_prefab, transform.position, Quaternion.identity);
            buff.transform.localScale = Vector3.zero;
            buff.transform
                .DOScale(Vector3.one, 0.15f);
            buff.transform
                .DOMoveY(buff.transform.position.y + 1, 0.2f);
        }
    }
}
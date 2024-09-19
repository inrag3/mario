using UnityEngine;

namespace Interactables
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Tile : Interactable
    {
        [SerializeField] private Sprite _sprite;

        private SpriteRenderer _spriteRenderer;

        protected override void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            base.Awake();
        }

        protected override void DoActionBeforeAnimations()
        {
            ChangeSprite();

        }

        protected override void DoActionBetweenAnimations()
        {
        }

        protected override void DoActionAfterAnimations()
        {
            Destroy(this); //Убиваем скрипт
        }

        private void ChangeSprite()
        {
            _spriteRenderer.sprite = _sprite;
        }
    }
}
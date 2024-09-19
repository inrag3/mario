using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace Interactables
{
    [RequireComponent(typeof(BoxCollider2D))]

    public abstract class Interactable : MonoBehaviour
    {
        [SerializeField] private float _duration = 0.25f;

        private BoxCollider2D _collider;
        private ((float, float), float) _interval;
        private Vector3 _position;
        private bool _isAnimating;

        protected virtual void Awake()
        {
            _collider = GetComponent<BoxCollider2D>();
            var bounds = _collider.bounds;
            _interval.Item1 = (bounds.center.x - 0.2f, bounds.center.x + 0.2f);
            _interval.Item2 = bounds.center.y - bounds.extents.y;
            _position = transform.position;
        }
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (_isAnimating)
                return;
            if (!other.gameObject.TryGetComponent(out IContactable _))
                return;
            var average = GetAverageContact(other.contacts);
            if (_interval.Contains(average))
            {
                Animate();
            }
        }
        private void Animate()
        {
            _isAnimating = true;
            DoActionBeforeAnimations();
            transform
                .DOMoveY(_position.y + 0.5f, _duration)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    DoActionBetweenAnimations();
                    transform
                        .DOMoveY(_position.y, _duration)
                        .SetEase(Ease.InQuad).OnComplete(() =>
                        {
                            _isAnimating = false;
                            DoActionAfterAnimations();
                        });
                });
        }

        protected abstract void DoActionBeforeAnimations();
        protected abstract void DoActionBetweenAnimations();
        protected abstract void DoActionAfterAnimations();
        private Vector2 GetAverageContact(ContactPoint2D[] points)
        {
            float x = points.Average(contact => contact.point.x);
            float y = points.Average(contact => contact.point.y);
            return new Vector2(x, y);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(
                new Vector2(_interval.Item1.Item1, _interval.Item2),
                new Vector2(_interval.Item1.Item2, _interval.Item2)
            );
        }
    }


    public static class Extentions
    {
        private const float Threshold = 0.01f;
        public static bool Contains(this ((float, float), float) interval, Vector2 value)
        {
            float difference = Math.Abs(interval.Item2 - value.y);
            if (difference > Threshold)
                return false;
            return value.x >= interval.Item1.Item1 && value.x <= interval.Item1.Item2;
        }
    }
}
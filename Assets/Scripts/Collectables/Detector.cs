using System;
using UnityEngine;

namespace Collectables
{
    public class Detector<T> : MonoBehaviour
    {
        public event Action<T> Detected;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent(out T collectable))
                return;
            Detected?.Invoke(collectable);
        }
    }
}
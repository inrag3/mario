using UnityEngine;

[CreateAssetMenu()]
public class Statistics : ScriptableObject
{
    [field: SerializeField] public float Speed { get; private set; }
}
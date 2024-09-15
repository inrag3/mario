using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int count;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;
        var player = collision.GetComponent<Player>();
        player.AddCoin(count);
        Destroy(gameObject);
    }
}

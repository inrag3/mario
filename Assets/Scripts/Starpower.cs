using System;
using UnityEngine;

public class Starpower : MonoBehaviour
{
    private const string Player = "Player";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag(Player))
            return;

        collision.GetComponent<Player>().StarPowerActive(3);
        Destroy(gameObject);
    }
}

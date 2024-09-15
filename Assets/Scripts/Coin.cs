using System;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private const string Player = "Player";
    [SerializeField] private int count;
    [SerializeField] private AudioClip audioClip;
    private GameObject player;
    private AudioSource audioSource;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag(Player);
        audioSource = player.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag(Player))
            return;
        var player = collision.GetComponent<Player>();
        player.AddCoin(count);
        audioSource.PlayOneShot(audioClip);
        Destroy(gameObject);
    }
}

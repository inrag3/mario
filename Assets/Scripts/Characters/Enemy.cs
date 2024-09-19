using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class Enemy : MonoBehaviour
{
    private const string Player = "Player";
    [SerializeField] private float speed;
    private Rigidbody2D rigidbody2d;
    private Vector2 direction = Vector2.left;
    private SpriteRenderer spriteRenderer;
    private Vector2 velocity;
    private float xMax;
    private Player player;

    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        var camera = Camera.main!;
        xMax = camera.orthographicSize * camera.aspect;
    }

    private void Start()
    {
        rigidbody2d.velocity = direction * speed;
        player = GameObject.FindGameObjectWithTag(Player).GetComponent<Player>();
    }

    private void FixedUpdate()
    {
        UpdateRotation(direction.x);
        UpdateVelocity();
    }

    private void UpdateVelocity()
    {
        if (transform.position.x < -xMax + 0.5f ||
            transform.position.x > xMax - 0.5f)
        {
            ChangeDirection(true);
        }
    }


    private void UpdateRotation(float horizontal)
    {
        spriteRenderer.flipX = horizontal switch
        {
            < 0 => false,
            > 0 => true,
            _ => spriteRenderer.flipX
        };
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag(Player))
        {
            ChangeDirection(!collision.contacts.All(contact => contact.point.y < transform.position.y));
            return;
        }
        bool dead = collision.contacts.All(contact => contact.point.y > transform.position.y);
        if (dead || player.StarPower)
        {
            Destroy(gameObject);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }

    private void ChangeDirection(bool makeChange)
    {
        if (!makeChange)
            return;
        direction = -direction;
        var velocity = rigidbody2d.velocity;
        velocity.x = direction.x * speed;
        rigidbody2d.velocity = velocity;
    }
}

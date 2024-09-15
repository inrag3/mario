using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
public class Player : MonoBehaviour
{
    private const string Horizontal = "Horizontal";
    private const string Ground = "Ground";
    private static readonly int State = Animator.StringToHash("State");

    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private Text scoreText;

    private int score;
    private Vector2 velocity;
    private bool isJumping;
    private bool isGrounded;
    private float xMax;
    private Rigidbody2D rigidbody2d;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        //Вообще не должно быть тут)
        var camera = Camera.main!;
        xMax = camera.orthographicSize * camera.aspect;
    }

    private void FixedUpdate()
    {
        float horizontal = Input.GetAxis(Horizontal); //И этого тоже!
        UpdateVelocity(horizontal);
        UpdateAnimaton(horizontal);
        UpdateRotation(horizontal);
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Space) || !isGrounded)
            return;

        Jump();
        isGrounded = false;
    }

    private void UpdateAnimaton(float horizontal)
    {
        if (isGrounded)
        {
            animator.SetInteger(State, horizontal != 0 ? 1 : 0);
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

    private void UpdateVelocity(float horizontal)
    {
        velocity = rigidbody2d.velocity;

        if (transform.position.x < -xMax + 0.5f && horizontal <= 0 ||
            transform.position.x > xMax - 0.5f && horizontal >= 0)
        {
            velocity.x = 0;
        }
        else
        {
            velocity.x = horizontal * speed;
        }

        rigidbody2d.velocity = velocity;
    }

    public void AddCoin(int count)
    {
        score += count;
        scoreText.text = $"{score}";
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isGrounded && collision.gameObject.CompareTag(Ground))
        {
            isGrounded = collision.contacts.All(contact => contact.point.y < transform.position.y);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Ground))
        {
            isGrounded = !collision.contacts.All(contact => contact.point.y > transform.position.y);
        }
    }

    private void Jump()
    {
        animator.SetInteger(State, 2);
        rigidbody2d.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }
}

using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class Player : MonoBehaviour
{
    private const string Horizontal = "Horizontal";
    private const string Ground = "Ground";

    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;

    private Vector2 velocity;
    private bool isJumping;
    private bool isGrounded;
    private float xMax;
    private Rigidbody2D rigidbody2d;


    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        //Вообще не должно быть тут)
        var camera = Camera.main!;
        xMax = camera.orthographicSize * camera.aspect;
    }

    private void FixedUpdate()
    {
        //И этого тоже!
        float inputAxis = Input.GetAxis(Horizontal);
        velocity = rigidbody2d.velocity;

        if (transform.position.x < -xMax + 0.5f && inputAxis <= 0 ||
            transform.position.x > xMax - 0.5f && inputAxis >= 0)
        {
            velocity.x = 0;
        }
        else
        {
            velocity.x = inputAxis * speed;
        }

        rigidbody2d.velocity = velocity;
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Space) || !isGrounded)
            return;

        Jump();
        isGrounded = false;
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
        rigidbody2d.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }
}

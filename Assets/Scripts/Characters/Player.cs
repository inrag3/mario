using System;
using System.Collections;
using System.Linq;
using Collectables;
using Collectables.Buffs;
using Collectables.Coins;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
[RequireComponent(typeof(BuffDetector), typeof(CoinDetector))]
public class Player : MonoBehaviour, IContactable, IBuffable
{
    private const string Horizontal = "Horizontal";
    private const string Ground = "Ground";
    private static readonly int State = Animator.StringToHash("State");

    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float speedCoefficient;

    private BuffDetector buffDetector;
    private CoinDetector coinDetector;

    private int score;
    private Vector2 velocity;
    private bool isJumping;
    private bool isGrounded;
    private float xMax;
    private Rigidbody2D rigidbody2d;
    private SpriteRenderer spriteRenderer;

    private Animator animator;
    private Bank.Bank _bank;

    public bool StarPower { get; private set; }

    public void Initialize(Bank.Bank bank)
    {
        _bank = bank;
    }

    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        buffDetector = GetComponent<BuffDetector>();
        coinDetector = GetComponent<CoinDetector>();
        animator = GetComponent<Animator>();
        //Вообще не должно быть тут)
        var camera = Camera.main!;
        xMax = camera.orthographicSize * camera.aspect;
    }

    private void OnEnable()
    {
        buffDetector.Detected += OnBuffDetected;
        coinDetector.Detected += OnCoinDetected;
    }

    private void OnDisable()
    {
        buffDetector.Detected -= OnBuffDetected;
    }

    private void OnCoinDetected(Coin coin)
    {
        _bank.Add(coin.Value);
        coin.Collect();
    }

    private void OnBuffDetected(Buff buff)
    {
        buff.Apply(this);
        buff.Collect();
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

    public void ActivateStarPower(float duration = 5f)
    {
        StartCoroutine(StarPowerAnimation(duration));
    }

    private IEnumerator StarPowerAnimation(float duration)
    {
        StarPower = true;
        speed *= speedCoefficient;
        var elapsed = 0f;
        while (elapsed < duration)
        {
            if (Time.frameCount % 4 == 0)
            {
                spriteRenderer.color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
            }
            yield return null;
            elapsed += Time.deltaTime;
        }
        speed /= speedCoefficient;
        spriteRenderer.color = Color.white;
        StarPower = false;
    }


    private void Jump()
    {
        animator.SetInteger(State, 2);
        rigidbody2d.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }
}

public interface IContactable
{
}
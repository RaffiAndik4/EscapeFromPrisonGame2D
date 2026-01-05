using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Ladder")]
    public LayerMask ladderLayer;      // set ke Layer Ladder
    public float climbSpeed = 4f;      // kecepatan naik/turun tangga

    [Header("Animator (optional)")]
    public Animator animator;          // drag animator player (optional)
    public string speedParam = "speed";
    public string groundedParam = "isGrounded";
    public string climbingParam = "isClimbing";

    [Header("SFX")]
    public AudioClip jumpSfx;
    public AudioClip footstepSfx;
    [Tooltip("Jarak waktu antar langkah (detik). Semakin kecil semakin sering bunyi step.")]
    public float stepInterval = 0.35f;
    [Tooltip("Volume footstep (0-1).")]
    [Range(0f, 1f)] public float footstepVolume = 0.6f;
    [Tooltip("Volume jump (0-1).")]
    [Range(0f, 1f)] public float jumpVolume = 0.8f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isClimbing;
    private float moveX;
    private float moveY;
    private float defaultGravity;

    // footstep timer
    private float stepTimer;

    // flip yang aman (tidak konflik dengan animasi scale)
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        defaultGravity = rb.gravityScale;

        if (animator == null) animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        ReadInput();
        CheckGround();
        CheckLadder();     // cek apakah sedang menyentuh tangga
        HandleJump();
        HandleFootstepSfx(); // tambahan SFX (tidak mengubah logic lama)
        UpdateAnimator();
    }

    void FixedUpdate()
    {
        HandleMove();
        HandleClimb();
    }

    void ReadInput()
    {
        moveX = Input.GetAxisRaw("Horizontal"); // A/D atau Left/Right
        moveY = Input.GetAxisRaw("Vertical");   // W/S atau Up/Down
    }

    void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
    }

    void CheckLadder()
    {
        Collider2D ladderHit = Physics2D.OverlapCircle(transform.position, 0.2f, ladderLayer);

        bool touchingLadder = ladderHit != null;

        if (touchingLadder && Mathf.Abs(moveY) > 0.01f)
        {
            isClimbing = true;
        }
        else if (!touchingLadder)
        {
            isClimbing = false;
        }

        rb.gravityScale = isClimbing ? 0f : defaultGravity;
    }

    void HandleMove()
    {
        // sama fungsinya: gerak X
        rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);

        // flip sprite kiri/kanan (lebih aman dari scale)
        if (spriteRenderer != null && Mathf.Abs(moveX) > 0.001f)
        {
            spriteRenderer.flipX = moveX < 0f;
        }
    }

    void HandleClimb()
    {
        if (!isClimbing) return;

        // sama fungsinya: kontrol Y saat di tangga
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, moveY * climbSpeed);
    }

    void HandleJump()
    {
        if (isClimbing) return;

        if ((Input.GetKeyDown(KeyCode.Space) ||
             Input.GetKeyDown(KeyCode.UpArrow) ||
             Input.GetKeyDown(KeyCode.W)) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

            // SFX Jump
            if (jumpSfx != null && SFXManager.Instance != null)
                SFXManager.Instance.Play(jumpSfx, jumpVolume);
        }
    }

    void HandleFootstepSfx()
    {
        // Step hanya saat di ground, bergerak horizontal, dan tidak sedang climb
        if (!isGrounded || isClimbing || Mathf.Abs(moveX) <= 0.1f || footstepSfx == null)
        {
            stepTimer = 0f;
            return;
        }

        stepTimer -= Time.deltaTime;
        if (stepTimer <= 0f)
        {
            if (SFXManager.Instance != null)
                SFXManager.Instance.Play(footstepSfx, footstepVolume);

            stepTimer = stepInterval;
        }
    }

    void UpdateAnimator()
    {
        if (animator == null) return;

        animator.SetFloat(speedParam, Mathf.Abs(moveX));
        animator.SetBool(groundedParam, isGrounded);
        animator.SetBool(climbingParam, isClimbing);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.2f);
    }
}

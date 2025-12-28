using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Ladder Settings")]
    public LayerMask ladderLayer;     // set ke Layer "Ladder"
    public float ladderSpeed = 4f;

    private Rigidbody2D rb;
    private bool isGrounded;

    private float moveInputX;
    private float moveInputY;

    // Ladder state
    private bool isOnLadder;
    private bool isClimbing;
    private float defaultGravity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        defaultGravity = rb.gravityScale;
    }

    void Update()
    {
        ReadInput();
        CheckGround();
        CheckLadder();

        HandleHorizontalMove();
        HandleJump();
        HandleLadderClimb();
        FlipSprite();
    }

    void ReadInput()
    {
        moveInputX = Input.GetAxisRaw("Horizontal"); // A/D, Left/Right
        moveInputY = Input.GetAxisRaw("Vertical");   // W/S, Up/Down
    }

    void HandleHorizontalMove()
    {
        // Saat climbing, kita tetap boleh jalan kiri/kanan (opsional).
        // Kalau mau dikunci saat naik tangga, tinggal set moveX = 0 saat isClimbing.
        float vx = moveInputX * moveSpeed;
        rb.linearVelocity = new Vector2(vx, rb.linearVelocity.y);
    }

    void HandleJump()
    {
        // Jangan bisa lompat saat sedang climbing tangga
        if (isClimbing) return;

        if ((Input.GetKeyDown(KeyCode.Space) ||
             Input.GetKeyDown(KeyCode.UpArrow) ||
             Input.GetKeyDown(KeyCode.W)) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    void CheckGround()
    {
        if (groundCheck == null) return;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
    }

    void CheckLadder()
    {
        // Cara paling aman: cek overlap circle kecil di posisi player (bisa juga pakai collider trigger event)
        // Kalau kamu sudah pakai collider trigger, ini tetap aman sebagai backup.
        isOnLadder = Physics2D.OverlapCircle(transform.position, 0.15f, ladderLayer);
    }

    void HandleLadderClimb()
    {
        // Kalau tidak berada di tangga -> pastikan kondisi normal
        if (!isOnLadder)
        {
            if (isClimbing)
            {
                isClimbing = false;
                rb.gravityScale = defaultGravity;
            }
            return;
        }

        // Di area tangga:
        // Jika player menekan atas/bawah, mulai climbing
        if (Mathf.Abs(moveInputY) > 0.01f)
        {
            isClimbing = true;
        }

        if (isClimbing)
        {
            rb.gravityScale = 0f;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, moveInputY * ladderSpeed);
        }
        else
        {
            // masih di area ladder tapi tidak climbing -> tetap normal (gravity normal)
            rb.gravityScale = defaultGravity;
        }
    }

    void FlipSprite()
    {
        if (moveInputX != 0)
            transform.localScale = new Vector3(Mathf.Sign(moveInputX), 1f, 1f);
    }

    // OPTIONAL CROUCH (↓ or S)
    public bool IsCrouching()
    {
        return Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S);
    }

    // Debug ground check circle (optional)
    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
    }
}

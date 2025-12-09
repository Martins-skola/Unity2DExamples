using UnityEngine;

public class PlayerMovementGroundCheckObject : MonoBehaviour
{
    [Header("Rörelse")]
    [Tooltip("Hur snabbt spelaren rör sig åt sidorna")]
    public float moveSpeed = 5f;

    [Header("Hopp")]
    [Tooltip("Hur högt spelaren hoppar")]
    public float jumpForce = 10f;

    [Header("Mark-kontroll")]
    [Tooltip("GroundCheck-objekt vars position används för att kolla om spelaren står på marken")]
    public Transform groundCheckObject;

    [Tooltip("Hur stor radie vi kollar för mark")]
    public float groundCheckRadius = 0.2f;

    [Tooltip("Vilket lager är marken/plattformar på?")]
    public LayerMask groundLayer;

    // Privata variabler
    private Rigidbody2D rb;
    private bool isGrounded;

    void Start()
    {
        // Hämta Rigidbody2D-komponenten
        rb = GetComponent<Rigidbody2D>();

        // Kollar om Rigidbody2D-komponenten finns och ger felmeddelande om den saknas
        if (rb == null) 
        {
            Debug.LogError("Rigidbody2D-komponenten hittades inte på spelaren!");
            this.enabled = false; // Inaktivera skriptet om Rigidbody2D saknas
        }

        // Kollar omGroundCheck-ibjekt finns och ger felmeddelande om den saknas
        if (groundCheckObject == null)
        {
            Debug.LogError("Referens till ground-check-objekt hittades inte!");
            this.enabled = false; // Inaktivera skriptet om referensen saknas
        }
    }

    void Update()
    {
        // Kolla om spelaren står på marken
        CheckIfGrounded();

        // Hantera hopp
        HandleJump();
    }

    void FixedUpdate()
    {
        // Hantera sidledes rörelse
        HandleMovement();
    }

    void HandleMovement()
    {
        // Läs input från tangentbordet (-1 till 1)
        float moveInput = Input.GetAxis("Horizontal");

        // Sätt hastigheten på x-axeln, behåll y-hastigheten
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    void HandleJump()
    {
        // Om spelaren trycker på mellanslag OCH står på marken
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            // Sätt uppåthastigheten till jumpForce
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    void CheckIfGrounded()
    {
        // Skapa en cirkel vid groundCheck-positionen och kolla om den överlappar med groundLayer
        isGrounded = Physics2D.OverlapCircle(groundCheckObject.position, groundCheckRadius, groundLayer);
    }

    // Rita ut groundCheck-cirkeln i Scene-vyn (för att hjälpa till med inställningar)
    void OnDrawGizmosSelected()
    {
        if (groundCheckObject != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheckObject.position, groundCheckRadius);
        }
    }
}
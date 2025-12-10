using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Rörelse")]
    [Tooltip("Hur snabbt spelaren rör sig åt sidorna")]
    public float moveSpeed = 5f;

    [Header("Hopp")]
    [Tooltip("Hur högt spelaren hoppar")]
    public float jumpForce = 10f;

    // Privata variabler
    private Rigidbody2D rb;

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
    }

    void Update()
    {
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
        // Om spelaren trycker på mellanslag
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Sätt uppåthastigheten till jumpForce
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }
}

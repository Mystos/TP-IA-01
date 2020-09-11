using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool facingRight = true;

    public GameObject landingDustPrefab;
    public GameObject jumpDustPrefab;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    public float moveSpeed;
    public float jumpForce;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public bool isGrounded;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask collisionLayers;

    public Rigidbody2D rb;
    public GameObject dialogueBox;
    private Vector3 velocity = Vector3.zero;

    private bool isJumping = false;
    private int doubleJump = 1;
    private bool isDoubleJumping = false;
    private bool hasJustPressedJump = false;
    private bool hasJustLanded = false;
    private bool isInInteraction = false;

    private float horizontalMovement;
    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, collisionLayers); // Dessine une ligne entre les deux transforme
        isInInteraction = dialogueBox.activeSelf;

        if (isGrounded)
        {
            doubleJump = 1;
        }

        animator.SetBool("IsGrounded", isGrounded);

        if (Input.GetButtonDown("Jump") && isGrounded && !isJumping && !animator.GetBool("IsInInteraction"))
        {
            hasJustPressedJump = true;
        }

        // Double saut
        if (Input.GetButtonDown("Jump") && !isGrounded && doubleJump >= 1)
        {
            isDoubleJumping = true;
            doubleJump--;
        }

        animator.SetBool("IsInInteraction", isInInteraction);
        animator.SetBool("IsDoubleJumping", isDoubleJumping);
        animator.SetBool("HasJustPressedJump", hasJustPressedJump);
        animator.SetBool("HasJustLanded", hasJustLanded);
        animator.SetInteger("JumpCount", doubleJump);

        Flip(rb.velocity.x);

        float characterVelocity = Mathf.Abs(rb.velocity.x);
        animator.SetFloat("xSpeed", characterVelocity);
        animator.SetFloat("ySpeed", rb.velocity.y);



    }

    void FixedUpdate()
    {
        horizontalMovement = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;

        MovePlayer(horizontalMovement);
    }

    void MovePlayer(float horizontalMovement)
    {
        // Movement Horizontal
        Vector3 targetVelocity = new Vector2(horizontalMovement, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, 0.05f);

        // Nuancier de saut
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        if (isJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(new Vector2(0f, jumpForce));
            isJumping = false;
        }
        if (isDoubleJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(new Vector2(0f, jumpForce));
            isDoubleJumping = false;
        }
    }

    void Flip(float velocity)
    {

        Vector3 localScale = transform.localScale;
        if (velocity < -0.1)
        {
            facingRight = false;
        }
        else if (velocity > 0.1)
        {
            facingRight = true;
        }
        if (((facingRight) && (localScale.x < 0)) || ((!facingRight) && (localScale.x > 0)))
        {
            localScale.x *= -1;
        }
        transform.localScale = localScale;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    public void AnimationEndAlerts(string message)
    {
        if (message.Equals("PlayerBeforeJumpEnd"))
        {
            hasJustPressedJump = false;
            isJumping = true;
        }
        if (message.Equals("PlayerAfterJumpEnd"))
        {
            hasJustLanded = false;
        }
        if (message.Equals("PlayerLandedStart"))
        {
            Instantiate(landingDustPrefab, new Vector3(this.transform.position.x, this.transform.position.y - 0.25f), Quaternion.identity);
        }
        if (message.Equals("PlayerJumpStart"))
        {
            Instantiate(jumpDustPrefab, new Vector3(this.transform.position.x, this.transform.position.y), Quaternion.identity);
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.isTrigger && collision.CompareTag("Interactable"))
        {
            if (Input.GetButtonDown("Fire2"))
            {
                collision.SendMessageUpwards("TriggerDialogue");
            }
        }
    }
}

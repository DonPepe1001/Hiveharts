using System.Collections;
using UnityEngine;

public class JimMovement : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator animator;
    bool derecha = true;
    float mh = 0f;
    public float vel = 10;
    public float jmp = 5;
    public Transform Spawner;
    public GameObject prefab;
    public Transform groundCheck;
    public LayerMask groundLayer;
    bool isOnGround = true;
    private bool canDash = true;
    private bool isDashing;
    private float dashingTime = 0.15f;
    private float dashingCooldown = 1f;
    private float inputX;

    [Header("Dash Settings")]
    public float horizontalDashPower = 30f;
    public float verticalDashPower = 20f;
    public float diagonalDashPower = 25f;

    [Header("WallJump")]
    [SerializeField] private Transform WallController;
    [SerializeField] private Vector3 WallBoxDimensions;
    [SerializeField] private Vector3 GroundBoxDimensions;
    private bool onWall;
    private bool sliding;
    [SerializeField] private float slideVelocity;
    [SerializeField] private float jumpforceX;
    [SerializeField] private float jumpforceY;
    [SerializeField] private float wallJumpTime;
    private bool wallJumping;

    [SerializeField] private TrailRenderer tr;
    bool wKeyPressed = false;
    private bool slowMoActive = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        bool wKeyPressed = Input.GetKey(KeyCode.W);
        bool waKeyPressed = Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A);
        bool wdKeyPressed = Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D);

        isOnGround = Physics2D.OverlapCapsule(groundCheck.position, new Vector2(0.05f, 0.45f), CapsuleDirection2D.Horizontal, 0, groundLayer);
        inputX = Input.GetAxisRaw("Horizontal");

        if (isDashing)
        {
            return;
        }

        animator.SetBool("Sliding", sliding);

        if (Input.GetAxis("Jump") > 0 && isOnGround && !sliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0, jmp), ForceMode2D.Impulse);
        }

        if (Input.GetAxis("Jump") > 0 && onWall && sliding)
        {
            WallJump();
        }

        if (isOnGround)
        {
            animator.SetBool("groundDetection", true);
        }
        animator.SetBool("groundDetection", !isOnGround);

        mh = inputX * vel;

        if (mh > 0 && !derecha)
        {
            Flip();
        }
        else if (mh < 0 && derecha)
        {
            Flip();
        }

        if (!wallJumping)
        {
            rb.velocity = new Vector2(mh * vel, rb.velocity.y);
        }

        animator.SetFloat("Velocity", Mathf.Abs(mh));

        // Check if W key is pressed
        if (Input.GetKeyDown(KeyCode.W))
        {
            wKeyPressed = true;
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            wKeyPressed = false;
        }

        //check if WA is pressed
        if (Input.GetKeyDown(KeyCode.W) && (Input.GetKeyDown(KeyCode.A)))
        {
            waKeyPressed = true;
        }
        else if (Input.GetKeyUp(KeyCode.W) || (Input.GetKeyUp(KeyCode.A)))
        {
            waKeyPressed = false;
        }

        //check if WD is pressed
        if (Input.GetKeyDown(KeyCode.W) && (Input.GetKeyDown(KeyCode.D)))
        {
            wdKeyPressed = true;
        }
        else if (Input.GetKeyUp(KeyCode.W) || (Input.GetKeyUp(KeyCode.D)))
        {
            wdKeyPressed = false;
        }



        // Horizontal Dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && !wKeyPressed)
        {
            StartCoroutine(Dash(Vector2.right * transform.localScale.x * horizontalDashPower));
        }

        // Upward Dash
        if (wKeyPressed && Input.GetKeyDown(KeyCode.LeftShift) && canDash && !waKeyPressed && !wdKeyPressed)
        {
            StartCoroutine(Dash(Vector2.up * verticalDashPower));
        }


        //upleft dash
        if (waKeyPressed && Input.GetKeyDown(KeyCode.LeftShift) && canDash )
        {
            StartCoroutine(Dash((Vector2.up + Vector2.left).normalized * diagonalDashPower));
        }

        //upright dash
        if (wdKeyPressed && Input.GetKeyDown(KeyCode.LeftShift) && canDash )
        {
            StartCoroutine(Dash((Vector2.up + Vector2.right).normalized * diagonalDashPower));
        }

        if (!isOnGround && onWall && inputX != 0)
        {
            sliding = true;
        }
        else
        {
            sliding = false;
        }
        //SlowMo ability
        if (Input.GetMouseButtonDown(1)) 
        {
            slowMoActive = !slowMoActive;
            Time.timeScale = slowMoActive ? 0.5f : 1.0f;
        }
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        PlayerShooting();
    }

    private void FixedUpdate()
    {
        onWall = Physics2D.OverlapCapsule(WallController.position, WallBoxDimensions, 0f, groundLayer);

        if (sliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -slideVelocity, float.MaxValue));
        }

        if (isDashing)
        {
            return;
        }

    }

    private void WallJump()
    {
        wallJumping = true;
        StartCoroutine(DisableWallJump());

        Vector2 jumpDirection = new Vector2((derecha ? -1 : 1) * jumpforceX, jumpforceY);
        rb.velocity = Vector2.zero;
        rb.AddForce(jumpDirection, ForceMode2D.Impulse);

        sliding = false;
    }

    IEnumerator DisableWallJump()
    {
        yield return new WaitForSeconds(wallJumpTime);
        wallJumping = false;
    }

    private void Flip()
    {
        derecha = !derecha;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    private void PlayerShooting()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            GameObject bullet = Instantiate(prefab, Spawner.position, Spawner.rotation);
            CatPew catPew = bullet.GetComponent<CatPew>();

            if (catPew != null)
            {
                Vector2 direction = (derecha) ? Vector2.right : Vector2.left;
                catPew.SetVelocity(direction);
            }
        }
    }

    private IEnumerator Dash(Vector2 dashDirection)
    { 
        //original time scale
        float originalTimeScale = Time.timeScale;

        //slowmo effect
        Time.timeScale = 0.5f; 

        // Wait for a short duration for the slow-motion effect
        yield return new WaitForSeconds(0.1f);

        // Reset the time scale to its original value
        Time.timeScale = originalTimeScale;

        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = dashDirection;
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(groundCheck.position, GroundBoxDimensions);
        Gizmos.DrawWireCube(WallController.position, WallBoxDimensions);
    }
}

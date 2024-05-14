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
    private float dashingPower = 30f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;
    private float inputX;

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


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        isOnGround = Physics2D.OverlapCapsule(groundCheck.position, new Vector2(0.05f, 0.45f), CapsuleDirection2D.Horizontal, 0, groundLayer);

        inputX = Input.GetAxisRaw ("Horizontal");

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
            voltear();
        }
        else if (mh < 0 && derecha)
        {
            voltear();
        }

        if (!wallJumping)
        {
            rb.velocity = new Vector2(mh * vel, rb.velocity.y);

        }

        animator.SetFloat("Velocity", Mathf.Abs(mh));


        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }

        if (!isOnGround && onWall && inputX != 0)
        {
            sliding = true;
        }
        else
        {
            sliding = false;
        }

        

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
        onWall = false;
        rb.velocity = new Vector2(jumpforceX * -inputX, jumpforceY);
        StartCoroutine(ChangeWallJump());
    }

    IEnumerator ChangeWallJump()
    {
        wallJumping = true;
        yield return new WaitForSeconds(wallJumpTime);
        wallJumping = false;
    }


    private void voltear()
    {
        derecha = !derecha;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    //private void PlayerShooting()
    //{
    //    if (Input.GetButtonDown("Fire1"))
    //    {
    //        Instantiate(prefab, Spawner.position, Spawner.rotation);
    //    }
    //}
    private void PlayerShooting()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            GameObject bullet = Instantiate(prefab, Spawner.position, Spawner.rotation);
            CatPew catPew = bullet.GetComponent<CatPew>();

            if (catPew != null)
            {
                // Determine direction based on player's facing direction
                Vector2 direction = (derecha) ? Vector2.right : Vector2.left;
                catPew.SetVelocity(direction);
            }
        }
    }






    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
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

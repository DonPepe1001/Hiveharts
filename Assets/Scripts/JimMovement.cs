using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using Unity.VisualScripting;
using Unity.Mathematics;
using UnityEngine.AI;
using Unity.VisualScripting.Dependencies.Sqlite;

public class JimMovement : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator animator;
    bool derecha = true;
    float mh;
    public float vel = 10;
    public float jmp = 5;
    public Transform Spawner;
    public GameObject prefab;
    public Transform groundCheck;
    public Transform wallCheck;
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    bool isOnGround = true;
    bool isTouchingWall = false;
    bool isSlidin = false;
    private bool isTouchingLeft;
    private bool isTouchingRight;
    public float wallspeed;
    private float lastUsedTime;
    private float wallJumpingDirection;
    private bool wallJumping;
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 30f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;
    private float touchingLeftOrRgiht;

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
        if ( isDashing)
        {
            return;
        }

        isOnGround = Physics2D.OverlapCapsule(groundCheck.position,new Vector2(0.05f,0.45f),CapsuleDirection2D.Horizontal,0,groundLayer);

        isTouchingLeft = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x - 0.5f, gameObject.transform.position.y), new Vector2(0.2f, 0.9f), 0f, wallLayer);

        isTouchingRight = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x + 0.5f, gameObject.transform.position.y), new Vector2(0.2f, 0.9f), 0f, wallLayer);

        if(isTouchingLeft)
        {
            touchingLeftOrRgiht = 1;
        }
        else if(isTouchingRight)
        {
            touchingLeftOrRgiht = -1;
        }

        if(Input.GetKeyDown("space") && (isTouchingRight || isTouchingLeft) && !isOnGround)
        {
            wallJumping = true;
            Invoke("SetJumpingToFalse", 0.08f);
        }

        if(wallJumping)
        {
            rb.velocity = new Vector2(vel * touchingLeftOrRgiht, jmp);
        }
        

        isTouchingWall = Physics2D.OverlapCircle(wallCheck.position, 0.5f, wallLayer);
        if (isTouchingWall == true)
        {
            Debug.Log("aaaa");
        }
        if(Input.GetAxis("Jump")>0 && isOnGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0, jmp), ForceMode2D.Impulse);
        }
        if (isOnGround)
        {
            animator.SetBool("groundDetection", true);
        }
        animator.SetBool("groundDetection", !isOnGround);
        mh = Input.GetAxis("Horizontal");
        if(mh>0 && !derecha)
        {
            voltear();
        }
        else if (mh < 0 && derecha)
        {
            voltear();
        }

        if((!isTouchingLeft && !isTouchingRight) || isOnGround)
        {
            rb.velocity = new Vector2(mh * vel, rb.velocity.y);
        }
        animator.SetFloat("Velocity", Mathf.Abs(mh));


        if(Input.GetKeyDown(KeyCode.LeftShift)&& canDash)
        {
            StartCoroutine(Dash());
        }

                
        PlayerShooting();
        WallSlide();
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
    }
    private void voltear()
    {
        derecha = !derecha;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    private void PlayerShooting()
    {
        if (Input.GetButtonDown("Fire1")) 
        {
            Instantiate(prefab,Spawner.position,Spawner.rotation);
        }
    }

    private bool isWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void WallSlide()
    {
        if (!isOnGround && isTouchingWall)
        {
            isSlidin = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallspeed, float.MaxValue));
        }
        else
        {
            isSlidin = false;
        }
    }

    private void SetJumpingToFalse()
    {
        wallJumping = false;
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
}

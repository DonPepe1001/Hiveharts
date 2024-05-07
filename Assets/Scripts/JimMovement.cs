using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using Unity.VisualScripting;
using Unity.Mathematics;
using UnityEngine.AI;

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
    private bool isWallJumping;
    public float wallspeed;
    private float lastUsedTime;
    private float wallJumpingDirection;
    private float wallJumpingTime = 4f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(10f, 3f);


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
        isOnGround = Physics2D.OverlapCapsule(groundCheck.position,new Vector2(0.05f,0.45f),CapsuleDirection2D.Horizontal,0,groundLayer);
        isTouchingWall = Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
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
        rb.velocity = new Vector2(mh * vel, rb.velocity.y);
        animator.SetFloat("Velocity", Mathf.Abs(mh));
        PlayerShooting();
        WallSlide();
        WallJump();

    }

    private void FixedUpdate()
    {
        if(!isWallJumping)
        {
            rb.velocity = new Vector2(mh * vel, rb.velocity.y); 
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

    private bool IsWalled()
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

    private void WallJump()
    {
        if(isSlidin)
        {
            //isWallJumping = false;
            //wallJumpingDirection = -transform.localScale.x;  
            wallJumpingCounter = wallJumpingTime;
            //CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }
        if(Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            //isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    } 
}

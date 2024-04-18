using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class JimMovement : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator animator;
    bool derecha = true;
    public float vel = 10;
    public float jmp = 5;
    public Transform Spawner;
    public GameObject prefab;
    public Transform groundCheck;
    public LayerMask groundLayer;
    bool isOnGround = true;

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
        float mh = Input.GetAxis("Horizontal");
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
}

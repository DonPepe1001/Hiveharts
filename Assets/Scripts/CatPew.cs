using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatPew : MonoBehaviour
{
public float vel = 10;
    Rigidbody2D rb;
    GameObject player;
    Transform transformPlayer;
    public float lifeTime;
    public static float damage=2;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        transformPlayer = player.transform;
    }
    private void Start()
    {
        if(transformPlayer.localScale.x > 0)
        {
            rb.velocity = new Vector2(vel, rb.velocity.y);
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        } 
        else if (transformPlayer.localScale.x < 0)
        {
            rb.velocity = new Vector2(-vel, rb.velocity.y);
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject,lifeTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            Destroy(gameObject);
        }
    }
}

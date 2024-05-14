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
    public static float damage = 2;
    // Start is called before the first frame update



     

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }
    void Start()
    {
        float direction = Mathf.Sign(rb.velocity.x);
        transform.localScale = new Vector3(direction * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    // Method to set the initial velocity of the bullet based on direction
    public void SetVelocity(Vector2 direction)
        {
            rb.velocity = direction * vel;
        }


    



    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, lifeTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Destroy(gameObject);
        }
    }
}

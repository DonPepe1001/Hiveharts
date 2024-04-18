using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyManager : MonoBehaviour
{
    public float LifeEnemy=10;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="CatPew")
        {
            LifeEnemy -= CatPew.damage;
            if(LifeEnemy<=0)
            {
                Destroy(gameObject);
            }
        }
        if(collision.tag=="Player")
        {
            SceneManager.LoadScene("LoseScene");
        }
    }
}
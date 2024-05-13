using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{
    public float LifeEnemy=100;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="CatPew")
        {
            LifeEnemy -= CatPew.damage;
            if(LifeEnemy<=0)
            {
                SceneManager.LoadScene("SeaLevel");
            }
        }else if(collision.tag=="Player")
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}

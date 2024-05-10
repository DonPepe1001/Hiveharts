using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class stingerAttack : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Debug.Log("morision");
            SceneManager.LoadScene("LoseScene");
        }
        if(collision.tag == "Delete")
        {
            Destroy(gameObject);
        }
    }
}

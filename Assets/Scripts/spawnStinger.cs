using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class spawnStinger : MonoBehaviour
{
    public Transform spawner;
    public GameObject prefab;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Instantiate(prefab,spawner.position,spawner.rotation);
        }
    }
}

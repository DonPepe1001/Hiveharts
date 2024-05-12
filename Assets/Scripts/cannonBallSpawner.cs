using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class cannonBallSpawner : MonoBehaviour
{
    public GameObject cannonBall;
    public Transform cannonBallPos;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(timer > 1)
        {
            timer = 0;
            shoot();
        }
        
    }
    void shoot()
    {
        Instantiate(cannonBall, cannonBallPos.position, Quaternion.identity);
    }
}

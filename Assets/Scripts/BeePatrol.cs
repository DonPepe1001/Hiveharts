using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeePatrol : MonoBehaviour
{
    public GameObject startPoint;
    public GameObject endPoint;
    public float vel=5;
    bool derecha;
    // Start is called before the first frame update
    void Start()
    {
        if(!derecha)
        {
            transform.position = startPoint.transform.position;
        }
        else
        {
            transform.position = endPoint.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!derecha)
        {
            transform.position = Vector3.MoveTowards(transform.position,endPoint.transform.position,vel*Time.deltaTime);
            if(transform.position == endPoint.transform.position)
            {
                derecha = true;
                GetComponent<SpriteRenderer>().flipX = true;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, startPoint.transform.position, vel * Time.deltaTime);
            if (transform.position == startPoint.transform.position)
            {
                derecha = false;
                GetComponent<SpriteRenderer>().flipX = false;
            }
        }
    }
}
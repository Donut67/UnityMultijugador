using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fixedForward : MonoBehaviour
{
    //public Camera fixedCamera;
    public float speed = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = new Vector3(speed * Time.deltaTime, 0, 0);
        
         transform.position = new Vector3(transform.position.x + 4f * Time.deltaTime, transform.position.y, transform.position.z);
        
    }
}

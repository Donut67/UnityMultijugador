using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class adaptativeCamera : MonoBehaviour
{
    
    public float zoomFactor = 2f;//dist ente jug
    public float followTimeDelta = 0.8f;
    public GameObject Player1;
    public GameObject Player2;
    public Camera cam;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        // Midpoint we're after
        Vector3 midpoint = (Player1.transform.position + Player2.transform.position) / 2f;

        // Distance between objects
        float distance = (Player1.transform.position - Player2.transform.position).magnitude;

        // Move camera a certain distance
        Vector3 cameraDestination = midpoint - cam.transform.forward * distance * zoomFactor;

        if (cam.orthographic)
        {
            // The camera's forward vector is irrelevant, only this size will matter
            /*if (distance > 5 && distance < 20)
            {
                cam.orthographicSize = distance;
            }*/
            cam.orthographicSize = distance;

        }
        // You specified to use MoveTowards instead of Slerp
        cam.transform.position = Vector3.Slerp(cam.transform.position, cameraDestination, followTimeDelta);

        // Snap when close enough to prevent annoying slerp behavior
        if ((cameraDestination - cam.transform.position).magnitude <= 0.05f)
            cam.transform.position = cameraDestination;
    }
}

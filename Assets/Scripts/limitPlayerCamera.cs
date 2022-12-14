using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class limitPlayerCamera : MonoBehaviour
{
    // Start is called before the first frame update

    public Camera Camera;
    private double amplada;
    private PlayerController jug;
    
    void Awake()
    {
        jug = GetComponent<PlayerController>();
    }
    void Start()
    {
        amplada = Camera.aspect * 2f * Camera.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        if (Camera.isActiveAndEnabled) {
            
            if (Mathf.Abs(Camera.gameObject.transform.position.x - transform.position.x) < amplada) {
                jug.esPotMoure = false;
            }
            else
            {
                jug.esPotMoure = true;
            }
        }
    }
}

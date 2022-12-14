using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveSaw : MonoBehaviour
{
    // Start is called before the first frame update
    public int distancia = 50;
    public float speed = 2.5f;
    private bool endavant;
    private Vector3 posInicial;
    void Start()
    {
        endavant = true;
        posInicial = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (endavant)
        {
            if (transform.position.x < posInicial.x + distancia)
            {
                transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
            }
            else
            {
                endavant = false;
            }
        }
        else
        {
            if (transform.position.x > posInicial.x )
            {
                transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);
            }
            else
            {
                endavant = true;
            }
        }   
    }
}

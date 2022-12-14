using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class textScript : MonoBehaviour
{
    InputHandler inputHandler;
    TextMesh textMesh;
    private void Awake()
    {
        inputHandler = GetComponentInParent<InputHandler>();
        textMesh = GetComponent<TextMesh>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (inputHandler.isLocal)
        {
            textMesh.text = "Local";
        }
        else
        {
            textMesh.text = "Remote";
        }
    }
}

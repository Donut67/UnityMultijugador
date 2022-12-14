using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followCamera : MonoBehaviour
{
    public GameObject Player;       
          

    void Start()
    {
    }

    void LateUpdate()
    {
		transform.position = new Vector3(Player.gameObject.transform.position.x, Player.gameObject.transform.position.y, transform.position.z);

	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damageMap : MonoBehaviour
{
    private ServerHandler serverHandler;
    private void Awake()
    {
        var g = GameObject.FindWithTag("Handler");
        serverHandler = g.GetComponent<ServerHandler>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
		if (collision.GetComponent <HealthSystem>()) {
			collision.GetComponent<HealthSystem>().takeDamage(100);
            serverHandler.SendPlayerDead(collision.GetComponent<PlayerController>().isPlayer1);
		}
    }
}

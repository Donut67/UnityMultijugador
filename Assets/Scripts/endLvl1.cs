using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class endLvl1 : MonoBehaviour
{
	Animator animator;
	private int playersEntered = 0;

	private ServerHandler serverHandler;
	private void Awake() {
		animator = GetComponent<Animator>();
		var g = GameObject.FindWithTag("Handler");
		serverHandler = g.GetComponent<ServerHandler>();
	}
	private void OnTriggerEnter2D(Collider2D collision) {
		if(collision.tag == "Player") {
			animator.SetTrigger("playerEntered");
			collision.gameObject.SetActive(false);
			playersEntered++;
			if(playersEntered >= 2) {
				StartCoroutine(LoadScene());;
			}
			serverHandler.SendEndLevelDoorEntered(collision.GetComponent<PlayerController>().isPlayer1);

		}
	}




	IEnumerator LoadScene() {
		yield return new WaitForSeconds(1);
		SceneManager.LoadScene(3);
	}
}

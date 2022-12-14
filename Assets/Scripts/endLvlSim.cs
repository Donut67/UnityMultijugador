using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class endLvlSim : MonoBehaviour
{
	Animator animator;
	private int playersEntered = 0;

	private void Awake() {
		animator = GetComponent<Animator>();
	}
	public void EnterDoor() {
		animator.SetTrigger("playerEntered");
		playersEntered++;
		/*if(playersEntered >= 2) {
			StartCoroutine(LoadScene());;
		}*/
	}




	IEnumerator LoadScene() {
		yield return new WaitForSeconds(1);
		SceneManager.LoadScene(3);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class endGameScreen : MonoBehaviour
{
    public void QuitGame() {
		Application.Quit();
	}

	public void LoadCoop() {
		SceneManager.LoadScene(1);
	}
	public void LoadPvP() {
		SceneManager.LoadScene(2);
	}

	public void LoadMainMenu() {
		SceneManager.LoadScene(0);
	}

}

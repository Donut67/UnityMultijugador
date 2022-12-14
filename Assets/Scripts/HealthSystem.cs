using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] int maxHP;
	[SerializeField] simpleFlash flash;
	private PlayerController playerController;
    private int currentHP;
	public bool dead = false;


	private void Awake() {
		playerController = GetComponent<PlayerController>();
	}
	// Start is called before the first frame update
	void Start()
    {
        currentHP = maxHP;
    }

    public void takeDamage(int damage)
    {
        currentHP -= damage;
		flash.Flash();
		playerController.justGotHit = 1;
        if(currentHP <= 0) { Die(); }
    }

    private void Die()
    {
        if(gameObject.tag == "Player") {
			StartCoroutine(LoadScene());
			playerController.isAlive = false;
			playerController.Alive = 0;
			dead = true;
		}
		else {
			Destroy(gameObject);
		}
    }

	IEnumerator LoadScene() {
		yield return new WaitForSeconds(1);
		if (SceneManager.GetActiveScene().buildIndex == 1) {
			SceneManager.LoadScene(4);
		}
		else {
			SceneManager.LoadScene(3);
		}
	}
}

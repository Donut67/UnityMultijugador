using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class button : MonoBehaviour
{
	[SerializeField] Animator aController;
	private SpriteRenderer spriteRenderer;
	[SerializeField] Sprite normalButton;
	[SerializeField] Sprite pressedButton;

	private ServerHandler serverHandler;

	private void Awake() {
		spriteRenderer = GetComponent<SpriteRenderer>();
		var g = GameObject.FindWithTag("Handler");
		serverHandler = g.GetComponent<ServerHandler>();
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		aController.SetBool("buttonPressed", true);
		spriteRenderer.sprite = pressedButton;
		serverHandler.SendButtonPressed(true);
	}

	private void OnTriggerExit2D(Collider2D collision) {
		aController.SetBool("buttonPressed", false);
		spriteRenderer.sprite = normalButton;
		serverHandler.SendButtonPressed(false);
	}
}

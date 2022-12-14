using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonSimulation : MonoBehaviour
{
	[SerializeField] Animator aController;
	private SpriteRenderer spriteRenderer;
	[SerializeField] Sprite normalButton;
	[SerializeField] Sprite pressedButton;

	private void Awake() {
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	public void buttonPressed()
	{
		aController.SetBool("buttonPressed", true);
		spriteRenderer.sprite = pressedButton;
	}

	public void buttonNotPressed()
	{
		aController.SetBool("buttonPressed", false);
		spriteRenderer.sprite = normalButton;
	}

}

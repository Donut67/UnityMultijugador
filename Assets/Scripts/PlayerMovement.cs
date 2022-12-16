using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public CharacterController2D controller;
	public Animator animator;

	public float runSpeed = 40f;

	public bool simulated = false;
	public int player;

	float horizontalMove = 0f;
	bool jump = false;
	bool crouch = false;
	
	// Update is called once per frame
	void Update () {
		if(!simulated) {
			horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
			animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
			if (Input.GetButtonDown("Vertical")) {
				jump = true;
				animator.SetBool("IsJumping", true);
			}
		} 
		// else {
		// 	horizontalMove = Input.GetAxisRaw("Horizontal2") * runSpeed;
		// 	if (Input.GetButtonDown("Vertical2")) jump = true;
		// }

	}

	public void OnLanding() {
		animator.SetBool("IsJumping", false);
	}

	void FixedUpdate (){
		controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
		jump = false;
	}
}

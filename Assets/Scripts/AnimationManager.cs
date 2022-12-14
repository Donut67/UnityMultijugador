using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
	private Animator animator;
	[SerializeField] private PlayerController playerController;
	private HealthSystem healthSystem;
	private string currentState;


	//Animation States
	const string PLAYER_IDLE = "playerIdle";
	const string PLAYER_RUN = "playerRun";
	const string PLAYER_JUMP_DOWN = "jumpDown";
	const string PLAYER_JUMP_UP = "jumpUp";
	const string PLAYER_ATTACK = "playerAttack";
	const string PLAYER_DEATH = "playerDeath";



	private void Awake() {
		animator = GetComponent<Animator>();
		healthSystem = playerController.GetComponent<HealthSystem>();
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (healthSystem.dead) {
			ChangeAnimationState(PLAYER_DEATH);
		}
		else if (playerController.isAttacking) {
			ChangeAnimationState(PLAYER_ATTACK);
		}
		else if (!playerController.isGrounded()) {
			if (playerController.myRigidbody.velocity.y >= 0) ChangeAnimationState(PLAYER_JUMP_UP);
			else ChangeAnimationState(PLAYER_JUMP_DOWN);
		}
		else if (Mathf.Abs(playerController.myRigidbody.velocity.x) <= 0.1f) {
			ChangeAnimationState(PLAYER_IDLE);
		}
		else {
			ChangeAnimationState(PLAYER_RUN);
		}
    }

	void ChangeAnimationState(string newState) {
		if (newState == currentState) return;
		animator.Play(newState);
		currentState = newState;
	}
}

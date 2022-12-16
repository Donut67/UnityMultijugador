using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public CharacterController2D controller;
	public Animator animator;
	[SerializeField] public GameObject RalentPrefab;
	public float runSpeed = 40f;
	public bool simulated = false;
	public int player;
	public float cooldown = 3.0f;
	public float tempsRestant = 0.0f;

	private float tempsRelantitzat = 0.0f;
	float horizontalMove = 0f;
	bool jump = false;
	bool crouch = false;
	string habilitat = "Ralentitzar";
	
	// Update is called once per frame
	void Update () {
		if(!simulated) {
			horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
			animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
			if (Input.GetButtonDown("Jump")) {
				jump = true;
				animator.SetBool("IsJumping", true);
			}
			if(Input.GetButtonDown("Ability") && tempsRestant == 0.0f) {
				if(habilitat == "Dash") {}
				else if(habilitat == "Ralentitzar") {Ralentitzar();}
				else if(habilitat == "Potencia") {}
			}
			if(tempsRestant > 0.0f) tempsRestant -= Time.deltaTime;
			else tempsRestant = 0.0f;

			if(tempsRelantitzat <= 0.0f) runSpeed = 40f;
			else tempsRelantitzat -= Time.deltaTime;
		}
		// else {
		// 	horizontalMove = Input.GetAxisRaw("Horizontal2") * runSpeed;
		// 	if (Input.GetButtonDown("Vertical2")) jump = true;
		// }

	}

	public void OnLanding() {
		animator.SetBool("IsJumping", false);
	}

	void FixedUpdate () {
		controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
		jump = false;
	}

	public void Ralentitzar() {
		// Es crea l'area a ralentitzar
		GameObject go = Instantiate(RalentPrefab, gameObject.transform.position, Quaternion.identity);
		// Es posa aquesta area a la capa de l'equip contrari
		// go.layer = LayerMask.NameToLayer("Team1");
		go.layer = LayerMask.NameToLayer("Team2");
		tempsRestant = cooldown;
	}

	public void RalentitzarJugador(float temps) {
		// Ralentitzar el jugador durant un temps
		runSpeed = 10.0f;
		tempsRelantitzat = temps;
	}

}

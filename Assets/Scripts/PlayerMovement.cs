using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public CharacterController2D controller;
	public Animator animator;
	[SerializeField] public GameObject RalentPrefab;
	public float runSpeed = 40f;
	public int vida = 4;
	public bool simulated = true;
	public int player;
	public float cooldown = 10.0f;
	public FunctionTimer cooldownTimer = null;
	public FunctionTimer habilitatTimer = null;
	public FunctionTimer mortTimer = null;
	private string habilitat = "Potencia";
	private bool potActivar = true;
	float horizontalMove = 0f;
	bool jump = false;
	bool crouch = false;
    private ClientHandler ch = null;
	
	void Awake(){
        ch = GameObject.FindWithTag("Handler").GetComponent<ClientHandler>();
	}

	// Update is called once per frame
	void Update () {
		if(!simulated) {
			float hMove = Input.GetAxisRaw("Horizontal") * runSpeed;
			bool j = Input.GetButtonDown("Jump");
			bool a = Input.GetButtonDown("Ability");

			if(hMove != 0 || j || a) ch.SendToServer("INPUTS," + hMove.ToString("0.00") + "," + (j? "true" : "false") + "," + (a? "true" : "false"));
			
			if(cooldownTimer != null) cooldownTimer.Update();
			if(habilitatTimer != null) habilitatTimer.Update();
			if(mortTimer != null) mortTimer.Update();
		}
	}

	public void SetHabilitat(string nom) {
		habilitat = nom;
	}

	public void OnLanding() {
		animator.SetBool("IsJumping", false);
	}

	public void ControlPlayer() {
		simulated = false;
	}

	void FixedUpdate () {
		controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
		jump = false;
	}

	public void RecieveServerInfo(float hMove, bool j, bool h){
		horizontalMove = hMove;
		jump = j;

		if(h && potActivar) {
			if(habilitat == "Dash") {}
			else if(habilitat == "Ralentitzar") {Ralentitzar();}
			else if(habilitat == "Potencia") {Potencia();}
		}

		animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
		if(jump) animator.SetBool("IsJumping", true);
	}

	public void Ralentitzar() {
		// Es crea l'area a ralentitzar
		GameObject go = Instantiate(RalentPrefab, gameObject.transform.position, Quaternion.identity);
		// Es posa aquesta area a la capa de l'equip contrari
		// go.layer = LayerMask.NameToLayer("Team1");
		go.layer = LayerMask.NameToLayer("Team2");
		Destroy(go, 1);
		potActivar = false;
		cooldownTimer = new FunctionTimer(AcabarCooldown, cooldown);
	}

	public void AcabarCooldown() {
		potActivar = true;
	}

	public void Reviure() {
		vida = 4;
	}

	public void VelocitatBase() {
		runSpeed = 40.0f;
	}

	public void RalentitzarJugador(float temps) {
		// Ralentitzar el jugador durant un temps
		runSpeed = 10.0f;
		habilitatTimer = new FunctionTimer(VelocitatBase, temps);
	}

    public void Potencia(){
        runSpeed = 80.0f;
		potActivar = false;
		habilitatTimer = new FunctionTimer(VelocitatBase, 1.2f);
		cooldownTimer = new FunctionTimer(AcabarCooldown, cooldown);
    }

	public void RebreMal() {
		vida -= 1;
		if(vida == 0) mortTimer = new FunctionTimer(Reviure, 2.0f);
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour {

	public CharacterController2D controller;
	public Animator animator;
	[SerializeField] public GameObject RalentPrefab;
	[SerializeField] public GameObject AtacPrefab;
	public float runSpeed = 80f;
	public int vida = 4;
	public bool control = false;
	public int player;
	public int team;
	public float cooldown = 10.0f;
	public FunctionTimer cooldownTimer = null;
	public FunctionTimer atacTimer = null;
	public FunctionTimer habilitatTimer = null;
	public FunctionTimer mortTimer = null;
	private string habilitat = "Potencia";
	private bool potActivar = true;
	private bool potAtacar  = true;
	float horizontalMove = 0f;
	bool jump = false;
	bool crouch = false;
    private ClientHandler ch = null;

	float hMovePrevi = 0;

	void Awake(){
        ch = GameObject.FindWithTag("Handler").GetComponent<ClientHandler>();
	}

	// Update is called once per frame
	void Update () {
		if(control) {
			float hMove = Input.GetAxisRaw("Horizontal") * runSpeed;
			bool j = Input.GetButtonDown("Jump");
			bool a = Input.GetButtonDown("Ability");
			bool q = Input.GetButtonDown("Atack");

			if(hMove != hMovePrevi || j || a || q) 
				ch.SendToServer("INPUTS|" + hMove.ToString("0.00") + "|" + (j? "true" : "false") + "|" + (a? "true" : "false") + "|" + (q? "true" : "false"));

			if(cooldownTimer != null) cooldownTimer.Update();
			if(habilitatTimer != null) habilitatTimer.Update();
			if(mortTimer != null) mortTimer.Update();
			if(atacTimer != null) atacTimer.Update();

			hMovePrevi = hMove;
		}
	}

	public void SetPlayer(int id) {
		player = id;
		transform.parent.gameObject.GetComponent<PlayerTag>().SetPlayerId(player);
	}

	public void SetTeam(int t){
		team = t;
		transform.gameObject.layer = LayerMask.NameToLayer("Team" + team);;
	}

	public void SetHabilitat(string nom) {
		habilitat = nom;
	}

	public void OnLanding() {
		animator.SetBool("IsJumping", false);
		if(habilitat == "Doble salt") potActivar = true;
	}

	public void ControlPlayer() {
		control = true;
	}

	void FixedUpdate () {
		controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
		jump = false;
	}

	public void RecieveServerVida(int v){
		vida = v;
		if(vida == 0) {
			GetComponent<Rigidbody>().velocity = Vector3.zero;
			GetComponent<Rigidbody>().isKinematic = true;
			mortTimer = new FunctionTimer(Reviure, 2.0f);
		}
	}

	public void RecieveServerInfo(float hMove, bool j, bool h, bool q){
		horizontalMove = hMove;
		jump = (j && controller.m_Grounded) || (habilitat == "Doble salt" && h && potActivar);

		if(h && potActivar) {
			if(habilitat == "Dash") {Dash();}
			else if(habilitat == "Ralentitzar") {Ralentitzar();}
			else if(habilitat == "Potencia") {Potencia();}
			else if(habilitat == "Doble salt") {Salt();}
		}

		if(q && potAtacar) Atacar();

		animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
		if(jump) animator.SetBool("IsJumping", true);
	}

	public void Ralentitzar() {
		GameObject go = Instantiate(RalentPrefab, gameObject.transform.position, Quaternion.identity);
		go.layer = LayerMask.NameToLayer("Slow" + (team == 1? 2 : 1));
		potActivar = false;
		cooldownTimer = new FunctionTimer(AcabarCooldown, cooldown);
		Destroy(go, 1);
	}

	public void Atacar() {
		Vector3 p = new Vector3(gameObject.transform.position.x + (controller.m_FacingRight? 1 : -1), gameObject.transform.position.y, gameObject.transform.position.z);
		GameObject go = Instantiate(AtacPrefab, p, Quaternion.identity);
		go.layer = LayerMask.NameToLayer("Slow" + (team == 1? 2 : 1));
		potAtacar = false;
		atacTimer = new FunctionTimer(AcabarAtacCooldown, 1);
		Destroy(go, 0.1f);
	}

	public void AcabarCooldown() {
		potActivar = true;
	}

	public void AcabarAtacCooldown() {
		potAtacar = true;
	}

	public void Reviure() {
		GetComponent<Rigidbody>().isKinematic = false;
		vida = 4;
	}

	public void VelocitatBase() {
		runSpeed = 80.0f;
		GameObject.FindWithTag("Chat").GetComponent<TextMeshProUGUI>().text = "Velocitat Base";
	}

	public void GravityBase() {
		runSpeed = 80.0f;
		GetComponent<Rigidbody2D>().gravityScale = 6.0f;
		GameObject.FindWithTag("Chat").GetComponent<TextMeshProUGUI>().text = "Gravity Base";
	}

	public void RalentitzarJugador(float temps) {
		// Ralentitzar el jugador durant un temps
		runSpeed = 30.0f;
		habilitatTimer = new FunctionTimer(VelocitatBase, temps);
		GameObject.FindWithTag("Chat").GetComponent<TextMeshProUGUI>().text = "Ralentitzat";
	}

    public void Potencia(){
        runSpeed = 100.0f;
		potActivar = false;
		habilitatTimer = new FunctionTimer(VelocitatBase, 1.2f);
		cooldownTimer = new FunctionTimer(AcabarCooldown, cooldown);
    }

	public void Dash() {
		GameObject.FindWithTag("Chat").GetComponent<TextMeshProUGUI>().text = "Dash";
		potActivar = false;
		runSpeed = 750.0f;
		GetComponent<Rigidbody2D>().gravityScale = 0.0f;
		habilitatTimer = new FunctionTimer(GravityBase, 0.05f);
		cooldownTimer = new FunctionTimer(AcabarCooldown, cooldown);
	}

	public void Salt() {
		potActivar = false;
		jump = true;
	}

	public void RebreMal() {
		if(control) ch.SendToServer("VIDA|" + (vida - 1));
	}

	public void OnTriggerEnter2D(Collider2D col) {
        if(col.gameObject.tag == "Ralentitzar" && runSpeed != 20.0f) RalentitzarJugador(2.5f);
        else if(col.gameObject.tag == "Finish") ch.SendToServer("FINISH|" + transform.gameObject.layer);
		else if(col.gameObject.tag == "AreaAtac") RebreMal();
    }

}

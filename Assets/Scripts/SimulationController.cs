using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationController : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D myRigidbody;
	[SerializeField] float jumpForce = 10f;
	[SerializeField] GameObject projectilePrefab;
	[SerializeField] Transform shootingPoint;
	[SerializeField] LayerMask groundLayer;
	[SerializeField] simpleFlash flash;
	private bool facingRight = true;
	public float fireRate = 0.3f;
	private float nextFire = 0f;
	private BoxCollider2D boxCollider2D;
	public bool isAttacking = false;
	public bool esPotMoure = true;
	public bool isAlive = true;


	public float dir;
	// Start is called before the first frame update

	private void Awake() {
		myRigidbody = GetComponent<Rigidbody2D>();
		boxCollider2D = GetComponent<BoxCollider2D>();
	}



	public void Shoot() {
		GameObject proj = GameObject.Instantiate(projectilePrefab, shootingPoint.position, transform.rotation);
		Vector3 theScale = new Vector3(Mathf.Sign(transform.localScale.x) * proj.transform.localScale.x, proj.transform.localScale.y, proj.transform.localScale.z);
		proj.transform.localScale = theScale;
		if (facingRight) {
			proj.GetComponent<projectile>().SetSpeed(1);
			//peer.sendShoot(1);
		}
		else {
			proj.GetComponent<projectile>().SetSpeed(-1);
			//peer.sendShoot(1);
		}
	}

	public void Jump() {
		if(isGrounded()) myRigidbody.AddForce(Vector2.up * jumpForce);
	}

	public void Die() {
		isAlive = false;
		myRigidbody.velocity = Vector2.zero;
		myRigidbody.gravityScale = 0f;
		boxCollider2D.enabled = false;
	}

	public void FlashWhite() {
		flash.Flash();
	}

	private void Update()
	{
		HorizontalMovement();
	}

	public void HorizontalMovement() {

		myRigidbody.velocity = new Vector2(dir*4.5f, myRigidbody.velocity.y);
		if(dir < 0 && facingRight) {
			facingRight = false;
			Vector3 theScale = transform.localScale;
			theScale.x *= -1;
			transform.localScale = theScale;
			//peer.flip();
		}
		else if (dir > 0 && !facingRight) {
			facingRight = true;
			Vector3 theScale = transform.localScale;
			theScale.x *= -1;
			transform.localScale = theScale;
			//peer.flip();
		}
	}


	public bool isGrounded() {
		float extraHeightTest = 0.03f;
		RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, extraHeightTest, groundLayer);

		/*Color rayColor;
		if(raycastHit.collider != null) {
			rayColor = Color.green;
		}
		else {
			rayColor = Color.red;
		}
		Debug.DrawRay(boxCollider2D.bounds.center + new Vector3(boxCollider2D.bounds.extents.x, 0), Vector2.down * (boxCollider2D.bounds.extents.y + extraHeightTest), rayColor);
		Debug.DrawRay(boxCollider2D.bounds.center - new Vector3(boxCollider2D.bounds.extents.x, 0), Vector2.down * (boxCollider2D.bounds.extents.y + extraHeightTest), rayColor);
		Debug.DrawRay(boxCollider2D.bounds.center - new Vector3(boxCollider2D.bounds.extents.x, boxCollider2D.bounds.extents.y), Vector2.right * (boxCollider2D.bounds.extents.x), rayColor);*/
		return raycastHit.collider != null;
	}
}

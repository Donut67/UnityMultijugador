using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
	[SerializeField] float speed = 15f;
	private Animator animator;
	private Rigidbody2D rb;
	private BoxCollider2D boxCollider2D;
	private void Awake() {
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		boxCollider2D = GetComponent<BoxCollider2D>();
	}
	// Start is called before the first frame update
	public void SetSpeed(int dir)
    {
		Vector2 spd = new Vector2(speed*dir, 0);
		rb.velocity = spd;
    }

	private void OnTriggerEnter2D(Collider2D collision) {
		if(collision.GetComponent<HealthSystem>()) {
			collision.GetComponent<HealthSystem>().takeDamage(10);
		}
		Vector2 spd = new Vector2(0, 0);
		rb.velocity = spd;
		boxCollider2D.enabled = false;
		animator.SetTrigger("destroy");
		StartCoroutine(Die());
	}


	IEnumerator Die() {
		yield return new WaitForSeconds(1);
		Destroy(gameObject);
	}

}

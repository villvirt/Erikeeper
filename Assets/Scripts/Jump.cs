using UnityEngine;
using System.Collections;

public class Jump : MonoBehaviour {
	private Animator anim;
	private AudioSource sound;
	public float jumpRate = 0.1f;
	bool canJump = true;
	public float jumpPower=5;
	void Start(){
		anim = transform.root.gameObject.GetComponent<Animator> ();
		sound = transform.root.gameObject.GetComponent <AudioSource> ();
	}

	void Update() {
		if (Input.GetMouseButtonDown (0)) {
			JumpAction ();
			//TryJump ();
		}
	}
	
	IEnumerator JumpCooldown() {
		canJump = false;
		yield return new WaitForSeconds(jumpRate);
		canJump = true;
		yield break;
	}
	
	void TryJump() {
		if (!canJump) return;
		JumpAction();
		StartCoroutine(JumpCooldown());
	}
	
	void JumpAction() {
		this.GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
		anim.Play("Flap");
		anim.Play("FlapAgain");
		sound.Play ();
		GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);
	}








}

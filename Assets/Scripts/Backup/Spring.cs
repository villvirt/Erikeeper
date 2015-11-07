using UnityEngine;
using System.Collections;

public class Spring : MonoBehaviour {
	private Animator anim;
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag.Equals("Player")){
		anim = other.transform.root.gameObject.GetComponent<Animator> ();
		anim.Play ("FlowerBounce");
		other.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 15), ForceMode2D.Impulse);
		other.gameObject.transform.Find ("BonusEffect").gameObject.SetActive (true);
		}
	}
}

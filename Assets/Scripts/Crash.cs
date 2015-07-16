using UnityEngine;
using System.Collections;

public class Crash : MonoBehaviour {
	private Animator anim;
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag.Equals("Player")){
			anim = other.transform.root.gameObject.GetComponent<Animator> ();
			anim.Play("Death");
			other.enabled=false;
			other.gameObject.GetComponent<Jump>().enabled = false;
		}
	}
}





using UnityEngine;
using System.Collections;

public class Spring : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other)
	{
		other.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 15), ForceMode2D.Impulse);
	}
}

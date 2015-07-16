using UnityEngine;
using System.Collections;

public class MoveObject : MonoBehaviour
{
	public Vector2 velocity = new Vector2(4, 0);
	
	// Use this for initialization
	void Start()
	{
		GetComponent<Rigidbody2D>().velocity = velocity;
	}
	void Update(){
		Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
		if (screenPosition.x < -1)
		{
			Destroy (gameObject);
		}

	}
}
using UnityEngine;
using System.Collections;

public class Crash : MonoBehaviour {
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag.Equals("Player")){
		Application.LoadLevel(Application.loadedLevel);
		}
	}
}

using UnityEngine;
using System.Collections;

public class Replay : MonoBehaviour {

	// Use this for initialization
	public void Restart () {
		Application.LoadLevel(Application.loadedLevel);
	}

}

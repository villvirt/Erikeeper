using UnityEngine;
using System.Collections;

public class Mute : MonoBehaviour {

	public void mute(){
		AudioListener.volume = 0f;
	}
}

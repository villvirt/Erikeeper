using UnityEngine;
using System.Collections;

public class SoundOn : MonoBehaviour {

	public void soundOn(){
		AudioListener.volume = 1f;
	}
}

using UnityEngine;
using System.Collections;
public class GenerateFlower : MonoBehaviour {
	
	//Spawn this object
	public GameObject flower;
	
	public float maxTime = 5;
	public float minTime = 2;
	
	//current time
	private float time;
	
	//The time to spawn the object
	private float spawnTime;
	
	void Start(){
		SetRandomTime();
		time = minTime;
	}
	
	void FixedUpdate(){
		
		//Counts up
		time += Time.deltaTime;
		
		//Check if its the right time to spawn the object
		if(time >= spawnTime){
			SpawnFlower();
			SetRandomTime();
		}
		
	}
	
	
	//Spawns the obstacle resets the time
	void SpawnFlower(){
		time = 0;
		Instantiate (flower);
	}
	
	
	//Sets the random time between minTime and maxTime
	void SetRandomTime(){
		spawnTime = Random.Range(minTime, maxTime);
	}
	
}

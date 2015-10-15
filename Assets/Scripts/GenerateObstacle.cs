using UnityEngine;
using System.Collections;
public class GenerateObstacle : MonoBehaviour {
	
	//Spawn these object
	public GameObject obstacle_mid;

	public float maxTime = 3;
	public float minTime = 2;
	public float maxHeight = 0.5f;
	public float minHeight = -0.5f;
	
	//current time
	private float time;
	
	//The time to spawn the object
	private float spawnTime;
	private float spawnHeight;
	
	void Start(){
		SetRandomTime();
		time = minTime;
	}
	
	void FixedUpdate(){
		
		//Counts up
		time += Time.deltaTime;
		
		//Check if its the right time to spawn the object
		if(time >= spawnTime){
			SpawnObstacle();
			SetRandomTime();
		}
		
	}
	
	
	//Spawns the obstacle resets the time
	void SpawnObstacle(){
		SetRandomHeight();
		Vector3 pos = new Vector3(12, spawnHeight, 0);
		time = 0;
		Instantiate (obstacle_mid, pos, Quaternion.identity);
	}
	

	//Sets the random time between minTime and maxTime
	void SetRandomTime(){
		spawnTime = Random.Range(minTime, maxTime);
	}
	void SetRandomHeight(){
		spawnHeight = Random.Range(minHeight, maxHeight);
	}

}

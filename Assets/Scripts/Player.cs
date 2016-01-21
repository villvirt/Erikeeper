using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using com.shephertz.app42.paas.sdk.csharp;    
using com.shephertz.app42.paas.sdk.csharp.game;
using System;

public class Player : MonoBehaviour
{
	private Animator anim;
	private Animator textAnim;
	private int pointCounter=0;
	public float bouncePower=15;
	public Font customFont;
	public GameObject deathCanvas;
	public Text scoreText;
	public Text introText;
	public GameObject BG;
	private GameObject[] obstacles;
	private bool started=false;
	private bool death=false;
	private bool drawScreen=true;
	private String gameName = "Erikeeper";
	// Use this for initialization
	void Start(){
		Time.timeScale=0.00001f;
		anim = transform.root.gameObject.GetComponent<Animator> ();
		textAnim = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<Animator>();
		death = false;
		
	}
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag.Equals ("Gate")) {
			this.gameObject.transform.GetChild (2).gameObject.GetComponent<AudioSource>().Play();
			pointCounter++;
			textAnim.Play("ScoreAnimation");
			scoreText.text=pointCounter.ToString();


		}
		if (other.tag.Equals ("Obstacle")) {
			anim.Play("Death");
			stopBG ();
			this.gameObject.transform.GetChild (1).gameObject.GetComponent<AudioSource>().Play();
			this.gameObject.GetComponent<CircleCollider2D>().enabled=false;
			this.gameObject.GetComponent<Jump>().enabled = false;
			this.GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
			GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GenerateObstacle>().enabled=false;
			obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
			for(int i=0; i<obstacles.Length ; i++){
				obstacles[i].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			}

		}

	}
	void Update ()
	{

		if(!started && Input.GetMouseButtonDown (0)){
			started=true;
			firstClick();

		}
			
		// Die by being off screen
		Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
		if (screenPosition.y < -100f)
		{
			this.gameObject.GetComponent<Jump>().enabled = false;
			death=true;
			Time.timeScale=0.00001f;
			if(drawScreen){
			deathScreen();
			}
		}
	}

	private void firstClick(){
		introText.enabled = false;
		scoreText.text="0";
		Time.timeScale=1f;
		
	}

	private void deathScreen(){
		drawScreen = false;
		Vector3 pos = new Vector3(0, 0, 0);
		deathCanvas.transform.Find ("Text").gameObject.GetComponent<UnityEngine.UI.Text>().text="Pisteet: "+pointCounter.ToString();
		if (AudioListener.volume.Equals (0)) {
			deathCanvas.transform.GetChild (4).gameObject.SetActive (true);
			deathCanvas.transform.GetChild (5).gameObject.SetActive (false);
		} else {
			deathCanvas.transform.GetChild (4).gameObject.SetActive(false);
			deathCanvas.transform.GetChild (5).gameObject.SetActive(true);
		}
		Instantiate (deathCanvas, pos, Quaternion.identity);
		scoreText.text="";
		postScore();

	}
	public double getScore(){
		return pointCounter;
	}

	private void stopBG(){
		foreach(Transform child in BG.transform){
			if (child.gameObject.name.Equals("Particles")) {
				
			} else {
				child.gameObject.GetComponent<Animator> ().enabled = false;
			}
		}
	}



	private void postScore(){
		Debug.Log(FBContainer.playerName);  
		double gameScore = getScore();
		if(FBContainer.highScore<gameScore && gameScore >1){
			FBContainer.scoreBoardService.SaveUserScore(gameName,FBContainer.playerName, gameScore, new PostScoreCallBack()); 
			FBContainer.highScore=gameScore;
		}
		else{
			Debug.Log("Score was lower than highscore");
		}
	}


	public class PostScoreCallBack : App42CallBack  
	{  
		public void OnSuccess(object response)  
		{  
			Game game = (Game) response;       
			App42Log.Console("gameName is " + game.GetName());   
			for(int i = 0;i<game.GetScoreList().Count;i++)  
			{  
				App42Log.Console("userName is : " + game.GetScoreList()[i].GetUserName());  
				App42Log.Console("score is : " + game.GetScoreList()[i].GetValue());  
				App42Log.Console("scoreId is : " + game.GetScoreList()[i].GetScoreId());  
			}  
		}  

		public void OnException(Exception e)  
		{  
			App42Log.Console("Exception : " + e);  
		}  
	}  

}


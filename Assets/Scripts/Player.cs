using UnityEngine;

public class Player : MonoBehaviour
{
	private Animator anim;
	private int pointCounter=0;
	private int bonusCounter=0;
	public float bouncePower=15;
	private GameObject[] obstacles;
	private GameObject[] flowers;
	private bool started=false;
	// Use this for initialization
	void Start(){
		Time.timeScale=0.00001f;
		anim = transform.root.gameObject.GetComponent<Animator> ();
		
	}
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag.Equals ("Gate")) {
			if(bonusCounter>0){
				pointCounter=pointCounter+2;
				bonusCounter--;
			}

			else{
			pointCounter++;
			}
			//Debug.Log (pointCounter);
			Debug.Log(bonusCounter);

		}
		if (other.tag.Equals ("Obstacle")) {
			anim.Play("Death");
			this.gameObject.GetComponent<BoxCollider2D>().enabled=false;
			this.gameObject.GetComponent<Jump>().enabled = false;
			this.GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
			GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GenerateObstacle>().enabled=false;
			obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
			flowers =GameObject.FindGameObjectsWithTag("Flower");
			for(int i=0; i<flowers.Length ; i++){
				flowers[i].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			}
			for(int i=0; i<obstacles.Length ; i++){
				obstacles[i].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			}

		}
		if (other.tag.Equals ("Flower")) {
			this.GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
			anim.Play ("FlowerBounce");
			this.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (0, bouncePower), ForceMode2D.Impulse);
			this.gameObject.transform.Find ("BonusEffect").gameObject.SetActive (true);
			this.bonusCounter = 3;
		}

	}
	void Update ()
	{

		if(!started && Input.GetMouseButtonDown (0)){
			started=true;
			firstClick();

		}
		if(bonusCounter==0){
			this.gameObject.transform.Find ("BonusEffect").gameObject.SetActive (false);
			
		}
		
		// Die by being off screen
		Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
		if (screenPosition.y < 0)
		{
			Die();
		}
	}

void Die()
{
	Application.LoadLevel(Application.loadedLevel);
		Debug.Log("kuolema");
}

	void OnGUI(){
		GUI.Box(new Rect(10,10,100,90), pointCounter.ToString());
		if(!started){
		GUI.Label (new Rect (Screen.width/2,Screen.height/2,90,90), "Click to start");
		}


	}

	private void firstClick(){
		Time.timeScale=1f;
		
	}

}


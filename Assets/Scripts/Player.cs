using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
	private Animator anim;
	private int pointCounter=0;
	private int bonusCounter=0;
	public float bouncePower=15;
	public GameObject deathCanvas;
	private GameObject[] obstacles;
	private GameObject[] flowers;
	private bool started=false;
	private bool death=false;
	private bool drawScreen=true;
	// Use this for initialization
	void Start(){
		Time.timeScale=0.00001f;
		anim = transform.root.gameObject.GetComponent<Animator> ();
		death = false;

		
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

		}
		if (other.tag.Equals ("Obstacle")) {
			anim.Play("Death");
			this.gameObject.GetComponent<CircleCollider2D>().enabled=false;
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

		if(death && Input.GetMouseButtonDown(0)){
			Die();
		}

		if(bonusCounter==0){
			this.gameObject.transform.Find ("BonusEffect").gameObject.SetActive (false);
			
		}
		
		// Die by being off screen
		Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
		if (screenPosition.y < -100f)
		{
			death=true;
			Time.timeScale=0.00001f;
			if(drawScreen){
			deathScreen();
			}
		}
	}

void Die()
{
	Application.LoadLevel(Application.loadedLevel);
}

	void OnGUI(){
		if (!death) {
			GUI.Box (new Rect (10, 10, 100, 90), pointCounter.ToString ());
		}
		if(!started){
		GUI.Label (new Rect (Screen.width/2-45,Screen.height/2,90,90), "Click to start");
		}		

	}

	private void firstClick(){
		Time.timeScale=1f;
		
	}

	private void deathScreen(){
		drawScreen = false;
		Vector3 pos = new Vector3(0, 0, 0);
		deathCanvas.transform.Find ("Text").gameObject.GetComponent<UnityEngine.UI.Text>().text="Score: "+pointCounter.ToString();
		Instantiate (deathCanvas, pos, Quaternion.identity);
	

	}

}


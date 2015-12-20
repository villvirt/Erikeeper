using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
	private Animator anim;
	private int pointCounter=0;
	private int bonusCounter=0;
	public float bouncePower=15;
	public Font customFont;
	public GameObject deathCanvas;
	public Text scoreText;
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

			pointCounter++;
			scoreText.text=pointCounter.ToString();

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
			death=true;
			Time.timeScale=0.00001f;
			if(drawScreen){
			deathScreen();
			}
		}
	}

	void OnGUI(){
		GUIStyle myStyle = new GUIStyle();
		myStyle.font = customFont;
		myStyle.fontSize=36;
		myStyle.normal.textColor=Color.yellow;
		if(!started){
			GUI.Label (new Rect (Screen.width/2-45,Screen.height/2,90,90), "Click to start", myStyle);
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
		scoreText.text="";

	}
	public string getScore(){
		return pointCounter.ToString();
	}

}


using UnityEngine;

public class Player : MonoBehaviour
{
	public float jumpPower=5;
	void Update ()
	{
		if(Input.GetMouseButtonDown(0)){
			GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);
		}
		
		// Die by being off screen
		Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
		if (screenPosition.y > Screen.height || screenPosition.y < 0)
		{
			Die();
		}
	}

void Die()
{
	Application.LoadLevel(Application.loadedLevel);
		Debug.Log("kuolema");
}
}
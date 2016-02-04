using UnityEngine;
using System.Collections;

public class LeaderboardsMenu : MonoBehaviour {
	public GameObject mainMenu;
	public GameObject leaderBoardsMenu;
	// Use this for initialization
	public void getLeaderBoardsMenu(){
		mainMenu.SetActive(false);
		leaderBoardsMenu.SetActive(true);
	}

	public void getMainMenu(){
		mainMenu.SetActive(true);
		leaderBoardsMenu.SetActive(false);
	}	
}

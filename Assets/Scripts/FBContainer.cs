﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
using Facebook.MiniJSON;
public class FBContainer : MonoBehaviour {
	// Include Facebook namespace
	// Awake function from Unity's MonoBehavior
	void Awake ()
	{
		if (!FB.IsInitialized) {
			// Initialize the Facebook SDK
			FB.Init(InitCallback, OnHideUnity);
		} else {
			// Already initialized, signal an app activation App Event
			FB.ActivateApp();
		}
	}

	private void InitCallback ()
	{
		if (FB.IsInitialized) {
			// Signal an app activation App Event
			FB.ActivateApp();
			FBlogin();
		} else {
			Debug.Log("Failed to Initialize the Facebook SDK");
		}
	}

	private void FBlogin(){
		//wanted permissions
		var perms = new List<string>(){"public_profile","email","user_friends, publish_actions"};
		if(FB.IsLoggedIn){
			Debug.Log("FB logged in");
		}
		else{
			FB.LogInWithReadPermissions(perms, AuthCallback);
		}
	}



	private void OnHideUnity (bool isGameShown)
	{
		if (!isGameShown) {
			// Pause the game - we will need to hide
			Time.timeScale = 0;
		} else {
			// Resume the game - we're getting focus again
			Time.timeScale = 1;
		}
	}

	private void AuthCallback (ILoginResult result) {
		if (FB.IsLoggedIn) {
			// AccessToken class will have session details
			var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
			// Print current access token's User ID
			Debug.Log(aToken.UserId);
			// Print current access token's granted permissions
			foreach (string perm in aToken.Permissions) {
				Debug.Log(perm);
			}
		} else {
			Debug.Log("User cancelled login");
		}
	} 


	public void QueryScores(){
		FB.API("/app/scores?fields=score,user.limit(5)",HttpMethod.GET,ScoresCallback);
	}

	private void ScoresCallback(IGraphResult result){
		Debug.Log("Scores callback:" + result.RawResult);
		var dict = Json.Deserialize(result.RawResult) as Dictionary<string,object>;
		List<object> scoresList = dict["data"] as List<object>;
		foreach(object score in scoresList){
			var entry = (Dictionary<string,object>) score;
			var user = (Dictionary<string,object>)entry["user"];
			Debug.Log(user["name"]+" - "+entry["score"]);

		}
	}
	public void PostScore(){
		var scoreData = new Dictionary<string,string>();
		scoreData["score"] = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().getScore();
		Debug.Log(GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().getScore());
		Debug.Log(scoreData["score"].ToString());
			FB.API("/me/scores", HttpMethod.POST, delegate(IGraphResult result){
			Debug.Log("Score submit result:" +result.ToString());
		}, scoreData);
	}

}



﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
using Facebook.MiniJSON;
using System;
using com.shephertz.app42.paas.sdk.csharp;    
using com.shephertz.app42.paas.sdk.csharp.game;
public class FBContainer : MonoBehaviour {
	public static string playerName;
	public static List<string> UserFBFriends;
	public static ScoreBoardService scoreBoardService;
	public static double highScore=-2;
	private String gameName="Erikeeper";
	void Awake ()
	{
		initializeApp42();
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
		var perms = new List<string>(){"public_profile", "user_friends"};
		if(FB.IsLoggedIn){
			Debug.Log("FB logged in");

			getUserName();

		}
		else{
			FB.LogInWithReadPermissions(perms, AuthCallback);
		}
	}

	private void getUserName(){
		FB.API("me?fields=name", HttpMethod.GET, NameCallBack);
	}

	private void getUserFriends(){
		FB.API("me/friends",  HttpMethod.GET, FriendsCallBack);
	}
		

	private void initializeApp42(){
		App42Log.SetDebug(true);        //Print output in your editor console  
		App42API.Initialize("29c9339d400c12ada360fcb5dd06663303e560a350f5d28d68ef5342cf34a9e4","3669352d0d3512d7c0467d730ae64684e10a3572b32a951e0d08d45b715af92a");  
		scoreBoardService = App42API.BuildScoreBoardService();
	}

	void NameCallBack(IGraphResult result)
	{
		//Debug.Log("nameCallback"+playerName);
		//Debug.Log(result.RawResult);
		//Debug.Log(result.RawResult);
		IDictionary dict = Facebook.MiniJSON.Json.Deserialize(result.RawResult) as IDictionary;
		string fbname = dict["name"].ToString();
		string fbid = dict["id"].ToString();
		playerName=fbname+"("+fbid+")";
		getHighScore();
		getUserFriends();
	}
	void FriendsCallBack(IGraphResult result)
	{
		var dict = Facebook.MiniJSON.Json.Deserialize(result.RawResult) as Dictionary<string,object>;
		var friendList = new List<object>();
		friendList = (List<object>)(dict["data"]);

		int _friendCount = friendList.Count;
		Debug.Log("Found friends on FB, _friendCount ... " +_friendCount);
		List<string> friends = new List<string>();
		for (int i=0; i<_friendCount; i++) {
			string friendFBID = getDataValueForKey( (Dictionary<string,object>)(friendList[i]), "id");
			string friendName =    getDataValueForKey( (Dictionary<string,object>)(friendList[i]), "name");
			friends.Add(friendName+"("+friendFBID+")");
		}
		friends.Add(playerName);
		UserFBFriends=friends;
	}
	private string getDataValueForKey(Dictionary<string, object> dict, string key) {
		object objectForKey;
		if (dict.TryGetValue(key, out objectForKey)) {
			return (string)objectForKey;
		} else {
			return "";
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
			getUserName();
			var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
			// Print current access token's User ID
			Debug.Log(aToken.UserId);
			// Print current access token's granted permissions
			foreach (string perm in aToken.Permissions) {
				Debug.Log(perm);
			}
			Debug.Log("authCallback "+playerName);
		} else {
			Debug.Log("User cancelled login");
		}
	} 

	private void getHighScore(){
		FBContainer.scoreBoardService.GetHighestScoreByUser(gameName, playerName, new GetHighScoreCallBack());   
	}


	public class GetHighScoreCallBack : App42CallBack  
	{  
		public void OnSuccess(object response)  
		{  
			Game game = (Game) response;       
			App42Log.Console("gameName is " + game.GetName());   
			App42Log.Console("userName is : " + game.GetScoreList()[0].GetUserName());  
			App42Log.Console("score is : " + game.GetScoreList()[0].GetValue());  
			App42Log.Console("scoreId is : " + game.GetScoreList()[0].GetScoreId());
			highScore=game.GetScoreList()[0].GetValue();
		}  

		public void OnException(Exception e)  
		{  
			App42Log.Console("Exception : " + e);
			highScore=-1;
		}  
	}  

}



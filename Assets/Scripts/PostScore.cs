using UnityEngine;
using System.Collections;
using System;
using com.shephertz.app42.paas.sdk.csharp;    
using com.shephertz.app42.paas.sdk.csharp.game;
using Facebook.Unity;
using Facebook.MiniJSON;

public class PostScore : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		App42Log.SetDebug(true);        //Print output in your editor console  
		App42API.Initialize("bfb5429188ed021ef0fbe16dc84001cfb7a1592d82ad7c54e1f3173f4332b782","8f5999388c9fc4bba5a72d3488a38871e05adb1b207b31f77e91c267cb41ecb3");  
		//getUserName();
		//Debug.Log("start "+userName);
	}
		
	public void postScore(){
		Debug.Log(FBContainer.playerName);
		String gameName = "Erikeeper";  
		double gameScore = getScore();
		ScoreBoardService scoreBoardService = App42API.BuildScoreBoardService();   
		scoreBoardService.SaveUserScore(gameName,FBContainer.playerName, gameScore, new UnityCallBack()); 
	}

	private double getScore(){
		return GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().getScore();
	}
	/*
	private void getUserName(){
		FB.API("me?fields=name", HttpMethod.GET, NameCallBack);
	}

	void NameCallBack(IGraphResult result)
	{
		Debug.Log("nameCallback"+userName);
		Debug.Log(result.RawResult);
		Debug.Log(result.RawResult);
		IDictionary dict = Facebook.MiniJSON.Json.Deserialize(result.RawResult) as IDictionary;
		String fbname = dict["name"].ToString();
		userName="asdasd";
		Debug.Log("namecallback "+userName);

	}*/

	public class UnityCallBack : App42CallBack  
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

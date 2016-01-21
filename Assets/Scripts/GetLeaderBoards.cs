using UnityEngine;
using System.Collections;
using com.shephertz.app42.paas.sdk.csharp;    
using com.shephertz.app42.paas.sdk.csharp.game;
using System;

public class GetLeaderBoards : MonoBehaviour {
	private String gameName="Erikeeper";
	private int max = 100;
	private void getUserRanking(){
		FBContainer.scoreBoardService.GetUserRanking(gameName, FBContainer.playerName, new UnityCallBack());
	}

	private void getLeaderBoards(){
		FBContainer.scoreBoardService.GetTopNRankings(gameName,max, new UnityCallBack()); 
	}

	  
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

	public class GetPlayerRankingCallBack : App42CallBack  
	{  
		public void OnSuccess(object response)  
		{  
			Game game = (Game) response;       
			App42Log.Console("gameName is " + game.GetName());   
			for(int i = 0;i<game.GetScoreList().Count;i++)  
			{  
				App42Log.Console("userName is : " + game.GetScoreList()[i].GetUserName());  
				App42Log.Console("rank is : " + game.GetScoreList()[i].GetRank());  
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

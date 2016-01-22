using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.shephertz.app42.paas.sdk.csharp;    
using com.shephertz.app42.paas.sdk.csharp.game;
using System;
using UnityEngine.UI;

public class GetLeaderBoards : MonoBehaviour {
	private String gameName="Erikeeper";
	private int max = 100;
	public GameObject Ranks;
	public GameObject Names;
	public GameObject Scores;
	public GameObject OwnRank;
	public GameObject OwnName;
	public GameObject OwnScore;
	public Button Up;
	public Button Down;
	public Button Global;
	public Button Friends;
	private static IList<Game.Score> GlobalScoreList;
	private static IList<Game.Score> OwnScoreList;
	private static IList<Game.Score> ActiveScoreList;
	private static IList<Game.Score> FriendsScoreList;
	private static bool userDone=false;
	private static bool friendsDone=false;
	private static bool othersDone=false;
	public int currentIndex=0;
	private static int rankInFriends;

	private IEnumerator getLeaderBoardsCorout(){
		while(FBContainer.playerName==null){
			yield return null;
		}
		FBContainer.scoreBoardService.GetTopNRankings(gameName,max, new LeaderboardsCallBack());
		FBContainer.scoreBoardService.GetUserRanking(gameName, FBContainer.playerName, new GetPlayerRankingCallBack());
		while(FBContainer.UserFBFriends==null){
			yield return null;
		}
		FBContainer.scoreBoardService.GetTopNRankersByGroup(gameName,FBContainer.UserFBFriends,new FriendsLeaderboardCallBack());
		while(!userDone || !othersDone){
			yield return null;
		}
		ActiveScoreList=GlobalScoreList;
		setText(0);
		setOwnText();
		Up.interactable=true;
		Down.interactable=true;
		Global.interactable=true;
		Friends.interactable=true;


	}

	public void getLeaderBoards(){
		StartCoroutine(getLeaderBoardsCorout());
	}

	public void setFriendsActive(){
		ActiveScoreList=FriendsScoreList;
		setText(0);
		setOwnRankForFriends();
		currentIndex=0;
	}

	public void setGlobalActive(){
		ActiveScoreList=GlobalScoreList;
		setText(0);
		setOwnText();
		currentIndex=0;
		}

	private void setOwnRankForFriends(){
		if(rankInFriends!=0){
		OwnRank.GetComponent<UnityEngine.UI.Text>().text=rankInFriends+".";
		}
	}
		

	public void getNext(){
		if(currentIndex<5 && ActiveScoreList.Count>(currentIndex+1)*10){
			setText(currentIndex+1);
			currentIndex=currentIndex+1;
			Debug.Log("Current index"+currentIndex);
		}
		else{
			Debug.Log("Out of bounds");
			Debug.Log("Current index"+currentIndex);
		}
	}
	public void getPrevious(){
		if(currentIndex>0){
			setText(currentIndex-1);
			currentIndex=currentIndex-1;
			Debug.Log("Current index"+currentIndex);
		}
		else{
			Debug.Log("Out of bounds");
			Debug.Log("Current index"+currentIndex);
		}
	}

	private void setText(int index){
		String names="";
		String ranks="";
		String scores="";
		if(ActiveScoreList!=null){
			for(int i=index*10;i<ActiveScoreList.Count;i++){
				if(i==index*10+10){
					break;
				}
			ranks=ranks + (i+1)+".\n";
			names=names + "" + ActiveScoreList[i].GetUserName().Split('(')[0]+"\n";
			scores=scores + "" + ActiveScoreList[i].GetValue()+"\n";
		}
		Ranks.GetComponent<UnityEngine.UI.Text>().text=ranks;
		Names.GetComponent<UnityEngine.UI.Text>().text=names;
		Scores.GetComponent<UnityEngine.UI.Text>().text=scores;
		}
		else{
			Names.GetComponent<UnityEngine.UI.Text>().text="Ei tuloksia";
		}
	}
		
	  
	private void setOwnText(){
		if(OwnScoreList!=null){
			OwnRank.GetComponent<UnityEngine.UI.Text>().text=OwnScoreList[0].GetRank()+".";
			OwnName.GetComponent<UnityEngine.UI.Text>().text=OwnScoreList[0].GetUserName().Split('(')[0];
			OwnScore.GetComponent<UnityEngine.UI.Text>().text=OwnScoreList[0].GetValue().ToString();
		}
	}

	public class LeaderboardsCallBack : App42CallBack  
	{  
		
		public void OnSuccess(object response)  
		{  
			Game game = (Game) response;
			GlobalScoreList=game.GetScoreList();
			App42Log.Console("gameName is " + game.GetName());   
			for(int i = 0;i<game.GetScoreList().Count;i++)  
			{  	
				
				App42Log.Console("userName is : " + game.GetScoreList()[i].GetUserName());  
				App42Log.Console("score is : " + game.GetScoreList()[i].GetValue());  
				App42Log.Console("scoreId is : " + game.GetScoreList()[i].GetScoreId());
				othersDone=true;
			}  
		}  

		public void OnException(Exception e)  
		{  
			App42Log.Console("Exception : " + e);
			othersDone=true;
		}  
	}

	public class FriendsLeaderboardCallBack : App42CallBack  
	{  

		public void OnSuccess(object response)  
		{  
			Game game = (Game) response;
			FriendsScoreList=game.GetScoreList();
			App42Log.Console("gameName is " + game.GetName());   
			for(int i = 0;i<game.GetScoreList().Count;i++)  
			{  	
				if(game.GetScoreList()[i].GetUserName().Equals(FBContainer.playerName)){
					rankInFriends=i+1;
				}
				App42Log.Console("userName is : " + game.GetScoreList()[i].GetUserName());  
				App42Log.Console("score is : " + game.GetScoreList()[i].GetValue());  
				App42Log.Console("scoreId is : " + game.GetScoreList()[i].GetScoreId());
				friendsDone=true;
			}  
		}  

		public void OnException(Exception e)  
		{  
			App42Log.Console("Exception : " + e);
			friendsDone=true;
		}  
	}  

	public class GetPlayerRankingCallBack : App42CallBack  
	{  
		public void OnSuccess(object response)  
		{  
			Game game = (Game) response; 
			OwnScoreList = game.GetScoreList();
			App42Log.Console("gameName is " + game.GetName());   
			for(int i = 0;i<game.GetScoreList().Count;i++)  
			{  
				App42Log.Console("userName is : " + game.GetScoreList()[i].GetUserName());
				App42Log.Console("rank is : " + game.GetScoreList()[i].GetRank());  
				App42Log.Console("score is : " + game.GetScoreList()[i].GetValue());  
				App42Log.Console("scoreId is : " + game.GetScoreList()[i].GetScoreId());
				userDone=true;
			}  
		}  

		public void OnException(Exception e)  
		{  
			App42Log.Console("Exception : " + e);  
			userDone=true;
		}  
	} 

}

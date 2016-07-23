using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.IO;

public class Constants : MonoBehaviour {

	public static Vector3[] towercardsPos = new [] { new Vector3(0f,0f,0f), new Vector3(-56f,-58f,0f), new Vector3(56f,-58f,0f), new Vector3(-112f,-116f,0f) ,new Vector3(0f,-116f,0f), new Vector3(112f,-116f,0f) , new Vector3(-168f,-174f,0f), new Vector3(-56f,-174,1f), new Vector3(56f,-174,0f), new Vector3(168f,-174,1f)};
//	public static Vector3[] towercardsPos = new [] { new Vector3(0f,0f,0f), new Vector3(-43f,-50f,1f), new Vector3(43f,-50f,0f), new Vector3(-86f,-100f,1f) ,new Vector3(0f,-100f,0f), new Vector3(86f,-100f,1f) , new Vector3(-129f,-150f,0f), new Vector3(-43f,-150,1f), new Vector3(43f,-150,0f), new Vector3(129f,-150,1f)};
	public static int[] deckCardsRemaining = new int[]{1,3,7,11,16,22,29,37,46,56,67,79,92,106,121,137,154,172,191,211};
	public static List<GameObject> towerCardslist = new List<GameObject> ();
	public static List<GameObject> deckCardslist = new List<GameObject> ();
	public static List<GameObject> blankCardslist = new List<GameObject> ();
	public static List<GameObject> stackCards1list = new List<GameObject> ();
	public static List<GameObject> stackCards2list = new List<GameObject> ();
	public static List<GameObject> tower1Cardslist = new List<GameObject> ();
	public static List<GameObject> tower2Cardslist = new List<GameObject> ();
	public static List<GameObject> tower3Cardslist = new List<GameObject> ();
	public static List<GameObject> stackCardslist = new List<GameObject> ();
	public static int noOfdeckCards = 22;
	public static bool setdeckCard = false;
	public static bool set2StackCard = false;
	public static int TimerTime = 90;
	public static bool isGamePlay = false;
	public static int HeartCount = 0; 
	public static string hostName = "http://104.238.101.127/spacetowers/index.php/";
	public static int NoOfTowerIsCompleted = 0;
	public static int currTimeCompleted = 0;

	public static int callCounter = 0;

	//User details variables.....
	public static string user_emailId;
	public static string user_name;
	public static string user_bestscore;
	public static string user_credits;
	public static string user_rank;
	public static string user_country;
	public static string user_city;
	public static string user_reg_no;
	public static int virtulScore = 0;
	public static int virtulRound = 0;
	public static bool gameTypeFreeMode = false;
	public static bool gameTypeTournament = false;
	public static Sprite[] mov1;
	public static Sprite[] mov2;

	// prefab keys......
	public static string PlayerScore = "PlayerScore";
	public static string PlayerRound = "PlayerRound";
	public static string PlayerScoreFreeMode = "PlayerScoreFreeMode";
	public static string PlayerRoundFreeMode = "PlayerRoundFreeMode";
	public static string PlayerRank = "PlayerRank";
	public static string PlayerHighScore = "PlayerHighScore";
	public static string Music = "Music";
	public static string Sound = "Sound";
	public static string Player_name="Player_name";
	public static string Player_reg_id="Player_reg_id";
	public static string Player_credits="Player_credits";
	public static string Player_email="Player_email";
	public static string PlayerLogin="PlayerLogin";
	public static string Player_City="Player_City";
	public static string Player_Country="Player_Country";

	public static string FreePlayMode="FreePlayMode";
	public static string TournamentPlayMode="TournamentPlayMode";
	public static string FreeModeGameNum = "FreeModeGameNum";
	public static string FreeModeTimeCompleted = "FreeModeTimeCompleted";

	public static GameObject AddItem(GameObject itemObj,GameObject parentObj,Vector3 position,Vector3 scale)
	{
		GameObject obj = GameObject.Instantiate (itemObj) as GameObject;
		obj.transform.parent = parentObj.transform;
		obj.transform.localPosition =position;
		obj.transform.localScale = scale;
		return obj;
	}

	public static void ClearData()
	{
		towerCardslist.Clear ();
		deckCardslist.Clear ();
		blankCardslist.Clear ();
		stackCards1list.Clear ();
		stackCards2list.Clear ();
		tower1Cardslist.Clear ();
		tower2Cardslist.Clear ();
		tower3Cardslist.Clear ();
		stackCardslist.Clear ();
		setdeckCard = false;
		set2StackCard = false;
		TimerTime = 90;
		HeartCount = 0; 
		NoOfTowerIsCompleted = 0;
	}

	public static bool isInternetConnected(){

		string HtmlText = GetHtmlFromUri("http://google.com");
		if(HtmlText == "")
		{
			//No connection
			return false;
		}
		else if(!HtmlText.Contains("schema.org/WebPage"))
		{
			//Redirecting since the beginning of googles html contains that 
			//phrase and it was not found

			return false;
		}
		else
		{
			//success

			return true;
		}

	}


	public static string GetHtmlFromUri(string resource)
	{
		string html = string.Empty;
		HttpWebRequest req = (HttpWebRequest)WebRequest.Create(resource);
		try
		{
			using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
			{
				bool isSuccess = (int)resp.StatusCode < 299 && (int)resp.StatusCode >= 200;
				if (isSuccess)
				{
					using (StreamReader reader = new StreamReader(resp.GetResponseStream()))
					{
						//We are limiting the array to 80 so we don't have
						//to parse the entire html document feel free to 
						//adjust (probably stay under 300)
						char[] cs = new char[80];
						reader.Read(cs, 0, cs.Length);
						foreach(char ch in cs)
						{
							html +=ch;
						}
					}
				}
			}
		}
		catch
		{
			return "";
		}
		return html;
	}
}

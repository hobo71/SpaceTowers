using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using LitJson;
using Completed;
using System;

public class RoundCompletedPop : MonoBehaviour {

	public GameObject playerPointLabel;
	public GameObject playercurrentroundLabel;
	public GameObject playerHighScoreLabel;
	public GameObject playerRankLabel;
	public GameObject transImg;
	private bool lastRound = true;
	public static bool allTowerComp = false;
	private bool scoreIsUpdate = false;
	GameObject video;
	int movieNo;
	// Use this for initialization
	void Start () {
		video = GameObject.Find ("Video");
		movieNo = UnityEngine.Random.Range (1, 2);
		scoreIsUpdate = false;
		allTowerComp = false;
		StartCoroutine (AutoRoundCompleteEvent());
	}
	
	// Update is called once per frame
	void Update () {

		playerPointLabel.GetComponent<UILabel> ().text = Constants.virtulScore.ToString();
		playercurrentroundLabel.GetComponent<UILabel> ().text = Constants.virtulRound.ToString()+"/10";
		playerHighScoreLabel.GetComponent<UILabel> ().text = PlayerPrefs.GetInt (Constants.PlayerHighScore).ToString();
		playerRankLabel.GetComponent<UILabel> ().text = PlayerPrefs.GetInt (Constants.PlayerRank).ToString();

		if (allTowerComp) {
			allTowerComp = false;
//			int timeleft = Constants.currTimeCompleted * (1000 * Constants.virtulRound);
//			Constants.virtulScore = Constants.virtulScore+timeleft;
		}


		if (Constants.virtulRound == 10) {
			if (lastRound) {
				lastRound = false;
//				SoundManager.instance.PlaySingle (SoundManager.instance.movie);
			}
		}
	
	}

	public void RightClick()
	{
		StartCoroutine (RoundCompleteEvent());
	}

	IEnumerator AutoRoundCompleteEvent()
	{
		yield return new WaitForSeconds (3f);
		StartCoroutine (RoundCompleteEvent());
	}

	IEnumerator RoundCompleteEvent()
	{
		Constants.isGamePlay = false;
		if (Constants.virtulRound == 10) {
			if (Constants.virtulScore > PlayerPrefs.GetInt (Constants.PlayerHighScore)) {
				PlayerPrefs.SetInt (Constants.PlayerHighScore, Constants.virtulScore);
				print ("its high score");
				if (Constants.isInternetConnected () || Constants.gameTypeTournament) {
					StartCoroutine (Update_best_Score_Event (Constants.virtulScore));
					while (!scoreIsUpdate)
						yield return null;
					scoreIsUpdate = false;
				}
				print ("completed high score .....");
			}
			if (Constants.gameTypeTournament) {
				StartCoroutine (tournamentRoundCompletedEvent ());
				while (!scoreIsUpdate)
					yield return null;
			}
			Constants.virtulScore = 0;
			Constants.virtulRound = 0;
			if (Constants.gameTypeFreeMode) {
				PlayerPrefs.SetInt (Constants.FreeModeGameNum,PlayerPrefs.GetInt (Constants.FreeModeGameNum)+1);
				PlayerPrefs.SetInt (Constants.PlayerRoundFreeMode, 0);
				PlayerPrefs.SetInt (Constants.PlayerScoreFreeMode, 0);

				if (PlayerPrefs.GetInt (Constants.FreeModeGameNum) == 1) {
					TimeSpan addtime = TimeSpan.Parse ("0.01:00:00");
					DateTime currdate = System.DateTime.Now.Add (addtime);
					PlayerPrefs.SetString (Constants.FreeModeTimeCompleted, currdate.ToString ());
				}

				if (PlayerPrefs.GetInt (Constants.FreeModeGameNum) == 2) {
					PlayerPrefs.SetString (Constants.FreePlayMode, "false");
				}

				SoundManager.instance.PlaySingle (SoundManager.instance.movie);
				video.GetComponentInChildren<BoxCollider> ().enabled = true;
				video.GetComponentInChildren<UI2DSpriteAnimation> ().enabled =true;
//				if(movieNo == 1)
					video.GetComponentInChildren<UI2DSpriteAnimation> ().frames = Constants.mov1;
//				if(movieNo == 2)
//					video.GetComponentInChildren<UI2DSpriteAnimation> ().frames = Constants.mov2;
				print("Complted Round Enter");
//				yield return new WaitForSeconds (1f);
				while (video.GetComponentInChildren<UI2DSpriteAnimation> ().isPlaying) {
					yield return null;
				}
				SoundManager.instance.efxSource.Stop ();
				print("Complted Round End");

				SceneManager.LoadScene ("HighScoreScene");
//				StartCoroutine ( freeModeCompletedEvent());
			}
			if (Constants.gameTypeTournament) {
				PlayerPrefs.SetInt (Constants.PlayerRound, 0);
				PlayerPrefs.SetInt (Constants.PlayerScore, 0);

				print("Complted tournament Round Enter");
				SoundManager.instance.PlaySingle (SoundManager.instance.movie);
				video.GetComponentInChildren<BoxCollider> ().enabled = true;
				video.GetComponentInChildren<UI2DSpriteAnimation> ().enabled =true;
//				if(movieNo == 1)
					video.GetComponentInChildren<UI2DSpriteAnimation> ().frames = Constants.mov1;
//				if(movieNo == 2)
//					video.GetComponentInChildren<UI2DSpriteAnimation> ().frames = Constants.mov2;
				while (video.GetComponentInChildren<UI2DSpriteAnimation> ().isPlaying) {
					yield return null;
				}
				SoundManager.instance.efxSource.Stop ();
				print("Complted tournament Round End");

				SceneManager.LoadScene ("TournamentScoreScene");
			}
			SoundManager.instance.efxSource.Stop ();
		} else {
			if (Constants.gameTypeFreeMode) {
				PlayerPrefs.SetInt (Constants.PlayerScoreFreeMode, Constants.virtulScore);
				PlayerPrefs.SetInt (Constants.PlayerRoundFreeMode, Constants.virtulRound + 1);
			}
			if (Constants.gameTypeTournament) {
				StartCoroutine (tournamentCompletedEvent());
				while (!scoreIsUpdate)
					yield return null;
				PlayerPrefs.SetInt (Constants.PlayerScore, Constants.virtulScore);
				PlayerPrefs.SetInt (Constants.PlayerRound, Constants.virtulRound + 1);
			}
			SoundManager.instance.PlaySingle1 (SoundManager.instance.NR5);
			SceneManager.LoadScene ("NewGameScene");
			Constants.ClearData ();
			print ("Completed");
		}
	}

	IEnumerator Update_best_Score_Event(int bestScore)
	{
		LoadingBar.isVisible = true;
		string ourPostData = UpdateScoreToJson();
		Dictionary<string,string> headers = new Dictionary<string,string>();
		headers.Add("Content-Type", "application/json");
		byte[] pData = System.Text.Encoding.ASCII.GetBytes(ourPostData.ToCharArray());
		WWW www = new WWW(Constants.hostName+"update_best_score", pData, headers);
		print ("Loading.....");
		yield return www;
		print ("Data Loaded ."+www.text);
		JsonData jsonResponse = JsonMapper.ToObject(www.text);
		string isSuccessRes = (string)jsonResponse["isSuccess"];
		LoadingBar.isVisible = false;
		if (isSuccessRes == "true") {
			string responseRes = (string)jsonResponse["response"];
			print ("Sucess Responce : "+responseRes);
		} else {
			string responseRes = (string)jsonResponse["response"];
			print ("Responce : "+responseRes);
		}
		scoreIsUpdate = true;
	}

	IEnumerator freeModeCompletedEvent()
	{
		LoadingBar.isVisible = true;
		string ourPostData = UpdateScoreToJson();
		Dictionary<string,string> headers = new Dictionary<string,string>();
		headers.Add("Content-Type", "application/json");
		byte[] pData = System.Text.Encoding.ASCII.GetBytes(ourPostData.ToCharArray());
		WWW www = new WWW(Constants.hostName+"update_play_freemode", pData, headers);
		print ("Loading.....");
		yield return www;
		print ("Data Loaded ."+www.text);
		JsonData jsonResponse = JsonMapper.ToObject(www.text);
		string isSuccessRes = (string)jsonResponse["isSuccess"];
		LoadingBar.isVisible = false;
		if (isSuccessRes == "true") {
			string timeletfRes = (string)jsonResponse["time_left"]["date_f"];
			FreeModeHighScore.nextGameActivateTime = TimeSpan.Parse (timeletfRes);
			print ("Sucess Responce : "+timeletfRes);
		} else {
			string responseRes = (string)jsonResponse["response"];
			print ("Responce : "+responseRes);
		}
//		scoreIsUpdate = true;
		SceneManager.LoadScene ("HighScoreScene");
	}

	IEnumerator tournamentCompletedEvent()
	{
		print ("tournamentCompletedEvent...............................................");
		LoadingBar.isVisible = true;
		string ourPostData = UpdateScoreToJson();
		Dictionary<string,string> headers = new Dictionary<string,string>();
		headers.Add("Content-Type", "application/json");
		byte[] pData = System.Text.Encoding.ASCII.GetBytes(ourPostData.ToCharArray());
		WWW www = new WWW(Constants.hostName+"set_round_score", pData, headers);
		print ("Loading.....");
		yield return www;
		print ("Data Loaded ."+www.text);
		JsonData jsonResponse = JsonMapper.ToObject(www.text);
		string isSuccessRes = (string)jsonResponse["isSuccess"];
		LoadingBar.isVisible = false;
		if (isSuccessRes == "true") {
			string timeletfRes = (string)jsonResponse ["createdOn"];
			print ("Sucess Responce : "+timeletfRes);
		} else {
			string responseRes = (string)jsonResponse["response"];
			print ("Responce : "+responseRes);
		}
		scoreIsUpdate = true;
	}

	IEnumerator tournamentRoundCompletedEvent()
	{
		LoadingBar.isVisible = true;
		string ourPostData = UpdateScoreToJson();
		Dictionary<string,string> headers = new Dictionary<string,string>();
		headers.Add("Content-Type", "application/json");
		byte[] pData = System.Text.Encoding.ASCII.GetBytes(ourPostData.ToCharArray());
		WWW www = new WWW(Constants.hostName+"clear_round_score", pData, headers);
		print ("Loading.....");
		yield return www;
		print ("Data Loaded ."+www.text);
		JsonData jsonResponse = JsonMapper.ToObject(www.text);
		string isSuccessRes = (string)jsonResponse["isSuccess"];
		LoadingBar.isVisible = false;
		if (isSuccessRes == "true") {
			string timeletfRes = (string)jsonResponse ["createdOn"];
			print ("Sucess Responce : "+timeletfRes);
		} else {
			string responseRes = (string)jsonResponse["response"];
			print ("Responce : "+responseRes);
		}
		scoreIsUpdate = true;

	}


	public  string UpdateScoreToJson() {
		UserDetailValues user = new UserDetailValues();
		user.email  = PlayerPrefs.GetString(Constants.Player_email);
		user.score  = Constants.virtulScore.ToString();
		user.round  = (Constants.virtulRound+1).ToString();
		user.reg_no  = PlayerPrefs.GetInt (Constants.Player_reg_id).ToString();
		user.best_score  = PlayerPrefs.GetInt (Constants.PlayerHighScore).ToString();
		user.update_can_play = "false";
		string json_user = JsonMapper.ToJson(user); 
		Debug.Log(json_user);
		return json_user;
	}

}
	
	
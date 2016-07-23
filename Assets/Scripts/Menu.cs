using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using LitJson;
using System;

public class Menu : MonoBehaviour {

	private TimeSpan freemodetimeleft;
	TimeSpan addtime ;
	DateTime updatedate;
	DateTime currdate;
	TimeSpan diffdate;
	// Use this for initialization
	void Start () {
//		PlayerPrefs.DeleteAll ();
//		Time.timeScale = 1;
		Constants.ClearData ();
		Constants.callCounter++;
//		StartCoroutine (UserDetailEvent());
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey ("escape")) {
			gameObject.GetComponent<MenuResources> ().transImg.SetActive (true);
			gameObject.GetComponent<MenuResources> ().ExitPopUp.SetActive (true);
		}
	}

	public void PlayTournamentClick()
	{
		if (Constants.isInternetConnected ()) {
			SceneManager.LoadScene ("TournamentScoreScene");
		} else {
			ToastScript.showToast ("Network Connection Is Required");
		}
	}
	public void PlayFreeModeClick()
	{
		Constants.gameTypeFreeMode = true;
		Constants.gameTypeTournament = false;
		print (PlayerPrefs.GetString (Constants.FreeModeTimeCompleted));
		if (PlayerPrefs.GetString (Constants.FreeModeTimeCompleted) != "") {
			updatedate = DateTime.Parse (PlayerPrefs.GetString (Constants.FreeModeTimeCompleted));
			currdate = System.DateTime.Now;
			diffdate = updatedate.Subtract(currdate);
			if (diffdate.TotalSeconds <= 0) {
				PlayerPrefs.SetString (Constants.FreePlayMode, "true");
				PlayerPrefs.SetInt (Constants.FreeModeGameNum, 0);
				SceneManager.LoadScene ("NewGameScene");
			} else {
				if (PlayerPrefs.GetString (Constants.FreePlayMode) == "false") {
						SceneManager.LoadScene ("HighScoreScene");
				} else {
					SceneManager.LoadScene ("NewGameScene");
				}
			}
		} else {
			SceneManager.LoadScene ("NewGameScene");
		}
	}
//	public void PlayFreeModeClick()
//	{
//		Constants.gameTypeFreeMode = true;
//		Constants.gameTypeTournament = false;
//		if(PlayerPrefs.GetString (Constants.FreeModeTimeCompleted) != ""){
//			updatedate = DateTime.Parse (PlayerPrefs.GetString (Constants.FreeModeTimeCompleted));
//			currdate = System.DateTime.Now;
//			diffdate = updatedate.Subtract(currdate);
//			if (diffdate.TotalSeconds <= 0) {
//				PlayerPrefs.SetString (Constants.FreeModeTimeCompleted, "");
//				PlayerPrefs.SetString (Constants.FreePlayMode,"true");
//				PlayerPrefs.SetInt (Constants.FreeModeGameNum,0);
//				print ("Time is zero");
//			}
//		}
//		if (PlayerPrefs.GetString (Constants.FreePlayMode) != "true") {
//			if (PlayerPrefs.GetString (Constants.FreePlayMode) == "false") {
//				SceneManager.LoadScene ("HighScoreScene");
//			} else {
//				PlayerPrefs.SetString (Constants.FreePlayMode,"true");
//				SceneManager.LoadScene ("NewGameScene");
//			}
//		}
//		else
//			SceneManager.LoadScene ("NewGameScene");
//	}
	public void HighScoreClick()
	{
//		ToastScript.showToast1 ("Coming Soon...");
	}

	public void BuyCreditClick(GameObject iapDialog,GameObject transImg)
	{
		iapDialog.SetActive (true);
		transImg.SetActive (true);
	}

	public void ExitClick(GameObject ExitDialog,GameObject transImg)
	{
		ExitDialog.SetActive (true);
		transImg.SetActive (true);
	}

	IEnumerator UserDetailEvent()
	{
		LoadingBar.isVisible = true;
		string ourPostData = UserDetailToJson();
		Dictionary<string,string> headers = new Dictionary<string,string>();
		headers.Add("Content-Type", "application/json");
		byte[] pData = System.Text.Encoding.ASCII.GetBytes(ourPostData.ToCharArray());
		WWW www = new WWW(Constants.hostName+"user_detail", pData, headers);
		print ("Loading.....");
		yield return www;
		print ("Data Loaded .");
		JsonData jsonResponse = JsonMapper.ToObject(www.text);
		string isSuccessRes = (string)jsonResponse["isSuccess"];
		LoadingBar.isVisible = false;

		if (isSuccessRes == "true") {

			//			PlayerPrefs.SetString("LoginSuccess","yes");
			//			PlayerPrefs.SetString("LoginUserID",loginUsername.value);
			//			PlayerPrefs.SetString("LoginPass",loginPassword.value);
			Constants.user_reg_no = (string)jsonResponse ["user"] ["reg_no"];
			Constants.user_name = (string)jsonResponse ["user"] ["name"];
			Constants.user_emailId = (string)jsonResponse ["user"] ["email"];
			Constants.user_country = (string)jsonResponse ["user"] ["country"];
			Constants.user_city = (string)jsonResponse ["user"] ["city"];
			Constants.user_bestscore = (string)jsonResponse ["user"] ["best_score"];
			Constants.user_credits = (string)jsonResponse ["user"] ["credits"];
			Constants.user_rank = (string)jsonResponse ["user"] ["rank"];

		} else {
			string responseRes = (string)jsonResponse["response"];
			print ("Responce : "+responseRes);
		}
	}

	IEnumerator freeModeEvent()
	{
		LoadingBar.isVisible = true;
		string ourPostData = UserDetailToJson();
		Dictionary<string,string> headers = new Dictionary<string,string>();
		headers.Add("Content-Type", "application/json");
		byte[] pData = System.Text.Encoding.ASCII.GetBytes(ourPostData.ToCharArray());
		WWW www = new WWW(Constants.hostName+"play_freemode", pData, headers);
		print ("Loading.....");
		yield return www;
		print ("Data Loaded .");
		JsonData jsonResponse = JsonMapper.ToObject(www.text);
		string isSuccessRes = (string)jsonResponse["isSuccess"];
		LoadingBar.isVisible = false;

		if (isSuccessRes == "true") {
			//			PlayerPrefs.SetString("LoginSuccess","yes");
			//			PlayerPrefs.SetString("LoginUserID",loginUsername.value);
			//			PlayerPrefs.SetString("LoginPass",loginPassword.value);
			string can_play = (string)jsonResponse ["can_play"];
			if (can_play == "true") {
				Constants.gameTypeFreeMode = true;
				Constants.gameTypeTournament = false;
				SceneManager.LoadScene ("NewGameScene");
			} else {
				
			}

		} else {
			string responseRes = (string)jsonResponse["response"];
			print ("Responce : "+responseRes);
		}
	}

	public  string UserDetailToJson() {
		UserDetailValues user = new UserDetailValues();
		user.email  = PlayerPrefs.GetString(Constants.Player_email)/*"ankit.abc@ratufa.com"*/;
		user.best_score  = PlayerPrefs.GetInt(Constants.PlayerHighScore).ToString()/*"500"*/;
		string json_user = JsonMapper.ToJson(user); 
		Debug.Log(json_user);
		return json_user;
	}


}
public class UserDetailValues {

	public string   email;
	public string   best_score;
	public string   credits;
	public string   update_can_play;
	public string   score;
	public string   round;
	public string   reg_no;
}
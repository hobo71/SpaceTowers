using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using LitJson;
using System;

public class Tournament : MonoBehaviour {

	public TimeSpan todaytime ;
	public TimeSpan weektime;
	public TimeSpan monthtime;
	private float t= 0;
	private TimeSpan currtimeShow;
	private bool timedIsVisible = false;


	void Awake()
	{
		StartCoroutine (tournamentCompletedEvent());
	}

	void Start()
	{
		StartCoroutine (today_tournament_Detail());
		currtimeShow = todaytime;
		print ("Start");
//		 addtime = TimeSpan.Parse ("1.00:00:00");
//		 updatedate = System.DateTime.Now.Add (addtime);
//		 currdate = DateTime.Parse ("03/09/2016 11:34:00");
//		diffdate = currdate.Subtract(updatedate);
//
//		print ("Current Time : "+ currdate.ToString ());
//		print ("Add 1 day in Current Time : "+updatedate.ToString ());
	}



	// Update is called once per frame
	void Update () {
		
		if (Input.GetKey ("escape")) {
			SceneManager.LoadScene ("MenuScene");
		}
		if (timedIsVisible) {
			t += Time.deltaTime;
			GetComponent<TournamentResources> ().currenttimeLeftLabel.GetComponent<UILabel> ().text = timeCalCulate (currtimeShow);
			GetComponent<TournamentResources> ().dailyLabel.GetComponent<UILabel> ().text = timeCalCulate (todaytime);
			GetComponent<TournamentResources> ().weekLabel.GetComponent<UILabel> ().text = timeCalCulate (weektime);
			GetComponent<TournamentResources> ().monthLabel.GetComponent<UILabel> ().text = timeCalCulate (monthtime);
		}

	}

	string timeCalCulate(TimeSpan ts)
	{
		string ss = null;
		ts=TimeSpan.FromSeconds(ts.TotalSeconds - t);
		int day = ts.Days;
		int hour = ts.Hours;
		int min = ts.Minutes;
		if (day == 0)
			ss = hour + " hours and " + min + " minutes left";
		else
			ss = day+" days "+hour + " hours and " + min + " minutes left";
		
		return ss;
	}


	public void BackToMainMenuClick()
	{
		SceneManager.LoadScene ("MenuScene");
	}

	public void PlayClick()
	{
		Constants.gameTypeFreeMode = false;
		Constants.gameTypeTournament = true;
		StartCoroutine (tournamentPlayCheckEvent());

	}

	public void BuyCreditClick(GameObject Dialog,GameObject transImg)
	{
		Dialog.SetActive (true);
		transImg.SetActive (true);
	}

	public void dailyClick()
	{
		StartCoroutine (today_tournament_Detail());
		currtimeShow = /*todaytime*/TimeSpan.Parse("0.12:15:10");
	}

	public void weekClick()
	{
		StartCoroutine (week_tournament_Detail());
		currtimeShow = /*weektime*/TimeSpan.Parse("5.18:15:10");
	}

	public void monthClick()
	{
		StartCoroutine (month_tournament_Detail());
		currtimeShow = /*monthtime*/TimeSpan.Parse("25.15:15:10");
	}

	void OnApplicationPause(bool pauseStatus) {

		if (!pauseStatus) {
			StartCoroutine (time_retmaining_Detail());
			print ("OnApplicationPause: " + pauseStatus);

		}
	}

	IEnumerator tournamentCompletedEvent()
	{
		LoadingBar.isVisible = true;
		string ourPostData = UserDetailToJson();
		Dictionary<string,string> headers = new Dictionary<string,string>();
		headers.Add("Content-Type", "application/json");
		byte[] pData = System.Text.Encoding.ASCII.GetBytes(ourPostData.ToCharArray());
		WWW www = new WWW(Constants.hostName+"get_round_score", pData, headers);
		print ("Loading.....");
		yield return www;
		print ("Data Loaded ."+www.text);
		JsonData jsonResponse = JsonMapper.ToObject(www.text);
		string isSuccessRes = (string)jsonResponse["isSuccess"];

		LoadingBar.isVisible = false;
		if (isSuccessRes == "true") {
//			string timeletfRes = (string)jsonResponse ["tournament_round_scores"]["create_date"];
			print ("Sucess Responce : "+(string)jsonResponse ["tournament_round_scores"][0]["score"]);
			PlayerPrefs.SetInt (Constants.PlayerScore, int.Parse((string)jsonResponse ["tournament_round_scores"][0]["score"]));
			PlayerPrefs.SetInt (Constants.PlayerRound, int.Parse((string)jsonResponse ["tournament_round_scores"][0]["round"]));
		} else {
			string responseRes = (string)jsonResponse["response"];
			print ("Responce : "+responseRes);
		}
	}

	IEnumerator tournamentPlayCheckEvent()
	{
		LoadingBar.isVisible = true;
		string ourPostData = UserDetailToJson();
		Dictionary<string,string> headers = new Dictionary<string,string>();
		headers.Add("Content-Type", "application/json");
		byte[] pData = System.Text.Encoding.ASCII.GetBytes(ourPostData.ToCharArray());
		WWW www = new WWW(Constants.hostName+"get_tournament_can_play", pData, headers);
		print ("Loading.....");
		yield return www;
		print ("Data Loaded ."+www.text);
		JsonData jsonResponse = JsonMapper.ToObject(www.text);
		string isSuccessRes = (string)jsonResponse["isSuccess"];
		LoadingBar.isVisible = false;
		if (isSuccessRes == "true") {
			if ((string)jsonResponse ["can_play"] == "true") {
				SceneManager.LoadScene ("NewGameScene");
			} else {
				if (PlayerPrefs.GetInt (Constants.Player_credits) >= 2) {
					StartCoroutine (Update_Credit_Event ());
				} else {
					ToastScript.showToast ("No More Credits ");
				}
			}
		} else {
			string responseRes = (string)jsonResponse["response"];
			print ("Responce : "+responseRes);
		}

	}


	IEnumerator today_tournament_Detail()
	{
		GetComponent<TournamentResources> ().notFoundLabel.SetActive (false);
		string ourPostData = UserDetailToJson();
		Dictionary<string,string> headers = new Dictionary<string,string>();
		headers.Add("Content-Type", "application/json");
		byte[] pData = System.Text.Encoding.ASCII.GetBytes(ourPostData.ToCharArray());
		WWW www = new WWW(Constants.hostName+"todays_top_users", pData, headers);
		print ("Loading.....");
		LoadingBar.isVisible=true;
		yield return www;
		print ("Data Loaded ."+www.text);
		JsonData jsonResponse = JsonMapper.ToObject(www.text);
		string isSuccessRes = (string)jsonResponse["isSuccess"];
		LoadingBar.isVisible=false;
		foreach (Transform child in GetComponent<TournamentResources>().scoreItemparent.transform) {
			GameObject.Destroy(child.gameObject);
		}
		if (isSuccessRes == "true") {

			string time_left_res = (string)jsonResponse ["time_left"] ["str"];
			print ((string)jsonResponse ["top_users"].Count.ToString());

			for (int i = 0; i < jsonResponse ["top_users"].Count; i++) {
				string no = (i+1).ToString ();
				string name = (string)jsonResponse ["top_users"] [i] ["name"];
				string score = (string)jsonResponse ["top_users"][i]["score"];
				DateTime date =DateTime.Parse((string)jsonResponse ["top_users"][i]["score_date"]);
				Show_Score(no,name,score,date);
			}

		} else {
			string responseRes = (string)jsonResponse["response"];
			print ("Responce : "+responseRes);
			GetComponent<TournamentResources> ().notFoundLabel.SetActive (true);

		}
	}

	IEnumerator week_tournament_Detail()
	{
		GetComponent<TournamentResources> ().notFoundLabel.SetActive (false);
		string ourPostData = UserDetailToJson();
		Dictionary<string,string> headers = new Dictionary<string,string>();
		headers.Add("Content-Type", "application/json");
		byte[] pData = System.Text.Encoding.ASCII.GetBytes(ourPostData.ToCharArray());
		WWW www = new WWW(Constants.hostName+"weeks_top_users", pData, headers);
		LoadingBar.isVisible=true;
		print ("Loading.....");
		yield return www;
		print ("Data Loaded ."+www.text);
		JsonData jsonResponse = JsonMapper.ToObject(www.text);
		string isSuccessRes = (string)jsonResponse["isSuccess"];
		LoadingBar.isVisible=false;
		foreach (Transform child in GetComponent<TournamentResources>().scoreItemparent.transform) {
			GameObject.Destroy(child.gameObject);
		}
		if (isSuccessRes == "true") {

			string time_left_res = (string)jsonResponse ["time_left"] ["str"];
			print ((string)jsonResponse ["top_users"].Count.ToString());

			for (int i = 0; i < jsonResponse ["top_users"].Count; i++) {
				string no = (i+1).ToString ();
				string name = (string)jsonResponse ["top_users"] [i] ["name"];
				string score = (string)jsonResponse ["top_users"][i]["score"];
				DateTime date =DateTime.Parse((string)jsonResponse ["top_users"][i]["score_date"]);
				Show_Score(no,name,score,date);
			}

		} else {
			string responseRes = (string)jsonResponse["response"];
			print ("Responce : "+responseRes);
			GetComponent<TournamentResources> ().notFoundLabel.SetActive (true);

		}
	}

	IEnumerator month_tournament_Detail()
	{
		LoadingBar.isVisible=true;
		GetComponent<TournamentResources> ().notFoundLabel.SetActive (false);
		string ourPostData = UserDetailToJson();
		Dictionary<string,string> headers = new Dictionary<string,string>();
		headers.Add("Content-Type", "application/json");
		byte[] pData = System.Text.Encoding.ASCII.GetBytes(ourPostData.ToCharArray());
		WWW www = new WWW(Constants.hostName+"months_top_users", pData, headers);
		print ("Loading.....");
		yield return www;
		print ("Data Loaded ."+www.text);
		JsonData jsonResponse = JsonMapper.ToObject(www.text);
		string isSuccessRes = (string)jsonResponse["isSuccess"];
		LoadingBar.isVisible=false;
		foreach (Transform child in GetComponent<TournamentResources>().scoreItemparent.transform) {
			GameObject.Destroy(child.gameObject);
		}

		if (isSuccessRes == "true") {

			string time_left_res = (string)jsonResponse ["time_left"] ["str"];
			print ((string)jsonResponse ["top_users"].Count.ToString());

			for (int i = 0; i < jsonResponse ["top_users"].Count; i++) {
				string no = (i+1).ToString ();
				string name = (string)jsonResponse ["top_users"] [i] ["name"];
				string score = (string)jsonResponse ["top_users"][i]["score"];
				DateTime date =DateTime.Parse((string)jsonResponse ["top_users"][i]["score_date"]);
				Show_Score(no,name,score,date);
			}

		} else {
			string responseRes = (string)jsonResponse["response"];
			print ("Responce : "+responseRes);
			GetComponent<TournamentResources> ().notFoundLabel.SetActive (true);

		}
	}


	IEnumerator time_retmaining_Detail()
	{

		GetComponent<TournamentResources> ().notFoundLabel.SetActive (false);
		string ourPostData = UserDetailToJson();
		Dictionary<string,string> headers = new Dictionary<string,string>();
		headers.Add("Content-Type", "application/json");
		byte[] pData = System.Text.Encoding.ASCII.GetBytes(ourPostData.ToCharArray());
		WWW www = new WWW(Constants.hostName+"get_remaining_time", pData, headers);
		print ("Loading.....");
		LoadingBar.isVisible=true;
		yield return www;
		print ("Data Loaded ."+www.text);
		JsonData jsonResponse = JsonMapper.ToObject(www.text);
		string isSuccessRes = (string)jsonResponse["isSuccess"];
		t = 0;
		LoadingBar.isVisible=false;
		foreach (Transform child in GetComponent<TournamentResources>().scoreItemparent.transform) {
			GameObject.Destroy(child.gameObject);
		}
		if (isSuccessRes == "true") {
			string time_left_day = (string)jsonResponse ["time_left_in_day"] ["date_f"];
			string time_left_week = (string)jsonResponse ["time_left_in_week"] ["date_f"];
			string time_left_month = (string)jsonResponse ["time_left_in_month"] ["date_f"];
			print (time_left_day + "," + time_left_week + "," + time_left_month);
			todaytime = TimeSpan.Parse(time_left_day);
			weektime  = TimeSpan.Parse (time_left_week);
			monthtime = TimeSpan.Parse (time_left_month);
			print (todaytime.TotalSeconds);

		} else {
			string responseRes = (string)jsonResponse["response"];
			print ("Responce : "+responseRes);
			GetComponent<TournamentResources> ().notFoundLabel.SetActive (true);

		}
		t = 0;
		if (!timedIsVisible) {
			timedIsVisible = true;
		}
		for (int i = 0; i < 3; i++) {
			if (GetComponent<TournamentResources> ().toggles [i].GetComponent<UIToggle> ().value) {
				if(i==0)
					currtimeShow = todaytime;
				if(i==1)
					currtimeShow = weektime;
				if(i==2)
					currtimeShow = monthtime;

			}
		}
	}


	void Show_Score(string itemNo,string name,string score,DateTime date)
	{
		GameObject scoreitem = Constants.AddItem (GetComponent<TournamentResources>().scoreItem,GetComponent<TournamentResources>().scoreItemparent,Vector3.zero,Vector3.one);
		scoreitem.transform.FindChild ("NoLabel").GetComponent<UILabel> ().text = itemNo+".";
		scoreitem.transform.FindChild ("NameLabel").GetComponent<UILabel> ().text = name;
		scoreitem.transform.FindChild ("ScoreLabel").GetComponent<UILabel> ().text = score;
		scoreitem.transform.FindChild ("DateLabel").GetComponent<UILabel> ().text = ((DateTime)date).ToString("dd.MM.yyyy");
		scoreitem.SetActive (true);
		GetComponent<TournamentResources> ().scoreItemparent.GetComponent<UIGrid> ().repositionNow = true;
	}

	public  string UserDetailToJson() {
		UserDetailValues user = new UserDetailValues();
		user.email  = PlayerPrefs.GetString(Constants.Player_email);
		user.reg_no  = PlayerPrefs.GetInt (Constants.Player_reg_id).ToString();
		user.best_score  = PlayerPrefs.GetInt (Constants.PlayerHighScore).ToString();
		user.credits  = (PlayerPrefs.GetInt(Constants.Player_credits)-2).ToString();
		string json_user = JsonMapper.ToJson(user); 
		Debug.Log(json_user);
		return json_user;
	}

	IEnumerator Update_Credit_Event()
	{
		LoadingBar.isVisible = true;
		string ourPostData = UserDetailToJson();
		Dictionary<string,string> headers = new Dictionary<string,string>();
		headers.Add("Content-Type", "application/json");
		byte[] pData = System.Text.Encoding.ASCII.GetBytes(ourPostData.ToCharArray());
		WWW www = new WWW(Constants.hostName+"update_credit", pData, headers);
		print ("Loading.....");
		yield return www;
		print ("Data Loaded ."+www.text);
		JsonData jsonResponse = JsonMapper.ToObject(www.text);
		string isSuccessRes = (string)jsonResponse["isSuccess"];
//		LoadingBar.isVisible = false;
		if (isSuccessRes == "true") {
			string responseRes = (string)jsonResponse["response"];
			print ("Sucess Responce : "+responseRes);
			PlayerPrefs.SetInt (Constants.Player_credits, PlayerPrefs.GetInt (Constants.Player_credits) - 2);

		} else {
			string responseRes = (string)jsonResponse["response"];
			print ("Responce : "+responseRes);
		}
		StartCoroutine (set_can_play_tournament_Event ());
	}

	IEnumerator set_can_play_tournament_Event()
	{
		LoadingBar.isVisible = true;
		string ourPostData = UserDetailToJson();
		Dictionary<string,string> headers = new Dictionary<string,string>();
		headers.Add("Content-Type", "application/json");
		byte[] pData = System.Text.Encoding.ASCII.GetBytes(ourPostData.ToCharArray());
		WWW www = new WWW(Constants.hostName+"set_tournament_can_play", pData, headers);
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
		SceneManager.LoadScene ("NewGameScene");
	}

}

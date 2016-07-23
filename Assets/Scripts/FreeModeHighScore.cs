using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using LitJson;
using System;

public class FreeModeHighScore : MonoBehaviour {

	public static TimeSpan nextGameActivateTime;
	private float t= 0;
	TimeSpan addtime ;
	DateTime updatedate;
	DateTime currdate;
	TimeSpan diffdate;

	void Start()
	{
		if (Constants.isInternetConnected ()) {
			StartCoroutine (local_high_scores_Detail ());
		}
		else {
			Show_Score_first("1",PlayerPrefs.GetString(Constants.Player_name),PlayerPrefs.GetInt (Constants.PlayerHighScore).ToString(),DateTime.Parse("1899-02-19 01:05 PM"));
		}
		OntoggleAvailable ();
	}

	// Update is called once per frame
	void Update () {
		
		if (Input.GetKey ("escape")) {
			SceneManager.LoadScene ("MenuScene");
		}
//		t += Time.deltaTime;
//		if(nextGameActivateTime != null)
		if (PlayerPrefs.GetString (Constants.FreeModeTimeCompleted) != ""){
			updatedate = DateTime.Parse (PlayerPrefs.GetString (Constants.FreeModeTimeCompleted));
		currdate = System.DateTime.Now;
		diffdate = updatedate.Subtract (currdate);
		if (diffdate.TotalSeconds <= 0) {
			PlayerPrefs.SetString (Constants.FreePlayMode, "true");
			GetComponent<FreeModeHighScoreResources> ().timerleftLabel.GetComponent<UILabel> ().text = "00:00:00";
		} else
			GetComponent<FreeModeHighScoreResources> ().timerleftLabel.GetComponent<UILabel> ().text = diffdate.Hours.ToString () + ":" + diffdate.Minutes.ToString () + ":" + diffdate.Seconds.ToString ();
	}
//		GetComponent<FreeModeHighScoreResources> ().timerleftLabel.GetComponent<UILabel> ().text = timeUpdate (nextGameActivateTime);
		
	}
	void OnApplicationPause(bool pauseStatus) {
		print ("Pause");
		OntoggleAvailable ();
	}
	void OntoggleAvailable()
	{
		if (Constants.isInternetConnected ()) {
			GetComponent<FreeModeHighScoreResources> ().toggles [1].GetComponent<BoxCollider> ().enabled = true;
			GetComponent<FreeModeHighScoreResources> ().toggles [2].GetComponent<BoxCollider> ().enabled = true;
			GetComponent<FreeModeHighScoreResources> ().toggles [3].GetComponent<BoxCollider> ().enabled = true;
		} else {
			GetComponent<FreeModeHighScoreResources> ().toggles [1].GetComponent<BoxCollider> ().enabled = false;
			GetComponent<FreeModeHighScoreResources> ().toggles [2].GetComponent<BoxCollider> ().enabled = false;
			GetComponent<FreeModeHighScoreResources> ().toggles [3].GetComponent<BoxCollider> ().enabled = false;
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
	public void MenuClick()
	{
		SceneManager.LoadScene ("MenuScene");
	}

	public void BuyCreditClick(GameObject iapDialog,GameObject transImg)
	{
		iapDialog.SetActive (true);
		transImg.SetActive (true);
	}
	public void tabLocal()
	{
		print ("Local");
		if(Constants.isInternetConnected())
			StartCoroutine (local_high_scores_Detail());
		else
			Show_Score_first("1",PlayerPrefs.GetString(Constants.Player_name),PlayerPrefs.GetInt (Constants.PlayerHighScore).ToString(),DateTime.Parse("1899-02-19 01:05 PM"));
//		StartCoroutine (local_high_scores_Detail());
	}
	public void tabNational()
	{
		print ("National");
		if(Constants.isInternetConnected())
			StartCoroutine (national_high_scores_Detail());
		else
			GetComponent<FreeModeHighScoreResources> ().notFoundLabel.SetActive (true);
//		StartCoroutine (national_high_scores_Detail());
	}
	public void tabGlobal()
	{
		print ("Global");
		if(Constants.isInternetConnected())
			StartCoroutine (global_high_scores_Detail());
		else
			GetComponent<FreeModeHighScoreResources> ().notFoundLabel.SetActive (true);
//		StartCoroutine (global_high_scores_Detail());
	}
	public void tabHOF()
	{
		print ("HOF");
		if(Constants.isInternetConnected())
			StartCoroutine (hall_of_fame_high_scores_Detail());
		else
			GetComponent<FreeModeHighScoreResources> ().notFoundLabel.SetActive (true);
//		StartCoroutine (hall_of_fame_high_scores_Detail());
	}
	string timeUpdate(TimeSpan ts)
	{
		 ts=TimeSpan.FromSeconds(ts.TotalSeconds - t);
		string time = ts.Hours.ToString () + ":" + ts.Minutes.ToString () + ":" + ts.Seconds.ToString ();
		return time;
	}

	IEnumerator local_high_scores_Detail()
	{
		foreach (Transform child in GetComponent<FreeModeHighScoreResources>().ScoreItemParent.transform) {
			GameObject.Destroy(child.gameObject);
		}
		GetComponent<FreeModeHighScoreResources> ().notFoundLabel.SetActive (false);
		GetComponent<FreeModeHighScoreResources> ().ScoreItem.SetActive (false);
		LoadingBar.isVisible=true;
		string ourPostData = UserDetailToJson();
		Dictionary<string,string> headers = new Dictionary<string,string>();
		headers.Add("Content-Type", "application/json");
		byte[] pData = System.Text.Encoding.ASCII.GetBytes(ourPostData.ToCharArray());
		WWW www = new WWW(Constants.hostName+"local_high_scores", pData, headers);
		print ("Loading.....");
		yield return www;
		print ("Data Loaded ."+www.text);
		LoadingBar.isVisible=false;
		JsonData jsonResponse = JsonMapper.ToObject(www.text);
		string isSuccessRes = (string)jsonResponse["isSuccess"];

		foreach (Transform child in GetComponent<FreeModeHighScoreResources>().ScoreItemParent.transform) {
			GameObject.Destroy(child.gameObject);
		}
		if (isSuccessRes == "true") {

			print ((string)jsonResponse ["top_users"].Count.ToString());

			for (int i = 0; i < jsonResponse ["top_users"].Count; i++) {
				string no = (i+1).ToString ();
				string name = (string)jsonResponse ["top_users"] [i] ["name"];
				string score = (string)jsonResponse ["top_users"][i]["best_score"];
				DateTime date =DateTime.Parse((string)jsonResponse ["top_users"][i]["create_date"]);
				if(i==0)
					Show_Score_first(no,name,score,date);
				else
					Show_Score(no,name,score,date);
			}

		} else {
			string responseRes = (string)jsonResponse["response"];
			print ("Responce : "+responseRes);
			GetComponent<FreeModeHighScoreResources> ().notFoundLabel.SetActive (true);

		}
	}

	IEnumerator national_high_scores_Detail()
	{
		LoadingBar.isVisible=true;
		foreach (Transform child in GetComponent<FreeModeHighScoreResources>().ScoreItemParent.transform) {
			GameObject.Destroy(child.gameObject);
		}
		GetComponent<FreeModeHighScoreResources> ().ScoreItem.SetActive (false);
		GetComponent<FreeModeHighScoreResources> ().notFoundLabel.SetActive (false);
		string ourPostData = UserDetailToJson();
		Dictionary<string,string> headers = new Dictionary<string,string>();
		headers.Add("Content-Type", "application/json");
		byte[] pData = System.Text.Encoding.ASCII.GetBytes(ourPostData.ToCharArray());
		WWW www = new WWW(Constants.hostName+"national_high_scores", pData, headers);
		print ("Loading.....");
		yield return www;
		print ("Data Loaded ."+www.text);
		JsonData jsonResponse = JsonMapper.ToObject(www.text);
		string isSuccessRes = (string)jsonResponse["isSuccess"];
		LoadingBar.isVisible=false;
		foreach (Transform child in GetComponent<FreeModeHighScoreResources>().ScoreItemParent.transform) {
			GameObject.Destroy(child.gameObject);
		}
		if (isSuccessRes == "true") {

			print ((string)jsonResponse ["top_users"].Count.ToString());

			for (int i = 0; i < jsonResponse ["top_users"].Count; i++) {
				string no = (i+1).ToString ();
				string name = (string)jsonResponse ["top_users"] [i] ["name"];
				string score = (string)jsonResponse ["top_users"][i]["best_score"];
				DateTime date =DateTime.Parse((string)jsonResponse ["top_users"][i]["create_date"]);
				if(i==0)
					Show_Score_first(no,name,score,date);
				else
					Show_Score(no,name,score,date);
			}

		} else {
			string responseRes = (string)jsonResponse["response"];
			print ("Responce : "+responseRes);
			GetComponent<FreeModeHighScoreResources> ().notFoundLabel.SetActive (true);

		}
	}

	IEnumerator global_high_scores_Detail()
	{
		foreach (Transform child in GetComponent<FreeModeHighScoreResources>().ScoreItemParent.transform) {
			GameObject.Destroy(child.gameObject);
		}
		LoadingBar.isVisible=true;
		GetComponent<FreeModeHighScoreResources> ().ScoreItem.SetActive (false);
		GetComponent<FreeModeHighScoreResources> ().notFoundLabel.SetActive (false);
		string ourPostData = UserDetailToJson();
		Dictionary<string,string> headers = new Dictionary<string,string>();
		headers.Add("Content-Type", "application/json");
		byte[] pData = System.Text.Encoding.ASCII.GetBytes(ourPostData.ToCharArray());
		WWW www = new WWW(Constants.hostName+"global_high_scores", pData, headers);
		print ("Loading.....");
		yield return www;
		print ("Data Loaded ."+www.text);
		JsonData jsonResponse = JsonMapper.ToObject(www.text);
		string isSuccessRes = (string)jsonResponse["isSuccess"];
		LoadingBar.isVisible=false;
		foreach (Transform child in GetComponent<FreeModeHighScoreResources>().ScoreItemParent.transform) {
			GameObject.Destroy(child.gameObject);
		}

		if (isSuccessRes == "true") {

			print ((string)jsonResponse ["top_users"].Count.ToString());

			for (int i = 0; i < jsonResponse ["top_users"].Count; i++) {
				string no = (i+1).ToString ();
				string name = (string)jsonResponse ["top_users"] [i] ["name"];
				string score = (string)jsonResponse ["top_users"][i]["best_score"];
				DateTime date =DateTime.Parse((string)jsonResponse ["top_users"][i]["create_date"]);
				if(i==0)
					Show_Score_first(no,name,score,date);
				else
					Show_Score(no,name,score,date);
			}

		} else {
			string responseRes = (string)jsonResponse["response"];
			print ("Responce : "+responseRes);
			GetComponent<FreeModeHighScoreResources> ().notFoundLabel.SetActive (true);

		}
	}
	IEnumerator hall_of_fame_high_scores_Detail()
	{
		foreach (Transform child in GetComponent<FreeModeHighScoreResources>().ScoreItemParent.transform) {
			GameObject.Destroy(child.gameObject);
		}
		LoadingBar.isVisible=true;
		GetComponent<FreeModeHighScoreResources> ().ScoreItem.SetActive (false);
		GetComponent<FreeModeHighScoreResources> ().notFoundLabel.SetActive (false);
		string ourPostData = UserDetailToJson();
		Dictionary<string,string> headers = new Dictionary<string,string>();
		headers.Add("Content-Type", "application/json");
		byte[] pData = System.Text.Encoding.ASCII.GetBytes(ourPostData.ToCharArray());
		WWW www = new WWW(Constants.hostName+"hall_of_fame_high_scores", pData, headers);
		print ("Loading.....");
		yield return www;
		print ("Data Loaded ."+www.text);
		JsonData jsonResponse = JsonMapper.ToObject(www.text);
		string isSuccessRes = (string)jsonResponse["isSuccess"];
		foreach (Transform child in GetComponent<FreeModeHighScoreResources>().ScoreItemParent.transform) {
			GameObject.Destroy(child.gameObject);
		}
		LoadingBar.isVisible=false;

		if (isSuccessRes == "true") {
			print ((string)jsonResponse ["top_users"].Count.ToString());

			for (int i = 0; i < jsonResponse ["top_users"].Count; i++) {
				string no = (i+1).ToString ();
				string name = (string)jsonResponse ["top_users"] [i] ["name"];
				string score = (string)jsonResponse ["top_users"][i]["best_score"];
				DateTime date =DateTime.Parse((string)jsonResponse ["top_users"][i]["create_date"]);
				if(i==0)
					Show_Score_first(no,name,score,date);
				else
					Show_Score(no,name,score,date);
			}

		} else {
			string responseRes = (string)jsonResponse["response"];
			print ("Responce : "+responseRes);
			GetComponent<FreeModeHighScoreResources> ().notFoundLabel.SetActive (true);

		}
	}

	void Show_Score(string itemNo,string name,string score,DateTime date)
	{
		GameObject scoreitem = Constants.AddItem (GetComponent<FreeModeHighScoreResources>().Scoreitem1,GetComponent<FreeModeHighScoreResources>().ScoreItemParent,Vector3.zero,Vector3.one);
		scoreitem.transform.FindChild ("NoLabel").GetComponent<UILabel> ().text = itemNo+".";
		scoreitem.transform.FindChild ("NameLabel").GetComponent<UILabel> ().text = name;
		scoreitem.transform.FindChild ("ScoreLabel").GetComponent<UILabel> ().text = score;
		scoreitem.SetActive (true);
		GetComponent<FreeModeHighScoreResources> ().ScoreItemParent.GetComponent<UIGrid> ().repositionNow = true;
	}

	void Show_Score_first(string itemNo,string name,string score,DateTime date)
	{
		GameObject scoreitem = GetComponent<FreeModeHighScoreResources>().ScoreItem;
		scoreitem.transform.FindChild ("NoLabel").GetComponent<UILabel> ().text = itemNo+".";
		scoreitem.transform.FindChild ("NameLabel").GetComponent<UILabel> ().text = name;
		scoreitem.transform.FindChild ("ScoreLabel").GetComponent<UILabel> ().text = score;
		if(date != DateTime.Parse("1899-02-19 01:05 PM"))
		scoreitem.transform.FindChild ("DateLabel").GetComponent<UILabel> ().text = ((DateTime)date).ToString("dd-MM-yyyy HH:mm:ss");
		scoreitem.SetActive (true);
		GetComponent<FreeModeHighScoreResources> ().ScoreItemParent.GetComponent<UIGrid> ().repositionNow = true;
	}

	public  string UserDetailToJson() {
		UserRegisterValues user = new UserRegisterValues();
		user.email  = PlayerPrefs.GetString(Constants.Player_email);
		user.country =PlayerPrefs.GetString(Constants.Player_Country);
		user.city  = PlayerPrefs.GetString(Constants.Player_City);
		string json_user = JsonMapper.ToJson(user); 
		Debug.Log(json_user);
		return json_user;
	}
}


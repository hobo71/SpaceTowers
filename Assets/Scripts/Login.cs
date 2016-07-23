using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using LitJson;

public class Login : MonoBehaviour {

	public UIInput email;
	public UIInput password;
	public GameObject transImg;
	public GameObject ExitPopImg;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKey ("escape")) {
			Application.Quit ();
		}
	
	}

	public void loginClick()
	{
		StartCoroutine (LoginEvent());
	}

	public void registerClick()
	{
		SceneManager.LoadScene ("RegisterScene");
	}

	public void ExitClick(GameObject ExitDialog,GameObject transImg)
	{
		ExitDialog.SetActive (true);
		transImg.SetActive (true);
	}

	IEnumerator LoginEvent()
	{
		LoadingBar.isVisible = true;
		string ourPostData = LoginToJson();
		Dictionary<string,string> headers = new Dictionary<string,string>();
		headers.Add("Content-Type", "application/json");
		byte[] pData = System.Text.Encoding.ASCII.GetBytes(ourPostData.ToCharArray());
		WWW www = new WWW(Constants.hostName+"verify_login", pData, headers);
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
			PlayerPrefs.SetString (Constants.Player_name,Constants.user_name);
			PlayerPrefs.SetString (Constants.Player_email,Constants.user_emailId);
			PlayerPrefs.SetInt (Constants.PlayerHighScore,int.Parse(Constants.user_bestscore));
			PlayerPrefs.SetInt (Constants.Player_credits,int.Parse(Constants.user_credits));
			PlayerPrefs.SetInt (Constants.PlayerRank,int.Parse(Constants.user_rank));
			PlayerPrefs.SetInt (Constants.Player_reg_id,int.Parse(Constants.user_reg_no));
			PlayerPrefs.SetString (Constants.PlayerLogin,"true");
			PlayerPrefs.SetString (Constants.Player_City,Constants.user_city);
			PlayerPrefs.SetString (Constants.Player_Country,Constants.user_country);
			SceneManager.LoadScene ("MenuScene");
		} else {
			string responseRes = (string)jsonResponse["response"];
			print ("Responce : "+responseRes);

		}
	}


	public  string LoginToJson() {
		UserLoginValues user = new UserLoginValues();
		user.email  = email.value;
		user.password = password.value;
		string json_user = JsonMapper.ToJson(user); 
		Debug.Log(json_user);
		return json_user;
	}
//
//	public  void ToastVisi(string txt,int sw)
//	{
//		toastobj.GetComponent<UILabel> ().text = txt;
//		toastobj.SetActive (true);
//		StartCoroutine (StarAnimate (sw));
//	}
}

public class UserLoginValues {

	public string   email;
	public string   password;
}

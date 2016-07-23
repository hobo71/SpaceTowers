using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using LitJson;

public class Register : MonoBehaviour {

	public UIInput user_name;
	public UIInput email;
	public UIInput password;
	public UIInput country;
	public UIInput city;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKey ("escape")) {
			SceneManager.LoadScene ("LoginScene");
		}

	}

	public void registerClick()
	{
		StartCoroutine (RegisterEvent());
	}

	public void cancelClick()
	{
		SceneManager.LoadScene ("LoginScene");
	}

	IEnumerator RegisterEvent()
	{
		LoadingBar.isVisible = true;
		string ourPostData = RegisterToJson();
		Dictionary<string,string> headers = new Dictionary<string,string>();
		headers.Add("Content-Type", "application/json");
		byte[] pData = System.Text.Encoding.ASCII.GetBytes(ourPostData.ToCharArray());
		WWW www = new WWW(Constants.hostName+"register", pData, headers);
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

			SceneManager.LoadScene ("LoginScene");
		} else {
			string responseRes = (string)jsonResponse["response"];
			print ("Responce : "+responseRes);
		}
	}


	public  string RegisterToJson() {
		UserRegisterValues user = new UserRegisterValues();
		user.name  = user_name.value;
		user.email = email.value;
		user.password  = password.value;
		user.country = country.value;
		user.city  = city.value;
		string json_user = JsonMapper.ToJson(user); 
		Debug.Log(json_user);
		return json_user;
	}
}

public class UserRegisterValues {

	public string   name;
	public string   email;
	public string   password;
	public string   country;
	public string   city;
}



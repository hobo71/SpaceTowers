using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadingSplash : MonoBehaviour {



	// Use this for initialization
	void Start () {
		Constants.mov1 = Resources.LoadAll<Sprite>("Movie_1/");
//		Constants.mov2 = Resources.LoadAll<Sprite>("Movie_2/");
//		print ("Mov1:"+Constants.mov1.Length.ToString()+",Mov2:"+Constants.mov2.Length.ToString());
		StartCoroutine (LoadSplash());
	
	}

	IEnumerator LoadSplash()
	{
		yield return new WaitForSeconds (.01f);
		if(PlayerPrefs.GetString (Constants.PlayerLogin)=="true")
			SceneManager.LoadScene ("MenuScene");
		else
		   SceneManager.LoadScene ("LoginScene");
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKey ("escape")) {
			Application.Quit ();
		}

	}
}

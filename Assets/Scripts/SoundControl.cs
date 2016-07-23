using UnityEngine;
using System.Collections;
using Completed;

public class SoundControl : MonoBehaviour {

	private bool MusicIsOn ;
	private bool SoundIsOn ;

	public GameObject SoundOnObj;
	public GameObject SoundOffObj;

	// Use this for initialization
	void Start () {



//		if (PlayerPrefs.GetString (Constants.Music) == "true") {
//			MusicOnObj.GetComponent<UIToggle> ().startsActive = true;
//		} else {
//			MusicOffObj.GetComponent<UIToggle> ().startsActive = true;
//		}

		if (PlayerPrefs.GetString (Constants.Sound) == "true") {
			SoundOnObj.GetComponent<UIToggle> ().value = true;
		} else {
			SoundOffObj.GetComponent<UIToggle> ().value = true;
		}

	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

//	public void MusicOnClick(){
//
//		SoundManager.instance.musicSource.Play ();
//		MusicIsOn = true;
//	}
//
//	public void MusicOffClick(){
//
//		SoundManager.instance.musicSource.Stop ();
//		MusicIsOn = false;
//	}

	public void SoundOnClick(){
		if(!SoundManager.instance.musicSource.isPlaying)
		  SoundManager.instance.musicSource.Play ();
		PlayerPrefs.SetString (Constants.Sound, "true");

	}

	public void SoundOffClick(){
		if(SoundManager.instance.musicSource.isPlaying)
			SoundManager.instance.musicSource.Stop ();
		PlayerPrefs.SetString (Constants.Sound, "false");
	}

//	public void SetMusicAndSound(){
//
//		if (MusicIsOn) {
//			PlayerPrefs.SetString (Constants.Music, "true");
//			SoundManager.instance.musicSource.Play ();
//		} else {
//			PlayerPrefs.SetString (Constants.Music, "false");
//			SoundManager.instance.musicSource.Stop ();
//		}
//
//		if (SoundIsOn) {
//			PlayerPrefs.SetString (Constants.Sound, "true");
//			SoundManager.instance.efxSource.Play ();
//		} else {
//			PlayerPrefs.SetString (Constants.Sound, "false");
//			SoundManager.instance.efxSource.Stop ();
//		}
//	}
//
//	public void CancelClick(){
//
//		if (PlayerPrefs.GetString (Constants.Music)== "true") {
//			SoundManager.instance.musicSource.Play ();
//		} else {
//			SoundManager.instance.musicSource.Stop ();
//		}
//		
//		if (PlayerPrefs.GetString (Constants.Sound) ==  "true") {
//			SoundManager.instance.efxSource.Play ();
//		} else {
//			SoundManager.instance.efxSource.Stop ();
//		}
//	}

	public void exitdialog(GameObject Dialog,GameObject transImg)
	{
		Dialog.SetActive (false);
		transImg.SetActive (false);
	}

}

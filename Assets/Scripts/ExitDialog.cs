using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Completed;

public class ExitDialog : MonoBehaviour {

	public string MenuScene = "MenuScene";
	public string HighScoreScene = "HighScoreScene";
	public string TournamentScoreScene = "TournamentScoreScene";
	public string QuitApp = "Quit";

	void OnEnable()
	{
		SoundManager.instance.efxSource.Pause ();
		SoundManager.instance.efxSource1.Pause ();
	}

	public void exitdialog(GameObject Dialog,GameObject transImg)
	{
		Time.timeScale = 1;
		Dialog.SetActive (false);
		transImg.SetActive (false);
		SoundManager.instance.efxSource.UnPause ();
		SoundManager.instance.efxSource1.UnPause ();
	}

	public void rightClick(string switchScene)
	{
		Time.timeScale = 1;
		Constants.isGamePlay = false;
		SoundManager.instance.efxSource.Stop ();
		SoundManager.instance.efxSource1.Stop ();
		if (switchScene != null) {
			SceneManager.LoadScene (switchScene);
		}
		else
			Application.Quit ();
	}
	public void QuitApplication()
	{
		Application.Quit ();
	}
}

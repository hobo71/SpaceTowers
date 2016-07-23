using UnityEngine;
using System.Collections;

public class TournamentResources : MonoBehaviour {

	public GameObject currentScoreLabel;
	public GameObject currenttimeLeftLabel;
	public GameObject personalBestScoreLabel;
	public GameObject scoreItem;
	public GameObject scoreItemparent;
	public GameObject dailyLabel;
	public GameObject weekLabel;
	public GameObject monthLabel;
	public GameObject notFoundLabel;
	public GameObject popUpNoMoreCredits;
	public GameObject[] toggles;

	
	// Update is called once per frame
	void Update () {
		personalBestScoreLabel.GetComponent<UILabel> ().text = PlayerPrefs.GetInt (Constants.PlayerHighScore).ToString();
	}
}

using UnityEngine;
using System.Collections;

public class MenuResources : MonoBehaviour {

	public GameObject userName;
	public GameObject userId;
	public GameObject userEmail;
	public GameObject bestScore;
	public GameObject credits;
	public GameObject hofRank;

	public GameObject transImg;
	public GameObject ExitPopUp;

	void Update()
	{
		userName.GetComponent<UILabel> ().text = PlayerPrefs.GetString (Constants.Player_name);
		userId.GetComponent<UILabel> ().text = "id "+PlayerPrefs.GetInt (Constants.Player_reg_id).ToString();
		userEmail.GetComponent<UILabel> ().text = PlayerPrefs.GetString (Constants.Player_email);
		bestScore.GetComponent<UILabel> ().text = "Best Score : "+PlayerPrefs.GetInt (Constants.PlayerHighScore).ToString();
		credits.GetComponent<UILabel> ().text = "Credits : "+PlayerPrefs.GetInt (Constants.Player_credits).ToString();
		hofRank.GetComponent<UILabel> ().text = "h.o.f. rank : "+PlayerPrefs.GetInt (Constants.PlayerRank).ToString();

//		userName.GetComponent<UILabel> ().text = Constants.user_name;
//		userId.GetComponent<UILabel> ().text = "id "+Constants.user_reg_no;;
//		userEmail.GetComponent<UILabel> ().text = Constants.user_emailId;
//		bestScore.GetComponent<UILabel> ().text = "Best Score : "+Constants.user_bestscore;
//		credits.GetComponent<UILabel> ().text = "Credits : "+Constants.user_credits;
//		hofRank.GetComponent<UILabel> ().text = "h.o.f. rank : "+Constants.user_rank;

//		userName.GetComponent<UILabel> ().text = "Kelvin Sterken";
//		userId.GetComponent<UILabel> ().text = "id "+"0034380";
//		userEmail.GetComponent<UILabel> ().text = "email@gmail.com";
//		bestScore.GetComponent<UILabel> ().text = "Best Score : "+"11.230.330";
//		credits.GetComponent<UILabel> ().text = "Credits : "+"08";
//		hofRank.GetComponent<UILabel> ().text = "h.o.f. rank : "+"77";
	}

}

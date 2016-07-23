using UnityEngine;
using System.Collections;

public class AllResource : MonoBehaviour {

	public GameObject PlayerPointLabel;
	public GameObject PlayerRoundLabel;
	public GameObject PlayerChanceLabel;
	public GameObject PlayerTimer;
	public GameObject DeckCardsLabel;
	public GameObject RoundCompletedPopUp;
	private float timer=0;

	void Update()
	{
		if (CardsControl.cardsIsdistributed) {
			DeckCardsLabel.SetActive (true);
			timer += Time.deltaTime;

			if (timer <= (Constants.TimerTime-(Constants.virtulRound*5))) {
				PlayerTimer.GetComponent<UISlider> ().value = 1-(timer / ( Constants.TimerTime-(Constants.virtulRound*5)));
//				print ((1-(timer / Constants.TimerTime)).ToString ());
				Constants.currTimeCompleted = (int)((Constants.TimerTime-(Constants.virtulRound*5))-timer);
//				print (Constants.currTimeCompleted);
			} else {
				CardsControl.deckCardisfinsh = true;
				CardsControl.deckCardIsClick = true;
			}
		}

		PlayerPointLabel.GetComponent<UILabel> ().text = Constants.virtulScore.ToString();
		PlayerRoundLabel.GetComponent<UILabel> ().text = Constants.virtulRound.ToString()+"/10";
		PlayerChanceLabel.GetComponent<UILabel> ().text = CardsControl.chance.ToString();

		if (Constants.deckCardslist.Count != 0)
			DeckCardsLabel.GetComponent<UILabel> ().text = Constants.deckCardslist.Count.ToString ();
		else
			DeckCardsLabel.SetActive (false);
		
	}

	public void exitdialogShow(GameObject Dialog,GameObject transImg)
	{
		Time.timeScale = 0;
		Dialog.SetActive (true);
		transImg.SetActive (true);
	}

	public void ClearDataForNewGame()
	{
		Constants.ClearData ();
	}
}

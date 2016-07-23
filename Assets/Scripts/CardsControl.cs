using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Completed;

public class CardsControl : MonoBehaviour {

	public GameObject[] towers;
	public GameObject[] towersDiamond;
	public GameObject[] towersAnim;
	public bool[] towerComplete = new bool[3];
	public  List<int> cardlist;
	public GameObject carditem;
	public GameObject deckcarditem;
	public GameObject blankcarditem;
	private static Sprite[] CardFaces = null ;
	public static bool deckCardisfinsh = false;
	private bool checkdeckCardisfinsh = false;
	public static bool cardsIsdistributed = false;
	public static bool deckCardIsClick = false;
	public static int chance = 0;
	public GameObject transImg;
	public GameObject ExitPopUp;
	public bool gameIsCompleted = false;
	private int deckCard = 0;
	public GameObject CoinPrefab;


	void Awake(){
//		PlayerPrefs.DeleteAll ();
		if (Constants.gameTypeFreeMode) {
			if (PlayerPrefs.GetInt (Constants.PlayerRoundFreeMode) == 0)
				PlayerPrefs.SetInt (Constants.PlayerRoundFreeMode, 1);
		}
		if (Constants.gameTypeTournament) {
			if (PlayerPrefs.GetInt (Constants.PlayerRound) == 0)
				PlayerPrefs.SetInt (Constants.PlayerRound, 1);
		}
	}

	void Start () {
		if (Constants.gameTypeFreeMode) {
			Constants.virtulScore = PlayerPrefs.GetInt (Constants.PlayerScoreFreeMode);
			Constants.virtulRound = PlayerPrefs.GetInt (Constants.PlayerRoundFreeMode);
		}
		if (Constants.gameTypeTournament) {
			Constants.virtulScore = PlayerPrefs.GetInt (Constants.PlayerScore);
			Constants.virtulRound = PlayerPrefs.GetInt (Constants.PlayerRound);
		}
		deckCardisfinsh = false;
		CardFaces = null ;
		deckCardisfinsh = false;
		checkdeckCardisfinsh = false;
		cardsIsdistributed = false;
		deckCardIsClick = false;
		chance = 0;
		gameIsCompleted = false;
		Constants.isGamePlay = true;
		Deck deck = new Deck ();
		cardlist = deck.cards;
		CardFaces = Resources.LoadAll<Sprite>("Cards_design_update");
		deckCard = 0;
		if (Constants.virtulRound < 9)
			deckCard = Constants.virtulRound;
		else
			deckCard = 8;
		for (int j = 0; j < Constants.noOfdeckCards-(deckCard-1); j++) {
			GameObject cards = Constants.AddItem (deckcarditem,GameObject.Find("DeckCards"),Vector3.zero/*new Vector3(-180f,-313f,0f)*/,Vector3.one);
			cards.GetComponent<CardDeck> ().cardNo = cardlist [j];
			cards.GetComponent<BoxCollider> ().enabled = false;
			Constants.deckCardslist.Add (cards);
		}
		StartCoroutine (StartcardDistribute());
	}

	void Update () {

		if (Input.GetKey ("escape")) {
			transImg.SetActive (true);
			ExitPopUp.SetActive (true);
//			Application.Quit ();
			Time.timeScale = 0;
		}

		if (Constants.setdeckCard) {
			Constants.setdeckCard = false;
			Constants.deckCardslist [Constants.deckCardslist.Count - 1].GetComponent<BoxCollider> ().enabled = true;
			StartCoroutine(Constants.deckCardslist [Constants.deckCardslist.Count - 1].GetComponent<CardDeck> ().cardShow ());
			SoundManager.instance.PlaySingle (SoundManager.instance.appear1);
		}
		if (!checkdeckCardisfinsh)
			StartCoroutine (DeckFinish());

		if (Constants.set2StackCard) {
			Constants.set2StackCard = false;
			if(Constants.deckCardslist.Count>0)
				StartCoroutine(Constants.deckCardslist [Constants.deckCardslist.Count - 1].GetComponent<CardDeck> ().SecondStackShow ());
			SoundManager.instance.PlaySingle (SoundManager.instance.appear2);
		}
		if(cardsIsdistributed )
			towerCompleted ();

		if (deckCardisfinsh) {
			for (int i = 0; i < Constants.towerCardslist.Count; i++) {		
				if (Constants.towerCardslist [i].GetComponent<CardTower> ().cardVisible) {
					Constants.towerCardslist [i].transform.FindChild("LockSprite").gameObject.SetActive(true);
				}
			}
			if (Constants.stackCards1list.Count != 0) 
				Constants.stackCards1list [Constants.stackCards1list.Count - 1].transform.FindChild("LockSprite").gameObject.SetActive(true);
			if (Constants.stackCards2list.Count != 0) 
				Constants.stackCards2list [Constants.stackCards2list.Count - 1].transform.FindChild("LockSprite").gameObject.SetActive(true);
//			print ("End gameIsCompleted : "+gameIsCompleted);
			if (!gameIsCompleted) {
				print ("Dialog Show ");
				gameIsCompleted = true;
				StartCoroutine (ShowPopUP());
			}
		}

		if (deckCardIsClick) {
			deckCardIsClick = false;
			for (int i = 0; i < Constants.deckCardslist.Count; i++) {		
				Constants.deckCardslist [i].GetComponent<BoxCollider>().enabled = false;
			}
		}
	}

	IEnumerator ShowPopUP()
	{
		yield return new WaitForSeconds (2f);
		transImg.SetActive (true);
		gameObject.GetComponent<AllResource> ().RoundCompletedPopUp.SetActive (true); 
	}

	IEnumerator DeckFinish()
	{
		yield return 0;

		if(Constants.deckCardslist.Count == 0 && !deckCardisfinsh && Constants.isGamePlay)
		{
			checkdeckCardisfinsh = true;
			for (int i = 0; i < Constants.towerCardslist.Count; i++) {		
				if (Constants.towerCardslist [i].GetComponent<CardTower> ().cardVisible) {
					Rank rank = Constants.towerCardslist [i].GetComponent<CardTower> ().rank;
					if (isCardRight (rank)) {
						checkdeckCardisfinsh = false;
						return false;
					}
				} 
			}
			deckCardisfinsh = true;
			cardsIsdistributed = false;
			print ("Not Found....");
		}
	}

	public void towerCompleted()
	{
		if (Constants.tower1Cardslist.Count == 0 && !towerComplete[0]) {
			Constants.NoOfTowerIsCompleted += 1;
			towerComplete[0] = true;
			towersDiamond [0].SetActive (true);
			towersAnim [0].SetActive (true);
			print ("Tower 1 Completed");
			TowerCompletedBonus (towersDiamond [0]);
			SoundManager.instance.PlaySingle (SoundManager.instance.NR4);
		}
		if (Constants.tower2Cardslist.Count == 0 && !towerComplete[1]) {
			Constants.NoOfTowerIsCompleted += 1;
			towerComplete[1] = true;
			towersDiamond [1].SetActive (true);
			towersAnim [1].SetActive (true);
			print ("Tower 2 Completed");
			TowerCompletedBonus (towersDiamond [1]);
			SoundManager.instance.PlaySingle (SoundManager.instance.NR4);
		}
		if (Constants.tower3Cardslist.Count == 0 && !towerComplete[2]) {
			Constants.NoOfTowerIsCompleted += 1;
			towerComplete[2] = true;
			towersDiamond [2].SetActive (true);
			towersAnim [2].SetActive (true);
			print ("Tower 3 Completed");
			TowerCompletedBonus (towersDiamond [2]);
			SoundManager.instance.PlaySingle (SoundManager.instance.NR4);
		}

		if (towerComplete [0] && towerComplete [1] && towerComplete [2]) {
			cardsIsdistributed = false;
			RoundCompletedPop.allTowerComp = true;
			SoundManager.instance.PlaySingle1 (SoundManager.instance.NR5);
			print ("All Tower is completed.");
			deckCardIsClick = true;
			deckCardisfinsh = true;
			int deckCardleft = (3600 + (Constants.virtulRound * 800)) * Constants.deckCardsRemaining[Constants.deckCardslist.Count-1];
			Constants.virtulScore = Constants.virtulScore+deckCardleft;
			print ("Deck Card Left : "+deckCardleft+","+(Constants.deckCardslist.Count-1));
			StartCoroutine (remainingTimeCalculated());
		}
	}

	public void TowerCompletedBonus(GameObject diamondObj)
	{
		int score = 0;
		if(Constants.NoOfTowerIsCompleted == 1)
		 score = 28000 + (Constants.virtulRound * 4000);
		else if(Constants.NoOfTowerIsCompleted == 2)
			score = 28000 + (Constants.virtulRound * 4000);
		else if(Constants.NoOfTowerIsCompleted == 3)
			score = 70000 + (Constants.virtulRound * 10000);
		Constants.virtulScore = Constants.virtulScore+score;
		coinGernate ("Yellow_coin", score,diamondObj);
	}

	public void coinGernate(string coinName,int point,GameObject diamondObj)
	{
		GameObject coin = Constants.AddItem (CoinPrefab,GameObject.Find("AllCoins"),transform.localPosition,Vector3.one);
		coin.GetComponentInChildren<UILabel> ().text = point.ToString();
		coin.GetComponent<UI2DSprite> ().sprite2D = Resources.Load<Sprite> (coinName);
		coin.GetComponent<TweenPosition> ().from = new Vector3(diamondObj.transform.localPosition.x,diamondObj.transform.localPosition.y-200f,diamondObj.transform.localPosition.z);
		coin.GetComponent<TweenPosition> ().to = new Vector3(diamondObj.transform.localPosition.x,diamondObj.transform.localPosition.y+100f,diamondObj.transform.localPosition.z);
		coin.GetComponent<TweenPosition> ().enabled = true;
		coin.GetComponent<TweenAlpha> ().enabled = true;

	}

	IEnumerator remainingTimeCalculated()
	{
		if (SoundManager.instance.efxSource1.isPlaying)
			yield return null;
		SoundManager.instance.PlaySingle1 (SoundManager.instance.NR6);
		int deckCardleft = (3600 + (Constants.virtulRound * 800)) * Constants.deckCardsRemaining[Constants.deckCardslist.Count-1];
//		Constants.virtulScore = Constants.virtulScore+deckCardleft;
//		print ("Deck Card Left : "+deckCardleft+","+(Constants.deckCardslist.Count-1));
		int timeleft = Constants.currTimeCompleted * (1000 * Constants.virtulRound);
//		Constants.virtulScore = Constants.virtulScore+timeleft;
		int totalbounus = timeleft + deckCardleft;
		Constants.virtulScore = Constants.virtulScore+totalbounus;
		yield return new WaitForSeconds (1f);
		coinGernate ("Yellow_coin", totalbounus,towersDiamond [1]);
//		print ("Time Left : "+timeleft+","+Constants.currTimeCompleted);
	}

	IEnumerator StartcardDistribute()
	{
		yield return new WaitForSeconds (1f);
		StartCoroutine (cardDistribute());
	}

	IEnumerator cardDistribute()
	{
		int l = 0;
		int h = 0;
//		SoundManager.instance.efxSource.loop = true;
//		SoundManager.instance.PlaySingle (SoundManager.instance.appear2);
		for(int i =0;i < 4;i++)
		{
			l = l+i;
			for (int j = 0; j < towers.Length; j++) {
				for (int k = 0; k < i + 1; k++) {
					int no = l + k;
					if (i == 3 && j == 1 && (k == 0 || k == 3))
						continue;
					
					GameObject cards = Constants.AddItem (carditem, towers [j], GameObject.Find("DeckCards").transform.localPosition, Vector3.one);
					cards.name = "Crads:" + h;
					cards.SetActive (true);
					GameObject demoBoxColl =  Constants.AddItem (blankcarditem, towers [j], GameObject.Find("DeckCards").transform.localPosition, Vector3.one);
					demoBoxColl.SetActive (false);
					cards.GetComponent<UI2DSprite> ().depth = 4+ i;
					cards.transform.FindChild("LockSprite").gameObject.GetComponent<UISprite> ().depth = 5+ i;
					cards.AddComponent<TweenPosition> ();
					cards.GetComponent<TweenPosition> ().from = GameObject.Find("DeckCards").transform.localPosition;
					cards.GetComponent<TweenPosition> ().duration = .2f;
					cards.GetComponent<CardTower> ().towerNo = j;
					if (j == 0) {
						Constants.tower1Cardslist.Add (cards);
						cards.GetComponent<TweenPosition> ().to = Constants.towercardsPos [no] + new Vector3 (-335f, 140f, 0f);
					}
					if (j == 1) {
						Constants.tower2Cardslist.Add (cards);
						cards.GetComponent<TweenPosition> ().to = Constants.towercardsPos [no] + new Vector3 (0f, 140f, 0f);
					}
					if (j == 2) {
						Constants.tower3Cardslist.Add (cards);
						cards.GetComponent<TweenPosition> ().to = Constants.towercardsPos [no] + new Vector3 (335f, 140f, 0f);
					}
					Constants.blankCardslist.Add (demoBoxColl);
					Constants.towerCardslist.Add (cards);
					h++;
					yield return new WaitForSeconds (.1f);
				}
			}
		}
		SoundManager.instance.efxSource.loop = false;
		yield return new WaitForSeconds (.2f);
		SoundManager.instance.PlaySingle (SoundManager.instance.ending);
		for (int j = 0; j < Constants.noOfdeckCards-(deckCard-1); j++) {
			Constants.deckCardslist[j].GetComponent<BoxCollider> ().enabled = true;
		}
		Constants.setdeckCard = true;
		for (int i = 0; i < Constants.towerCardslist.Count; i++) {
			Constants.towerCardslist [i].transform.localPosition = Constants.towerCardslist [i].transform.localPosition + new Vector3 (0f, 0f, -(float)i * 10);
			Constants.blankCardslist [i].transform.localPosition = Constants.towerCardslist [i].transform.localPosition /*+ new Vector3 (0f, 0f, -5f)*/;
			Constants.towerCardslist [i].transform.Find ("Neonlight").gameObject.SetActive (true);
			Constants.towerCardslist [i].GetComponent<TweenPosition> ().from = Constants.towerCardslist [i].transform.localPosition;
			Constants.towerCardslist [i].AddComponent<TweenRotation> ();
			Constants.towerCardslist [i].GetComponent<TweenRotation> ().from = new Vector3 (0f,-180f,0f); 
			Constants.towerCardslist [i].GetComponent<TweenRotation> ().to = new Vector3 (0f,0f,0f);
			Constants.towerCardslist [i].GetComponent<TweenRotation> ().duration = 0.2f;
			Constants.towerCardslist [i].GetComponent<TweenRotation> ().enabled = false;
			Constants.towerCardslist [i].GetComponent<TweenRotation> ().onFinished.Add (new EventDelegate(Constants.towerCardslist [i].GetComponent<CardTower> ().rotateFinish));
			Constants.towerCardslist [i].GetComponent<CardTower> ().cardNo = cardlist[Constants.noOfdeckCards+i];
		}
		for (int i = Constants.towerCardslist.Count-1; i >= Constants.towerCardslist.Count - 10; i--) {
			Constants.towerCardslist [i].GetComponent<TweenRotation> ().enabled = true;
			Constants.towerCardslist [i].GetComponent<TweenRotation> ().PlayForward ();
			Constants.towerCardslist [i].GetComponent<UI2DSprite> ().sprite2D = CardFaces [Constants.towerCardslist [i].GetComponent<CardTower> ().cardNo];
		}

		cardsIsdistributed = true;
		for (int i = 0; i < Constants.towerCardslist.Count; i++) {
			if(Constants.blankCardslist [i] != null)
				Constants.blankCardslist [i].SetActive (true);
			Constants.towerCardslist [i].GetComponent<CardTower>().nodeLeft.GetComponent<RayTest>().enabled = true;
			Constants.towerCardslist [i].GetComponent<CardTower>().nodeRight.GetComponent<RayTest>().enabled = true;
			Constants.towerCardslist [i].GetComponent<CardTower> ().enabled = true;
			Constants.towerCardslist [i].GetComponent<TweenPosition> ().onFinished.Add (new EventDelegate(Constants.towerCardslist [i].GetComponent<CardTower> ().Animationfinshed));
			yield return new WaitForSeconds (.01f);
		}
	}

	private bool isCardRight(Rank rank)
	{
		int no = 0;
		if (Constants.stackCards1list [Constants.stackCards1list.Count - 1].GetComponent<CardTower> () != null) 
			no = (int)Constants.stackCards1list [Constants.stackCards1list.Count - 1].GetComponent<CardTower> ().rank;
		if (Constants.stackCards1list [Constants.stackCards1list.Count - 1].GetComponent<CardDeck> () != null) 
			no = (int)Constants.stackCards1list [Constants.stackCards1list.Count - 1].GetComponent<CardDeck> ().rank;

		if (no == 0) {
			if ( (int)rank == 12 || no == (int)rank + 1 || no == (int)rank - 1 ) {
				return true;
			}
		} else if (no == 12) {
			if (rank == 0 || no == (int)rank + 1 || no == (int)rank - 1) {
				return true;
			}
		} else {
			if (no == (int)rank + 1 || no == (int)rank - 1) {
				return true;
			}
		}
		return false;
	}

}

public enum Suit : byte
{
	Diamonds = 0,
	Clubs,
	Hearts,
	Spades
}

public enum Rank 
{
	Two = 0,
	Three,
	Four,
	Five,
	Six,
	Seven,
	Eight,
	Ninth,
	Ten,
	Jack,
	Queen,
	King,
	Ace ,
}

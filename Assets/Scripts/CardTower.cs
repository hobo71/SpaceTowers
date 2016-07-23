using UnityEngine;
using System.Collections;
using Completed;

class CardTower : MonoBehaviour {

	public bool cardVisible = false;
	public Rank rank;
	public Suit suit;
	public int cardNo;
	public int towerNo;
	public GameObject nodeLeft;
	public GameObject nodeRight;
	public GameObject CoinPrefab;
	private bool flag = true;
	private static Sprite[] CardFaces ;
	private bool goStack = false;
	private bool iconIsVisible = false; 
	int score = 0;

	void Start () {
		score = 3600 + (Constants.virtulRound * 800);
		CardFaces = Resources.LoadAll<Sprite> ("Cards_design_update");
	}

	public void OnClick()
	{
		SoundManager.instance.PlaySingle (SoundManager.instance.NR1);

		if (cardVisible) {
			if (isStack1CardRight ()) {
				CardsControl.chance += 1;
				IconShow ();
				Constants.stackCards1list.Add (this.gameObject);
				Constants.stackCardslist.Add (this.gameObject);
				if (Constants.stackCards1list.Count == 4)
					Constants.set2StackCard = true;
				cardVisible = false;
				RemoveCardFromTower ();
				transform.Find ("Neonlight").gameObject.SetActive (false);
				int addScore = 0;
				if (CardsControl.chance == 10) {
					SoundManager.instance.PlaySingle1 (SoundManager.instance.NR3);
//					addScore = score * 4;
//					coinGernate ("Yellow_coin", addScore);
				} else if (CardsControl.chance == 7) {
					SoundManager.instance.PlaySingle1 (SoundManager.instance.NR3);
//					addScore = score * 3;
//					coinGernate ("Yellow_coin", addScore);
				}else if (CardsControl.chance == 3) {
					SoundManager.instance.PlaySingle1 (SoundManager.instance.NR3);
//					addScore = score * 2;
//					coinGernate ("Yellow_coin", addScore);
				}  
//				else {
					addScore = score*CardsControl.chance;
					coinGernate ("Green_coin", addScore);
//				}
				Constants.virtulScore = Constants.virtulScore+addScore;
				TweenPosition.Begin (this.gameObject, .3f, GameObject.Find ("DeckStack1Cards").transform.localPosition); 
			} 
			else if (Constants.stackCards2list.Count != 0 && isStack2CardRight ()) {
				goStack = true;
				CardsControl.chance += 1;
				IconShow ();
				Constants.stackCards2list.Add (this.gameObject);
				Constants.stackCardslist.Add (this.gameObject);
				cardVisible = false;
				RemoveCardFromTower ();
				transform.Find ("Neonlight").gameObject.SetActive (false);
				int addScore = 0;
				if (CardsControl.chance == 10) {
					SoundManager.instance.PlaySingle1 (SoundManager.instance.NR3);
//					addScore = score * 4;
//					coinGernate ("Yellow_coin", addScore);
				} else if (CardsControl.chance == 7) {
					SoundManager.instance.PlaySingle1 (SoundManager.instance.NR3);
//					addScore = score * 3;
//					coinGernate ("Yellow_coin", addScore);
				}else if (CardsControl.chance == 3) {
					SoundManager.instance.PlaySingle1 (SoundManager.instance.NR3);
//					addScore = score * 2;
//					coinGernate ("Yellow_coin", addScore);
				}  
//				else {
					addScore = score*CardsControl.chance;
					coinGernate ("Green_coin", addScore);
//				}
				Constants.virtulScore = Constants.virtulScore+addScore;
				TweenPosition.Begin (this.gameObject, .3f, GameObject.Find ("DeckStack2Cards").transform.localPosition); 
			}else {
				print ("Wrong Card");
				score = 6000 + (Constants.virtulRound * 2000);
				Constants.virtulScore = Constants.virtulScore-score;
				coinGernate("Red_coin",-score);
			}
		}
	}

	public void coinGernate(string coinName,int point)
	{
		GameObject coin = Constants.AddItem (CoinPrefab,GameObject.Find("AllCoins"),transform.localPosition,Vector3.one);
		coin.GetComponentInChildren<UILabel> ().text = point.ToString();
		coin.GetComponent<UI2DSprite> ().sprite2D = Resources.Load<Sprite> (coinName);
		coin.GetComponent<TweenPosition> ().from = transform.localPosition;
		coin.GetComponent<TweenPosition> ().to = new Vector3(transform.localPosition.x,transform.localPosition.y+200f,transform.localPosition.z);
		coin.GetComponent<TweenPosition> ().enabled = true;
		coin.GetComponent<TweenAlpha> ().enabled = true;
		if (iconIsVisible) {
			iconIsVisible = false;
			if(suit == Suit.Hearts)
			 coin.transform.FindChild ("heartsAnim").gameObject.SetActive (true);
		}
	}

	void IconShow()
	{
		if (Constants.stackCardslist.Count > 0) {
			if (Constants.stackCardslist [Constants.stackCardslist.Count - 1].GetComponent<CardTower> ().suit == suit) {
				Constants.HeartCount++;
				print ("Constants.HeartCount : "+Constants.HeartCount);
				if (Constants.HeartCount == 2) {
					// excute Heart Power here
					iconIsVisible = true; 
					// after Power Up
					Constants.HeartCount = 0;
				} 
			} else
				Constants.HeartCount = 0;
		}
		
	}

	IEnumerator CoinSound()
	{
		yield return new WaitForSeconds (.2f);

	}

	private void RemoveCardFromTower()
	{
		if (towerNo == 0) {
			Constants.tower1Cardslist.Remove (this.gameObject);
		}else if (towerNo == 1) {
			Constants.tower2Cardslist.Remove (this.gameObject);
		}else if (towerNo == 2) {
			Constants.tower3Cardslist.Remove (this.gameObject);
		}
	}

	public  void rotateFinish()
	{
		cardVisible = true;
		int cardNum = int.Parse( transform.name.Substring (6));
		Destroy (Constants.blankCardslist[cardNum]);
		rank = (Rank)(cardNo % Settings.NUM_RANKS);
		suit = (Suit)(cardNo / Settings.NUM_RANKS);
	}

	void Update(){

		if (flag ) {
			if (nodeLeft.GetComponent<RayTest> ().freeNode && nodeRight.GetComponent<RayTest> ().freeNode) {
				int cardNum = int.Parse( transform.name.Substring (6));
				if(cardNum < 18)
					StartCoroutine(rotateCard());
				flag = false;
			}
		}
	}

	IEnumerator rotateCard()
	{
		transform.rotation = Quaternion.Euler (new Vector3(0f, -180f, 0f));
		TweenRotation.Begin (this.gameObject, .2f, Quaternion.Euler(new Vector3(0f,0f,0f))); 
		yield return new WaitForSeconds (.1f);
		GetComponent<UI2DSprite> ().sprite2D  = CardFaces[cardNo];
		yield return new WaitForSeconds (.1f);
	}

	public void Animationfinshed()
	{
		if (!goStack) {
			GameObject changeparent = GameObject.Find ("DeckStack1Cards");
			this.transform.parent = changeparent.transform;
			gameObject.GetComponent<UI2DSprite> ().depth = 3 + Constants.stackCards1list.Count;
			gameObject.transform.Find("LockSprite").gameObject.GetComponent<UISprite> ().depth = 4+ Constants.stackCards1list.Count;
			Constants.towerCardslist.Remove (this.gameObject);
		} else {
			GameObject changeparent = GameObject.Find ("DeckStack2Cards");
			this.transform.parent = changeparent.transform;
			gameObject.GetComponent<UI2DSprite> ().depth = 3 + Constants.stackCards2list.Count;
			gameObject.transform.Find("LockSprite").gameObject.GetComponent<UISprite> ().depth = 4+ Constants.stackCards2list.Count;
			Constants.towerCardslist.Remove (this.gameObject);
		}
	}

	private bool isStack1CardRight()
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

	private bool isStack2CardRight()
	{
		int no = 0;
		if (Constants.stackCards2list [Constants.stackCards2list.Count - 1].GetComponent<CardTower> () != null) 
			no = (int)Constants.stackCards2list [Constants.stackCards2list.Count - 1].GetComponent<CardTower> ().rank;
		if (Constants.stackCards2list [Constants.stackCards2list.Count - 1].GetComponent<CardDeck> () != null) 
			no = (int)Constants.stackCards2list [Constants.stackCards2list.Count - 1].GetComponent<CardDeck> ().rank;

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

//		public bool SameSuit(CCard card) {
//			return Suit == card.Suit;
//		}
}
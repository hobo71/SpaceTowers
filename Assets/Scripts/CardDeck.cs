using UnityEngine;
using System.Collections;
using System;
using Completed;

public class CardDeck : MonoBehaviour {

	private static Sprite[] CardFaces ;
	public int cardNo;
	public Rank rank;
	public Suit suit;
	private bool add2stack = false;

	// Use this for initialization
	void Start () {
		CardFaces = Resources.LoadAll<Sprite>("Cards_design_update");
		rank = (Rank)(cardNo % Settings.NUM_RANKS);
		suit = (Suit)(cardNo / Settings.NUM_RANKS);
	}


	public IEnumerator cardShow()
	{
		GetComponent<BoxCollider> ().enabled = false;
		GetComponent<TweenPosition> ().enabled = true;
		GetComponent<TweenRotation> ().enabled = true;
		yield return new WaitForSeconds (.05f);
		transform.GetComponent<UI2DSprite> ().sprite2D = CardFaces [cardNo];
	}

	public IEnumerator SecondStackShow()
	{
		print ( GameObject.Find ("DeckStack2Cards").transform.localPosition.x);
		GetComponent<TweenPosition> ().to = new Vector3( GameObject.Find ("DeckStack2Cards").transform.localPosition.x+230,0f,0f);
		add2stack = true;
		GetComponent<BoxCollider> ().enabled = false;
		GetComponent<TweenPosition> ().enabled = true;
		GetComponent<TweenRotation> ().enabled = true;
		yield return new WaitForSeconds (.05f);
		transform.GetComponent<UI2DSprite> ().sprite2D = CardFaces [cardNo];
	}

	public void deckCardClick()
	{
		SoundManager.instance.PlaySingle (SoundManager.instance.NR1);
		for(int i = 0 ;i < Constants.stackCards1list.Count;i++)
			Destroy (Constants.stackCards1list[i]);
		for(int i = 0 ;i < Constants.stackCards2list.Count;i++)
			Destroy (Constants.stackCards2list[i]);
		Constants.stackCards1list.Clear ();
		Constants.stackCards2list.Clear ();
		Constants.stackCardslist.Clear ();
		CardsControl.chance = 0;
		StartCoroutine (cardShow());
	}

	public void Animationfinshed()
	{
		if (add2stack) {
			add2stack = false;
			GameObject changeparent = GameObject.Find ("DeckStack2Cards");
			this.transform.parent = changeparent.transform;
			gameObject.GetComponent<UI2DSprite> ().depth = 3 + Constants.stackCards2list.Count;
			Constants.deckCardslist.Remove (this.gameObject);
			Constants.stackCards2list.Add (this.gameObject);
		} else {
			GameObject changeparent = GameObject.Find ("DeckStack1Cards");
			this.transform.parent = changeparent.transform;
			gameObject.GetComponent<UI2DSprite> ().depth = 3 + Constants.stackCards1list.Count;
			Constants.deckCardslist.Remove (this.gameObject);
			Constants.stackCards1list.Add (this.gameObject);
		}

	}
}

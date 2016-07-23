using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Deck
{
	public List<int> cards;
	public Vector3 startPos;

	public Deck()
	{
		startPos.Set(0,0, 0);
		cards = new List<int>();
		for(int i = 0; i < 52; i++)
		{
			cards.Add(i);
		}
		ShuffleDeck ();
	}

	public void ShuffleDeck()
	{
		int n = cards.Count;
		while (n > 1)
		{
			n--;
			int k = Random.Range(0, 51);
			int val = cards[k];
			cards[k] = cards[n];
			cards[n] = val;
		}
	}
}

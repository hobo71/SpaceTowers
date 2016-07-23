using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Settings : MonoBehaviour
{
	private static bool aceLow = false;
	private static int numberOfDecks = 1;
	public static int NUM_SUITS = 4;
	public static int NUM_RANKS = 13;
	public static int NUM_CARDS = 52;

	private int[] cardCounts; 

	void start()
	{
		// Default settings --> 1 of each card
		cardCounts = new int[52];
		for (int i = 0; i < 52; i++) cardCounts [i] = 1;
	}

	public void AddDeck()
	{
		numberOfDecks++;
		for (int i = 0; i < 13; i++) 
		{
			cardCounts [i]++;
		}
	}

	public void RemoveDeck()
	{
		if (numberOfDecks == 0) return;
		numberOfDecks--;
		for (int i = 0; i < 13; i++) {
			if (cardCounts [i] > 0)
			{
				cardCounts [i]--;
			}
		}
	}

	public void SetCardCount(int card, int count)
	{
		cardCounts [card] = count;
	}

	public void SetDefault()
	{
		for (int i = 0; i < 52; i++) 
		{
			SetCardCount(i, 1);
		}
		numberOfDecks = 1;
		AceLow = false;
	}

	public static bool AceLow
	{
		get
		{
			return aceLow;
		}
		set
		{
			aceLow = value;
		}
	}

	public int NumberOfDecks
	{
		get
		{
			return numberOfDecks;
		}
	}
}

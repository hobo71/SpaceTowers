using UnityEngine;
using System.Collections;

public class LoadingBar : MonoBehaviour
{
	public static LoadingBar instance = null; 
	public GameObject bar;
	public static bool isVisible = false;

	// Use this for initialization

	void Awake(){
		if (instance == null)
			//if not, set it to this.
			instance = this;
		//If instance already exists:
		else if (instance != this)
			//Destroy this, this enforces our singleton pattern so there can only be one instance of SoundManager.
			Destroy (gameObject);
		DontDestroyOnLoad (gameObject);
		isVisible = false;

	}

	void Start ()
	{
		isVisible = false;

	}

	// Update is called once per frame
	void Update ()
	{

		if (isVisible) {
			bar.SetActive (true);
		} else {
			bar.SetActive (false);
		}

	}
}
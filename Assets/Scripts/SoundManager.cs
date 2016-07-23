using UnityEngine;
using System.Collections;

namespace Completed
{
	public class SoundManager : MonoBehaviour 
	{
		public AudioSource efxSource;                   //Drag a reference to the audio source which will play the sound effects.
		public AudioSource musicSource;
		public AudioSource efxSource1;                 //Drag a reference to the audio source which will play the music.
		//Drag a reference to the audio source which will play the music.
		public static SoundManager instance = null;     //Allows other scripts to call functions from SoundManager.             
		public float lowPitchRange = .95f;              //The lowest a sound effect will be randomly pitched.
		public float highPitchRange = 1.05f;            //The highest a sound effect will be randomly pitched.
		public AudioClip NR1;
		public AudioClip NR2;
		public AudioClip NR3;
		public AudioClip NR4;
		public AudioClip NR5;
		public AudioClip NR6;
		public AudioClip appear1;
		public AudioClip appear2;
		public AudioClip dealing;
		public AudioClip ending;
		public AudioClip looping;
		public AudioClip movie;

		void Awake ()
		{
//			if (PlayerPrefs.GetString (Constants.Music) == "")
//				PlayerPrefs.SetString (Constants.Music, "true");
			if (PlayerPrefs.GetString (Constants.Sound) == "")
				PlayerPrefs.SetString (Constants.Sound, "true");
			//Check if there is already an instance of SoundManager
			if (instance == null)
				//if not, set it to this.
				instance = this;
			//If instance already exists:
			else if (instance != this)
				//Destroy this, this enforces our singleton pattern so there can only be one instance of SoundManager.
				Destroy (gameObject);
			
			//Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
			DontDestroyOnLoad (gameObject);
		}
		
		
		//Used to play single sound clips.
		public void PlaySingle(AudioClip clip)
		{
			//Set the clip of our efxSource audio source to the clip passed in as a parameter.
			efxSource.clip = clip;
			
			//Play the clip.
			if(PlayerPrefs.GetString (Constants.Sound) == "true") 
			efxSource.Play ();
//			if(PlayerPrefs.GetString (Constants.Music) == "true") 
//				musicSource.Play ();
		}

		//Used to play single sound clips.
		public void PlaySingle1(AudioClip clip)
		{
			//Set the clip of our efxSource audio source to the clip passed in as a parameter.
			efxSource1.clip = clip;

			//Play the clip.
			if(PlayerPrefs.GetString (Constants.Sound) == "true") 
				efxSource1.Play ();
			//			if(PlayerPrefs.GetString (Constants.Music) == "true") 
			//				musicSource.Play ();
		}
		
		
		//RandomizeSfx chooses randomly between various audio clips and slightly changes their pitch.
		public void RandomizeSfx (params AudioClip[] clips)
		{
			//Generate a random number between 0 and the length of our array of clips passed in.
			int randomIndex = Random.Range(0, clips.Length);
			
			//Choose a random pitch to play back our clip at between our high and low pitch ranges.
			float randomPitch = Random.Range(lowPitchRange, highPitchRange);
			
			//Set the pitch of the audio source to the randomly chosen pitch.
			efxSource.pitch = randomPitch;
			
			//Set the clip to the clip at our randomly chosen index.
			efxSource.clip = clips[randomIndex];
			
			//Play the clip.
			if(PlayerPrefs.GetString (Constants.Sound) == "true") 
			efxSource.Play();
		}
	}
}

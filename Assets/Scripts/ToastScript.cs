using UnityEngine;
using System.Collections;
using System.Threading;

public class ToastScript : MonoBehaviour {

	public GameObject toastObj;
	public GameObject toastObj1;
	static string toastLabel;
	static string toastLabel1;
	 static bool visibletoast;
	static bool visibletoast1;

//	void dsad(string toastLabel)
//	{
//		sas(toastLabel);
//	}


//	void sas(string toastLabel){
//		StartCoroutine (toastVisible(toastLabel));
//	}

	public static void showToast(string text)
	{
		toastLabel = text;
		visibletoast = true;

	}

	public static void showToast1(string text)
	{
		toastLabel1 = text;
		visibletoast1 = true;

	}

	void Update()
	{
		if (visibletoast) {
			visibletoast = false;
			StartCoroutine (toastVisible ());
		}
		if (visibletoast1) {
			visibletoast1 = false;
			StartCoroutine (toastVisible1 ());
		}
	}


	public IEnumerator toastVisible()
	{
		toastObj.SetActive (true);
		toastObj.GetComponentInChildren<UILabel> ().text = toastLabel;
		yield return new WaitForSeconds (2);
		toastObj.SetActive (false);

	}
	public IEnumerator toastVisible1()
	{
		toastObj1.SetActive (true);
		toastObj1.GetComponentInChildren<UILabel> ().text = toastLabel1;
		yield return new WaitForSeconds (2);
		toastObj1.SetActive (false);

	}
}

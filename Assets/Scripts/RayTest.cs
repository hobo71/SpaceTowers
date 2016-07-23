using UnityEngine;
using System.Collections;

public class RayTest : MonoBehaviour {

	private RaycastHit hit;
	public bool freeNode = false;
	private LineRenderer lineRendrer;
	private bool flag = true;

	void Update() {

		if (flag) {
			if (Physics.Raycast (transform.position, transform.TransformDirection (Vector3.back) * 10000f, out hit)) {
				Debug.DrawRay (transform.position, transform.TransformDirection (Vector3.back) * 10000f, Color.red);
			} else {
				freeNode = true;
				flag = false;
			}
		}
	}
}

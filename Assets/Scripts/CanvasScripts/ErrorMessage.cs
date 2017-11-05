using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ErrorMessage : MonoBehaviour {

	public static ErrorMessage me { get; private set; }

	void Awake () {
		if (me != null && me != this) {
			Destroy (this);
		} else {
			me = this;
		}
	}

	private void Start () {
		GetComponent<Text> ().text = string.Empty;
		StartCoroutine (ShowError ());
	}

	public void PassErrorMessage (string s) {
		GetComponent<Text> ().text = s;
	}

	private IEnumerator ShowError () {
		while (true) {
			yield return new WaitForSeconds(0.1f);
			if(GetComponent<Text>().text != string.Empty){
				yield return new WaitForSeconds (0.5f);
				GetComponent<Text>().text = string.Empty;
			}
		}
	}
}

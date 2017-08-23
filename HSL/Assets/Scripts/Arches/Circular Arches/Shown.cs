using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//is attached to input fields, and Activate method is called from toggles upon the change of values
//is used when an input field needs to appear when its parent toggle is checked, and disappears when unchecked
public class Shown : MonoBehaviour {


	public void Activate (bool boolean) {
		//if the toggle is inactive, deactivate the input field; and the opposite
		this.gameObject.SetActive (boolean);
		//if the toggle is inactive, clean the text in the field so that next time when the toggle is activated
		//no leftover values from previous input appear
		if (!boolean)
			this.gameObject.GetComponent<InputField> ().text = "";
	}
}

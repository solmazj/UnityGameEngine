using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//is used when an input field needs to appear when its parent toggle is checked, and disappears when unchecked
public class Shown : MonoBehaviour {


	public void Activate (bool boolean) {
			this.gameObject.SetActive (boolean);
		if (!boolean)
			this.gameObject.GetComponent<InputField> ().text = "";
	}
}

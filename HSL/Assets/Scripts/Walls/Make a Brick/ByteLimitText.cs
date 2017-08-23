using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

//reflects valid RGB values used from ColorRGB script in the InputField text. This script can be optimized
//by figuring out how to access each separate input field from single script and consolidated with ColorRGB code
public class ByteLimitText : MonoBehaviour {


	public void Start () {
		this.GetComponent<InputField> ().onEndEdit.AddListener (delegate {ByteLimit (this.GetComponent<InputField> ());});
	}

	void ByteLimit (InputField inputField) {
		int integer;
		if (String.IsNullOrEmpty (inputField.text)) {
			integer = 255;
		} else {
			integer = int.Parse (inputField.text);
			if (integer > 255) {
				integer = 255;
			}
			if (integer < 0) {
				integer = 0;
			}
		}
		inputField.text = integer.ToString ();  
	}
}

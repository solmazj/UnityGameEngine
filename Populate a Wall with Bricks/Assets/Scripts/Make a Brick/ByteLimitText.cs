using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ByteLimitText : MonoBehaviour {

	public InputField field;

	public void Start ()
	{
		field.onEndEdit.AddListener (delegate {
			ByteLimit (field);
		});
	}

	void ByteLimit (InputField inputField)
	{
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

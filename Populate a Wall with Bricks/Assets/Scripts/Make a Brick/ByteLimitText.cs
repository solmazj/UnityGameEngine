using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ByteLimitText : MonoBehaviour {

	public InputField field;

	void ByteLimit (InputField inputField)
	{
		int integer = int.Parse (inputField.text);
		if (integer > 255) {
			integer = 255;
		}
		if (integer < 0) {
			integer = 0;
		}
		inputField.text = integer.ToString ();  
	}

	public void Start ()
	{
		field.onEndEdit.AddListener (delegate {
			ByteLimit (field);
		});
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ExceptionTest : MonoBehaviour {

	public void Test (){
		if (GameObject.Find ("Cube") == null) {
			throw new Exception ("I am throwing an exception");
		} else {
			Debug.Log ("All is good");
		}
	}
}

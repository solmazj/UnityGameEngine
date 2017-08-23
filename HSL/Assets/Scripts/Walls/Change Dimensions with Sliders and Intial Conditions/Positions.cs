using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//this means that the script runs even in the edit mode
[ExecuteInEditMode]
//attach to any box whose dimensions need to be changed
public class Positions : MonoBehaviour {

	//always runs on update, even in edit mode. NOT THE BEST PRACTICE
	void Update () {
		//If attached to a wall-to-be cube and plane as a ground at a point (x,0,z)
		//looks like the wall rises up and grows sideways from this point
		this.transform.position = new Vector3 (this.transform.position.x, this.transform.localScale.y / 2, 
			this.transform.position.z);
	}
}

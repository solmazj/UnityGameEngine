using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Even in edit mode
[ExecuteInEditMode]

public class PlaneAsGroundPosition : MonoBehaviour {

	//If attached to a wall-to-be cube and plane as a ground at (0,0,0)
	//Looks like the wall rises up from the starting point, and grows sideways from this point

	void Update () {
		float yScale = this.transform.localScale.y;
		this.transform.position = new Vector3 (this.transform.position.x, yScale / 2, this.transform.position.z);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorObject : MonoBehaviour {

	//works only when z scale = 1
	//attaches to a wall
	private GameObject wall;
	private GameObject anchorObject;
	float xScale;
	float yScale;

	void Awake () {

		//z component is not that important now
		wall = this.gameObject;
		xScale = wall.transform.localScale.x;
		yScale = wall.transform.localScale.y;

		anchorObject = new GameObject ();
		//position is at the lower right corner of the wall;
		//because it rises from the ground, we make y=0.5f (the center of the first brick)
		anchorObject.transform.position = new Vector3(-xScale/2 + 1, 0.5f, 0);
	}

	void Start () {
		WallBuild.wallBuild (xScale/2, yScale, anchorObject);
		wall.SetActive (false);
	}
}

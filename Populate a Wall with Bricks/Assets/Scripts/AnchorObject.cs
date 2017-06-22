using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorObject : MonoBehaviour {

	//works only when z scale = 1
	private GameObject wall;
	private GameObject anchorObject;
	float xScale;
	float yScale;

	void Awake () {

		wall = this.gameObject;
		xScale = wall.transform.localScale.x;
		yScale = wall.transform.localScale.y;
		anchorObject = new GameObject();
		anchorObject.transform.position = new Vector3(-xScale/2 + 1, -yScale/2 + 0.5f, 0);
	}

	void Start () {
		WallBuild.wallBuild (xScale/2, yScale, anchorObject);
		wall.SetActive (false);
	}
}

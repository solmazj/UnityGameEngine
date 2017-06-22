using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickOutOfAWall : MonoBehaviour {

	// mess up this part of the code 
	//private GameObject wall;
	private float xLength;
	private float yLength;

	void Awake () {
		wall = this.gameObject;
		xLength = wall.transform.localScale.x;
		yLength = wall.transform.localScale.y;
	}

	void Start () {

		WallBuild.wallBuild (xLength/2, yLength, wall);
		wall.SetActive (false);

	}
}
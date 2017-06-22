using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickWallBuild : MonoBehaviour {

	public int horizontalAmount;
	public int verticalAmount;

	void Start () {

		WallBuild.wallBuild (horizontalAmount, verticalAmount, this.gameObject);
	}
}

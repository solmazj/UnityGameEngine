using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBuild : MonoBehaviour {
	
	//anchor is a bad name, works only when z scale = 1, only one row of bricks
	public static void wallBuild (float hor, float ver, GameObject anchor) {

		GameObject prefab = Resources.Load ("brickRed") as GameObject;
		for (int i = 0; i < ver; i++) {
			float offset = i % 2;
			for (int j = 0; j < hor; j++) {
				GameObject brick = Instantiate (prefab) as GameObject;
				brick.transform.position = new Vector3 (anchor.transform.position.x + j * 2 + offset,
					anchor.transform.position.y + i, anchor.transform.position.z);
			}
		}
	}
}
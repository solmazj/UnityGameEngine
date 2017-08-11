using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class CorbelTunnel : MonoBehaviour {

	public void Tunnel () {

		GameObject parent = new GameObject ("Tunnel");

		float brickDepth = GameObject.Find("Corbel").GetComponent<Corbel>().brickDepth;
		float vaultDepth = GameObject.Find("Corbel").GetComponent<Corbel>().vaultDepth;
		float brickHeight = GameObject.Find("Corbel").GetComponent<Corbel>().brickHeight;
		float height = brickHeight + GameObject.Find ("Corbel").GetComponent<Corbel> ().yPos;;
		float length = GameObject.Find("Corbel").GetComponent<Corbel>().freeSpan;
		float overhang = GameObject.Find("Corbel").GetComponent<Corbel>().overhang;
		float depth;

		StackTrace stackTrace = new StackTrace();
		if (stackTrace.GetFrame (1).GetMethod ().Name == "CorbelledArch") {
			depth = brickDepth;
		}
		else {
			depth = vaultDepth;
		}

		int count = GameObject.Find ("Corbel").GetComponent<Corbel> ().count;

		for (int t = 0; t < count; t++) {
			GameObject cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
			cube.transform.tag = "Tunnel";
			cube.transform.parent = parent.transform;
			cube.transform.localScale = new Vector3 (length, height, depth);
			cube.transform.position = new Vector3 (0, height/2, 0);
			length -= 2 * overhang;
			height += brickHeight;
		}
	}
}

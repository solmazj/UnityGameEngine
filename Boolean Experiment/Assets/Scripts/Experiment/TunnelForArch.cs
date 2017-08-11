using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class TunnelForArch : MonoBehaviour {

	public void Tunnel () {

		int increment = 8;

		GameObject parent = new GameObject ("Tunnel");

		BezierCurve arc = GameObject.Find("Circular").GetComponent<BezierCurve>();
		float brickDepth = GameObject.Find("Circular").GetComponent<ArchBuild>().brickDepth;
		float vaultDepth = GameObject.Find("Circular").GetComponent<ArchBuild>().vaultDepth;
		StackTrace stackTrace = new StackTrace();
		float depth;
		if (stackTrace.GetFrame (1).GetMethod ().Name == "BuildAnArch") {
			depth = brickDepth;
		}
		else {
			depth = vaultDepth;
		}
		for (int t = 0; t < increment; t++) {
			GameObject cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
			cube.transform.tag = "Tunnel";
			cube.transform.parent = parent.transform;
			float height = Mathf.Abs(arc.GetPoint((t + 1) / (2f * increment)).y);
			float length = 2 * Mathf.Abs(arc.GetPoint(t / (2f * increment)).x);
			cube.transform.localScale = new Vector3 (length, height, depth);
			cube.transform.position = new Vector3 (0, height/2, 0);
		}
	}
}

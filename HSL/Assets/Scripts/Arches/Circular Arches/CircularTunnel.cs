using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

//is attached to Circular object in the Hierarchy
public class CircularTunnel : MonoBehaviour {

	public void Tunnel () {

		//how many rectangular pieces are going to be in the tunnel
		int increment = this.gameObject.GetComponent<ArchBuild>().numberOfBricks/2;

		GameObject parent = new GameObject ("Tunnel");

		//dimensions
		BezierCurve arc = this.gameObject.GetComponent<BezierCurve>();
		float brickDepth = this.gameObject.GetComponent<ArchBuild>().brickDepth;
		float vaultDepth = this.gameObject.GetComponent<ArchBuild>().vaultDepth;


		float depth;
		StackTrace stackTrace = new StackTrace();
		//assigning depth values based on which method calls this method. See CorbelTunnel for details
		if (stackTrace.GetFrame (1).GetMethod ().Name == "BuildAnArch") {
			depth = brickDepth;
		}
		else {
			depth = vaultDepth;
		}

		for (int t = 0; t < increment; t++) {
			GameObject cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
			cube.transform.parent = parent.transform;
			cube.transform.tag = "Tunnel";
			//the length is the length of a brick (call it Orig brick). See Final ReadMe for detail
			float length = 2 * Mathf.Abs(arc.GetPoint(t / (2f * increment)).x) + 0.02f;
			//and the height is the height of the brick that is on top of the Orig brick
			float height = Mathf.Abs(arc.GetPoint((t + 1) / (2f * increment)).y) + 0.02f;

			cube.transform.localScale = new Vector3 (length, height, depth);
			cube.transform.position = new Vector3 (0, height/2, 0);
		}
	}
}

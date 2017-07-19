using UnityEngine;

public class BezierCurve : MonoBehaviour {

	public float freeSpan;
	public float archHeight;
	[HideInInspector] //makes the variable public, but not visible in the Inspector
	public Vector3[] points;

	//need to work on this, on how the arc is presented in the scene view
	public void ArchInfoReady () {
		if (freeSpan <= 0 || archHeight <= 0) {
			Debug.Log ("Inapplicable input values");
		} else {
			points = new Vector3[] {
				new Vector3 (-freeSpan / 2, 0f, 0f),
				new Vector3 (-freeSpan / 4, archHeight / 0.75f, 0f),
				new Vector3 (freeSpan / 4, archHeight / 0.75f, 0f),
				new Vector3 (freeSpan / 2, 0f, 0f)
			};
		}
	}

	public Vector3 GetPoint (float t) {
		return transform.TransformPoint(Bezier.GetPoint(points[0], points[1], points[2], points[3], t));
	}
	
	public Vector3 GetVelocity (float t) {
		//velocity is affected by the position of the curve, so we need to have that subtraction present
		return transform.TransformPoint(Bezier.GetFirstDerivative(points[0], points[1], points[2], points[3], t)) - transform.position;
	}

	//makes velocity vector's magnitude equal to one, so just shows the direction of the velocity
	public Vector3 GetDirection (float t) {
		return GetVelocity(t).normalized;
	}
}
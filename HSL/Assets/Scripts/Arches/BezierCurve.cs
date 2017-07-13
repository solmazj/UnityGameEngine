using UnityEngine;

public class BezierCurve : MonoBehaviour {

	public Vector3[] points;
	
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

	//a special Unity method, which is called by the editor when the component is created or reset
	//uses this as a default array of Vector3s
	public void Reset () {
		points = new Vector3[] {
			new Vector3(1f, 0f, 0f),
			new Vector3(2f, 0f, 0f),
			new Vector3(3f, 0f, 0f),
			new Vector3(4f, 0f, 0f)
		};
	}
}
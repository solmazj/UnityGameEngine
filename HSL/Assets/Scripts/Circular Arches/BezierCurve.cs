using UnityEngine;
[ExecuteInEditMode]
public class BezierCurve : MonoBehaviour {

	public float freeSpan;
	public float archHeight;
	[HideInInspector] //makes the variable public, but not visible in the Inspector
	public Vector3[] points;

	//need to work on this, on how the arc is presented in the scene view and not Awake
	void Awake () {
		if (freeSpan <= 0 || archHeight < 0) {
			Debug.Log ("Inapplicable input values");
		}
		else {
			float angle;
			//the case where angle is 0 degrees
			float inset = 0.5f;

			if (archHeight == 0)
				angle = 0;
			else {
				float radius = (freeSpan * freeSpan / (8 * archHeight)) + (archHeight / 2);
				angle = (2 * Mathf.Asin (freeSpan / (2 * radius))) * Mathf.Rad2Deg;
			
				if (172.5 < angle && angle <= 180)
					inset = 0.05f;
				else if (157.5 < angle && angle <= 172.5)
					inset = 0.1f;
				else if (142.5 < angle && angle <= 157.5)
					inset = 0.15f;
				else if (127.5 < angle && angle <= 142.5)
					inset = 0.175f;
				else if (112.5 < angle && angle <= 127.5)
					inset = 0.2f;
				else if (97.5 < angle && angle <= 112.5)
					inset = 0.225f;
				else if (75 <= angle && angle <= 97.5)
					inset = 0.25f;
				else if (0 < angle && angle < 75)
					inset = 0.275f;
			}
			points = new Vector3[] {
				new Vector3 (-freeSpan / 2, 0f, 0f),
				new Vector3 (-freeSpan/2 + freeSpan * inset, archHeight / 0.75f, 0f),
				new Vector3 (freeSpan/2 - freeSpan * inset, archHeight / 0.75f, 0f),
				new Vector3 (freeSpan / 2, 0f, 0f)
			};
			//Print the free span, height and arc of embrasure of the modeled arch
			Debug.Log("The free span of the arch is " + freeSpan.ToString("n2"));
			Debug.Log("The height of the arch is " + archHeight.ToString("n2"));
			Debug.Log ("The arc of embrasure is " + angle.ToString("n2") + " degrees");
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
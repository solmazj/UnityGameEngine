using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BezierCurve))]
public class BezierCurveInspector : Editor {

	void OnSceneGUI () {

		//initiations
		BezierCurve curve = target as BezierCurve;

		//drawing the wire arc
		Handles.DrawBezier(curve.points[0], curve.points[3], curve.points[1], curve.points[2], Color.red, null, 2f);
	}
}
using UnityEditor;
using UnityEngine;

//Does not get built into the final game. Needed just to display the bezier curve in scene view (not evem seen in game view)
[CustomEditor(typeof(BezierCurve))]
public class BezierCurveInspector : Editor {

	void OnSceneGUI () {

		//target the bezier curve selected in Hierarchy
		BezierCurve curve = target as BezierCurve;

		//drawing (aka displaying, it already exists) the wire arc in scene view
		Handles.DrawBezier(curve.points[0], curve.points[3], curve.points[1], curve.points[2], Color.red, null, 2f);
	}
}
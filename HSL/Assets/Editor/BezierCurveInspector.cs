using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BezierCurve))]
public class BezierCurveInspector : Editor {

	BezierCurve curve;
	Transform handleTransform;
	Quaternion handleRotation;

	const float handleSize = 0.04f;
	const float pickSize = 0.06f;
	int selectedIndex = -1;

	//if ShowDirections is used
	 private const int lineSteps = 50;
	 private const float directionScale = 0.5f;

	private void OnSceneGUI () {
		//initiations
		curve = target as BezierCurve;
		handleTransform = curve.transform;
		handleRotation = Tools.pivotRotation == PivotRotation.Local ?
			handleTransform.rotation : Quaternion.identity;

//		Vector3 p0 = ShowPoint (0);

		//drawing the wire arc
		Handles.DrawBezier(curve.points[0], curve.points[3], curve.points[1], curve.points[2], Color.red, null, 2f);

//		ShowDirections();
	}

	//shows "velocity"
	private void ShowDirections () {
		Handles.color = Color.green;
		//initial direction
		Vector3 point = curve.GetPoint(0f);
		Handles.DrawLine(point, point + curve.GetDirection(0f) * directionScale);
		//iteration
		for (int i = 1; i <= lineSteps; i++) {
			point = curve.GetPoint(i / (float)lineSteps);
			Handles.DrawLine(point, point + curve.GetDirection(i / (float)lineSteps) * directionScale);
		}
	}

	//shows the control points[index] on the scene view, and they become free to edit
	private Vector3 ShowPoint (int index) {
		//transforms position from local to world space
		Vector3 point = handleTransform.TransformPoint(curve.points[index]);
		//use the camera to calculate suitable size of the handle in the world space
		float size = HandleUtility.GetHandleSize (point);
		Handles.color = Color.white;
		if (Handles.Button(point, handleRotation, size * handleSize, size * pickSize, Handles.DotHandleCap)) {
			selectedIndex = index;
		}
		if (selectedIndex == index) {
			EditorGUI.BeginChangeCheck ();
			point = Handles.DoPositionHandle (point, handleRotation);
			if (EditorGUI.EndChangeCheck ()) {
				Undo.RecordObject (curve, "Move Point");
				//translate back into local space
				curve.points [index] = handleTransform.InverseTransformPoint (point);
			}
		}
		return point;
	}
}
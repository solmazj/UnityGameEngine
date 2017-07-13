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

		//showing the control points
		Vector3 p0 = ShowPoint(0);
		Vector3 p1 = ShowPoint(1);
		Vector3 p2 = ShowPoint(2);
		Vector3 p3 = ShowPoint(3);

		//handles for 2 middle control points
		Handles.color = Color.gray;
		Handles.DrawLine(p0, p1);
		Handles.DrawLine(p2, p3);

		//drawing the wire arc
		Handles.DrawBezier(p0, p3, p1, p2, Color.red, null, 2f);
		//ShowDirections();
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
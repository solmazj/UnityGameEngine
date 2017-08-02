﻿using UnityEngine;
using System;
using System.Diagnostics;


//IN PROGRESS. Works with given height and free span only, for now
//[ExecuteInEditMode]
public class BezierCurve : MonoBehaviour {

	[HideInInspector] //makes it public, yet not displayed in the Inspector
	public Vector3[] points;
	[HideInInspector]
	public bool fs, height, embrasure, same = true;
	float freeSpan = -1f, archHeight = -1f, angle;
	//the case where angle is 0 degrees
	float inset = 0.5f;
	float yPos = 0f;

	public void SetSpringLine (string bla) {
		yPos = this.gameObject.GetComponent<Abutments> ().wallHeight;
		CheckUp ();
	}


	public void SetFreeSpan (string input) {
		freeSpan = ConditionCheck(input);
		if (freeSpan != -1f)
			fs = true;
		else
			fs = false;
		CheckUp ();
	}

	public void SetArchHeight (string input) {
		archHeight = ConditionCheck(input);
		if (archHeight != -1f) {
			height = true;
			if (archHeight == 0)
				angle = 0;
		}
		else
			height = false;
		CheckUp ();
	}
		
	public void SetArcOfEmbrasure (string input) {
		angle = ConditionCheck(input);
		if (angle != -1f) {
			embrasure = true;
			if (angle == 0)
				archHeight = 0;
		}
		else
			embrasure = false;
		CheckUp ();
	}

	float ConditionCheck (string input) {
		if (String.IsNullOrEmpty (input)) {
			return -1f;
		}
		return float.Parse(input);
	}
		
	public void ArchFreeSpan (bool isOn) {
		if (!isOn)
			SetFreeSpan ("");
		CheckUp ();
	}

	public void TheArchHeight (bool isOn) {
		if (!isOn)
			SetArchHeight ("");
		CheckUp ();
	}

	public void ArcOfEmbrasure (bool isOn) {
		if (!isOn)
			SetArcOfEmbrasure ("");
		CheckUp ();
	}


	void CheckUp () {
		//if no parameter is provided
		if (!fs && !height && !embrasure)
			return;
		//if only one parameter is provided
		else if ((fs && !height && !embrasure) || (!fs && height && !embrasure) || (!fs && !height && embrasure))
			return; 
		//if all three are given, check if they mathematically agree with each other
		else if (height && embrasure && fs) {
			float inputAngle = angle;
			AngleCalc ();
			if (!Mathf.Approximately (inputAngle, Mathf.Round(angle)))
				same = false;
		}
		//if two parameters are provided
		else {
			same = true;

			if (fs && height && !embrasure) {
				AngleCalc ();
			}

			if (fs && embrasure && !height) {
				HeightCalc ();
			}

			if (height && embrasure && !fs) {
				FreeSpanCalc ();
			}

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

			points = new Vector3[] {
				new Vector3 (-freeSpan / 2, yPos, 0f),
				new Vector3 (-freeSpan / 2 + freeSpan * inset, archHeight / 0.75f + yPos, 0f),
				new Vector3 (freeSpan / 2 - freeSpan * inset, archHeight / 0.75f + yPos, 0f),
				new Vector3 (freeSpan / 2, yPos, 0f)
			};
			this.gameObject.GetComponent<Abutments> ().SetAbutmentPositions (-freeSpan / 2);
			StackTrace stackTrace = new StackTrace();
			if (stackTrace.GetFrame (1).GetMethod ().Name != "SetSpringLine") {
				PrintingParameters ();
			}
		}
	}


	void PrintingParameters () {
		//Print the free span, height and arc of embrasure of the modeled arch
		UnityEngine.Debug.Log ("The free span of the arch is " + freeSpan.ToString ("n2"));
		UnityEngine.Debug.Log ("The height of the arch is " + archHeight.ToString ("n2"));
		UnityEngine.Debug.Log ("The arc of embrasure is " + angle.ToString ("n2") + " degrees");
	}

	void AngleCalc () {
		if (freeSpan <= 0 || archHeight < 0) {
			throw new Exception ("Inapplicable input value(s), check input");
		}
		else {
				float radius = (freeSpan * freeSpan / (8 * archHeight)) + (archHeight / 2);
				angle = (2 * Mathf.Asin (freeSpan / (2 * radius))) * Mathf.Rad2Deg;
		}
	}

	void HeightCalc () {
		//potentially cases where angle is <0 and >180
		if (freeSpan <= 0) {
			throw new Exception ("Inapplicable input value(s), check input");
		}
		else {
				archHeight = (freeSpan/2*(1-Mathf.Cos(Mathf.Deg2Rad*angle/2)))/Mathf.Sin(Mathf.Deg2Rad*angle/2);
		}
		
	}

	void FreeSpanCalc () {
		//potentially cases where angle is <0 and >180
		if (archHeight < 0) {
			throw new Exception ("Inapplicable input value(s), check input");
		}
		else {
			freeSpan = 2 * archHeight * Mathf.Sin (Mathf.Deg2Rad * angle / 2) / (1 - Mathf.Cos (Mathf.Deg2Rad * angle / 2));
		}
	}

	public Vector3 GetPoint (float t) {
		return transform.TransformPoint(Bezier.GetPoint(points[0], points[1], points[2], points[3], t));
	}

	//makes velocity vector's magnitude equal to one, so just shows the direction of the velocity
	public Vector3 GetDirection (float t) {
		return (transform.TransformPoint(Bezier.GetFirstDerivative(points[0], points[1], points[2], points[3], t)) - transform.position).normalized;
	}
}

/* in case angle and archHeight are provided, freeSpan can be calculated as 
 * freeSpan = 2 * archHeight * Mathf.Sin(Mathf.Deg2Rad * angle / 2) / (1 - Mathf.Cos(Mathf.Deg2Rad * angle / 2));
 * 
 * and in case angle and freeSpan are provided, archHeight can be calculated as
 * archHeight = (freeSpan / 2) * (1 - Mathf.Cos(Mathf.Deg2Rad * angle / 2)) / Mathf.Sin(Mathf.Deg2Rad * angle / 2);
 * 
 * These formulas should be incorporated in future, given any 2 of the 3 parameters, to be able to calculate
 * the third one, build a wireArc from it, and print all the parameters in the console */
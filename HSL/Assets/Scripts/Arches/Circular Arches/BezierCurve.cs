using UnityEngine;
using System;
using System.Diagnostics;

//some parts of this script are adapted from http://catlikecoding.com/unity/tutorials/curves-and-splines/
public class BezierCurve : MonoBehaviour {
	
	//makes it public, yet not displayed in the Inspector
	[HideInInspector] 
	//4 control point that make up the quadratic bezier curve used here
	public Vector3[] points;
	[HideInInspector]
	//these booleans track if the parameter was entered and still active (aka toggle was not turned off) in input fields
	//same boolean tracks if, input all 3 variables, they match each other. Set to true by default; the only case it is 
	//false is when input angle (aka arc of embrasure) is not the same as the angle calculated from other two parameters
	public bool fs, height, embrasure, same = true;
	//default values for parameters (-1 because they are not valid numbers)
	float freeSpan = -1f, archHeight = -1f, angle;
	//the inset is the factor by which points[0 and 3] need to be multiplied and offset in order to get desired curvature
	//See Final ReadMe for explanation of inset factor. 0.5 corresponds to the case where angle (aka arc of embrasure) is 0 degree
	float inset = 0.5f;
	//the distance between the spring line of the arch and the ground
	float yPos = 0f;


	//this method is activated when abutments for circular arches are built (button is pressed)
	public void SetSpringLine () {
		yPos = this.gameObject.GetComponent<Abutments> ().wallHeight;
		CheckUp ();
	}


	//following 3 functions: if the parameter toggle is turned off, it changes the parameter float value equal to default
	//-1, which then makes boolean variables false, and determines what action is taken in CheckUp function and in ArchBuild method
	public void ArchFreeSpan (bool isOn) {
		if (!isOn)
			SetFreeSpan ("");
	}


	public void TheArchHeight (bool isOn) {
		if (!isOn)
			SetArchHeight ("");
	}


	public void ArcOfEmbrasure (bool isOn) {
		if (!isOn)
			SetArcOfEmbrasure ("");
	}




	//following 3 functions deal with input from input fields for parameters. Each input field is connected to the corresponding function
	public void SetFreeSpan (string input) {
		freeSpan = ConditionCheck(input);
		if (freeSpan != -1f)
			//if after Condition check it is not =-1, then the input is valid, and the corresponding boolean variable is changed accordingly
			fs = true;
		else
			fs = false;
		CheckUp ();
	}


	public void SetArchHeight (string input) {
		archHeight = ConditionCheck(input);
		if (archHeight != -1f) {
			height = true;
			//if the arch height is equal to 0, then it is a flat arch, aka angle = 0
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
			//if angle is 0, then the arch is flat, aka height is 0
			if (angle == 0)
				archHeight = 0;
		}
		else
			embrasure = false;
		CheckUp ();
	}


	float ConditionCheck (string input) {
		//if the input is empty then keep the float value at default. This is also true when the toggle is turned off. See above functions
		if (String.IsNullOrEmpty (input)) {
			return -1f;
		}
		return float.Parse(input);
	}


	void CheckUp () {
		//reassign same every time, so that in case where it was set to false was corrected (eg three parameters were
		//input, they did not match each other, and the modeler turned one of the toggles off, which will now make a valid
		//case) the ArchBuild did not keep throwing exceptions because same is stuck to being false
		same = true;
		//if no parameter is provided
		if (!fs && !height && !embrasure)
			return;
		//if only one parameter is provided
		else if ((fs && !height && !embrasure) || (!fs && height && !embrasure) || (!fs && !height && embrasure))
			return; 
		//if all three are given, check if they mathematically agree with each other
		else if (height && embrasure && fs) {
			float inputAngle = angle;
			//in AngleCalc angle is assigned the value that agrees with other two parameters
			AngleCalc ();
			//if the calculated angle and the input angle do not agree, set same to false so 
			//that ArchBuild throw an exception when the modeler attempts to build something inapplicable
			if (!Mathf.Approximately (inputAngle, Mathf.Round (angle))) {
				same = false;
				//assign angle back its input value
				angle = inputAngle;
			}
		}
		//if two parameters are provided
		else {
			//if angle is not given calculate it
			if (fs && height && !embrasure) {
				AngleCalc ();
			}

			//if height is not given
			if (fs && embrasure && !height) {
				HeightCalc ();
			}

			//if free span is not given
			if (height && embrasure && !fs) {
				FreeSpanCalc ();
			}

			//assign inset factor based on what the angle is. See Final ReadMe for details
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

			//based on given/calculated parameters and the input factor, assign 4 control points
			points = new Vector3[] {
				new Vector3 (-freeSpan / 2, yPos, 0f),
				//See Final ReadMe for y positions of points[1 and 2]
				new Vector3 (-freeSpan / 2 + freeSpan * inset, archHeight / 0.75f + yPos, 0f),
				new Vector3 (freeSpan / 2 - freeSpan * inset, archHeight / 0.75f + yPos, 0f),
				new Vector3 (freeSpan / 2, yPos, 0f)
			};

			//feed the left point to the abutments (even if the abutments are not going to be built after all)
			this.gameObject.GetComponent<Abutments> ().SetAbutmentPositions (-freeSpan / 2);

			//if the calling method is SetSpringLine which is activated by the abutment related button, print the parameters in the console
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


	//See Final ReadMe for how to calculate the third parameter given 2 others
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


	//given the parameter t, which ranges from 0 (beginning) to 1 (end) and shows the progress along the curve, finds the local position of 
	//the point that corresponds to t progress along the curve (local) and translates this position into world space 
	public Vector3 GetPoint (float t) {
		return transform.TransformPoint(Bezier.GetPoint(points[0], points[1], points[2], points[3], t));
	}

	//makes velocity vector's magnitude equal to one, so just shows the direction of the velocity (first derivative of the curve)
	public Vector3 GetDirection (float t) {
		return (transform.TransformPoint(Bezier.GetFirstDerivative(points[0], points[1], points[2], points[3], t)) - transform.position).normalized;
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

//attach to the brick
public class CreateBrickPrefab : MonoBehaviour 
{

	public void TaskOnClick()
	{
		PrefabUtility.CreatePrefab ("Assets/Resources/createdBrick.prefab", this.gameObject);
	}
}

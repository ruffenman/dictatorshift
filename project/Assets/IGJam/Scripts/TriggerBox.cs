using UnityEngine;
using System.Collections;

public class TriggerBox : MonoBehaviour 
{
	public string FUNCTION_NAME_TO_CALL;
	public string VALID_NAME;
	public string VALID_TAG;
	public string HOW_TO_USE = "comments in .cs file";
	
	// HOW TO USE
	// STEP 1: Child this template to the GameObject containing the script you want to be AFFECTED
	// if you want to call DoStuff() in the MyStuff.cs script on MyObject, child this to MyObject
	// STEP 2: Enter the function name you want to call in MyStuff.cs, syntax for DoStuff(): DoStuff
	// STEP 3: Enter the names of GAME OBEJCTS

	void OnTriggerEnter(Collider other)
	{
		Debug.Log ("TriggerBox -- got trigger collision!");
		if(other.name == VALID_NAME || other.tag == VALID_TAG)
		{
			Debug.Log ("TriggerBox -- got valid collision!");
			other.SendMessageUpwards (FUNCTION_NAME_TO_CALL);
		}
	}
	
	void OnTriggerExit(Collider other)
	{
	
	}
}

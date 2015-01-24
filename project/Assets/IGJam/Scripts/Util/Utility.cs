using UnityEngine;
using System.Collections;

public class Utility 
{
	public delegate void CallbackFunction();
	public static IEnumerator Delay(float time, CallbackFunction function) 
	{
		yield return new WaitForSeconds(time);
		function();
	}
}

using UnityEngine;
using System.Collections;

public class DestroyIn : MonoBehaviour 
{
	public float time = 5;
	
	void Update()
	{
		time -= Time.deltaTime;
		if(time < 0)
		{
			Destroy (this.gameObject);
		}
	}
}

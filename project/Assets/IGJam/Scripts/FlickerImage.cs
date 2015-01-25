using UnityEngine;
using System.Collections;

public class FlickerImage : MonoBehaviour 
{
	float timeAlive = 0f;
	public float activateAfter;
	public float deactivateAfter;
	public SpriteRenderer image;
	
	void Update()
	{
		timeAlive += Time.deltaTime;
		if(timeAlive > activateAfter)
		{
			image.enabled = true;
		}
		if(timeAlive > deactivateAfter)
		{
			image.enabled = false;
		}
	}
}

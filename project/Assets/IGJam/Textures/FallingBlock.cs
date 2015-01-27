using UnityEngine;
using System.Collections;

public class FallingBlock : WorldObject 
{
	public float timeTilFall = 5f;
	public GameObject triggerObject;

	// Update is called once per frame
	public void InitiateFall() 
	{
		objectType = ObjectType.STABLE;
		Destroy(triggerObject);
		StartCoroutine(Utility.Delay(timeTilFall, Fall));
	}

	void Fall()
	{
		objectType = ObjectType.INTERACTIVE;
		Die();
		renderer.enabled = false;
		collider.enabled = false;
	}
}

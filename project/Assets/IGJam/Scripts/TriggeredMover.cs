using UnityEngine;
using System.Collections;

public class TriggeredMover : ToggleObject
{
	public Vector3 targetLocation;
	Vector3 startLocation;
	Vector3 movementStep;
	public float moveTime;
	float moveTimeRemaining;
	bool paused = false;
	
	void Update()
	{
		if(!paused)
		{
			moveTimeRemaining -= Time.deltaTime;
			this.gameObject.transform.Translate (movementStep * Time.deltaTime);
			if(moveTimeRemaining < 0)
			{
				paused = true;
			}
		}
	}

	protected override void Activate ()
	{
		paused = false;
		movementStep = targetLocation - startLocation;
		movementStep = movementStep / moveTime;
		moveTimeRemaining = moveTime;
	}
	
	protected override void Deactivate()
	{
		paused = true;
	}
}

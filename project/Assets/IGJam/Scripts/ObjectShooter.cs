using UnityEngine;
using System.Collections;

public class ObjectShooter : ToggleObject 
{
	public bool paused = false;
	public float delay = 3f;
	public Vector3 spawnVelocity;
	public GameObject spawnObject;

	public bool infiniteSpawn = true;

	float delayLeft = 0f;
	GameObject lastCreatedObject = null;

	public void SetPaused(bool pauseArg)
	{
		paused = pauseArg;
	}
	
	public void Shoot()
	{
		lastCreatedObject = (GameObject)Instantiate (spawnObject, this.gameObject.transform.position, this.gameObject.transform.rotation);
		if(lastCreatedObject.GetComponent<WorldObject>())
		{
			lastCreatedObject.GetComponent<WorldObject>().SetVelocity(spawnVelocity);
		}
	}
	
	protected override void Activate ()
	{
		SetPaused (false);
	}
	
	protected override void Deactivate()
	{
		SetPaused (true);
	}
	
	void Update()
	{
		if(!paused)
		{
			delayLeft -= Time.deltaTime;
			if(delayLeft < 0)
			{
				if ((lastCreatedObject == null) || (infiniteSpawn))
				{
					Shoot();
				}
				delayLeft = delay;
			}
		}
	}
	
	

}

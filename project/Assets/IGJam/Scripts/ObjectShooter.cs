using UnityEngine;
using System.Collections;

public class ObjectShooter : ToggleObject 
{
	public bool paused = false;
	public float delay = 3f;
	public Vector3 spawnVelocity;
	public GameObject spawnObject;
	
	float delayLeft = 0f;
	
	public void SetPaused(bool pauseArg)
	{
		paused = pauseArg;
	}
	
	public void Shoot()
	{
		GameObject go = (GameObject)Instantiate (spawnObject, this.gameObject.transform.position, this.gameObject.transform.rotation);
		go.GetComponent<WorldObject>().SetVelocity (spawnVelocity);
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
				Shoot ();
				delayLeft = delay;
			}
		}
	}
	
	

}

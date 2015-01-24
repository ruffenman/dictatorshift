using UnityEngine;
using System.Collections;

public class ToggleObject : WorldObject 
{
	public ToggleObject linkedComponent;
	public TYPE type;
	public COLLISION_TEST test;
	public string test_string;
	
	public enum TYPE
	{
		TRIGGER,
		TARGET
	}
	
	public enum COLLISION_TEST
	{
		NAME,
		TAG,
		HASCOMPONENT
	}
	
	void OnTriggerEnter(Collider other)
	{
		if(type == TYPE.TRIGGER)
		{;
			HandleTest (true, other);
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		if(type == TYPE.TRIGGER)
		{
			
			HandleTest (false, other);
		}
	}
	
	void HandleTest(bool enter, Collider other)
	{
		switch(test)
		{
			case COLLISION_TEST.NAME:
			{
				if(other.name == test_string)
				{
					Debug.Log ("Hitler");
					Debug.Log ("BOOL:" + enter);
					Toggle (enter);
				}
				break;
			}
		}
	}
	
	void Toggle(bool on)
	{
		if(on)
		{
			linkedComponent.Activate ();
		}
		else
		{
			linkedComponent.Deactivate();
		}
	}
	
	protected virtual void Activate()
	{
	
	}
	
	protected virtual void Deactivate()
	{
	
	}
	
}

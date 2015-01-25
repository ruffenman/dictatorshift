using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArmsCollider : MonoBehaviour 
{
	List<GameObject> interactibleCollisions = new List<GameObject>();

	public bool CanReturnInteractible()
	{
		return ((interactibleCollisions.Count > 0) && (interactibleCollisions[0] != null));
	}
	
	public GameObject GetFirstInteractible()
	{
		while(interactibleCollisions.Count > 0)
		{
			if(interactibleCollisions[0] == null)
			{
				interactibleCollisions.RemoveAt (0);
				Debug.LogWarning ("Arms.cs cleared invalid interactible from list");
			}
			else
			{
				return interactibleCollisions[0];
			}
		}
		return null;
	}
	
	public List<GameObject> GetAllInteractibles()
	{
		return interactibleCollisions;
	}
	
	// TODO MAKE SURE I DONT NEED ON COLLISION ENTER INSTEAD ******************************
	// TODO MAKE SURE I DONT NEED ON COLLISION ENTER INSTEAD ******************************
	
	void OnTriggerEnter(Collider other)
	{
		if(other.name != "Player" && other.GetComponent<WorldObject>() && 
            other.GetComponent<WorldObject>().objectType == WorldObject.ObjectType.INTERACTIVE)
		{
			ReportAddInteractiveCollision(other.gameObject);
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		if(other.name != "Player" && other.GetComponent<WorldObject>() && other.GetComponent<WorldObject>().objectType == WorldObject.ObjectType.INTERACTIVE) 
		{
			ReportRemoveInteractiveCollision(other.gameObject);
		}
	}
	
	// call from OnTriggerEnter() on pickup gameObject with collider/rigidbody
	void ReportAddInteractiveCollision(GameObject collision) 
	{
		if(interactibleCollisions.Contains (collision) == false)
		{
			Debug.Log ("ArmsCollider.cs -- new interact in range");
			interactibleCollisions.Add (collision);
		}
		else
		{
			Debug.LogWarning ("ArmsCollider.cs attempted to add duplicate interactible");
		}
		
	}
	
	// call from OnTriggerExit() on pickup gameObject with collider/rigidbody
	void ReportRemoveInteractiveCollision(GameObject collision)
	{
		if(interactibleCollisions.Contains (collision))
		{
			Debug.Log ("ArmsCollider.cs -- interact out of range");
			interactibleCollisions.Remove (collision);
		}
		else
		{
			Debug.LogWarning ("ArmsCollider.cs attempted to remove invalid interactible");
		}
	}
}
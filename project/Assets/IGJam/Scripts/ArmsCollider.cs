using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArmsCollider : MonoBehaviour 
{
	public static class TAGS
	{
		public static string NONE = "";
		public static string PICKUP = "pickup";
		public static string ATTACKABLE = "attackable";
	}
	
	List<GameObject> pickupCollisions = new List<GameObject>();
	List<GameObject> attackableCollisions = new List<GameObject>();
	
	public GameObject GetFirstPickup()
	{
		if(pickupCollisions.Count > 0)
	    {
			while(pickupCollisions.Count > 0 && pickupCollisions[0] == null)
			{
				pickupCollisions.RemoveAt (0);
				Debug.LogWarning ("Arms.cs cleared invalid pickup from list");
			}
			return pickupCollisions[0];
		}
		return null;
	}
	
	public GameObject GetFirstAttackable()
	{
		if(attackableCollisions.Count > 0)
		{
			while(attackableCollisions.Count > 0 && attackableCollisions[0] == null)
			{
				attackableCollisions.RemoveAt (0);
				Debug.LogWarning ("Arms.cs cleared invalid attackable from list");
			}
			return attackableCollisions[0];
		}
		return null;
	}
	
	// TODO MAKE SURE I DONT NEED ON COLLISION ENTER INSTEAD ******************************
	// TODO MAKE SURE I DONT NEED ON COLLISION ENTER INSTEAD ******************************
	
	void OnTriggerEnter(Collider other)
	{
		if(other.tag == TAGS.PICKUP)
		{
			ReportAddPickupCollision(other.gameObject);
		}
		else if(other.tag == TAGS.ATTACKABLE)
		{
			ReportAddAttackableCollision(other.gameObject);
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		if(other.tag == TAGS.PICKUP)
		{
			ReportRemovePickupCollision(other.gameObject);
		}
		else if(other.tag == TAGS.ATTACKABLE)
		{
			ReportRemoveAttackableCollision(other.gameObject);
		}
	}
	
	// call from OnTriggerEnter() on pickup gameObject with collider/rigidbody
	void ReportAddPickupCollision(GameObject collision) 
	{
		if(pickupCollisions.Contains (collision) == false)
		{
			Debug.Log ("ArmsCollider.cs -- new pickup in range");
			pickupCollisions.Add (collision);
		}
		else
		{
			Debug.LogWarning ("ArmsCollider.cs attempted to add duplicate attackable");
		}
		
	}
	
	// call from OnTriggerExit() on pickup gameObject with collider/rigidbody
	void ReportRemovePickupCollision(GameObject collision)
	{
		if(pickupCollisions.Contains (collision))
		{
			pickupCollisions.Remove (collision);
		}
		else
		{
			Debug.LogWarning ("ArmsCollider.cs attempted to remove invalid pickup");
		}
	}
	
	// call from OnTriggerEnter() on pickup gameObject with collider/rigidbody
	void ReportAddAttackableCollision(GameObject collision)
	{
		Debug.Log ("ArmsCollider.cs -- new attackable in range");
		if(attackableCollisions.Contains (collision) == false)
		{
			attackableCollisions.Add (collision);
		}
		else
		{
			Debug.LogWarning ("ArmsCollider.cs attempted to add duplicate attackable");
		}
	}
	
	// call from OnTriggerExit() on pickup gameObject with collider/rigidbody
	void ReportRemoveAttackableCollision(GameObject collision)
	{
		if(attackableCollisions.Contains (collision))
		{
			attackableCollisions.Remove (collision);
		}
		else
		{
			Debug.LogWarning ("ArmsCollider.cs attempted to remove invalid attackable");
		}
	}
}
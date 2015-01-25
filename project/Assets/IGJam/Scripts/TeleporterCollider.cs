using UnityEngine;
using System.Collections;

public class TeleporterCollider : WorldObject
{	
	void OnTriggerEnter(Collider other)
	{
		// if its the player do certain stuff
	}
	
	void OnTriggerExit(Collider other)
	{
		// I don't really expect any behavior to go in here
	}
}

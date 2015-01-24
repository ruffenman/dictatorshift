using UnityEngine;
using System.Collections;

public class DeadlyObject : WorldObject 
{
	float damage = 100.0f;

	void OnCollideWithTarget(ref WorldObject worldObject)
	{
		if (worldObject != null)
		{
			worldObject.TakeDamage(damage);
			Destroy(gameObject);
		}
	}
}
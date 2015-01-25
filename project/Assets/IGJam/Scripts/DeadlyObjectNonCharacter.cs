using UnityEngine;
using System.Collections;

public class DeadlyObjectNonCharacter : WorldObjectNonCharacter
{
	public bool destroyOnCollision = false;
	float damage = 100.0f;
	
	public void OnCollideWithTarget(GameObject target)
	{
		WorldObject worldObject = target.GetComponent<WorldObject>();
		if (worldObject != null)
		{
			worldObject.TakeDamage(damage);
			if(destroyOnCollision)
			{
				Destroy(gameObject);
			}
		}
	}
}
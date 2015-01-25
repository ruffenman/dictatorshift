using UnityEngine;
using System.Collections;

public class DeadlyObject : WorldObject 
{
	public bool destroyOnCollision = false;
	protected float damage = 100.0f;

	public virtual void OnCollideWithTarget(GameObject target)
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

	protected virtual void OnDestroyed()
	{
	}
}
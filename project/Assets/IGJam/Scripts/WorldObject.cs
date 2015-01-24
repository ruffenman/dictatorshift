using UnityEngine;
using System.Collections;

public class WorldObject : MonoBehaviour 
{
	public enum ObjectType
	{
		INTERACTIVE,
		STABLE
	}

	public ObjectType objectType;
	private float health = 100;

	public void TakeDamage(float damage)
	{
		if (objectType == ObjectType.INTERACTIVE)
		{
			health = Mathf.Max(0, health - damage);
			UpdateHealth();
		}
	}

	public void ApplyVelocity(Vector3 newVelocity)
	{
	}

	void UpdateHealth()
	{
		if (health == 0)
		{
			Destroy(gameObject);
		}
	}
}

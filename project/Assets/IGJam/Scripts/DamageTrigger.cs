using UnityEngine;
using System.Collections;

public class DamageTrigger : MonoBehaviour 
{
	public float damage = 500;

	void OnTriggerEnter(Collider collider)
	{
		WorldObject worldObject = collider.gameObject.GetComponent<WorldObject>();
		if (worldObject)
		{
			worldObject.TakeDamage(damage);
		}
	}
}

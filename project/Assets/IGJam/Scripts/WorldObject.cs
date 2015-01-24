using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class WorldObject : MonoBehaviour 
{
	static readonly float gravity = 2f;

	public enum ObjectType
	{
		INTERACTIVE,
		STABLE
	}

	// properties
	public ObjectType objectType;
	public float moveDepreciation = 0.9f;
	public float stopVelocitySquared = 1.0f;

	// utility
	private float health = 100;
	CharacterController controller;
	Vector3 velocity;

	public void Start()
	{
		controller = GetComponent<CharacterController>();
	}

	public virtual void Update()
	{
		if (objectType == ObjectType.INTERACTIVE)
		{
			Move();
		}
	}

	public void TakeDamage(float damage)
	{
		if (objectType == ObjectType.INTERACTIVE)
		{
			health = Mathf.Max(0, health - damage);
			UpdateHealth();
		}
	}

	public void AddVelocity(Vector3 addedVelocity)
	{
		SetVelocity(velocity + addedVelocity);
	}

	public void SetVelocity(Vector3 newVelocity)
	{
		velocity = newVelocity;
	}

	void UpdateHealth()
	{
		if (health == 0)
		{
			Die();
		}
	}

	protected virtual void Die()
	{
		Destroy(gameObject);
	}

	void Move()
	{
		Vector3 newVelocity = velocity;

		// gravity
		if (controller.isGrounded)
		{
			newVelocity.y = 0;
		}
		else
		{
			newVelocity.y -= gravity;
		}

		// drag
		if (newVelocity.sqrMagnitude > stopVelocitySquared)
		{
			newVelocity.x *= moveDepreciation;
		}
		else
		{
			newVelocity.x = 0;
		}

		controller.Move(newVelocity * Time.deltaTime);
		SetVelocity(newVelocity);
	}
}

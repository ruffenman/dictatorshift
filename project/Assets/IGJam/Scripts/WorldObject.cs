using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class WorldObject : MonoBehaviour 
{
	static readonly float gravity = 5f;

	public enum ObjectType
	{
		INTERACTIVE,
		STABLE
	}

	// properties
	public ObjectType objectType;
	public float moveDepreciation = 0.9f;
	public float stopVelocity = 1.0f;

	// utility
	protected float health = 100;
	protected CharacterController controller;
	protected Vector3 velocity;

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

	public Vector3 GetVelocity()
	{
		return velocity;
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
		if ((velocity.sqrMagnitude > 0) || (!controller.isGrounded))
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
			if (Mathf.Abs(newVelocity.x) > stopVelocity)
			{
				newVelocity.x *= moveDepreciation;
			}
			else
			{
				newVelocity.x = 0;
			}

			controller.Move((newVelocity + new Vector3(0.0001f, 0, 0)) * Time.deltaTime);
			SetVelocity(newVelocity);
		}
	}
}

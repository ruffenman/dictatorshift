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
	public float groundedOffset = 0.5f;
	public LayerMask groundedLayerMask;
	public bool useFixedUpdate = true;

	// utility
	protected bool grounded;
	protected float health = 100;
	private bool physicsEnabled = true;

	private CharacterController controller;
	private Vector3 velocity;

	public void Start()
	{
		controller = GetComponent<CharacterController>();
	}

	void Update()
	{
		if (!useFixedUpdate)
		{
			UpdateInternal();
		}
	}

	void FixedUpdate()
	{
		if (useFixedUpdate)
		{
			UpdateInternal();
		}
	}

	protected virtual void UpdateInternal()
	{
		if ((objectType == ObjectType.INTERACTIVE) && (physicsEnabled))
		{
			Move();
		}
		UpdateGrounded();
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

	public void SetPhysicsEnabled(bool enabled)
	{
		physicsEnabled = enabled;
		collider.enabled = enabled;
		controller.enabled = enabled;
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

	void UpdateGrounded()
	{
		Vector3 center = transform.position;
		center.y -= groundedOffset;
		float radius = transform.localScale.x / 2;
		grounded = Physics.CheckSphere(center, radius, groundedLayerMask);
	}

	/*void OnDrawGizmos()
	{
		Vector3 center = transform.position;
		center.y -= groundedOffset;
		float radius = transform.localScale.x / 2;
		Gizmos.DrawSphere(center, radius);
	}*/

	void Move()
	{
		if ((velocity.sqrMagnitude > 0) || (!grounded))
		{
			Vector3 newVelocity = velocity;

			// gravity
			if (grounded && (newVelocity.y < 0))
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

using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(CharacterController))]
public class WorldObject : MonoBehaviour 
{
	public float gravity = 5f;

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
	public GameObject deathParticles;

	// utility
	protected bool grounded;
	protected float health = 100;
	private bool physicsEnabled = true;

	private CharacterController controller;
	private Vector3 velocity;

    public Action OnDestroyed;

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
			UpdateGrounded();
		}
		if(transform.position.y < -100)
		{
			Destroy (gameObject);
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
		if (deathParticles)
		{
            foreach(SpriteRenderer sprite in GetComponentsInChildren<SpriteRenderer>())
            {
                sprite.color = Color.clear;
            }
			deathParticles.particleSystem.Play();
			StartCoroutine(Utility.Delay(deathParticles.particleSystem.duration + deathParticles.particleSystem.startLifetime, FinishDestroy));
		}
		else
		{
			FinishDestroy();
        }

        JamGame.instance.soundManager.PlaySfx(SoundManager.SFX_BREAK);
	}

	protected void FinishDestroy()
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

	void OnDrawGizmos()
	{
		if (objectType == ObjectType.INTERACTIVE)
		{
			Vector3 center = transform.position;
			center.y -= groundedOffset;
			float radius = transform.localScale.x / 2;
			Gizmos.DrawWireSphere(center, radius);
		}
	}

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

    private void OnDestroy()
    {
        if(!Application.isEditor)
        {
            if(OnDestroyed != null)
            {
                OnDestroyed();
            }
        }
    }
}

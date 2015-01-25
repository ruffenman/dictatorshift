using UnityEngine;
using System.Collections;

public class Drone : DeadlyObject 
{
	// properties
	public float explosionTime = 0.43f;
	public float flySpeed = 2;
	public float shouldFollowDistance = 10f;
	public GameObject target;
	public float hoverDistance = 1.5f;
	public LayerMask dontCollideLayermask;
	float frontOfDroneOffset = 1.0f;

	// utility
	private Animator animator;
	private bool dead = false;

	new void Start () 
	{
		base.Start();
		animator = GetComponent<Animator>();
		CombinedPlayer player = FindObjectOfType<CombinedPlayer>();
		target = player.gameObject;
	}

	void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(transform.position, shouldFollowDistance);
	}

	void Update () 
	{
		if ((target != null) && !dead)
		{
			Vector3 toTarget = target.transform.position - transform.position;
			float distance = toTarget.sqrMagnitude;
			if (distance < (shouldFollowDistance * shouldFollowDistance))
			{
				animator.Play("Fly");
				Vector3 velocity = toTarget.normalized * flySpeed;
				Vector3 origin = transform.position;
				origin.x -= frontOfDroneOffset;

				RaycastHit hit;
				Debug.DrawRay(origin, Vector3.down * hoverDistance, Color.red);
				if (Physics.Raycast(origin, Vector3.down, out hit, hoverDistance, dontCollideLayermask))
				{
					if (!hit.collider.gameObject.CompareTag("Player"))
					{
						velocity.y = 0;
					}
				}

				SetVelocity(velocity);
			}
		}
	}

	public override void OnCollideWithTarget(GameObject target)
	{
		WorldObject worldObject = target.GetComponent<WorldObject>();
		if (worldObject != null)
		{
			worldObject.TakeDamage(damage);
			if (destroyOnCollision)
			{
				animator.Play("Explode");
				dead = true;
				StartCoroutine(Utility.Delay(explosionTime, CompleteDestruction));
			}
		}
	}

	private void CompleteDestruction()
	{
		Destroy(gameObject);
	}
}

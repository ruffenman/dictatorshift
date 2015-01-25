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
	private bool dead;
	private float initialLocalScaleX;
	private bool goingLeft;

	new void Start () 
	{
		base.Start();
		animator = GetComponent<Animator>();
		CombinedPlayer player = FindObjectOfType<CombinedPlayer>();
		target = player.gameObject;
		initialLocalScaleX = transform.localScale.x;
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
				Vector3 newVelocity = toTarget.normalized * flySpeed;
				Vector3 origin = transform.position;

				goingLeft = newVelocity.x < 0;

				float offset = goingLeft ? -frontOfDroneOffset : frontOfDroneOffset;
				origin.x += offset;

				RaycastHit hit;
				Debug.DrawRay(origin, Vector3.down * hoverDistance, Color.red);
				if (Physics.Raycast(origin, Vector3.down, out hit, hoverDistance, dontCollideLayermask))
				{
					if (!hit.collider.gameObject.CompareTag("Player"))
					{
						newVelocity.y = 0;
					}
				}

				SetVelocity(newVelocity);
				animator.Play("Fly");

				if (newVelocity.x != 0)
				{
					float newScale = goingLeft ? -initialLocalScaleX : initialLocalScaleX;
					transform.localScale = new Vector3(newScale, transform.localScale.y, transform.localScale.z);
				}
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

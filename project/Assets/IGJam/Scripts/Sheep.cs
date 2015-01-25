using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sheep : DeadlyObject
{
	//properties
	public float walkRange = 10;
	public float sightRange = 15;
	public int numberOfRays = 3;
	public float closeEnoughToJumpDistance = 1f;
	public float closeEnoughToRunDistance = 2f;
	public float walkSpeed = 1f;
	public float jogSpeed = 2f;
	public float runSpeed = 3f;
	public float jumpSpeed = 10f;

	// utility
	private Vector3 startingPosition;
	private float leftBound;
	private float rightBound;
	private bool facingLeft;

	void OnDrawGizmos()
	{
		Vector3 start = transform.position;
		start.x -= walkRange / 2;

		Vector3 end = transform.position;
		end.x += walkRange / 2;

		Gizmos.DrawLine(start, end);
	}

	new void Start () 
	{
		base.Start();
		leftBound = transform.position.x - walkRange / 2;
		rightBound = transform.position.x + walkRange / 2;
	}

	protected override void UpdateInternal() 
	{
		List<GameObject> found = LookAhead();
		float speed = 0;
		Vector3 direction = facingLeft ? Vector3.left : Vector3.right;
		GameObject target = null;

		// if something in your way
		if (found != null)
		{
			GameObject closest = null;
			float closestDistance = float.MaxValue;

			// determine target
			foreach (GameObject go in found)
			{
				CombinedPlayer player = go.GetComponent<CombinedPlayer>();
				if (player != null)
				{
					target = go;
				}
				float distance = (go.transform.position - transform.position).sqrMagnitude;
				if (distance < closestDistance)
				{
					closestDistance = distance;
					closest = go;
				}
			}

			// if something is in your way and in jump range
			if ((closest != target) && (closestDistance <= (closeEnoughToJumpDistance * closeEnoughToJumpDistance)))
			{
				// jump
				Jump();
			}

			// if there's a target
			if (target != null)
			{
				direction = (target.transform.position.x < transform.position.x) ? Vector3.left : Vector3.right;

				// if in run range
				float distance = (target.transform.position - transform.position).sqrMagnitude;
				if (distance <= (closeEnoughToRunDistance * closeEnoughToRunDistance))
				{
					// run toward them
					speed = runSpeed;
				}
				else
				{
					// jog toward them
					speed = jogSpeed;
				}
			}
		}

		if(target == null)
		{
			// follow path at walk speed
			speed = walkSpeed;

			if (((transform.position.x < leftBound) && facingLeft) || ((transform.position.x > rightBound) && !facingLeft))
			{
				TurnAround();
				direction = -direction;
			}
		}

		AddVelocity(direction * speed);

		base.UpdateInternal();
	}

	void Jump()
	{
		AddVelocity(Vector3.up * jumpSpeed);
	}

	void TurnAround()
	{
		facingLeft = !facingLeft;
	}

	List<GameObject> LookAhead()
	{
		List<GameObject> allFound = new List<GameObject>();
		float raycastBuffer = 0.2f;

		Vector3 origin = transform.position;
		origin.y = (origin.y - (transform.localScale.y / 2f)) + raycastBuffer;
		Vector3 direction = facingLeft ? Vector3.left : Vector3.right;

		float increaseHeight = (transform.localScale.y - (2 * raycastBuffer)) / ((float)numberOfRays - 1);

		for (int i = 0; i < numberOfRays; ++i)
		{
			Debug.DrawRay(origin, direction * sightRange, Color.red);
			RaycastHit hit;
			if (Physics.Raycast(origin, direction, out hit, sightRange))
			{
				allFound.Add(hit.collider.gameObject);
			}
			origin.y += increaseHeight;
		}
		
		if (allFound.Count > 0)
		{
			return allFound;
		}
		return null;
	}
}

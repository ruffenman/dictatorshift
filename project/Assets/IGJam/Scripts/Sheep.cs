using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sheep : DeadlyObject
{
	//properties
	public float walkRange = 10;
	public float sightRange = 15;
	public float raycastWidth = 1.5f;
	public int numberOfRays = 3;
	public float closeEnoughToJumpDistance = 1f;
	public float closeEnoughToRunDistance = 2f;
	public float walkSpeed = 1f;
	public float jogSpeed = 2f;
	public float runSpeed = 3f;
	public float jumpSpeed = 10f;
	public float explosionTime = 0.43f;
	public float stopTime = 1f;
	public float perfectSpeedForJump = 5f;

	// utility
	private Vector3 startingPosition;
	private float leftBound;
	private float rightBound;
	private bool facingLeft;
	private Animator animator;
	private bool dead = false;
	private bool stopped = false;
	private GameObject target;

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
		animator = GetComponent<Animator>();
	}

	protected override void UpdateInternal() 
	{
		if (!dead)
		{
			List<GameObject> found = LookAhead();
			float speed = 0;
			Vector3 direction = facingLeft ? Vector3.left : Vector3.right;
			target = null;

			// if something in your way
			if (found != null)
			{
				GameObject closest = null;
				float closestDistance = float.MaxValue;

				// determine target
				foreach (GameObject go in found)
				{
					if (go.CompareTag("Player"))
					{
                        if(!target)
                        {
                            JamGame.instance.soundManager.PlaySfx(SoundManager.SFX_SHEEP);
                        }
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
					if (grounded)
					{
						Jump();
					}
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

			if ((target == null) && !stopped)
			{
				// follow path at walk speed
				speed = walkSpeed;

				if (((transform.position.x < leftBound) && facingLeft) || ((transform.position.x > rightBound) && !facingLeft))
				{
					InitiateTurnAround();
					direction = -direction;
				}
			}

			// animation
			string animState = "Walk";

			if (!grounded || (speed == jogSpeed))
			{
				animState = "Hop";
			}
			else if (speed == runSpeed)
			{
				animState = "Run";
			}
			else if (stopped)
			{
				animState = "Idle";
			}
			animator.Play(animState);

			AddVelocity(direction * speed);

			base.UpdateInternal();
		}
	}

	void Jump()
	{
		Vector3 direction = facingLeft ? Vector3.left : Vector3.right;
		float ratio = GetVelocity().x / perfectSpeedForJump;
        AddVelocity(((Vector3.up * jumpSpeed) + (direction * jumpSpeed / 3)) * ratio);
	}

	void Stop()
	{
		stopped = true;
		Vector3 vel = GetVelocity();
		SetVelocity(new Vector3(0, vel.y, 0));
	}

	void Resume()
	{
		stopped = false;
	}

	void TurnAround()
	{
		facingLeft = !facingLeft;
		transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
		if (stopped)
		{
			Resume();
		}
	}

	void InitiateTurnAround()
	{
		Stop();
		StartCoroutine(Utility.Delay(stopTime, TurnAround));
	}

	List<GameObject> LookAhead()
	{
		List<GameObject> allFound = new List<GameObject>();
		float raycastBuffer = 0.2f;

		Vector3 origin = transform.position;
		origin.y = (origin.y - (raycastWidth / 2f)) + raycastBuffer;
		Vector3 direction = facingLeft ? Vector3.left : Vector3.right;

		float increaseHeight = (raycastWidth - (2 * raycastBuffer)) / ((float)numberOfRays - 1);

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

	public override void OnCollideWithTarget(GameObject target)
	{
		WorldObject worldObject = target.GetComponent<WorldObject>();
		if (worldObject != null)
		{
			worldObject.TakeDamage(damage);
			if (destroyOnCollision)
			{
				SoundManager.instance.PlaySfx (SoundManager.SFX_EXPLOSION);
				animator.Play("Explode");
				dead = true;
				StartCoroutine(Utility.Delay(explosionTime, CompleteDestruction));

                JamGame.instance.soundManager.PlaySfx(SoundManager.SFX_EXPLOSION);
			}
		}
	}

	private void CompleteDestruction()
	{
		Destroy(gameObject);
	}
}

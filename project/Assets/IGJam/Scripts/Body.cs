using UnityEngine;
using System.Collections;

public class Body : BodyPart 
{
	//properties
	private float thrustSpeed = 10f;
	private float cooldownTime = 1f;
	private float speedDepreciation = 0.9f;
	private float stopSpeedSquared = 2;

	// utility
	private bool canThrust;
	private float cooldownTimer;
	private Vector3 bodyVelocity;

	// constructor
	public Body(int newPlayerIndex, Transform newSpriteTransform, CombinedPlayer newCombinedPlayer)
		: base(BodyPart.BodyPartType.BODY, newPlayerIndex, newSpriteTransform, newCombinedPlayer)
	{
		cooldownTimer = cooldownTime;
	}

	public override void Update()
	{
		UpdateCooldownTimer();
		if (bodyVelocity != Vector3.zero)
		{
			//Debug.Log("Adding jump"/*"Current velocity : " + combinedPlayer.GetVelocity()*/);
			combinedPlayer.AddVelocity(bodyVelocity);
			if (bodyVelocity.sqrMagnitude > stopSpeedSquared)
			{
				bodyVelocity = bodyVelocity * speedDepreciation;
			}
			else
			{
				bodyVelocity = Vector3.zero;
			}
		}
	}

	private void UpdateCooldownTimer()
	{
		if (!canThrust)
		{
			if (cooldownTimer > 0)
			{
				cooldownTimer -= Time.deltaTime;
			}

			// if timer is up, exit cooldown
			else
			{
				ExitCooldown();
			}
		}
	}

	private void EnterCooldown()
	{
		canThrust = false;
		cooldownTimer = cooldownTime;
	}

	private void ExitCooldown()
	{
		canThrust = true;
	}

	protected override void OnInputReceived()
	{
		// if not in cooldown
		if (canThrust)
		{
			// and received button press and direction
			if (lastInputState.actionJustPressed && (lastInputState.directionVec != Vector3.zero))
			{
				bodyVelocity += lastInputState.directionVec * thrustSpeed;
				EnterCooldown();
			}
		}
	}
}

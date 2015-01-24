using UnityEngine;
using System.Collections;

public class Body : BodyPart 
{
	//properties
	private float thrustSpeed = 2f;
	private float cooldownTime = 1f;

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
			combinedPlayer.AddPlayerVelocity(bodyVelocity);
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
			if (lastInputState.actionJustPressed && (lastInputState.direction != Vector3.zero))
			{
				bodyVelocity += lastInputState.direction * thrustSpeed;
				EnterCooldown();
			}
		}
	}
}

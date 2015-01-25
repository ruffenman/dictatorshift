using UnityEngine;
using System.Collections;

public class Body : BodyPart 
{
	//properties
	private float thrustSpeedVertical = 10f;
	private float thrustSpeedHorizontal = 10f;
	private float thrustSpeedDiagonal = 14f;
	private float cooldownTime = 0.5f;
	private float speedDepreciation = 0.9f;
	private float stopSpeedSquared = 2;

	// utility
	private bool canThrust;
	private float cooldownTimer;
	private Vector3 bodyVelocity;

    public bool isFacingLeft = false;

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
        switch(lastInputState.direction)
        {
            case IGJInputManager.InputDirection.UpRight:
            case IGJInputManager.InputDirection.Right:
            case IGJInputManager.InputDirection.DownRight:
            {
                isFacingLeft = false;
                spriteTransform.localScale = new Vector3(1, 1, 1);
            } break;

            case IGJInputManager.InputDirection.UpLeft:
            case IGJInputManager.InputDirection.Left:
            case IGJInputManager.InputDirection.DownLeft:
            {
                isFacingLeft = true;
                spriteTransform.localScale = new Vector3(-1, 1, 1);
            } break;
        }

		// if not in cooldown
		if (canThrust)
		{
			// and received button press and direction
			if (lastInputState.actionJustPressed)
			{
				SoundManager.instance.PlaySfx (SoundManager.SFX_JUMP);
				float speed = thrustSpeedVertical;
				if ((lastInputState.direction == IGJInputManager.InputDirection.DownLeft) 
					|| (lastInputState.direction == IGJInputManager.InputDirection.DownRight)
					|| (lastInputState.direction == IGJInputManager.InputDirection.UpLeft)
					|| (lastInputState.direction == IGJInputManager.InputDirection.UpRight))
				{
					speed = thrustSpeedDiagonal;
				}
				else if ((lastInputState.direction == IGJInputManager.InputDirection.Left) 
				|| (lastInputState.direction == IGJInputManager.InputDirection.Right))
				{
					speed = thrustSpeedHorizontal;
				}

				if (lastInputState.directionVec != Vector3.zero)
				{
					bodyVelocity += lastInputState.directionVec * speed;
				}
				else
				{
					bodyVelocity.y += speed;
				}

				EnterCooldown();
			}
		}
	}
}

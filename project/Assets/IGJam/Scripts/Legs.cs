using UnityEngine;
using System.Collections;

public class Legs : BodyPart 
{
	// properties
    private float runSpeed = 7f;
    private float walkSpeed = 4f;
    private float acceleration = 5f;
	private float speedDepreciation = 0.9f;
    private float stopSpeed = Mathf.Sqrt(2);
    private float stopSpeedSquared = 2;
	
	// utility
    private Vector3 legVelocity;
    private Vector3 legAcceleration;
	Animator animator;
	private bool goingLeft;

	public Legs(int newPlayerIndex, Transform newSpriteTransform, CombinedPlayer newCombinedPlayer)
		: base(BodyPart.BodyPartType.LEGS, newPlayerIndex, newSpriteTransform, newCombinedPlayer)
	{
		animator = newSpriteTransform.gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	public override void Update () 
	{
        // acceleration
        Vector3 deltaVel = Vector3.zero;
        if(legAcceleration != Vector3.zero)
        {
            deltaVel = legAcceleration * Time.deltaTime;
            legVelocity += deltaVel;

            // Make sure we are at least moving at stop speed
            if (legVelocity.x < 0)
            {
                legVelocity.x = Mathf.Min(legVelocity.x - stopSpeed, legVelocity.x);
            }
            else
            {
                legVelocity.x = Mathf.Max(legVelocity.x + stopSpeed, legVelocity.x);
            }

            // Speed tops out based on run button
            float speedCap = lastInputState.actionPressed ? runSpeed : walkSpeed;
            legVelocity.x = legVelocity.x < 0 ? Mathf.Max(-speedCap, legVelocity.x) : Mathf.Min(speedCap, legVelocity.x);
        }

		if (legVelocity != Vector3.zero)
		{
			combinedPlayer.AddVelocity(legVelocity);
        }

		if (legVelocity.sqrMagnitude > stopSpeedSquared)
		{
			legVelocity = legVelocity * speedDepreciation;
		}
		else
		{
			legVelocity = Vector3.zero;
        }

		SetAnimation();
	}

	void SetAnimation()
	{
		if (Mathf.Abs(legVelocity.x) > 0)
		{
			animator.Play("Walk");
		}
		else
		{
			animator.Play("Idle");
		}
	}

	protected override void OnInputReceived()
	{
		if (lastInputState.direction != IGJInputManager.InputDirection.None)
		{
            legAcceleration.x = lastInputState.directionVec.x < 0 ? -acceleration : acceleration;
            if(legAcceleration.x > 0 && legVelocity.x < 0 || legAcceleration.x < 0 && legVelocity.x > 0)
            {
                legVelocity.x = 0;
            }

			if ((!goingLeft && (lastInputState.directionVec.x < 0)) || (goingLeft && (lastInputState.directionVec.x > 0)))
			{
                goingLeft = lastInputState.directionVec.x < 0;
                spriteTransform.localScale = new Vector3(spriteTransform.localScale.x * (goingLeft ? -1 : 1), 
                    spriteTransform.localScale.y, spriteTransform.localScale.z);
			}
		}
        else
        {
            legAcceleration.x = 0;
        }
	}
}

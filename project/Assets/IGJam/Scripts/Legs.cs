using UnityEngine;
using System.Collections;

public class Legs : BodyPart 
{
	// properties
	private float runSpeed = 10f;
	private float speedDepreciation = 0.9f;
	private float stopSpeedSquared = 2;
	
	// utility
	private Vector3 legVelocity;
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
		if (lastInputState.actionJustPressed && (lastInputState.directionVec != Vector3.zero))
		{
			legVelocity.x += (lastInputState.directionVec.x * runSpeed);
			if ((!goingLeft && (lastInputState.directionVec.x < 0)) || (goingLeft && (lastInputState.directionVec.x > 0)))
			{
				SwitchDirection();
			}
		}
	}

	void SwitchDirection()
	{
		goingLeft = !goingLeft;
		spriteTransform.localScale = new Vector3(-spriteTransform.localScale.x, spriteTransform.localScale.y, spriteTransform.localScale.z);
	}
}

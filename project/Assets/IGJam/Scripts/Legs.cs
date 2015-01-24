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

	public Legs(int newPlayerIndex, Transform newSpriteTransform, CombinedPlayer newCombinedPlayer)
		: base(BodyPart.BodyPartType.LEGS, newPlayerIndex, newSpriteTransform, newCombinedPlayer)
	{
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
	}

	protected override void OnInputReceived()
	{
		if (lastInputState.actionJustPressed && (lastInputState.directionVec != Vector3.zero))
		{
			legVelocity.x += (lastInputState.directionVec.x * runSpeed);
		}
	}
}

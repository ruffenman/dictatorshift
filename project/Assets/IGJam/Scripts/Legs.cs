using UnityEngine;
using System.Collections;

public class Legs : BodyPart 
{
	private Vector3 legVelocity;
	private float runSpeed;

	public Legs(int newPlayerIndex, Transform newSpriteTransform, CombinedPlayer newCombinedPlayer)
		: base(BodyPart.BodyPartType.LEGS, newPlayerIndex, newSpriteTransform, newCombinedPlayer)
	{
	}
	
	// Update is called once per frame
	public override void Update () 
	{
		if (legVelocity != Vector3.zero)
		{
			combinedPlayer.AddPlayerVelocity(legVelocity);
		}
	}

	protected override void OnInputReceived()
	{
		if (lastInputState.actionJustPressed && (lastInputState.direction != Vector3.zero))
		{
			legVelocity.x += (lastInputState.direction.x * runSpeed);
		}
	}
}

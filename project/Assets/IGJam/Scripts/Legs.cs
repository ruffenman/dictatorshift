using UnityEngine;
using System.Collections;

public class Legs : BodyPart 
{
	private Vector3 legVelocity;
	private float runSpeed = 1f;

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
	}

	protected override void OnInputReceived()
	{
        if (lastInputState.actionJustPressed && (lastInputState.direction != IGJInputManager.InputDirection.None))
		{
			legVelocity.x += (lastInputState.direction.x * runSpeed);
		}
	}
}

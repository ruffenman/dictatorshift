using UnityEngine;
using System.Collections;

public class Body : BodyPart 
{
	public Body(int newPlayerIndex, Transform newSpriteTransform, CombinedPlayer newCombinedPlayer)
		: base(BodyPart.BodyPartType.BODY, newPlayerIndex, newSpriteTransform, newCombinedPlayer)
	{
	}
	
	// Update is called once per frame
	public override void Update() 
	{
	
	}
}

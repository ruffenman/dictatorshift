using UnityEngine;
using System.Collections;

public class Legs : BodyPart 
{
	public Legs(int newPlayerIndex, Transform newSpriteTransform, CombinedPlayer newCombinedPlayer)
		: base(BodyPart.BodyPartType.LEGS, newPlayerIndex, newSpriteTransform, newCombinedPlayer)
	{
	}
	
	// Update is called once per frame
	public override void Update () 
	{
	
	}
}

using UnityEngine;
using System.Collections;

public class Legs : BodyPart 
{
	public Legs(int newPlayerIndex, Transform newSpriteTransform)
		: base(BodyPart.BodyPartType.LEGS, newPlayerIndex, newSpriteTransform)
	{
	}
	
	// Update is called once per frame
	public override void Update () 
	{
	
	}
}

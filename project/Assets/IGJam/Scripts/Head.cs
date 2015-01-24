using UnityEngine;
using System.Collections;

public class Head : BodyPart
{
	public Head(int newPlayerIndex, Transform newSpriteTransform)
		: base(BodyPart.BodyPartType.HEAD, newPlayerIndex, newSpriteTransform)
	{
	}

	// Update is called once per frame
	public override void Update() 
	{
	
	}
}

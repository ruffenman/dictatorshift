using UnityEngine;
using System.Collections;

public class Body : BodyPart 
{	
	public Body(int newPlayerIndex, Transform newSpriteTransform)
		: base(BodyPart.BodyPartType.BODY, newPlayerIndex, newSpriteTransform)
	{
	}
	
	// Update is called once per frame
	public override void Update() 
	{
	
	}
}

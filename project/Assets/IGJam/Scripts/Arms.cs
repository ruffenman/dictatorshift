using UnityEngine;
using System.Collections;

public class Arms : BodyPart 
{	
	public Arms(int newPlayerIndex, Transform newSpriteTransform)
		: base(BodyPart.BodyPartType.ARMS, newPlayerIndex, newSpriteTransform)
	{
	}
	
	// Update is called once per frame
	public override void Update()
	{
	
	}
}

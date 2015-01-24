using UnityEngine;
using System.Collections;

public class Body : BodyPart 
{	
	public Body(int newPlayerIndex)
		: base(BodyPart.BodyPartType.BODY, newPlayerIndex)
	{
	}
	
	// Update is called once per frame
	public override void Update() 
	{
	
	}
}

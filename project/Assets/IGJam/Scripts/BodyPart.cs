using UnityEngine;
using System.Collections;

public class BodyPart
{
	public enum BodyPartType
	{
		HEAD,
		BODY,
		ARMS,
		LEGS,
		MAX
	}

	BodyPartType bodyPartType;
	int playerIndex;
	Transform spriteTransform;

	public BodyPart(BodyPartType newBodyPartType, int newPlayerIndex, Transform newSpriteTransform)
	{
		bodyPartType = newBodyPartType;
		playerIndex = newPlayerIndex;
		spriteTransform = newSpriteTransform;
	}

	// Update is called once per frame
	public virtual void Update () 
	{
	}
}

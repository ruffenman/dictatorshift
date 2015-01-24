using UnityEngine;
using System.Collections;

public class BodyPart
{
	public enum BodyPartType
	{
		HEAD,
		BODY,
		ARMS,
		LEGS
	}

	BodyPartType bodyPartType;
	int playerIndex;

	public BodyPart(BodyPartType newBodyPartType, int newPlayerIndex)
	{
		bodyPartType = newBodyPartType;
		playerIndex = newPlayerIndex;
	}

	// Update is called once per frame
	public void Update () 
	{
		Debug.Log(bodyPartType);
	}
}

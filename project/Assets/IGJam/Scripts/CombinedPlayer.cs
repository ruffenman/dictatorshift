using UnityEngine;
using System.Collections;

public class CombinedPlayer : MonoBehaviour 
{
	BodyPart[] bodyParts;

	// Use this for initialization
	void Start () 
	{
		bodyParts = new BodyPart[4];
		bodyParts[0] = new BodyPart(BodyPart.BodyPartType.HEAD, 0);
		bodyParts[1] = new BodyPart(BodyPart.BodyPartType.BODY, 1);
		bodyParts[2] = new BodyPart(BodyPart.BodyPartType.ARMS, 2);
		bodyParts[3] = new BodyPart(BodyPart.BodyPartType.LEGS, 3);
	}
	
	// Update is called once per frame
	void Update () 
	{
		foreach (BodyPart bodyPart in bodyParts)
		{
			bodyPart.Update();
		}	
	}
}

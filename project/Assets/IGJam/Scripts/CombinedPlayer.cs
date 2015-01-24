using UnityEngine;
using System.Collections;

public class CombinedPlayer : MonoBehaviour 
{
	BodyPart[] bodyParts;

	// Use this for initialization
	void Start () 
	{
		bodyParts = new BodyPart[4];
		bodyParts[0] = new Head(0);
		bodyParts[1] = new Body(1);
		bodyParts[2] = new Arms(2);
		bodyParts[3] = new Legs(3);
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

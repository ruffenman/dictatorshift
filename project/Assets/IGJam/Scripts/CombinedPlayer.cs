using UnityEngine;
using System.Collections;

public class CombinedPlayer : WorldObject 
{
	BodyPart[] bodyParts;
	Transform[] bodyPartTransforms;

	void Start () 
	{
		base.Start();

		// initialize sprites
		Transform graphicsTransform = transform.Find("Graphics");
		bodyPartTransforms = new Transform[(int)BodyPart.BodyPartType.MAX];
		bodyPartTransforms[(int)BodyPart.BodyPartType.HEAD] = graphicsTransform.Find("Head");
		bodyPartTransforms[(int)BodyPart.BodyPartType.BODY] = graphicsTransform.Find("Body");
		bodyPartTransforms[(int)BodyPart.BodyPartType.ARMS] = graphicsTransform.Find("Arms");
		bodyPartTransforms[(int)BodyPart.BodyPartType.LEGS] = graphicsTransform.Find("Legs");

		// initialize body parts
		bodyParts = new BodyPart[(int)BodyPart.BodyPartType.MAX];
		bodyParts[(int)BodyPart.BodyPartType.HEAD] = new Head(0, bodyPartTransforms[(int)BodyPart.BodyPartType.HEAD], this);
		bodyParts[(int)BodyPart.BodyPartType.BODY] = new Body(1, bodyPartTransforms[(int)BodyPart.BodyPartType.BODY], this);
		bodyParts[(int)BodyPart.BodyPartType.ARMS] = new Arms(2, bodyPartTransforms[(int)BodyPart.BodyPartType.ARMS], this);
		bodyParts[(int)BodyPart.BodyPartType.LEGS] = new Legs(3, bodyPartTransforms[(int)BodyPart.BodyPartType.LEGS], this);
	}

	public override void Update()
	{
		base.Update();
		UpdateBodyParts();
		UpdateSprites();
	}

	void UpdateBodyParts () 
	{
		foreach (BodyPart bodyPart in bodyParts)
		{
			bodyPart.Update();
		}	
	}

	void UpdateSprites()
	{
	}
}

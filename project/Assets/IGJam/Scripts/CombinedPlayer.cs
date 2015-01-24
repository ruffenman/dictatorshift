using UnityEngine;
using System.Collections;

public class CombinedPlayer : MonoBehaviour 
{
    public GameObject lazerPrefab;

	BodyPart[] bodyParts;
	Transform[] bodyPartTransforms;

	CharacterController controller;
	Vector3 velocity;
	float gravity = 2f;
	float moveDepreciation = 0.9f;
	float stopVelocitySquared = 1.0f;

	public void AddPlayerVelocity(Vector3 addedVelocity)
	{
		SetPlayerVelocity(velocity + addedVelocity);
	}

	public void SetPlayerVelocity(Vector3 newVelocity)
	{
		velocity = newVelocity;
	}

	void Start () 
	{
		controller = GetComponent<CharacterController>();

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

	void Update()
	{
		UpdateBodyParts();
		UpdateSprites();
		Move();
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

	void Move()
	{
		Vector3 newVelocity = velocity;

		// gravity
		//if (controller.isGrounded)
		{
			newVelocity.y = 0;
		}
		//else
		//{
		//	newVelocity.y -= gravity;
		//}

		// drag
		if (newVelocity.sqrMagnitude > stopVelocitySquared)
		{
			newVelocity.x *= moveDepreciation;
		}
		else
		{
			newVelocity.x = 0;
		}

		controller.Move(newVelocity * Time.deltaTime);
		SetPlayerVelocity(newVelocity);
	}

    public void FireTheLazer(Lazer.PowerLevel powerLevel, Vector3 direction)
    {
        const float xOffset = 1.3f;
        const float yOffset = 0.6f;
        GameObject Clone;
        Vector3 pos = bodyPartTransforms[(int)BodyPart.BodyPartType.HEAD].position;
        pos.x += xOffset;
        pos.y += yOffset;
        Clone = (Instantiate(lazerPrefab, pos, transform.rotation)) as GameObject;
        Lazer lazer = (Lazer)Clone.GetComponent(typeof(Lazer));
        lazer.SetLazerStrength(powerLevel, direction);
    }
}

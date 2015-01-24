using UnityEngine;
using System.Collections;

public class CombinedPlayer : WorldObject 
{
	//properties
    public GameObject lazerPrefab;
	private float respawnDelay = 0.5f;

	// utility
	BodyPart[] bodyParts;
	Transform[] bodyPartTransforms;
	bool dead;
	Transform lastRespawnPoint;

    public BodyPart GetBodyPart(BodyPart.BodyPartType partType)
    {
        return bodyParts[(int)partType];
    }

    public void ReceiveInput(IGJInputManager.InputState[] inputStates)
    {
        // TODO: Mess with player -> body assignments
		for (int i = 0; i < inputStates.Length; ++i)
		{
			bodyParts[i].ReceiveInput(inputStates[i]);
		}
        //bodyParts[(int)BodyPart.BodyPartType.BODY].ReceiveInput(inputStates[0]);
    }

	new void Start ()
	{
		base.Start();

		// initialize respawn
		float closestDistance = float.MaxValue;
		foreach (GameObject respawn in GameObject.FindGameObjectsWithTag("Respawn"))
		{
			float distance = Vector3.Distance(respawn.transform.position, transform.position);
			if (distance < closestDistance)
			{
				lastRespawnPoint = respawn.transform;
			}
		}

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
		if (!dead)
		{
			base.Update();
			UpdateBodyParts();
		}
	}

	void UpdateBodyParts () 
	{
		foreach (BodyPart bodyPart in bodyParts)
		{
			bodyPart.Update();
		}	
	}

	void OnTriggerEnter(Collider other)
	{
		DeadlyObject deadlyObject = other.GetComponent<DeadlyObject>();
		if (deadlyObject != null)
		{
			deadlyObject.OnCollideWithTarget(gameObject);
		}
	}

	protected override void Die()
	{
		dead = true;
		SetVisible(false);
		transform.position = lastRespawnPoint.position;
		StartCoroutine(Utility.Delay(respawnDelay, Respawn));
	}

	void Respawn()
	{
		dead = false;
		SetVisible(true);
	}

	void SetVisible(bool visible)
	{
		Renderer[] renderers = GetComponentsInChildren<Renderer>();
		foreach (Renderer renderer in renderers)
		{
			renderer.enabled = visible;
		}
	}

    public void FireTheLazer(Lazer.PowerLevel powerLevel, IGJInputManager.InputDirection direction)
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

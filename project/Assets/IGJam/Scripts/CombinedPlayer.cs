using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombinedPlayer : WorldObject 
{
	//properties
    public GameObject lazerPrefab;
	private float respawnDelay = 0.5f;
	private float respawnMoveTime = 0.15f;
    public GameObject deathParticles;

	// utility
	BodyPart[] bodyParts;
	Transform[] bodyPartTransforms;
	bool dead;
	Transform lastRespawnPoint;
    int[] playerToBodyMapping;

    // this is a debub value to be taken out later
    private int debugGetInput;

    public BodyPart GetBodyPart(BodyPart.BodyPartType partType)
    {
        return bodyParts[(int)partType];
    }

	public Transform GetBodyPartTransform(BodyPart.BodyPartType partType)
	{
		return bodyPartTransforms[(int)partType];
	}

    public void ReceiveInput(IGJInputManager.InputState[] inputStates)
    {
        if (JamGame.instance.debugBodyPartSwitching)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                //increment the current body part to get input
                debugGetInput++;
                debugGetInput %= (int)BodyPart.BodyPartType.MAX;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                // decrement the current body part to get input
                debugGetInput = ((debugGetInput - 1) + (int)BodyPart.BodyPartType.MAX) % (int)BodyPart.BodyPartType.MAX;
                debugGetInput %= (int)BodyPart.BodyPartType.MAX;
            }
            bodyParts[debugGetInput].ReceiveInput(inputStates[0]);
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.RightShift))
            {
                ShuffleInputs();
            }

            for (int i = 0; i < inputStates.Length; ++i)
            {
                bodyParts[playerToBodyMapping[i]].ReceiveInput(inputStates[i]);
            }
        }
    }

    public void ShuffleInputs()
    {
        List<int> partIndexesToAssign = new List<int>(new int[]{0, 1, 2, 3});
        for (int i = 0; i < playerToBodyMapping.Length; ++i)
        {
            int indexToAssign = Random.Range(0, partIndexesToAssign.Count);
            playerToBodyMapping[i] = partIndexesToAssign[indexToAssign];
            partIndexesToAssign.RemoveAt(indexToAssign);
        }
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

        playerToBodyMapping = new int[(int)BodyPart.BodyPartType.MAX] {0,1,2,3};
	}

	protected override void UpdateInternal()
	{
		if (!dead)
		{
			base.UpdateInternal();
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
		if (deathParticles != null)
		{
			deathParticles.particleSystem.Play();
		}
		dead = true;
		SetVisible(false);
		StartCoroutine(Utility.Delay(respawnDelay, Respawn));
	}

	void Respawn()
	{
		transform.position = lastRespawnPoint.position;
		StartCoroutine(Utility.Delay(respawnMoveTime, MakeAlive));
	}

	void MakeAlive()
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
        GameObject Clone;
        Vector3 pos = bodyPartTransforms[(int)BodyPart.BodyPartType.HEAD].position;
        Clone = (Instantiate(lazerPrefab, pos, transform.rotation)) as GameObject;
        Lazer lazer = (Lazer)Clone.GetComponent(typeof(Lazer));
        lazer.SetLazerStrength(powerLevel, direction);
    }
}

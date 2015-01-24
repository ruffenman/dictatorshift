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

	protected BodyPartType bodyPartType;
	protected int playerIndex;
	protected Transform spriteTransform;
	protected CombinedPlayer combinedPlayer;
	protected IGJInputManager.InputState lastInputState;
	
	public BodyPart(BodyPartType newBodyPartType, int newPlayerIndex, Transform newSpriteTransform, CombinedPlayer newCombinedPlayer)
	{
		bodyPartType = newBodyPartType;
		playerIndex = newPlayerIndex;
		spriteTransform = newSpriteTransform;
		combinedPlayer = newCombinedPlayer;
	}

	// Update is called once per frame
	public virtual void Update () 
	{
	}

    public void ReceiveInput(IGJInputManager.InputState inputState)
    {
        lastInputState.direction.x = inputState.direction.x;
        lastInputState.direction.y = inputState.direction.y;
        lastInputState.direction.z = 0;
        lastInputState.actionPressed = inputState.actionPressed;
        lastInputState.actionJustPressed = inputState.actionJustPressed;

        OnInputReceived();
    }

    protected virtual void OnInputReceived()
    {
	}
}

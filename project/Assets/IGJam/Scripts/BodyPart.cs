﻿using UnityEngine;
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

        lastInputState = new IGJInputManager.InputState() {directionVec=Vector3.zero};
	}

	// Update is called once per frame
	public virtual void Update () 
	{
	}

    public void ReceiveInput(IGJInputManager.InputState inputState)
    {
        lastInputState.directionVec.x = inputState.directionVec.x;
        lastInputState.directionVec.y = inputState.directionVec.y;
        lastInputState.directionVec.z = 0;
        lastInputState.direction = inputState.direction;
        lastInputState.actionPressed = inputState.actionPressed;
        lastInputState.actionJustPressed = inputState.actionJustPressed;
        lastInputState.actionJustReleased = inputState.actionJustReleased;

        OnInputReceived();
    }

    protected virtual void OnInputReceived()
    {
	}
}

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

    protected void OnInputReceived()
    { }

    protected IGJInputManager.InputState lastInputState;
}

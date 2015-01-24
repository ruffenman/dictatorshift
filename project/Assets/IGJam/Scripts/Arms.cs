using UnityEngine;
using System.Collections;

public class Arms : BodyPart
{
	public enum ANGLE
	{
		NONE,
		UP,
		DOWN,
		LEFT,
		RIGHT,
		UPLEFT,
		UPRIGHT,
		DOWNLEFT,
		DOWNRIGHT,
		COUNT
	}
	
	public class InputState
	{
		public bool inputButton = false;
		public bool buttonChanged = false;
		
		public ANGLE inputAngle = ANGLE.NONE;
		public bool angleChanged = false;
	}
	
	public bool isHoldingObject = false;
	public GameObject heldObject = null;
	
	
	GameObject armsColliderObject = null;
	ArmsCollider armsCollider = null;
	InputState inputState = null;
	
	Vector2[] armsOffsetsByAngle = new Vector2[(int)ANGLE.COUNT];
	Vector2[] armsVelocitiesByAngle = new Vector2[(int)ANGLE.COUNT];

	public Arms(int newPlayerIndex, Transform newSpriteTransform, CombinedPlayer newCombinedPlayer)
		: base(BodyPart.BodyPartType.ARMS, newPlayerIndex, newSpriteTransform, newCombinedPlayer)
	{
		armsColliderObject = new GameObject();
		armsColliderObject.AddComponent<BoxCollider>().size = new Vector3(1,1,1);
		armsColliderObject.AddComponent<Rigidbody>();
		armsColliderObject.AddComponent<ArmsCollider>();
		armsCollider = armsColliderObject.GetComponent<ArmsCollider>();
		
		// TODO FETCH THE CHARACTER ROOT ********************
		armsColliderObject.transform.parent = null;
		// TODO FETCH THE CHARACTER ROOT ********************
		
		armsColliderObject.transform.localPosition = new Vector3(0,0,0);
		
		armsOffsetsByAngle[(int)ANGLE.NONE] = new Vector2(0,0);
		armsOffsetsByAngle[(int)ANGLE.UP] = new Vector2(0,0);
		armsOffsetsByAngle[(int)ANGLE.DOWN] = new Vector2(0,0);
		armsOffsetsByAngle[(int)ANGLE.LEFT] = new Vector2(0,0);
		armsOffsetsByAngle[(int)ANGLE.RIGHT] = new Vector2(0,0);
		armsOffsetsByAngle[(int)ANGLE.UPLEFT] = new Vector2(0,0);
		armsOffsetsByAngle[(int)ANGLE.UPRIGHT] = new Vector2(0,0);
		armsOffsetsByAngle[(int)ANGLE.DOWNLEFT] = new Vector2(0,0);
		armsOffsetsByAngle[(int)ANGLE.DOWNRIGHT] = new Vector2(0,0);
		
		armsVelocitiesByAngle[(int)ANGLE.NONE] = new Vector2(0,0);
		armsVelocitiesByAngle[(int)ANGLE.UP] = new Vector2(0,0);
		armsVelocitiesByAngle[(int)ANGLE.DOWN] = new Vector2(0,0);
		armsVelocitiesByAngle[(int)ANGLE.LEFT] = new Vector2(0,0);
		armsVelocitiesByAngle[(int)ANGLE.RIGHT] = new Vector2(0,0);
		armsVelocitiesByAngle[(int)ANGLE.UPLEFT] = new Vector2(0,0);
		armsVelocitiesByAngle[(int)ANGLE.UPRIGHT] = new Vector2(0,0);
		armsVelocitiesByAngle[(int)ANGLE.DOWNLEFT] = new Vector2(0,0);
		armsVelocitiesByAngle[(int)ANGLE.DOWNRIGHT] = new Vector2(0,0);
		
		inputState = new InputState();
	}

	// Update is called once per frame
	public override void Update()
	{
		ParseInput();
	
		// UPDATE ARMS POSITION
		if(inputState.angleChanged)
		{
			Debug.Log ("Arms.cs -- update arm angle");
			armsColliderObject.transform.localPosition = armsOffsetsByAngle[(int)inputState.inputAngle];
			inputState.angleChanged = false;
			
			// TODO TALK TO GRAPHICS CONTROL AND UPDATE ****************************************************
			// TODO TALK TO GRAPHICS CONTROL AND UPDATE ****************************************************
		}
		
		// BUTTON PRESSED
		if(inputState.inputButton && inputState.buttonChanged)
		{
			GameObject pickup = armsCollider.GetFirstPickup();
			if(pickup != null)
			{
				Debug.Log ("Arms.cs -- pick up object");
				isHoldingObject = true;
				heldObject = pickup;
				heldObject.transform.parent = armsColliderObject.transform;
				heldObject.transform.localPosition = new Vector3(0,0,0);
			}
			
			GameObject attackable = armsCollider.GetFirstAttackable ();
			if(attackable != null)
			{
				Debug.Log ("Arms.cs -- attack object");
				// TODO INTERFACE *******************************************************
				// resolve attack on object
				// - Get script
				// - Call function
				// - Try to remove the collision if it destroys itself, otherwise no big deal its handled
				// TODO INTERFACE *******************************************************
			}
			
			inputState.buttonChanged = false;
		}
		
		// BUTTON RELEASED
		else if(!inputState.inputButton && inputState.buttonChanged && isHoldingObject)
		{
			Debug.Log ("Arms.cs -- drop object");
			isHoldingObject = false;
			heldObject = null;
			heldObject.transform.parent = null;
		
			inputState.buttonChanged = false;
		}
	}
	
	void ParseInput()
	{	
		int x;
		int y;
		bool down;
	
		if(lastInputState == null)
		{
			Debug.LogWarning ("Arms.cs -- no lastInputState initialized! Overriding with no movement!");
			x = 0;
			y = 0;
			down = false;
		}
		else
		{
			x = (int)lastInputState.directionVec.x;
			y = (int)lastInputState.directionVec.y;
			down = lastInputState.actionPressed;
		}
		
		if(x == 0)
		{
			if(y == 0)
			{
				inputState.inputAngle = ANGLE.NONE;
			}
			else if(y == 1)
			{
				inputState.inputAngle = ANGLE.UP;
			}
			else if(y == -1)
			{
				inputState.inputAngle = ANGLE.DOWN;
			}
		}
		else if(x == 1)
		{
			if(y == 0)
			{
				inputState.inputAngle = ANGLE.RIGHT;
			}
			else if(y == 1)
			{
				inputState.inputAngle = ANGLE.UPRIGHT;
			}
			else if(y == -1)
			{
				inputState.inputAngle = ANGLE.DOWNRIGHT;
			}
		}
		else if(x == -1)
		{
			if(y == 0)
			{
				inputState.inputAngle = ANGLE.LEFT;
			}
			else if(y == 1)
			{
				inputState.inputAngle = ANGLE.UPLEFT;
			}
			else if(y == -1)
			{
				inputState.inputAngle = ANGLE.DOWNLEFT;
			}
		}
		
		if(down != inputState.inputButton)
		{
			inputState.buttonChanged = true;
			inputState.inputButton = down;
		}
	}
}

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
	
	static float FULL = 1;
	static float FRAC = 0.6f;
	static float THROW_MULTIPLIER = 50f;
	static float FLAIL_MULTIPLIER = 5f;
	
	public class InputState
	{
		public bool inputButton = false;
		public bool buttonChanged = false;
		
		public ANGLE inputAngle = ANGLE.NONE;
		public bool angleChanged = false;
	}
	
	public bool isFlailing = false;
	public bool isHoldingObject = false;
	public GameObject heldObject = null;
	
	
	GameObject armsColliderObject = null;
	ArmsCollider armsCollider = null;
	InputState inputState = null;
	bool initialized = false;
	
	Vector2[] armsOffsetsByAngle = new Vector2[(int)ANGLE.COUNT];
	Vector2[] armsVelocitiesByAngle = new Vector2[(int)ANGLE.COUNT];

	public Arms(int newPlayerIndex, Transform newSpriteTransform, CombinedPlayer newCombinedPlayer)
		: base(BodyPart.BodyPartType.ARMS, newPlayerIndex, newSpriteTransform, newCombinedPlayer)
	{
		armsColliderObject = new GameObject();
		armsColliderObject.name = "Arms Collider -- owned by Arms.cs";
		armsColliderObject.AddComponent<ArmsCollider>();
		armsCollider = armsColliderObject.GetComponent<ArmsCollider>();
		BoxCollider bc = armsColliderObject.AddComponent<BoxCollider>();
		bc.size = new Vector3(1,1,1);
		bc.isTrigger = true;
		Rigidbody rb = armsColliderObject.AddComponent<Rigidbody>();
		rb.useGravity = false;
		rb.constraints = RigidbodyConstraints.FreezeAll;
		armsColliderObject.transform.parent = newCombinedPlayer.transform;
		armsColliderObject.transform.localPosition = new Vector3(0,0,0);
		
		armsOffsetsByAngle[(int)ANGLE.NONE] = new Vector2(FRAC,0);
		armsOffsetsByAngle[(int)ANGLE.UP] = new Vector2(0,FULL);
		armsOffsetsByAngle[(int)ANGLE.DOWN] = new Vector2(0,-FULL);
		armsOffsetsByAngle[(int)ANGLE.LEFT] = new Vector2(-FULL,0);
		armsOffsetsByAngle[(int)ANGLE.RIGHT] = new Vector2(FULL,0);
		armsOffsetsByAngle[(int)ANGLE.UPLEFT] = new Vector2(-FRAC,FRAC);
		armsOffsetsByAngle[(int)ANGLE.UPRIGHT] = new Vector2(FRAC,FRAC);
		armsOffsetsByAngle[(int)ANGLE.DOWNLEFT] = new Vector2(-FRAC,-FRAC);
		armsOffsetsByAngle[(int)ANGLE.DOWNRIGHT] = new Vector2(FRAC,-FRAC);
		
		armsVelocitiesByAngle[(int)ANGLE.NONE] = new Vector2(0,0);
		armsVelocitiesByAngle[(int)ANGLE.UP] = new Vector2(0,FULL);
		armsVelocitiesByAngle[(int)ANGLE.DOWN] = new Vector2(0,-FULL);
		armsVelocitiesByAngle[(int)ANGLE.LEFT] = new Vector2(-FULL,0);
		armsVelocitiesByAngle[(int)ANGLE.RIGHT] = new Vector2(0,FULL);
		armsVelocitiesByAngle[(int)ANGLE.UPLEFT] = new Vector2(-FRAC,FRAC);
		armsVelocitiesByAngle[(int)ANGLE.UPRIGHT] = new Vector2(FRAC,FRAC);
		armsVelocitiesByAngle[(int)ANGLE.DOWNLEFT] = new Vector2(-FRAC,-FRAC);
		armsVelocitiesByAngle[(int)ANGLE.DOWNRIGHT] = new Vector2(FRAC,-FRAC);
		
		inputState = new InputState();
		initialized = true;
	}

	// Update is called once per frame
	public override void Update()
	{
		if(!initialized)
		{
			return;
		}
	
		ParseInput();
	
		// UPDATE ARMS POSITION
		if(inputState.angleChanged)
		{
			armsColliderObject.transform.localPosition = armsOffsetsByAngle[(int)inputState.inputAngle];
			inputState.angleChanged = false;
			
			// TODO TALK TO GRAPHICS CONTROL AND UPDATE ****************************************************
			// TODO TALK TO GRAPHICS CONTROL AND UPDATE ****************************************************
		}
		
		// VALIDATE HELD OBJECT
		if(isHoldingObject && heldObject == null)
		{
			isHoldingObject = false;
			Debug.Log("Arms.cs -- held object was destroyed!");
		}
		
		// BUTTON PRESSED
		if(inputState.inputButton && inputState.buttonChanged)
		{
			// PICK UP OBJECT
			if(armsCollider.CanReturnInteractible() == true)
			{
				GameObject interactible = armsCollider.GetFirstInteractible();
				Debug.Log ("Arms.cs -- pick up object");
				isHoldingObject = true;
				heldObject = interactible;
				heldObject.transform.parent = armsColliderObject.transform;
				heldObject.transform.localPosition = new Vector3(0,0,0);
			}
			// START FLAILING
			else
			{
				isFlailing = true;
				// TODO TALK TO GRAPHICS CONTROL AND UPDATE ****************************************************
				// TODO TALK TO GRAPHICS CONTROL AND UPDATE ****************************************************
			}
			
			inputState.buttonChanged = false;
		}
		
		// BUTTON STILL HELD DOWN
		else if(inputState.inputButton && inputState.buttonChanged == false)
		{
			if(isHoldingObject == false)
			{
				// FLAIL AT OBJECT
				GameObject interactible = armsCollider.GetFirstInteractible();
				if(interactible != null)
				{
					Debug.Log ("Arms.cs -- flail at object");
					WorldObject obj = interactible.GetComponent<WorldObject>();
					if(obj != null)
					{
						obj.SetVelocity (armsVelocitiesByAngle[(int)inputState.inputAngle] * FLAIL_MULTIPLIER);
					}
					else
					{
						Debug.LogWarning ("Arms.cs -- no WorldObject script on held object!");
					}
				}
			}
			else
			{
				heldObject.transform.localPosition = new Vector3(0,0,0);
			}
		}
		
		// BUTTON RELEASED
		else if(!inputState.inputButton && inputState.buttonChanged)
		{
			// RELEASE/THROW OBJECT
			if(isHoldingObject)
			{
				Debug.Log ("Arms.cs -- throw object");
				heldObject.transform.parent = null;
				
				WorldObject obj = heldObject.GetComponent<WorldObject>();
				if(obj != null)
				{
					obj.SetVelocity (armsVelocitiesByAngle[(int)inputState.inputAngle] * THROW_MULTIPLIER);
				}
				else
				{
					Debug.LogWarning ("Arms.cs -- no WorldObject script on held object!");
				}
				
				isHoldingObject = false;
			}
			
			// STOP FLAILING
			if(isFlailing)
			{
				isFlailing = false;
				// TODO TALK TO GRAPHICS CONTROL AND UPDATE ****************************************************
				// TODO TALK TO GRAPHICS CONTROL AND UPDATE ****************************************************
			}
			
			inputState.buttonChanged = false;
		}
	}
	
	void ParseInput()
	{	
		if(lastInputState == null)
		{
			Debug.LogWarning ("Arms.cs -- no lastInputState initialized! Overriding with no movement!");
			return;
		}
		
		bool down;
		ANGLE old = inputState.inputAngle;
		IGJInputManager.InputDirection direction = lastInputState.direction;
		down = lastInputState.actionPressed;
		
		switch(direction)
		{
			case(IGJInputManager.InputDirection.Up): inputState.inputAngle = ANGLE.UP; break;
			case(IGJInputManager.InputDirection.Down): inputState.inputAngle = ANGLE.DOWN; break;
			case(IGJInputManager.InputDirection.Left): inputState.inputAngle = ANGLE.LEFT; break;
			case(IGJInputManager.InputDirection.Right): inputState.inputAngle = ANGLE.RIGHT; break;
			case(IGJInputManager.InputDirection.UpLeft): inputState.inputAngle = ANGLE.UPLEFT; break;
			case(IGJInputManager.InputDirection.UpRight): inputState.inputAngle = ANGLE.UPRIGHT; break;
			case(IGJInputManager.InputDirection.DownLeft): inputState.inputAngle = ANGLE.DOWNLEFT; break;
			case(IGJInputManager.InputDirection.DownRight): inputState.inputAngle = ANGLE.DOWNRIGHT; break;
			default: inputState.inputAngle = ANGLE.NONE; break;
		}
		
		if (inputState.inputAngle != old)
		{
			inputState.angleChanged = true;
		}
		
		if(down != inputState.inputButton)
		{
			inputState.buttonChanged = true;
			inputState.inputButton = down;
		}
	}
}

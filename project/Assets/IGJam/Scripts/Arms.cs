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
	
	public static float FULL = 1;
	public static float FRAC = 0.6f;
	
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
	
	Vector2[] armsOffsetsByAngle = new Vector2[(int)ANGLE.COUNT];
	Vector2[] armsVelocitiesByAngle = new Vector2[(int)ANGLE.COUNT];

	public Arms(int newPlayerIndex, Transform newSpriteTransform, CombinedPlayer newCombinedPlayer)
		: base(BodyPart.BodyPartType.ARMS, newPlayerIndex, newSpriteTransform, newCombinedPlayer)
	{
		armsColliderObject = new GameObject();
		armsColliderObject.name = "Arms Collider -- owned by Arms.cs";
		armsCollider = armsColliderObject.GetComponent<ArmsCollider>();
		BoxCollider bc = armsColliderObject.AddComponent<BoxCollider>();
		bc.size = new Vector3(1,1,1);
		bc.isTrigger = true;
		Rigidbody rb = armsColliderObject.AddComponent<Rigidbody>();
		rb.useGravity = false;
		rb.constraints = RigidbodyConstraints.FreezePositionZ;
		rb.constraints = RigidbodyConstraints.FreezeRotation;
		armsColliderObject.AddComponent<ArmsCollider>();
		armsColliderObject.transform.parent = newCombinedPlayer.transform;
		armsColliderObject.transform.localPosition = new Vector3(0,0,0);
		
		armsOffsetsByAngle[(int)ANGLE.NONE] = new Vector2(0,0);
		armsOffsetsByAngle[(int)ANGLE.UP] = new Vector2(0,FULL);
		armsOffsetsByAngle[(int)ANGLE.DOWN] = new Vector2(0,-FULL);
		armsOffsetsByAngle[(int)ANGLE.LEFT] = new Vector2(-FULL,0);
		armsOffsetsByAngle[(int)ANGLE.RIGHT] = new Vector2(0,FULL);
		armsOffsetsByAngle[(int)ANGLE.UPLEFT] = new Vector2(-FRAC,FRAC);
		armsOffsetsByAngle[(int)ANGLE.UPRIGHT] = new Vector2(FRAC,FRAC);
		armsOffsetsByAngle[(int)ANGLE.DOWNLEFT] = new Vector2(-FRAC,-FRAC);
		armsOffsetsByAngle[(int)ANGLE.DOWNRIGHT] = new Vector2(-FRAC,FRAC);
		
		armsVelocitiesByAngle[(int)ANGLE.NONE] = new Vector2(0,0);
		armsVelocitiesByAngle[(int)ANGLE.UP] = new Vector2(0,FULL);
		armsVelocitiesByAngle[(int)ANGLE.DOWN] = new Vector2(0,-FULL);
		armsVelocitiesByAngle[(int)ANGLE.LEFT] = new Vector2(-FULL,0);
		armsVelocitiesByAngle[(int)ANGLE.RIGHT] = new Vector2(0,FULL);
		armsVelocitiesByAngle[(int)ANGLE.UPLEFT] = new Vector2(-FRAC,FRAC);
		armsVelocitiesByAngle[(int)ANGLE.UPRIGHT] = new Vector2(FRAC,FRAC);
		armsVelocitiesByAngle[(int)ANGLE.DOWNLEFT] = new Vector2(-FRAC,-FRAC);
		armsVelocitiesByAngle[(int)ANGLE.DOWNRIGHT] = new Vector2(-FRAC,FRAC);
		
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
			GameObject interactible = armsCollider.GetFirstInteractible();
			if(interactible != null)
			{
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
			// FLAIL AT OBJECT
			GameObject interactible = armsCollider.GetFirstInteractible();
			if(interactible != null)
			{
				Debug.Log ("Arms.cs -- flail at object");
				WorldObject obj = interactible.GetComponent<WorldObject>();
				if(obj != null)
				{
					obj.AddVelocity (armsVelocitiesByAngle[(int)inputState.inputAngle]);
				}
				else
				{
					Debug.LogWarning ("Arms.cs -- no WorldObject script on held object!");
				}
			}
		}
		
		// BUTTON RELEASED
		else if(!inputState.inputButton && inputState.buttonChanged)
		{
			// THROW OBJECT
			if(isHoldingObject)
			{
				Debug.Log ("Arms.cs -- throw object");
				heldObject.transform.parent = null;
				
				WorldObject obj = heldObject.GetComponent<WorldObject>();
				if(obj != null)
				{
					obj.AddVelocity (armsVelocitiesByAngle[(int)inputState.inputAngle]);
				}
				else
				{
					Debug.LogWarning ("Arms.cs -- no WorldObject script on held object!");
				}
				
				heldObject = null;
				isHoldingObject = false;
			}
			
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
			x = (int)lastInputState.direction.x;
			y = (int)lastInputState.direction.y;
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

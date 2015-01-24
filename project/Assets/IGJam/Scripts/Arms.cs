using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
	
	List<GameObject> pickupCollisions = new List<GameObject>();
	List<GameObject> attackableCollisions = new List<GameObject>();
	GameObject armsPrefab = null;
	SpriteRenderer armsSprite = null;
	Transform armsTransform = null;
	InputState inputState = null;
	Vector2[] armsOffsetsByAngle = new Vector2[(int)ANGLE.COUNT];
	Vector2[] armsVelocitiesByAngle = new Vector2[(int)ANGLE.COUNT];
	Sprite[] armsSpritesByAngle = new Sprite[(int)ANGLE.COUNT];

	public Arms(int newPlayerIndex, Transform newSpriteTransform, CombinedPlayer newCombinedPlayer)
		: base(BodyPart.BodyPartType.ARMS, newPlayerIndex, newSpriteTransform, newCombinedPlayer)
	{
		
		armsPrefab = newSpriteTransform.gameObject;
		armsSprite = armsPrefab.GetComponent<SpriteRenderer>();
		armsTransform = armsPrefab.transform;
		
		// TODO THESE WILL PROBABLY BE HANDLED BY THE ANIMATION CONTROL *******************************
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
		
		armsSpritesByAngle[(int)ANGLE.NONE] = (Sprite)Resources.Load ("");
		armsSpritesByAngle[(int)ANGLE.UP] = (Sprite)Resources.Load ("");
		armsSpritesByAngle[(int)ANGLE.DOWN] = (Sprite)Resources.Load ("");
		armsSpritesByAngle[(int)ANGLE.LEFT] = (Sprite)Resources.Load ("");
		armsSpritesByAngle[(int)ANGLE.RIGHT] = (Sprite)Resources.Load ("");
		armsSpritesByAngle[(int)ANGLE.UPLEFT] = (Sprite)Resources.Load ("");
		armsSpritesByAngle[(int)ANGLE.UPRIGHT] = (Sprite)Resources.Load ("");
		armsSpritesByAngle[(int)ANGLE.DOWNLEFT] = (Sprite)Resources.Load ("");
		armsSpritesByAngle[(int)ANGLE.DOWNRIGHT] = (Sprite)Resources.Load ("");
		// TODO THESE WILL PROBABLY BE HANDLED BY THE ANIMATION CONTROL *******************************
		
		inputState = new InputState();
	}

	// Update is called once per frame
	public override void Update()
	{
		ParseInput();
	
		if(inputState.angleChanged)
		{
			armsTransform.localPosition = armsOffsetsByAngle[(int)inputState.inputAngle];
			inputState.angleChanged = false;
		}
		
		// BUTTON PRESSED
		if(inputState.inputButton && inputState.buttonChanged)
		{
			GameObject pickup = GetFirstPickup();
			if(pickup != null)
			{
				isHoldingObject = true;
				heldObject = pickup;
				heldObject.transform.parent = armsPrefab.transform;
			}
			
			GameObject attackable = GetFirstAttackable ();
			if(attackable != null)
			{
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
		else if(!inputState.inputButton && inputState.buttonChanged)
		{
			isHoldingObject = false;
			heldObject = null;
			heldObject.transform.parent = null;
			
			GameObject attackable = GetFirstAttackable ();
			if(attackable != null)
			{
				// TODO INTERFACE *******************************************************
				// resolve attack on object
				// - Get script
				// - Call function
				// - Try to remove the collision if it destroys itself, otherwise no big deal its handled
				// TODO INTERFACE *******************************************************
			}
		
			inputState.buttonChanged = false;
		}
	}
	
	void ParseInput()
	{
		// TODO INTERFACE *******************************************************
		int x = 0;
		int y = 0;
		bool down = false;
		// TODO INTERFACE *******************************************************
		
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
	
	GameObject GetFirstPickup()
	{
		if(pickupCollisions.Count > 0)
		{
			while(pickupCollisions.Count > 0 && pickupCollisions[0] == null)
			{
				pickupCollisions.RemoveAt (0);
			}
			return pickupCollisions[0];
		}
		return null;
	}
	
	GameObject GetFirstAttackable()
	{
		if(attackableCollisions.Count > 0)
		{
			while(attackableCollisions.Count > 0 && attackableCollisions[0] == null)
			{
				attackableCollisions.RemoveAt (0);
			}
			return attackableCollisions[0];
		}
		return null;
	}
	
	// call from OnTriggerEnter() on pickup gameObject with collider/rigidbody
	public void ReportAddPickupCollision(GameObject collision) 
	{
		pickupCollisions.Add (collision);
	}
	
	// call from OnTriggerExit() on pickup gameObject with collider/rigidbody
	public void ReportRemovePickupCollision(GameObject collision)
	{
		pickupCollisions.Remove (collision);
	}
	
	// call from OnTriggerEnter() on pickup gameObject with collider/rigidbody
	public void ReportAddAttackableCollision(GameObject collision)
	{
		attackableCollisions.Add (collision);
	}
	
	// call from OnTriggerExit() on pickup gameObject with collider/rigidbody
	public void ReportRemoveAttackableCollision(GameObject collision)
	{
		attackableCollisions.Remove (collision);
	}
	
	
}

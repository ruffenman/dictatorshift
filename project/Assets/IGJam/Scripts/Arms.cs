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
	
	public bool isFlailing = false;
	public bool isHoldingObject = false;
	public GameObject heldObject = null;
	
	
	GameObject armsColliderObject = null;
	ArmsCollider armsCollider = null;
	bool initialized = false;
    IGJInputManager.InputDirection lastDirection;
    Animator animator = null;
    private bool isFacingLeft = false;

    Vector3[] armsOffsetsByDir = new Vector3[(int)IGJInputManager.InputDirection.Max];
    Vector3[] armsVelocitiesByDir = new Vector3[(int)IGJInputManager.InputDirection.Max];

	public Arms(int newPlayerIndex, Transform newSpriteTransform, CombinedPlayer newCombinedPlayer)
		: base(BodyPart.BodyPartType.ARMS, newPlayerIndex, newSpriteTransform, newCombinedPlayer)
	{
        animator = newSpriteTransform.gameObject.GetComponent<Animator>();

		armsColliderObject = new GameObject();
		armsColliderObject.name = "Arms Collider -- owned by Arms.cs";
		armsColliderObject.AddComponent<ArmsCollider>();
		armsColliderObject.layer = newSpriteTransform.gameObject.layer;
		armsColliderObject.tag = newSpriteTransform.gameObject.tag;
		armsCollider = armsColliderObject.GetComponent<ArmsCollider>();
		BoxCollider bc = armsColliderObject.AddComponent<BoxCollider>();
		bc.size = new Vector3(1,1,1);
		bc.isTrigger = true;
		Rigidbody rb = armsColliderObject.AddComponent<Rigidbody>();
		rb.useGravity = false;
		rb.constraints = RigidbodyConstraints.FreezeAll;
		armsColliderObject.transform.parent = newCombinedPlayer.transform;
		armsColliderObject.transform.localPosition = new Vector3(0,0,0);
		
		armsOffsetsByDir[(int)IGJInputManager.InputDirection.None] = new Vector3(FRAC,0,0);
        armsOffsetsByDir[(int)IGJInputManager.InputDirection.Up] = new Vector3(0, FULL, 0);
        armsOffsetsByDir[(int)IGJInputManager.InputDirection.Down] = new Vector3(0, -FULL, 0);
        armsOffsetsByDir[(int)IGJInputManager.InputDirection.Left] = new Vector3(-FULL, 0, 0);
        armsOffsetsByDir[(int)IGJInputManager.InputDirection.Right] = new Vector3(FULL, 0, 0);
        armsOffsetsByDir[(int)IGJInputManager.InputDirection.UpLeft] = new Vector3(-FRAC, FRAC, 0);
        armsOffsetsByDir[(int)IGJInputManager.InputDirection.UpRight] = new Vector3(FRAC, FRAC, 0);
        armsOffsetsByDir[(int)IGJInputManager.InputDirection.DownLeft] = new Vector3(-FRAC, -FRAC, 0);
        armsOffsetsByDir[(int)IGJInputManager.InputDirection.DownRight] = new Vector3(FRAC, -FRAC, 0);

        armsVelocitiesByDir[(int)IGJInputManager.InputDirection.None] = new Vector3(0, -FULL, 0);
        armsVelocitiesByDir[(int)IGJInputManager.InputDirection.Up] = new Vector3(0, FULL, 0);
        armsVelocitiesByDir[(int)IGJInputManager.InputDirection.Down] = new Vector3(0, -FULL, 0);
        armsVelocitiesByDir[(int)IGJInputManager.InputDirection.Left] = new Vector3(-FULL, 0, 0);
        armsVelocitiesByDir[(int)IGJInputManager.InputDirection.Right] = new Vector3(FULL, 0, 0);
        armsVelocitiesByDir[(int)IGJInputManager.InputDirection.UpLeft] = new Vector3(-FRAC, FRAC, 0);
        armsVelocitiesByDir[(int)IGJInputManager.InputDirection.UpRight] = new Vector3(FRAC, FRAC, 0);
        armsVelocitiesByDir[(int)IGJInputManager.InputDirection.DownLeft] = new Vector3(-FRAC, -FRAC, 0);
        armsVelocitiesByDir[(int)IGJInputManager.InputDirection.DownRight] = new Vector3(FRAC, -FRAC, 0);
		
		initialized = true;
        lastDirection = IGJInputManager.InputDirection.None;
	}

    public override void Update()
    {
        // Update facing direction
        if((combinedPlayer.GetBodyPart(BodyPartType.BODY) as Body).isFacingLeft)
        {
            spriteTransform.localScale = new Vector3(-1, 1, 1);
            isFacingLeft = true;
        }
        else
        {
            spriteTransform.localScale = new Vector3(1, 1, 1);
            isFacingLeft = false;
        }
    }

    protected override void OnInputReceived()
    {
        if (!initialized)
        {
            return;
        }

        // UPDATE ARMS POSITION
        if (lastInputState.direction != lastDirection)
        {
            armsColliderObject.transform.localPosition = armsOffsetsByDir[(int)lastInputState.direction];

            // Handle facing direction
            IGJInputManager.InputDirection directionAfterFacingFlipping = lastInputState.direction;
            if (isFacingLeft)
            {
                switch (directionAfterFacingFlipping)
                {
                    case IGJInputManager.InputDirection.UpRight:
                        {
                            directionAfterFacingFlipping = IGJInputManager.InputDirection.UpLeft;
                        } break;

                    case IGJInputManager.InputDirection.Right:
                        {
                            directionAfterFacingFlipping = IGJInputManager.InputDirection.Left;
                        } break;

                    case IGJInputManager.InputDirection.DownRight:
                        {
                            directionAfterFacingFlipping = IGJInputManager.InputDirection.DownLeft;
                        } break;

                    case IGJInputManager.InputDirection.DownLeft:
                        {
                            directionAfterFacingFlipping = IGJInputManager.InputDirection.DownRight;
                        } break;

                    case IGJInputManager.InputDirection.Left:
                        {
                            directionAfterFacingFlipping = IGJInputManager.InputDirection.Right;
                        } break;

                    case IGJInputManager.InputDirection.UpLeft:
                        {
                            directionAfterFacingFlipping = IGJInputManager.InputDirection.UpRight;
                        } break;
                }
            }

            switch (directionAfterFacingFlipping)
            {
                case IGJInputManager.InputDirection.None:
                {
                    animator.Play("Idle");
                } break;

                case IGJInputManager.InputDirection.Up:
                {
                    animator.Play("Up");
                } break;

                case IGJInputManager.InputDirection.UpRight:
                {
                    animator.Play("UpRight");

                } break;

                case IGJInputManager.InputDirection.Right:
                {
                    animator.Play("Right");
                } break;

                case IGJInputManager.InputDirection.DownRight:
                {
                    animator.Play("DownRight");
                } break;

                case IGJInputManager.InputDirection.Down:
                {
                    animator.Play("Down");
                } break;

                case IGJInputManager.InputDirection.DownLeft:
                {
                    animator.Play("DownLeft");
                } break;

                case IGJInputManager.InputDirection.Left:
                {
                    animator.Play("Left");
                } break;

                case IGJInputManager.InputDirection.UpLeft:
                {
                    animator.Play("UpLeft");
                } break;
            }
        }

        // VALIDATE HELD OBJECT
        if (isHoldingObject && heldObject == null)
        {
            isHoldingObject = false;
            Debug.Log("Arms.cs -- held object was destroyed!");
        }

        // BUTTON PRESSED
        if (lastInputState.actionJustPressed)
        {
            // PICK UP OBJECT
            if (armsCollider.CanReturnInteractible() == true)
            {
                GameObject interactible = armsCollider.GetFirstInteractible();
                Debug.Log("Arms.cs -- pick up object");
                isHoldingObject = true;
                WorldObject obj = interactible.GetComponent<WorldObject>();
				obj.SetPhysicsEnabled(false);
                heldObject = interactible;
                heldObject.transform.parent = armsColliderObject.transform;
                heldObject.transform.localPosition = Vector3.zero;

				DeadlyObject deadlyObj = obj as DeadlyObject;
				if (deadlyObj != null)
				{
					deadlyObj.Defang();
				}
            }
            // START FLAILING
            else
            {
                isFlailing = true;
                // TODO TALK TO GRAPHICS CONTROL AND UPDATE ****************************************************
                // TODO TALK TO GRAPHICS CONTROL AND UPDATE ****************************************************
            }
        }

        // BUTTON STILL HELD DOWN
        else if (lastInputState.actionPressed && !lastInputState.actionJustPressed)
        {
            if (isHoldingObject == false)
            {
                // FLAIL AT OBJECT
                GameObject interactible = armsCollider.GetFirstInteractible();
                if (interactible != null)
                {
                    Debug.Log("Arms.cs -- flail at object");
                    WorldObject obj = interactible.GetComponent<WorldObject>();
                    if (obj != null)
                    {
                        obj.SetVelocity(armsVelocitiesByDir[(int)lastInputState.direction] * FLAIL_MULTIPLIER);
						DeadlyObject deadlyObj = obj as DeadlyObject;
						if (deadlyObj != null)
						{
							deadlyObj.Defang();
						}
                    }
                    else
                    {
                        Debug.LogWarning("Arms.cs -- no WorldObject script on held object!");
                    }
                }
            }
            else
            {
                heldObject.transform.localPosition = Vector3.zero;
            }
        }

        // BUTTON RELEASED
        else if (lastInputState.actionJustReleased)
        {
            // RELEASE/THROW OBJECT
            if (isHoldingObject)
            {
                Debug.Log("Arms.cs -- throw object");
                heldObject.transform.parent = null;

                WorldObject obj = heldObject.GetComponent<WorldObject>();
                obj.SetPhysicsEnabled(true);
                if (obj != null)
                {
                    obj.SetVelocity(armsVelocitiesByDir[(int)lastInputState.direction] * THROW_MULTIPLIER);
                }
                else
                {
                    Debug.LogWarning("Arms.cs -- no WorldObject script on held object!");
                }

                JamGame.instance.soundManager.PlaySfx(SoundManager.SFX_THROW);

                isHoldingObject = false;
            }

            // STOP FLAILING
            if (isFlailing)
            {
                isFlailing = false;
                // TODO TALK TO GRAPHICS CONTROL AND UPDATE ****************************************************
                // TODO TALK TO GRAPHICS CONTROL AND UPDATE ****************************************************
            }
        }

        lastDirection = lastInputState.direction;
    }
}

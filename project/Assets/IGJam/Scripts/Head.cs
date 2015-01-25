using UnityEngine;
using System.Collections;
using System;

public class Head : BodyPart
{
    private double mPowerLevelTimer = 0.0f;
    private bool mShouldFire = false;
    private Lazer.PowerLevel mCurrentPowerLevel = Lazer.PowerLevel.None;
    private IGJInputManager.InputDirection mDirection;
    private float mCooldown = 0.0f;
    private Animator animator;
    private bool isFacingLeft = false;

    const double mediumPowerLevelTime = 0.334f;
    const double highPowerLevelTime = 0.667f;
    private bool firingAnimationPlaying = false;

	public Head(int newPlayerIndex, Transform newSpriteTransform, CombinedPlayer newCombinedPlayer)
		: base(BodyPart.BodyPartType.HEAD, newPlayerIndex, newSpriteTransform, newCombinedPlayer)
    {
        animator = newSpriteTransform.GetComponent<Animator>();
	}

	// Update is called once per frame
	public override void Update()
    {
        // Update facing direction
        if ((combinedPlayer.GetBodyPart(BodyPartType.BODY) as Body).isFacingLeft)
        {
            isFacingLeft = true;
        }
        else
        {
            isFacingLeft = false;
        }

        if (mCooldown > 0.0f)
        {
            UpdateCooldown();
        }
        else
        {
            // check to see if this frame will be firing
            UpdateFiringStatus();

            //  if the button is pushed, then we can check for lazer firing
            if (mShouldFire)
            {
                FireTheLazer();
            }
        }

        // Idle animations
        if (!firingAnimationPlaying)
        {
            switch (mDirection)
            {
                case IGJInputManager.InputDirection.None:
                    {
                        if (isFacingLeft)
                        {
                            animator.Play("IdleLeft");
                        }
                        else
                        {
                            animator.Play("IdleRight");
                        }
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
        // Firing animations
        else
        {
            switch (mDirection)
            {
                case IGJInputManager.InputDirection.None:
                    {
                        if (isFacingLeft)
                        {
                            animator.Play("FireLeft");
                        }
                        else
                        {
                            animator.Play("FireRight");
                        }
                    } break;

                case IGJInputManager.InputDirection.Up:
                    {
                        animator.Play("FireUp");
                    } break;

                case IGJInputManager.InputDirection.UpRight:
                    {
                        animator.Play("FireUpRight");
                    } break;

                case IGJInputManager.InputDirection.Right:
                    {
                        animator.Play("FireRight");
                    } break;

                case IGJInputManager.InputDirection.DownRight:
                    {
                        animator.Play("FireDownRight");
                    } break;

                case IGJInputManager.InputDirection.Down:
                    {
                        animator.Play("FireDown");
                    } break;

                case IGJInputManager.InputDirection.DownLeft:
                    {
                        animator.Play("FireDownLeft");
                    } break;

                case IGJInputManager.InputDirection.Left:
                    {
                        animator.Play("FireLeft");
                    } break;

                case IGJInputManager.InputDirection.UpLeft:
                    {
                        animator.Play("FireUpLeft");
                    } break;
            }
        }
	}

    private void UpdateCooldown()
    {
        mCooldown -= Time.deltaTime;
    }

    private void UpdateFiringStatus()
    {
        if (lastInputState.actionPressed)
        {
            mPowerLevelTimer += Time.deltaTime;
        }
        else if (mPowerLevelTimer > 0.0f)
        {
            mShouldFire = true;
            UpdatePowerLevel();
        }
    }

    private void UpdatePowerLevel()
    {
        mCurrentPowerLevel = Lazer.PowerLevel.None;
        if (mPowerLevelTimer >= highPowerLevelTime)
        {
            mCurrentPowerLevel = Lazer.PowerLevel.High;
        }
        else if (mPowerLevelTimer >= mediumPowerLevelTime)
        {
            mCurrentPowerLevel = Lazer.PowerLevel.Medium;
        }
        else
        {
            mCurrentPowerLevel = Lazer.PowerLevel.Low;
        }
    }

    private void FireTheLazer()
    {
        mCooldown = 0.0f;
        if (mPowerLevelTimer >= highPowerLevelTime)
        {
            mCooldown = 1.0f;
        }
        else if (mPowerLevelTimer >= mediumPowerLevelTime)
        {
            mCooldown = 0.667f;
        }
        else
        {
            mCooldown = 0.334f;
        }

        mShouldFire = false;
        mPowerLevelTimer = 0.0f;

        // Handle facing direction
        IGJInputManager.InputDirection directionAfterFacingFlipping = mDirection;
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

        combinedPlayer.FireTheLazer(mCurrentPowerLevel, directionAfterFacingFlipping);

        firingAnimationPlaying = true;
        combinedPlayer.StartCoroutine(Utility.Delay(0.5f, OnFireAnimationDone));

        JamGame.instance.soundManager.PlaySfx(SoundManager.SFX_LASER_BEAM);
    }

    protected override void OnInputReceived()
    {
        mDirection = lastInputState.direction;
    }

    private void OnFireAnimationDone()
    {
        firingAnimationPlaying = false;
    }
}

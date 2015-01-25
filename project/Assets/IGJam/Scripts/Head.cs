using UnityEngine;
using System.Collections;
using System;

public class Head : BodyPart
{
    private double mPowerLevelTimer = 0.0f;
    private bool mShouldFire = false;
    private Lazer.PowerLevel mCurrentPowerLevel = Lazer.PowerLevel.None;
    private IGJInputManager.InputDirection mDirection;

	public Head(int newPlayerIndex, Transform newSpriteTransform, CombinedPlayer newCombinedPlayer)
		: base(BodyPart.BodyPartType.HEAD, newPlayerIndex, newSpriteTransform, newCombinedPlayer)
	{
	}

	// Update is called once per frame
	public override void Update()
    {
        mDirection = lastInputState.direction;

        // check to see if this frame will be firing
        UpdateFiringStatus();

        //  if the button is pushed, then we can check for lazer firing
        if (mShouldFire)
        {
            FireTheLazer();
        }
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
        const double mediumPowerLevelTime = 0.5f; // half a second
        const double highPowerLevelTime = 1.0f; // 1 second

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
        mShouldFire = false;
        mPowerLevelTimer = 0.0f;
        combinedPlayer.FireTheLazer(mCurrentPowerLevel, mDirection);
    }

    protected override void OnInputReceived()
    {
    }
}

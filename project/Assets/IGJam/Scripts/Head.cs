using UnityEngine;
using System.Collections;
using System;

public class Head : BodyPart
{
    private double mPowerLevelTimer = 0.0f;
    private bool mShouldFire = false;
    private Lazer.PowerLevel mCurrentPowerLevel = Lazer.PowerLevel.None;

	public Head(int newPlayerIndex, Transform newSpriteTransform, CombinedPlayer newCombinedPlayer)
		: base(BodyPart.BodyPartType.HEAD, newPlayerIndex, newSpriteTransform, newCombinedPlayer)
	{
	}

	// Update is called once per frame
	public override void Update() 
	{
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
        //if (Input.GetKeyDown(KeyCode.Space)) // this needs to be fixed yo
        {
            mPowerLevelTimer += Time.deltaTime;
        }
        else if (mPowerLevelTimer > 0.0f)
        {
            mShouldFire = true;
            UpdatePowerLevel();
            mPowerLevelTimer = 0.0f;
        }
    }

    private void UpdatePowerLevel()
    {
        const double mediumPowerLevelTime = 1.0f; // 1 second
        const double highPowerLevelTime = 2.0f;

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
        combinedPlayer.FireTheLazer(mCurrentPowerLevel, lastInputState.direction);
    }
}

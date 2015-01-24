using UnityEngine;
using System.Collections;
using System;

public class Head : BodyPart
{

    public enum PowerLevel
    {
        None,
        Low,
        Medium,
        High
    }

    private double mPowerLevelTimer = 0.0f;
    private bool mShouldFire = false;
    private PowerLevel mCurrentPowerLevel = PowerLevel.None;
    private Transform mTransform;

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
            mCurrentPowerLevel = PowerLevel.None;
        }
	}

    private void UpdateFiringStatus()
    {
        // check input on 
        if (Input.GetKey(KeyCode.Space))
        {
            mPowerLevelTimer += Time.deltaTime;
        }
        else if (mCurrentPowerLevel != PowerLevel.None)
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

        if (mPowerLevelTimer >= highPowerLevelTime)
        {
            mCurrentPowerLevel = PowerLevel.High;
        }
        else if (mPowerLevelTimer >= mediumPowerLevelTime)
        {
            mCurrentPowerLevel = PowerLevel.Medium;
        }
        else
        {
            mCurrentPowerLevel = PowerLevel.Low;
        }
    }

    private void FireTheLazer()
    {
    }
}

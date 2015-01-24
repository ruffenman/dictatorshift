using UnityEngine;
using System.Collections;

public class Head : BodyPart
{
    public enum PowerLevel
    {
        None,
        Low,
        Medium,
        High
    }

    private double powerLevelTimer = 0.0f;
    private bool shouldFire = false;
    private PowerLevel currentPowerLevel = PowerLevel.None;
    private Transform mTransform;
    public GameObject lazerPrefab;

	public Head(int newPlayerIndex, Transform newSpriteTransform)
		: base(BodyPart.BodyPartType.HEAD, newPlayerIndex, newSpriteTransform)
	{
	}

	// Update is called once per frame
	public override void Update() 
	{
        // check to see if this frame will be firing
        UpdateFiringStatus();

        //  if the button is pushed, then we can check for lazer firing
        if (shouldFire)
        {
            FireTheLazer();
            currentPowerLevel = PowerLevel.None;
        }
	}

    private void UpdateFiringStatus()
    {
        // check input on 
        if (Input.GetKey(KeyCode.Space))
        {
            powerLevelTimer += Time.deltaTime;
        }
        else if (currentPowerLevel != PowerLevel.None)
        {
            shouldFire = true;
            UpdatePowerLevel();
            powerLevelTimer = 0.0f;
        }
    }

    private void UpdatePowerLevel()
    {
        const double mediumPowerLevelTime = 1.0f; // 1 second
        const double highPowerLevelTime = 2.0f;

        if (powerLevelTimer >= highPowerLevelTime)
        {
            currentPowerLevel = PowerLevel.High;
        }
        else if (powerLevelTimer >= mediumPowerLevelTime)
        {
            currentPowerLevel = PowerLevel.Medium;
        }
        else
        {
            currentPowerLevel = PowerLevel.Low;
        }
    }

    private void FireTheLazer()
    {
        // first get the directional value
        //double lazerDirection = 180.0f;
        //GameObject Clone;
        //Clone = (Instantiate(lazerPrefab, transform.position, transform.rotation)) as GameObject;
    }
}

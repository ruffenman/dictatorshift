using UnityEngine;
using System.Collections;

public class Lazer : MonoBehaviour
{
    public enum PowerLevel
    {
        None,
        Low,
        Medium,
        High
    }

    private PowerLevel mPowerLevel = PowerLevel.None;
    private IGJInputManager.InputDirection mDirection;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	    //expand me PLEASE
	}

    public void SetLazerStrength(PowerLevel powerLevel, IGJInputManager.InputDirection direction)
    {
        mPowerLevel = powerLevel;
        mDirection = direction;

        if (mPowerLevel == PowerLevel.Low)
        {
            transform.localScale -= new Vector3(0.0f, 2.5f, 0.0f);
        }
        else if (mPowerLevel == PowerLevel.High)
        {
            transform.localScale += new Vector3(0.0f, 2.5f, 0.0f);
        }
    }
}

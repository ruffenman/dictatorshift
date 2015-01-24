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

    private Transform mMiddleSection = null;
    private Transform mEndSection = null;

	// Use this for initialization
	void Start ()
    {
        mMiddleSection = transform.FindChild("Pivot").FindChild("Middle");
        mEndSection = transform.FindChild("Pivot").FindChild("End");
	}
	
	// Update is called once per frame
	void Update ()
    {
        const float SCALE = 100.0f;
        float sizeIncrease = Time.deltaTime;
        mMiddleSection.position += new Vector3(sizeIncrease * SCALE, 0.0f, 0.0f);
        mMiddleSection.localScale += new Vector3(0.0f, sizeIncrease * 2.5f * SCALE, 0.0f);
        mEndSection.position += new Vector3(sizeIncrease * 2.0f * SCALE, 0.0f, 0.0f);

        if (mMiddleSection.localScale.y >= 50)
        {
            Destroy(gameObject);
        }
	}

    public void SetLazerStrength(PowerLevel powerLevel, IGJInputManager.InputDirection direction)
    {
        mPowerLevel = powerLevel;
        mDirection = direction;

        if (mPowerLevel == PowerLevel.Low)
        {
            //transform.localScale -= new Vector3(0.0f, 2.5f, 0.0f);
        }
        else if (mPowerLevel == PowerLevel.High)
        {
            transform.localScale += new Vector3(0.0f, 2.5f, 0.0f);
        }
    }
}

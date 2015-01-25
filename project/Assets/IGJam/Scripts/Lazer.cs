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
    private float mAngle = 0.0f;

    private Vector3 mMidPosVec;
    private Vector3 mMidScaleVec;
    private Vector3 mEndPosVec;

    private Vector3 mOffset;

    private Transform mPivot = null;
    private Transform mStartSection = null;
    private Transform mMiddleSection = null;
    private Transform mEndSection = null;
    private Transform mPlayerHead = null;

	// Use this for initialization
	void Start ()
    {
        mPivot = transform.FindChild("Pivot");
        mStartSection = mPivot.FindChild("Start");
        mMiddleSection = mPivot.FindChild("Middle");
        mEndSection = mPivot.FindChild("End");

        CombinedPlayer player = JamGame.instance.player;
        mPlayerHead = player.transform.FindChild("Graphics").FindChild("Head");

        mPivot.localRotation = Quaternion.AngleAxis(mAngle, Vector3.forward);

        mOffset = new Vector3(0.325f, 0.15f, 0.0f);

        //update the location
        transform.position = mPlayerHead.position + mOffset;
	}
	
	// Update is called once per frame
	void Update ()
    {
        //update the location
        transform.position = mPlayerHead.position + mOffset;

        //  scale the lazer
        const float SCALE = 1.0f;
        float sizeIncrease = Time.deltaTime;

        mMiddleSection.localPosition += new Vector3(sizeIncrease * SCALE, 0.0f, 0.0f);
        mMiddleSection.localScale += new Vector3(0.0f, sizeIncrease * 2.0f * SCALE, 0.0f);
        mEndSection.localPosition += new Vector3(sizeIncrease * 2.0f * SCALE, 0.0f, 0.0f);

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
            transform.localScale -= new Vector3(0.25f, 0.25f, 0.0f);
        }
        else if (mPowerLevel == PowerLevel.High)
        {
            transform.localScale += new Vector3(0.25f, 0.25f, 0.0f);
        }

        mAngle = 0.0f;
        switch (mDirection)
        {
            case IGJInputManager.InputDirection.UpRight:
                mAngle = 45.0f;
                break;
            case IGJInputManager.InputDirection.Up:
                mAngle = 90.0f;
                break;
            case IGJInputManager.InputDirection.UpLeft:
                mAngle = 135.0f;
                break;
            case IGJInputManager.InputDirection.Left:
                mAngle = 180.0f;
                break;
            case IGJInputManager.InputDirection.DownLeft:
                mAngle = 225.0f;
                break;
            case IGJInputManager.InputDirection.Down:
                mAngle = 270.0f;
                break;
            case IGJInputManager.InputDirection.DownRight:
                mAngle = 315.0f;
                break;
            case IGJInputManager.InputDirection.Right:
                // fallthrough
            default:
                // do nothing
                break;
        }
    }
}

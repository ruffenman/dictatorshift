using UnityEngine;
using System.Collections;

public class Lazer : WorldObject
{
    public enum PowerLevel
    {
        None,
        Low,
        Medium,
        High
    }

    public const float mLazerSpeed = 100.0f;
    public const float mLazerLength = 100.0f;
    public const float mLazerTimer = 1.35f;
    Vector3 mPowerSizeScale = new Vector3(0.5f, 0.0f, 0.0f);

    private PowerLevel mPowerLevel = PowerLevel.None;
    private IGJInputManager.InputDirection mDirection;
    private float mAngle = 0.0f;
    private Vector3 mOffset;

    private Transform mPivot = null;
    private Transform mStartSection = null;
    private Transform mMiddleSection = null;
    private Transform mEndSection = null;
    private Transform mPlayerHead = null;
    public float mLazerCurrentTimer;

	// Use this for initialization
    new void Start()
    {
        mPivot = transform.FindChild("Pivot");
        mStartSection = mPivot.FindChild("Start");
        mMiddleSection = mPivot.FindChild("Middle");
        mEndSection = mPivot.FindChild("End");

        CombinedPlayer player = JamGame.instance.player;
        mPlayerHead = player.transform.FindChild("Graphics");

        mPivot.localRotation = Quaternion.AngleAxis(mAngle, Vector3.forward);

        mOffset = new Vector3(0.275f, 0.2f, 0.0f);

        //update the location
        transform.position = mPlayerHead.position + mOffset;

        objectType = ObjectType.STABLE;

        if (mPowerLevel == PowerLevel.Low)
        {
            mStartSection.transform.localScale -= mPowerSizeScale;
            mMiddleSection.transform.localScale -= mPowerSizeScale;
            mEndSection.transform.localScale -= mPowerSizeScale;
        }
        else if (mPowerLevel == PowerLevel.High)
        {
            mStartSection.transform.localScale += mPowerSizeScale;
            mMiddleSection.transform.localScale += mPowerSizeScale;
            mEndSection.transform.localScale += mPowerSizeScale;
        }

        mLazerCurrentTimer = mLazerTimer;
	}

    public void SetLazerStrength(PowerLevel powerLevel, IGJInputManager.InputDirection direction)
    {
        mPowerLevel = powerLevel;
        mDirection = direction;

        mAngle = 0.0f;
        switch (mDirection)
        {
            case IGJInputManager.InputDirection.Right:
                mAngle = 0.0f;
                break;
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
            default:
                // do nothing
                break;
        }
    }

    protected override void UpdateInternal()
    {
        mLazerCurrentTimer -= Time.deltaTime;

        //update the location
        transform.position = mPlayerHead.position + mOffset;

        //  scale the lazer
        float sizeIncrease = Time.deltaTime;

        mMiddleSection.localPosition += new Vector3(sizeIncrease * mLazerSpeed, 0.0f, 0.0f);
        mMiddleSection.localScale += new Vector3(0.0f, sizeIncrease * 2.0f * mLazerSpeed, 0.0f);
        mEndSection.localPosition += new Vector3(sizeIncrease * 2.0f * mLazerSpeed, 0.0f, 0.0f);

        if (mLazerCurrentTimer < 0 || mPowerLevel == PowerLevel.None)
        {
            Destroy(gameObject);
        }

    }

    public void OnLowerPowerLevel()
    {
        if (mPowerLevel != PowerLevel.None)
        {
            --mPowerLevel;

            mStartSection.transform.localScale -= mPowerSizeScale;
            mMiddleSection.transform.localScale -= mPowerSizeScale;
            mEndSection.transform.localScale -= mPowerSizeScale;
        }
    }

    public PowerLevel GetPowerLevel()
    {
        return mPowerLevel;
    }
}

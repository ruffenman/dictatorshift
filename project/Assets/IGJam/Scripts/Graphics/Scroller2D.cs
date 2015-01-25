using UnityEngine;
using System.Collections;
using System;

public class Scroller2D : MonoBehaviour 
{
    [Serializable]
    public enum ScrollerLayer
    {
        Background0,
        Background1,
        Background2,
        Stage,
        Foreground0,
        Foreground1,
    }

    public ScrollerLayer scrollerLayer = ScrollerLayer.Stage;

	private void Awake()
	{
		mTransform = transform;
	}

	private void Start()
	{
		mLastCameraPosition = Camera.main.transform.position;
	}

	private void LateUpdate()
	{
		// offset sprite based on main camera position
		float offsetMod = 0;
        switch (scrollerLayer)
		{
            case ScrollerLayer.Background0:
			{
				offsetMod = 1;
				break;
			}

            case ScrollerLayer.Background1:
			{
				offsetMod = .75f;
				break;
			}

            case ScrollerLayer.Background2:
			{
				offsetMod = 0.5f;
				break;
			}

            case ScrollerLayer.Foreground0:
			{
				offsetMod = -1.25f;
				break;
			}

            case ScrollerLayer.Foreground1:
			{
				offsetMod = -2f;
				break;
			}
		}

		Vector3 cameraPos = Camera.main.transform.position;
		Vector3 cameraDelta = cameraPos - mLastCameraPosition;
		mTransform.localPosition += new Vector3(cameraDelta.x * offsetMod, cameraDelta.y * offsetMod, 0); 
		mLastCameraPosition = cameraPos;
	}

	private Transform mTransform;
	private Vector3 mLastCameraPosition;
}

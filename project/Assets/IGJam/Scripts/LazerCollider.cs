﻿using UnityEngine;
using System.Collections;

public class LazerCollider : WorldObject
{
    Lazer mMahLazer;

    public void Start()
    {
        mMahLazer = transform.parent.parent.gameObject.GetComponent<Lazer>();
        objectType = ObjectType.STABLE;
    }

    void OnTriggerEnter(Collider other)
    {
		if (mMahLazer == null)
		{
			mMahLazer = transform.parent.parent.gameObject.GetComponent<Lazer>();
		}
        if ((mMahLazer) && (mMahLazer.GetPowerLevel() != Lazer.PowerLevel.None))
        {
			WorldObject worldObject = other.GetComponent<WorldObject>();
            DeadlyObject deadlyObject = worldObject as DeadlyObject;
            
			if (worldObject != null && (!worldObject.gameObject.CompareTag("Player")))
            {
				bool canDestroy = true;

				if (deadlyObject)
				{
					canDestroy = deadlyObject.destroyOnCollision;
				}
                if (canDestroy)
                {
					if (deadlyObject)
					{
						deadlyObject.OnCollideWithTarget(gameObject);
					}
					else
					{
						worldObject.TakeDamage(500);
					}
					mMahLazer.OnLowerPowerLevel();
                }
            }
        }
    }
}

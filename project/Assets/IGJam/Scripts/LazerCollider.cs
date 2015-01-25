using UnityEngine;
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
        if (mMahLazer.GetPowerLevel() != Lazer.PowerLevel.None)
        {
            DeadlyObject deadlyObject = other.GetComponent<DeadlyObject>();
            if (deadlyObject != null)
            {
                deadlyObject.OnCollideWithTarget(gameObject);
                if (deadlyObject.destroyOnCollision)
                {
                    mMahLazer.OnLowerPowerLevel();
                }
            }
        }
    }
}

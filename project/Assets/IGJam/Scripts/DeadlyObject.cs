﻿using UnityEngine;
using System.Collections;

public class DeadlyObject : WorldObject 
{
	public bool destroyOnCollision = false;
	public float defangedTime = 0.5f;
	protected float damage = 100.0f;
	protected bool defanged = false;

	public virtual void OnCollideWithTarget(GameObject target)
	{
		Debug.Log ("CAUSED COLLIDE");
		WorldObject worldObject = target.GetComponent<WorldObject>();
		if ((worldObject != null) && !defanged)
		{
			Debug.Log ("CAUSED DAMAGE");
			worldObject.TakeDamage(damage);
			if(destroyOnCollision)
			{
				Destroy(gameObject);

                JamGame.instance.soundManager.PlaySfx(SoundManager.SFX_SAW);
			}
		}
	}

	public void Defang(bool shouldRefang = true)
	{
		Debug.Log("Defanged");
		defanged = true;
		StartCoroutine(Utility.Delay(defangedTime, Refang));
	}

	public void Refang()
	{
		Debug.Log("Refanged");
		defanged = false;
	}

	protected virtual void OnDestroyed()
	{
	}
}
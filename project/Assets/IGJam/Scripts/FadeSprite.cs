using UnityEngine;
using System.Collections;

public class FadeSprite : MonoBehaviour 
{
	float lifeTime = 0f;
	bool playing = false;
	bool done = false;
	
	public float startAfter;
	public SpriteRenderer sr;
	public float targetAlpha;
	public float sourceAlpha;
	public float runTime;
	
	void Update () 
	{
		if(!done)
		{
			lifeTime += Time.deltaTime;
			if(!playing)
			{
				if(startAfter < lifeTime)
				{
					playing = true;
					lifeTime = 0f;
				}
			}
			else
			{
				float fraction = lifeTime / runTime;
				if(fraction > 1)
				{
					sr.color = new Color(1,1,1,1);
					done = true;
				}
				else
				{
					sr.color = new Color(fraction, fraction, fraction, 1f);
				}
			}	
		}
	}
}

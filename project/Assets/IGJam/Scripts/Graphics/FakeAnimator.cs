using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FakeAnimator : MonoBehaviour 
{
	public float timeBetweenFrames;
	float timeUntilNext;
	
	public bool isPaused = false;
	public List<Sprite> frames;
	int index = 0;
	
	SpriteRenderer sr;

	void Start()
	{
		sr = GetComponent<SpriteRenderer>();
		timeUntilNext = timeBetweenFrames;
	}
	
	public void Flip(bool yes)
	{
		if(yes)
		{
			this.gameObject.transform.rotation = new Quaternion(0,180,0,0);
		}
		else
		{
			this.gameObject.transform.rotation = new Quaternion(0,0,0,0);
		}
	}
	
	public void PauseAndIdle()
	{
		index = 0;
		isPaused = true;
		Frame ();
	}
	
	public void Animate()
	{
		isPaused = false;
	}

	void Update()
	{
		if(!isPaused)
		{
			timeUntilNext -= Time.deltaTime;
			if(timeUntilNext < 0)
			{
				Frame();
				timeUntilNext = timeBetweenFrames;
				++index;
				if(index == frames.Count)
				{
					index = 0;
				}
			}
		}
	}
	
	void Frame()
	{
		sr.sprite = frames[index];
	}
	
}

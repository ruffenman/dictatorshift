using UnityEngine;
using System.Collections;

public class DisplayScreen : MonoBehaviour 
{
	public float duration = 1.0f;
	private float timer = -1;

	private bool showing = false;

	Vector3 firstPosition;

	// Update is called once per frame
	void Update () 
	{
		if (timer > 0)
		{
			timer -= Time.deltaTime;
		}
		else
		{
			if (showing)
			{
				Destroy(gameObject);
			}
		}
	}

	public void Show()
	{
		firstPosition = transform.position;
		timer = duration;
		renderer.enabled = true;
		transform.parent = Camera.main.transform;
		transform.localPosition = new Vector3(0,0,10);
		showing = true;
	}
}

using UnityEngine;
using System.Collections;

public class DisplayScreen : MonoBehaviour 
{
	public float duration = 1.0f;
	private float timer = -1;
	public bool winScreen = false;
	private bool showing = false;
	public GameObject teleporter;

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
				if (winScreen)
				{
					Application.LoadLevel("TitleScene");
				}
				Destroy(gameObject);
			}
		}
	}

	public void Show()
	{
		bool canShow = true;
		if (teleporter != null)
		{
			SmallTeleporter st = teleporter.GetComponent<SmallTeleporter>();
			if (st)
			{
				canShow = st.isActive;
			}
		}

		if (canShow)
		{
			firstPosition = transform.position;
			timer = duration;
			renderer.enabled = true;
			transform.parent = Camera.main.transform;
			transform.localPosition = new Vector3(0, 0, 10);
			showing = true;
		}
	}
}

using UnityEngine;
using System.Collections;

public class ChangeSceneAfter : MonoBehaviour 
{
	public GameObject playMusicPrefab;
	public float countdown;
	
	void Start()
	{
		GameObject.Instantiate (playMusicPrefab);
	}
	
	void Update()
	{
		countdown -= Time.deltaTime;
		if(countdown < 0)
		{
			Application.LoadLevel ("Scene1");
		}
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomManager : MonoBehaviour 
{
	public List<GameObject> rooms = new List<GameObject>();
	GameObject currentRoomObject;
	int currentRoomIndex = 0;
	bool isRoomInstantiated = false;
	
	void Start()
	{
		CreateRoom();
	}
	
	public void CreateRoom()
	{
		currentRoomObject = (GameObject)Instantiate (rooms[currentRoomIndex]);
		GameObject player = GameObject.Find ("Player");
		if(player == null) { Debug.LogError ("Why is there no player in the scene??"); return; }
		player.transform.position = GameObject.FindGameObjectWithTag("Respawn").transform.position;
	}
	
	public void DestroyRoom()
	{
		Destroy (currentRoomObject);
	}
	
	public void RoomComplete()
	{
		++currentRoomIndex;
		DestroyRoom ();
		CreateRoom ();
	}
	
	public void PlayerDeath()
	{
		DestroyRoom ();
		CreateRoom ();
	}
}

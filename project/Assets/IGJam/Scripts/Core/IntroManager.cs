using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IntroManager : MonoBehaviour 
{
	public string SCENE_TO_LOAD_NEXT = "UIScreen";
	float playerFakeGravityY = -3f;
	//float playerFloorPositionY = -3.44f;
	//float wallBoundsX = -5.4f;
	float playerFloorPositionY = -3.15f;
	float wallBoundsX = -6.4f;
	float playerFallSpeedY = -2f;
	float playerMoveSpeedX = 3f;
	float teleporterTriggerX = 5.3f;
	float beforeTeleporterTriggerX = 3f;
	bool canEnterTeleporter = false;

	public GameObject titleLogo;
	public GameObject activeEnvironment;
	public GameObject inactiveEnvironment;
	public List<bool> playersInGame;
	public List<bool> playersInTeleporter;
	public List<GameObject> fakePlayerObjects;
	public List<GameObject> playerStartPrompts;
	public List<GameObject> playerTeleportersWaiting;
	public List<GameObject> playerTeleportersDone;
	public IGJInputManager.InputState[] playerInputStates;
	public GameObject backgroundMusicPrefab;
	
	public static IntroManager introManager;

	public void ReceiveInput(IGJInputManager.InputState[] inputStates)
	{
		for (int i = 0; i < inputStates.Length; ++i)
		{
			playerInputStates[i] = inputStates[i];
		}
	}

	void Awake()
	{
		// assume direct control
		IGJInputManager.overrideInputForIntro = true;
		introManager = this;
		playerInputStates = new IGJInputManager.InputState[4] 
		{ 
			new IGJInputManager.InputState(){directionVec = Vector3.zero, actionJustPressed = false, actionPressed = false},
			new IGJInputManager.InputState(){directionVec = Vector3.zero, actionJustPressed = false, actionPressed = false},
			new IGJInputManager.InputState(){directionVec = Vector3.zero, actionJustPressed = false, actionPressed = false},
			new IGJInputManager.InputState(){directionVec = Vector3.zero, actionJustPressed = false, actionPressed = false},
		};
		
		GameObject.Instantiate (backgroundMusicPrefab);
	}

	void CloseAndStartGame()
	{
		// nuke this entire prefab
		IGJInputManager.overrideInputForIntro = false;
		Application.LoadLevel (SCENE_TO_LOAD_NEXT);
	}
	
	void Update()
	{
		// *********** DEBUG **********************************
		// spawn
		if(Input.GetKeyDown (KeyCode.Q))
		{
			playerInputStates[0].actionPressed = true;
		}
		if(Input.GetKeyDown (KeyCode.W))
		{
			playerInputStates[1].actionPressed = true;
		}
		if(Input.GetKeyDown (KeyCode.E))
		{
			playerInputStates[2].actionPressed = true;
		}
		if(Input.GetKeyDown (KeyCode.R))
		{
			playerInputStates[3].actionPressed = true;
		}
		// move right
		if(Input.GetKey (KeyCode.A))
		{
			playerInputStates[0].directionVec.x = 1;
		}
		if(Input.GetKey (KeyCode.S))
		{
			playerInputStates[1].directionVec.x = 1;
		}
		if(Input.GetKey (KeyCode.D))
		{
			playerInputStates[2].directionVec.x = 1;
		}
		if(Input.GetKey (KeyCode.F))
		{
			playerInputStates[3].directionVec.x = 1;
		}
		// move
		if(Input.GetKey (KeyCode.Z))
		{
			playerInputStates[0].directionVec.x = -1;
		}
		if(Input.GetKey (KeyCode.X))
		{
			playerInputStates[1].directionVec.x = -1;
		}
		if(Input.GetKey (KeyCode.C))
		{
			playerInputStates[2].directionVec.x = -1;
		}
		if(Input.GetKey (KeyCode.V))
		{
			playerInputStates[3].directionVec.x = -1;
		}
		// *********** DEBUG **********************************
	
		// The player input feed should be piped in automatically
		bool everyoneInGame = true;
		bool everyoneInTeleporter = true;
		for (int i = 0; i < playerInputStates.Length; ++i)
		{
			if(playersInTeleporter[i] == false)
			{
				everyoneInTeleporter = false;
			
				// Handle when a player joins
				if(playersInGame[i] == false && playerInputStates[i].actionPressed)
				{
					HandlePlayerJoined (i);
				}
				// If the player was already in the game
				else if(playersInGame[i] == true)
				{
					HandlePlayerMovement (i);
					CheckAndObeyBounds(i);
				}
				else
				{
					everyoneInGame = false;
				}
			}
		}
		if(everyoneInGame && canEnterTeleporter == false)
		{
			SwapToActiveEnvironment(true);
		}
		if(everyoneInTeleporter)
		{
			CloseAndStartGame ();
		}
	}
	
	void CheckAndObeyBounds(int i)
	{
		// test floor
		if(fakePlayerObjects[i].transform.position.y < playerFloorPositionY)
		{
			fakePlayerObjects[i].transform.position = new Vector3
				(
					fakePlayerObjects[i].transform.position.x, 
					playerFloorPositionY, 
					fakePlayerObjects[i].transform.position.z
				);
		}
		
		// test left wall
		if(fakePlayerObjects[i].transform.position.x < wallBoundsX)
		{
			fakePlayerObjects[i].transform.position = new Vector3
				(
					wallBoundsX, 
					fakePlayerObjects[i].transform.position.y, 
					fakePlayerObjects[i].transform.position.z
					);
		}
		// test right side before teleporter
		if(canEnterTeleporter == false)
		{
			
			if(fakePlayerObjects[i].transform.position.x > beforeTeleporterTriggerX)
			{
				fakePlayerObjects[i].transform.position = new Vector3
					(
						beforeTeleporterTriggerX, 
						fakePlayerObjects[i].transform.position.y, 
						fakePlayerObjects[i].transform.position.z
						);
			}
		}
		// test the actual teleporter
		else
		{			
			if(fakePlayerObjects[i].transform.position.x > teleporterTriggerX)
			{
				HandlePlayerEnteredPortal(i);
			}
		}
	}
	
	void HandlePlayerMovement(int i)
	{
		fakePlayerObjects[i].transform.position = new Vector3
		(
			fakePlayerObjects[i].transform.position.x + playerMoveSpeedX * playerInputStates[i].directionVec.x * Time.deltaTime, 
			fakePlayerObjects[i].transform.position.y - playerFakeGravityY * playerFallSpeedY * Time.deltaTime, 
			fakePlayerObjects[i].transform.position.z
		);
		
		if(playerMoveSpeedX * playerInputStates[i].directionVec.x != 0)
		{
			fakePlayerObjects[i].GetComponent<FakeAnimator>().Animate();
			if(playerMoveSpeedX * playerInputStates[i].directionVec.x < 0)
			{
				fakePlayerObjects[i].GetComponent<FakeAnimator>().Flip(true);
			}
			else if(playerMoveSpeedX * playerInputStates[i].directionVec.x > 0)
			{
				fakePlayerObjects[i].GetComponent<FakeAnimator>().Flip(false);
			}
		}
		else
		{
			fakePlayerObjects[i].GetComponent<FakeAnimator>().PauseAndIdle();
		}
	}
	
	void HandlePlayerJoined(int player)
	{
		playersInGame[player] = true;
		fakePlayerObjects[player].SetActive (true);
		playerStartPrompts[player].SetActive (false);
		playerTeleportersWaiting[player].SetActive (false);
		playerTeleportersDone[player].SetActive (true);
	}
	
	void HandlePlayerEnteredPortal(int player)
	{
		playersInTeleporter[player] = true;
		fakePlayerObjects[player].transform.Translate (new Vector3(0,0.3f,0));
		fakePlayerObjects[player].GetComponent<FakeAnimator>().PauseAndIdle();
		SpriteRenderer sr = fakePlayerObjects[player].GetComponent<SpriteRenderer>();
		sr.color = new Color(0,0,0,1f);
		StartCoroutine(Utility.Delay(1f, DisablePlayers));
	}
	
	void DisablePlayers()
	{
		for (int i = 0; i < playerInputStates.Length; ++i)
		{
			if(playersInTeleporter[i] == true)
			{
				fakePlayerObjects[i].SetActive (false);
			}
		}
	}
	
	void SwapToActiveEnvironment(bool yes)
	{
		canEnterTeleporter = true;
		activeEnvironment.SetActive (true);
		inactiveEnvironment.SetActive (false);
	}
	
}

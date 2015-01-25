using UnityEngine;
using System.Collections;
using System;

public class IGJInputManager : MonoBehaviour 
{
    public class InputState
    {
        public Vector3 directionVec;
        public InputDirection direction;
        public bool actionPressed;
        public bool actionJustPressed;
    }

    public enum InputDirection
    {
        None,
        Up,
        UpRight,
        Right,
        Down,
        DownRight,
        DownLeft,
        Left,
        UpLeft,
    }

	public float upBoundsAngle = (3f / 8f);
	public float rightBoundsAngle = (5f / 8f);
	public float leftBoundsAngle = (1f / 8f);
	public float downBoundsAngle = (15f / 8f);
    
	// Use this for initialization
	private void Start () 
    {
        inputStates = new InputState[4] 
        { 
            new InputState(){directionVec = Vector3.zero, actionJustPressed = false, actionPressed = false},
            new InputState(){directionVec = Vector3.zero, actionJustPressed = false, actionPressed = false},
            new InputState(){directionVec = Vector3.zero, actionJustPressed = false, actionPressed = false},
            new InputState(){directionVec = Vector3.zero, actionJustPressed = false, actionPressed = false},
        };
	}
	
	// Update is called once per frame
	private void Update () 
    {
        Action<string, string, string, InputState> processInputFunc = (xAxisName, yAxisName, actionButtonName, inputState) => 
        {
            float x = Input.GetAxis(xAxisName);
            float y = Input.GetAxis(yAxisName);
            Vector3 inputVec = new Vector3(x, y, 0);
            InputDirection direction = InputDirection.None;
            bool actionPressed = Input.GetButton(actionButtonName);
            bool actionJustPressed = Input.GetButtonDown(actionButtonName);

            Vector3 dirVec = Vector3.zero;
            if (x > Mathf.Cos(upBoundsAngle * Mathf.PI))
            {
                dirVec.x = 1;
            }
            else if (x < Mathf.Cos(rightBoundsAngle * Mathf.PI))
            {
                dirVec.x = -1;
            }
            if (y > Mathf.Sin(leftBoundsAngle * Mathf.PI))
            {
                dirVec.y = 1;
            }
            else if (y < Mathf.Sin(downBoundsAngle * Mathf.PI))
            {
                dirVec.y = -1;
            }

            if (dirVec.y > 0)
            {
                if (dirVec.x > 0)
                {
                    direction = InputDirection.UpRight;
                }
                else if (dirVec.x < 0)
                {
                    direction = InputDirection.UpLeft;
                }
                else
                {
                    direction = InputDirection.Up;
                }
            }
            else if (dirVec.y < 0)
            {
                if (dirVec.x > 0)
                {
                    direction = InputDirection.DownRight;
                }
                else if (dirVec.x < 0)
                {
                    direction = InputDirection.DownLeft;
                }
                else
                {
                    direction = InputDirection.Down;
                }
            }
            else
            {
                if (dirVec.x > 0)
                {
                    direction = InputDirection.Right;
                }
                else if (dirVec.x < 0)
                {
                    direction = InputDirection.Left;
                }
            }

            dirVec.Normalize();

            inputState.directionVec.x = dirVec.x;
            inputState.directionVec.y = dirVec.y;
            inputState.direction = direction;
            inputState.actionPressed = actionPressed;
            inputState.actionJustPressed = actionJustPressed;

            if (inputVec.sqrMagnitude > 0 || dirVec.sqrMagnitude > 0 || actionPressed || actionJustPressed)
            {
                int playerIndex = Array.IndexOf(inputStates, inputState) + 1;
                Debug.Log("\tp" + playerIndex + "_input_vec: " + inputVec + "\tp" + playerIndex + "_dir: " + dirVec + "\tp" + playerIndex + "_direction: " + direction + "\tp" + playerIndex + "_actionPressed: " + actionPressed + "\tp" + playerIndex + "_actionJustPressed: " + actionJustPressed);
            }
        };

	    // Process player 1
        processInputFunc(P1_X, P1_Y, P1_ACTION, inputStates[0]);

        // Process player 2
        processInputFunc(P2_X, P2_Y, P2_ACTION, inputStates[1]);

        // Process player 3
        processInputFunc(P3_X, P3_Y, P3_ACTION, inputStates[2]);

        // Process player 4
        processInputFunc(P4_X, P4_Y, P4_ACTION, inputStates[3]);

        CombinedPlayer player = JamGame.instance.player;
        player.ReceiveInput(inputStates);
	}

    private const string P1_X = "player1_x";
    private const string P1_Y = "player1_y";
    private const string P1_ACTION = "player1_action";
    private const string P2_X = "player2_x";
    private const string P2_Y = "player2_y";
    private const string P2_ACTION = "player2_action";
    private const string P3_X = "player3_x";
    private const string P3_Y = "player3_y";
    private const string P3_ACTION = "player3_action";
    private const string P4_X = "player4_x";
    private const string P4_Y = "player4_y";
    private const string P4_ACTION = "player4_action";

    private InputState[] inputStates;
}

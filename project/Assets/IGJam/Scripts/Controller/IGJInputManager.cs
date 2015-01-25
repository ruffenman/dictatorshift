using UnityEngine;
using System.Collections;

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
	private void Start () {
        inputStates = new InputState[4] 
        { 
            new InputState(){directionVec = Vector3.zero, actionJustPressed = false, actionPressed = false},
            new InputState(){directionVec = Vector3.zero, actionJustPressed = false, actionPressed = false},
            new InputState(){directionVec = Vector3.zero, actionJustPressed = false, actionPressed = false},
            new InputState(){directionVec = Vector3.zero, actionJustPressed = false, actionPressed = false},
        };
	}
	
	// Update is called once per frame
	private void Update () {
	    // Process player 1
        float p1_x = Input.GetAxis(P1_X);
        float p1_y = Input.GetAxis(P1_Y);
        Vector3 p1_inputVec = new Vector3(p1_x, p1_y, 0);
        InputDirection p1_direction = InputDirection.None;
        bool p1_actionPressed = Input.GetButton(P1_ACTION);
        bool p1_actionJustPressed = Input.GetButtonDown(P1_ACTION);

        Vector3 p1_dirVec = Vector3.zero;
        if(p1_x > Mathf.Cos(upBoundsAngle * Mathf.PI))
        {
            p1_dirVec.x = 1;
        }
        else if (p1_x < Mathf.Cos(rightBoundsAngle * Mathf.PI))
        {
            p1_dirVec.x = -1;
        }
        if (p1_y > Mathf.Sin(leftBoundsAngle * Mathf.PI))
        {
            p1_dirVec.y = 1;
        }
        else if (p1_y < Mathf.Sin(downBoundsAngle * Mathf.PI))
        {
            p1_dirVec.y = -1;
        }

        if(p1_dirVec.y > 0)
        {
            if(p1_dirVec.x > 0)
            {
                p1_direction = InputDirection.UpRight;
            }
            else if(p1_dirVec.x < 0)
            {
                p1_direction = InputDirection.UpLeft;
            }
            else
            {
                p1_direction = InputDirection.Up;
            }
        }
        else if(p1_dirVec.y < 0)
        {
            if (p1_dirVec.x > 0)
            {
                p1_direction = InputDirection.DownRight;
            }
            else if (p1_dirVec.x < 0)
            {
                p1_direction = InputDirection.DownLeft;
            }
            else
            {
                p1_direction = InputDirection.Down;
            }
        }
        else
        {
            if (p1_dirVec.x > 0)
            {
                p1_direction = InputDirection.Right;
            }
            else if (p1_dirVec.x < 0)
            {
                p1_direction = InputDirection.Left;
            }
        }

        p1_dirVec.Normalize();

        inputStates[0].directionVec.x = p1_dirVec.x;
        inputStates[0].directionVec.y = p1_dirVec.y;
        inputStates[0].direction = p1_direction;
        inputStates[0].actionPressed = p1_actionPressed;
        inputStates[0].actionJustPressed = p1_actionJustPressed;

        if (p1_inputVec.sqrMagnitude > 0 || p1_dirVec.sqrMagnitude > 0 || p1_actionPressed || p1_actionJustPressed)
        {
            Debug.Log("\tp1_input_vec: " + p1_inputVec + "\tp1_dir: " + p1_dirVec + "\tp1_actionPressed: " + p1_actionPressed + "\tp1_actionJustPressed: " + p1_actionJustPressed);
        }

        // Process player 2
        float p2_x = Input.GetAxis(P2_X);
        float p2_y = Input.GetAxis(P2_Y);
        Vector3 p2_inputVec = new Vector3(p2_x, p2_y, 0);
        InputDirection p2_direction = InputDirection.None;
        bool p2_actionPressed = Input.GetButton(P2_ACTION);
        bool p2_actionJustPressed = Input.GetButtonDown(P2_ACTION);

        Vector3 p2_dirVec = Vector3.zero;
        if (p2_x > Mathf.Cos(upBoundsAngle * Mathf.PI))
        {
            p2_dirVec.x = 1;
        }
        else if (p2_x < Mathf.Cos(rightBoundsAngle * Mathf.PI))
        {
            p2_dirVec.x = -1;
        }
        if (p2_y > Mathf.Sin(leftBoundsAngle * Mathf.PI))
        {
            p2_dirVec.y = 1;
        }
        else if (p2_y < Mathf.Sin(downBoundsAngle * Mathf.PI))
        {
            p2_dirVec.y = -1;
        }

        if (p2_dirVec.y > 0)
        {
            if (p2_dirVec.x > 0)
            {
                p2_direction = InputDirection.UpRight;
            }
            else if (p2_dirVec.x < 0)
            {
                p2_direction = InputDirection.UpLeft;
            }
            else
            {
                p2_direction = InputDirection.Up;
            }
        }
        else if (p2_dirVec.y < 0)
        {
            if (p2_dirVec.x > 0)
            {
                p2_direction = InputDirection.DownRight;
            }
            else if (p2_dirVec.x < 0)
            {
                p2_direction = InputDirection.DownLeft;
            }
            else
            {
                p2_direction = InputDirection.Down;
            }
        }
        else
        {
            if (p2_dirVec.x > 0)
            {
                p2_direction = InputDirection.Right;
            }
            else if (p2_dirVec.x < 0)
            {
                p2_direction = InputDirection.Left;
            }
        }

        p2_dirVec.Normalize();

        inputStates[1].directionVec.x = p2_dirVec.x;
        inputStates[1].directionVec.y = p2_dirVec.y;
        inputStates[1].direction = p2_direction;
        inputStates[1].actionPressed = p2_actionPressed;
        inputStates[1].actionJustPressed = p2_actionJustPressed;

        if (p2_inputVec.sqrMagnitude > 0 || p2_dirVec.sqrMagnitude > 0 || p2_actionPressed || p2_actionJustPressed)
        {
            Debug.Log("\tp2_input_vec: " + p2_inputVec + "\tp2_dir: " + p2_dirVec + "\tp2_actionPressed: " + p2_actionPressed + "\tp2_actionJustPressed: " + p2_actionJustPressed);
        }

        // Process player 3
        float p3_x = Input.GetAxis(P3_X);
        float p3_y = Input.GetAxis(P3_Y);
        Vector3 p3_inputVec = new Vector3(p3_x, p3_y, 0);
        InputDirection p3_direction = InputDirection.None;
        bool p3_actionPressed = Input.GetButton(P3_ACTION);
        bool p3_actionJustPressed = Input.GetButtonDown(P3_ACTION);

        Vector3 p3_dirVec = Vector3.zero;
        if (p3_x > Mathf.Cos(upBoundsAngle * Mathf.PI))
        {
            p3_dirVec.x = 1;
        }
        else if (p3_x < Mathf.Cos(rightBoundsAngle * Mathf.PI))
        {
            p3_dirVec.x = -1;
        }
        if (p3_y > Mathf.Sin(leftBoundsAngle * Mathf.PI))
        {
            p3_dirVec.y = 1;
        }
        else if (p3_y < Mathf.Sin(downBoundsAngle * Mathf.PI))
        {
            p3_dirVec.y = -1;
        }

        if (p3_dirVec.y > 0)
        {
            if (p3_dirVec.x > 0)
            {
                p3_direction = InputDirection.UpRight;
            }
            else if (p3_dirVec.x < 0)
            {
                p3_direction = InputDirection.UpLeft;
            }
            else
            {
                p3_direction = InputDirection.Up;
            }
        }
        else if (p3_dirVec.y < 0)
        {
            if (p3_dirVec.x > 0)
            {
                p3_direction = InputDirection.DownRight;
            }
            else if (p3_dirVec.x < 0)
            {
                p3_direction = InputDirection.DownLeft;
            }
            else
            {
                p3_direction = InputDirection.Down;
            }
        }
        else
        {
            if (p3_dirVec.x > 0)
            {
                p3_direction = InputDirection.Right;
            }
            else if (p3_dirVec.x < 0)
            {
                p3_direction = InputDirection.Left;
            }
        }

        p3_dirVec.Normalize();

        inputStates[2].directionVec.x = p3_dirVec.x;
        inputStates[2].directionVec.y = p3_dirVec.y;
        inputStates[2].direction = p3_direction;
        inputStates[2].actionPressed = p3_actionPressed;
        inputStates[2].actionJustPressed = p3_actionJustPressed;

        if (p3_inputVec.sqrMagnitude > 0 || p3_dirVec.sqrMagnitude > 0 || p3_actionPressed || p3_actionJustPressed)
        {
            Debug.Log("\tp3_input_vec: " + p3_inputVec + "\tp3_dir: " + p3_dirVec + "\tp3_actionPressed: " + p3_actionPressed + "\tp3_actionJustPressed: " + p3_actionJustPressed);
        }

        // Process player 4
        float p4_x = Input.GetAxis(P4_X);
        float p4_y = Input.GetAxis(P4_Y);
        Vector3 p4_inputVec = new Vector3(p4_x, p4_y, 0);
        InputDirection p4_direction = InputDirection.None;
        bool p4_actionPressed = Input.GetButton(P4_ACTION);
        bool p4_actionJustPressed = Input.GetButtonDown(P4_ACTION);

        Vector3 p4_dirVec = Vector3.zero;
        if (p4_x > Mathf.Cos(upBoundsAngle * Mathf.PI))
        {
            p4_dirVec.x = 1;
        }
        else if (p4_x < Mathf.Cos(rightBoundsAngle * Mathf.PI))
        {
            p4_dirVec.x = -1;
        }
        if (p4_y > Mathf.Sin(leftBoundsAngle * Mathf.PI))
        {
            p4_dirVec.y = 1;
        }
        else if (p4_y < Mathf.Sin(downBoundsAngle * Mathf.PI))
        {
            p4_dirVec.y = -1;
        }

        if (p4_dirVec.y > 0)
        {
            if (p4_dirVec.x > 0)
            {
                p4_direction = InputDirection.UpRight;
            }
            else if (p4_dirVec.x < 0)
            {
                p4_direction = InputDirection.UpLeft;
            }
            else
            {
                p4_direction = InputDirection.Up;
            }
        }
        else if (p4_dirVec.y < 0)
        {
            if (p4_dirVec.x > 0)
            {
                p4_direction = InputDirection.DownRight;
            }
            else if (p4_dirVec.x < 0)
            {
                p4_direction = InputDirection.DownLeft;
            }
            else
            {
                p4_direction = InputDirection.Down;
            }
        }
        else
        {
            if (p4_dirVec.x > 0)
            {
                p4_direction = InputDirection.Right;
            }
            else if (p4_dirVec.x < 0)
            {
                p4_direction = InputDirection.Left;
            }
        }

        p4_dirVec.Normalize();

        inputStates[3].directionVec.x = p4_dirVec.x;
        inputStates[3].directionVec.y = p4_dirVec.y;
        inputStates[3].direction = p4_direction;
        inputStates[3].actionPressed = p4_actionPressed;
        inputStates[3].actionJustPressed = p4_actionJustPressed;

        if (p4_inputVec.sqrMagnitude > 0 || p4_dirVec.sqrMagnitude > 0 || p4_actionPressed || p4_actionJustPressed)
        {
            //Debug.Log("\tp4_input_vec: " + p4_inputVec + "\tp4_dir: " + p4_dirVec + "\tp4_actionPressed: " + p4_actionPressed + "\tp4_actionJustPressed: " + p4_actionJustPressed);
        }

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

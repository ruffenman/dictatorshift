using UnityEngine;
using System.Collections;

public class IGJInputManager : MonoBehaviour 
{
    public class InputState
    {
        public Vector3 direction;
        public bool actionPressed;
        public bool actionJustPressed;
    }
    
	// Use this for initialization
	private void Start () {
        inputStates = new InputState[4] 
        { 
            new InputState(){direction = Vector3.zero, actionJustPressed = false, actionPressed = false},
            new InputState(){direction = Vector3.zero, actionJustPressed = false, actionPressed = false},
            new InputState(){direction = Vector3.zero, actionJustPressed = false, actionPressed = false},
            new InputState(){direction = Vector3.zero, actionJustPressed = false, actionPressed = false},
        };
	}
	
	// Update is called once per frame
	private void Update () {
	    // Process player 1
        float p1_x = Input.GetAxis(P1_X);
        float p1_y = Input.GetAxis(P1_Y);
        bool p1_actionPressed = Input.GetButton(P1_ACTION);
        bool p1_actionJustPressed = Input.GetButtonDown(P1_ACTION);
        Vector3 p1_dir = Vector3.zero;
        if(p1_x > Mathf.Cos(3 * Mathf.PI / 8))
        {
            p1_dir.x = 1;
        }
        else if (p1_x < Mathf.Cos(5 * Mathf.PI / 8))
        {
            p1_dir.x = -1;
        }

        if (p1_y > Mathf.Sin(1 * Mathf.PI / 8))
        {
            p1_dir.y = 1;
        }
        else if (p1_y < Mathf.Sin(15 * Mathf.PI / 8))
        {
            p1_dir.y = -1;
        }
        p1_dir.Normalize();
        inputStates[0].direction.x = p1_x;
        inputStates[0].direction.x = p1_y;
        inputStates[0].actionPressed = p1_actionPressed;
        inputStates[0].actionJustPressed = p1_actionJustPressed;

        Debug.Log("p1_dir: " + p1_dir);
        Debug.Log("p1_actionPressed: " + p1_actionPressed);
        Debug.Log("p1_actionJustPressed: " + p1_actionJustPressed);

        // Process player 2
        float p2_x = Input.GetAxis(P2_X);
        float p2_y = Input.GetAxis(P2_Y);
        bool p2_actionPressed = Input.GetButton(P2_ACTION);
        bool p2_actionJustPressed = Input.GetButtonDown(P2_ACTION);

        // Process player 3
        float p3_x = Input.GetAxis(P3_X);
        float p3_y = Input.GetAxis(P3_Y);
        bool p3_actionPressed = Input.GetButton(P3_ACTION);
        bool p3_actionJustPressed = Input.GetButtonDown(P3_ACTION);

        // Process player 4
        float p4_x = Input.GetAxis(P4_X);
        float p4_y = Input.GetAxis(P4_Y);
        bool p4_actionPressed = Input.GetButton(P4_ACTION);
        bool p4_actionJustPressed = Input.GetButtonDown(P4_ACTION);

        for(int i=0;i<bodyParts.Length;++i)
        {
            bodyParts[i].ReceiveInput(inputStates[i]);
        }
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

    [SerializeField]
    private BodyPart[] bodyParts;

    private InputState[] inputStates;
}

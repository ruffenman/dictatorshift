using UnityEngine;
using System.Collections;

public class JamGame : MonoBehaviour 
{
    public static JamGame instance;

    // Settings
    public bool isMusicEnabled = true;
    public bool isSFXEnabled = true;
    public bool debugBodyPartSwitching = false;

    public CombinedPlayer player;

    public SoundManager soundManager { get { return soundMgr; } }
    public IGJInputManager inputManager { get { return inputMgr; } }

    public void Awake()
    {
        if (!instance)
        {
            instance = this;
            inputMgr = GetComponent<IGJInputManager>();
            soundMgr = GetComponent<SoundManager>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("Tried to create multiple JamGames");
            Destroy(gameObject);
        }
    }

    private void Start()
    {

    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void OnLevelWasLoaded(int sceneIndex)
    {
        player = GameObject.FindObjectOfType<CombinedPlayer>();
    }

    private IGJInputManager inputMgr;
    private SoundManager soundMgr;
}

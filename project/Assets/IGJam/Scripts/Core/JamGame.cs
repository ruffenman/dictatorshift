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
            Destroy(this);
        }
    }

    private void Start()
    {

    }

    private IGJInputManager inputMgr;
    private SoundManager soundMgr;
}

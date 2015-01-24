using UnityEngine;
using System.Collections;

public class JamGame : MonoBehaviour 
{
    public static JamGame instance;

    public CombinedPlayer player;

    public void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Tried to create multiple JamGames");
        }
    }
}

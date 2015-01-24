using UnityEngine;
using System.Collections;

public class DebugUtil
{
    public static void Assert(bool condition, string message)
    {
        Assert(condition, message, false);
    }

    public static void Assert(bool condition, string message, bool isWarning)
    {
        if (!condition && Debug.isDebugBuild)
        {
            if (isWarning)
            {
                Debug.LogWarning(message);
            }
            else
            {
                Debug.LogError(message);
            }
        }
    }
}

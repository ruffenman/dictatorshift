using UnityEngine;
using UnityEditor;

// Taken from tigertrussel at http://forum.unity3d.com/threads/77444-How-to-create-new-GameObject-as-child-in-hierarchy
public class GameObjectCreationHelper : MonoBehaviour 
{	
    [MenuItem("GameObject/Wrap in Object #&w")]
    static void WrapInObject() 
	{
        if(Selection.gameObjects.Length == 0)
            return;
		
        GameObject go = new GameObject("Wrapper:NameMe");

        go.transform.parent = Selection.activeTransform.parent;

        go.transform.position = Vector3.zero;

        foreach(GameObject g in Selection.gameObjects)
		{
            g.transform.parent = go.transform;
        }
    }
}
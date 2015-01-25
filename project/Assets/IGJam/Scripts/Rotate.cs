using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour
{
	public Vector3 rotationVector;

	void Update()
	{
		this.gameObject.transform.Rotate (rotationVector);
	}
	
}

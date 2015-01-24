using UnityEngine;
using System.Collections;

public class SmoothFollow : MonoBehaviour
{
	public Transform target;
	public float smoothDampTime = 0.2f;
	[HideInInspector]
	public new Transform transform;
	public Vector3 cameraOffset;
	public bool useFixedUpdate = false;
	
	private CharacterController playerController;
	private Vector3 smoothDampVelocity;
	
	
	void Awake()
	{
		transform = gameObject.transform;
		playerController = target.GetComponent<CharacterController>();
	}
	
	
	void LateUpdate()
	{
		if( !useFixedUpdate )
			updateCameraPosition();
	}


	void FixedUpdate()
	{
		if( useFixedUpdate )
			updateCameraPosition();
	}


	void updateCameraPosition()
	{
		if( playerController == null )
		{
			transform.position = Vector3.SmoothDamp( transform.position, target.position - cameraOffset, ref smoothDampVelocity, smoothDampTime );
			return;
		}
		
		if( playerController.velocity.x > 0 )
		{
			transform.position = Vector3.SmoothDamp( transform.position, target.position - cameraOffset, ref smoothDampVelocity, smoothDampTime );
		}
		else
		{
			var leftOffset = cameraOffset;
			leftOffset.x *= -1;
			transform.position = Vector3.SmoothDamp( transform.position, target.position - leftOffset, ref smoothDampVelocity, smoothDampTime );
		}
	}
	
}

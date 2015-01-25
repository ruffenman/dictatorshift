using UnityEngine;
using System.Collections;

public class WorldObjectNonCharacter : MonoBehaviour 
{	
	public enum ObjectType
	{
		INTERACTIVE,
		STABLE
	}
	
	// properties
	public ObjectType objectType;
	
	// utility
	protected float health = 100;
	protected CharacterController controller;
	protected Vector3 velocity;
	
	public void Start()
	{

	}
	
	public virtual void Update()
	{
		if (objectType == ObjectType.INTERACTIVE)
		{
			Move();
		}
	}
	
	public void TakeDamage(float damage)
	{
		if (objectType == ObjectType.INTERACTIVE)
		{
			health = Mathf.Max(0, health - damage);
			UpdateHealth();
		}
	}
	
	public Vector3 GetVelocity()
	{
		return velocity;
	}
	
	public void AddVelocity(Vector3 addedVelocity)
	{
		SetVelocity(velocity + addedVelocity);
	}
	
	public void SetVelocity(Vector3 newVelocity)
	{
		velocity = newVelocity;
	}
	
	void UpdateHealth()
	{
		if (health == 0)
		{
			Die();
		}
	}
	
	protected virtual void Die()
	{
		Destroy(gameObject);
	}
	
	void Move()
	{
		if ((velocity.sqrMagnitude > 0))
		{
			Vector3 newVelocity = velocity;
			
			this.gameObject.transform.Translate (newVelocity);
			SetVelocity(newVelocity);
		}
	}
}

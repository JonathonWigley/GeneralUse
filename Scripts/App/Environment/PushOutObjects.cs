using UnityEngine;

/// <summary>
/// Created By: Jonathon Wigley - 12/02/2018
/// Last Edited By: Jonathon Wigley - 12/02/2018
/// </summary>

/// <summary>
/// Component attached to an object that will push object outside of it
/// </summary>
public class PushOutObjects : MonoBehaviour
{
	/// <summary>
	/// Layermask containing layers that this object should push out
	/// </summary>
	[SerializeField] protected LayerMask pushOutMask;

	/// <summary>
	/// Target that this object should push objects inside of it towards.
	/// If null, will push away from the center of this object.
	/// </summary>
	[SerializeField] protected Transform pushTarget;

	/// <summary>
	/// Force at which this object should push out the object inside of it
	/// </summary>
	[SerializeField] protected float pushForce = 1f;

	protected virtual void OnTriggerStay(Collider other)
	{
		// Find the other object's rigidbody
		Rigidbody otherBody = GetRigidBody(other.gameObject);

		// Push out the other object if it should be pushed out
		if(ShouldPushOutObject(otherBody))
			PushOutObject(otherBody);
	}

	/// <summary>
	/// Checks to see if an object should be pushed out by this object
	/// </summary>
	/// <param name="body">Rigidbody of the object being checked</param>
	/// <returns>True if the other object should be pushed out</returns>
	protected virtual bool ShouldPushOutObject(Rigidbody body)
	{
		// Push the other object out of this one if it is on
		// the correct layer and it has a rigidbody
		bool bObjectInPushOutLayer = pushOutMask == (pushOutMask | (1 << body.gameObject.layer));
		if(bObjectInPushOutLayer && body != null)
			return true;

		return false;
	}

	/// <summary>
	/// Applies a force to a rigidbody to push it out of this object
	/// </summary>
	/// <param name="body"></param>
	protected virtual void PushOutObject(Rigidbody body)
	{
		// Get the direction that the object should be pushed
		Vector3 forceDirection = Vector3.Normalize(pushTarget == null ?
			body.transform.position - transform.position :
			pushTarget.position - body.transform.position);

		// Push the object in the target direction
		if(body.IsSleeping())
			body.WakeUp();

		body.velocity = forceDirection * pushForce;
	}

	/// <summary>
	/// Returns the rigidbody found in an object or its parent
	/// </summary>
	/// <param name="other">GameObject to find the rigidbody on</param>
	/// <returns>Rigidbody found on the other object</returns>
	protected virtual Rigidbody GetRigidBody(GameObject other)
	{
		// Check to see if the collided object has a rigidbody or
		// is part of an object with a rigidbody
		Rigidbody otherBody = other.GetComponent<Rigidbody>();
		if(otherBody == null)
			otherBody = other.GetComponentInParent<Rigidbody>();

		return otherBody;
	}
}

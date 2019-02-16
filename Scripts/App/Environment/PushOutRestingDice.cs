using UnityEngine;

/// <summary>
/// Created By: Jonathon Wigley - 12/02/2018
/// Last Edited By: Jonathon Wigley - 12/02/2018
/// </summary>

/// <summary>
/// Component attached to an object that will push dice that have come to a rest outside of it
/// </summary>
public class PushOutRestingDice : PushOutObjects
{
	private void Awake()
	{
		// Set the layermask to the dice layer
		pushOutMask = LayerMask.GetMask("Dice");
	}

	/// <summary>
	/// Checks to see if an object should be pushed out by this object
	/// </summary>
	/// <param name="body">Rigidbody of the object being checked</param>
	/// <returns>True if the other object should be pushed out</returns>
	protected override bool ShouldPushOutObject(Rigidbody body)
	{
		if(base.ShouldPushOutObject(body) == false)
			return false;

		Die die = body.GetComponent<Die>();
		return die.bAtRest;
	}

	/// <summary>
	/// Applies a force to a rigidbody to push it out of this object
	/// </summary>
	/// <param name="body">Body of the object to push out</param>
	protected override void PushOutObject(Rigidbody body)
	{
		base.PushOutObject(body);
		body.angularVelocity = Vector3.zero;
	}
}

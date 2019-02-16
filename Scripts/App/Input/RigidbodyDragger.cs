using UnityEngine;
using TouchScript.Pointers;

/// <summary>
/// Created By: Jonathon Wigley - 08/24/2018
/// Last Edited By: Jonathon Wigley - 08/24/2018
///
/// Purpose:
/// Attached to an object that will act as a spring joint anchor
/// in order to drag dice around the world
/// <summary>

public class RigidbodyDragger : PooledObject
{
	// Settings
	[SerializeField] float springStrength = 200.0f; // Strength of the spring dragging the die, controls speed
	[SerializeField] float springDamping = 5.0f; // Amount of damping when dragging, controls bouncing on reaching dragger
	[SerializeField] float dieLinearDrag = 10.0f; // Amount of drag on the die, helps control bouncing
	[SerializeField] float dieAngularDrag = 20.0f; // Amount of angular drag on the die, helps control spinning
	[SerializeField] float maxSpringDistance = 0.01f; // Distance from the dragger at which the die can be at rest

	// References
	GameSettings settings;
	SpringJoint springJoint;
	Rigidbody body;

	// Cached values
	float cachedLinearDrag; // Die's linear drag when its picked up
	float cachedAngularDrag; // Die's angular drag when its picked up

	// States
	bool initialized; // True if this dragger has already been initialized

	/// <summary>
	/// Initializes the dragger with the rigid body it should be dragging
	/// </summary>
	/// <param name="bodyToAttach"></param>
	public RigidbodyDragger Init(Rigidbody bodyToAttach)
	{
		settings = GameManager.Instance.GameSettings;

		if(initialized == false)
		{
			body = GetComponent<Rigidbody>();
			body.isKinematic = true;

			springJoint = GetComponent<SpringJoint>();
			springJoint.autoConfigureConnectedAnchor = false;
			springJoint.anchor = Vector3.zero;
			springJoint.spring = springStrength;
			springJoint.damper = springDamping;
			springJoint.maxDistance = maxSpringDistance;

			initialized = true;
		}

		springJoint.connectedBody = bodyToAttach;
		cachedLinearDrag = bodyToAttach.drag;
		cachedAngularDrag = bodyToAttach.angularDrag;
		bodyToAttach.drag = dieLinearDrag;
		bodyToAttach.angularDrag = dieAngularDrag;

		return this;
	}

	/// <summary>
	/// Drag the dragger to the world position of the touch
	/// </summary>
	/// <param name="screenPosition">Screen position of the input</param>
	public void DragWithScreenPoint(Vector3 screenPosition)
	{
		RaycastHit hit = RaycastUtility.GetWorldHit(screenPosition, LayerMask.GetMask("Ground"));
        transform.position =  hit.point + new Vector3(0f, settings.DiceHoldHeight, 0f);
	}

	/// <summary>
	/// Drag the dragger to the world position given
	/// </summary>
	/// <param name="worldPoint">World position of the input</param>
	public void DragWithWorldPoint(Vector3 worldPoint)
	{
        transform.position =  worldPoint;
	}

	/// <summary>
	/// Breaks the rigidbody connection
	/// </summary>
	public void Disconnect()
	{
		springJoint.connectedBody.drag = cachedLinearDrag;
		springJoint.connectedBody.angularDrag = cachedAngularDrag;
		springJoint.connectedBody = null;
	}
}

using UnityEngine;

/// <summary>
/// Created By: Jonathon Wigley - 12/30/2018
/// Last Edited By: Jonathon Wigley - 12/30/2018
/// </summary>

/// <summary>
/// Partial class of die
/// Handles functionality dealing with the carry handler
/// </summary>
public partial class Die : PooledObject
{
	/// <summary>
	/// Reference to the carry handler component attached to the die
	/// </summary>
	private CarryHandler carryHandler;

	/// <summary>
	/// Find any references needed to interact with the carry handler
	/// </summary>
	private void FindCarryHandlerReference()
	{
		carryHandler = GetComponent<CarryHandler>();
	}

	/// <summary>
	/// Register to any carry handler events
	/// </summary>
	private void RegisterCarryHandlerEvents()
	{
		carryHandler.OnPickUp += CarryHandler_PickUp;
		carryHandler.OnUpdateCarry += CarryHandler_UpdateCarry;
		carryHandler.OnDrop += CarryHandler_Drop;
	}

	/// <summary>
	/// Unregister from any carry handler events
	/// </summary>
	private void UnregisterCarryHandlerEvents()
	{
		carryHandler.OnPickUp -= CarryHandler_PickUp;
		carryHandler.OnUpdateCarry -= CarryHandler_UpdateCarry;
		carryHandler.OnDrop -= CarryHandler_Drop;
	}

	/// <summary>
	/// Called whenever the die is picked up by the carry handler
	/// </summary>
	private void CarryHandler_PickUp()
	{
		ChangeState(DieState.Held);
	}

	/// <summary>
	/// Called in update whenever the die is being carried by the carry handler
	/// </summary>
	private void CarryHandler_UpdateCarry()
	{
	}

	/// <summary>
	/// Called when the die is dropped by the carry handler
	/// </summary>
	private void CarryHandler_Drop()
	{
		if(bInputIsTouching == false)
			return;

		ChangeState(DieState.Dropped);

		// Clamp the max throw velocity
		body.velocity = Vector3.ClampMagnitude(body.velocity, gameSettings.MaxThrowVelocityMagnitude);
	}
}

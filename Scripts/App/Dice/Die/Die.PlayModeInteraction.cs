using UnityEngine;
using VectorStudios.Dice.Math;

/// <summary>
/// Created By: Jonathon Wigley - 11/11/2018
/// Last Edited By: Jonathon Wigley - 11/11/2018
/// </summary>

/// <summary>
/// Handles dice interaction while in simulation play mode
/// </summary>
public partial class Die : PooledObject
{
	/// <summary>
	/// Functionality for the die being tapped
	/// </summary>
	/// <param name="sender"></param>
	private void HandleTapForPlayMode()
	{
		// Ignore the tap if the die is being held
		if(bHeld == true)
			return;

		bNeedsToBounce = true;
		Roll();
	}

	/// <summary>
	/// Functionality for the die being long pressed
	/// </summary>
	/// <param name="sender"></param>
	private void HandleLongPressForPlayMode()
	{
	}

	/// <summary>
	/// Functionality for the die being flicked
	/// </summary>
	/// <param name="sender"></param>
	private void HandleFlickForPlayMode()
	{
		InputData input = new InputData();
		input.ScreenPosition = flickGesture.ScreenPosition;
		input.Direction = flickGesture.ScreenFlickVector;

		// Die is on ground
		if(transform.position.y < gameSettings.DiceHoldHeight * .75f)
		{
			flickedOnGroundBehaviour.Interact(gameObject, input);
		}

		// Die is being held
		else
		{
			flickedWhileHeldBehaviour.Interact(gameObject, input);
		}

		// Tell the die that it needs to bounce after being thrown
		bNeedsToBounce = true;

		// Clamp the max throw velocity
		body.velocity = Vector3.ClampMagnitude(body.velocity, gameSettings.MaxThrowVelocityMagnitude);

		// Set our touch states to false
        bInputIsTouching = false;

		// Wake up the rigidbody in case it has been still/asleep
        body.WakeUp();

		// Set our die's state to rolling
		HandleStartRolling();
	}

	private void HandleSwipeForPlayMode(Vector3 swipeDirection)
	{
		InputData input = new InputData();
		input.ScreenPosition = dragGesture.ScreenPosition;
		input.Direction = swipeDirection;

		swipedBehaviour.Interact(gameObject, input);

		// Tell the die that it needs to bounce after being thrown
		bNeedsToBounce = true;

		// Clamp the max throw velocity
		body.velocity = Vector3.ClampMagnitude(body.velocity, gameSettings.MaxThrowVelocityMagnitude);

		// Set our touch states to false
        bInputIsTouching = false;

		// Wake up the rigidbody in case it has been still/asleep
        body.WakeUp();

		// Set our die's state to rolling
		HandleStartRolling();
	}
}
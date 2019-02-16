using UnityEngine;
using TouchScript.Pointers;

/// <summary>
/// Created By: Jonathon Wigley - 8/5/2018
/// Last Edited By: Jonathon Wigley - 09/04/2018
/// Last Edited By: Jonathon Wigley - 09/21/2018
///
/// Purpose:
/// Interface that should be implemented by any object that wants to be
/// interacted with by input
/// <summary>

public interface ITouchable
{
	bool HandleInputPressed(Pointer input);
	bool HandleInputMoved(Pointer input);
	bool HandleInputStationary(Pointer input);
	bool HandleInputReleased(Pointer input);
	bool HandleInputCancelled(Pointer input);

	// OnTouchEnter
	// OnTouchExit
	// OnTap
	// OnHoldStationary
	// OnHoldMoved
	// OnSwiped
	// OnPinched
}

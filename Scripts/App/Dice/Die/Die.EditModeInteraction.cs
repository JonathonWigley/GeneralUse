using UnityEngine;

/// <summary>
/// Created By: Jonathon Wigley - 11/11/2018
/// Last Edited By: Jonathon Wigley - 11/11/2018
/// </summary>

/// <summary>
/// Handles dice interaction while in simulation edit mode
/// </summary>
public partial class Die : PooledObject
{
    /// <summary>
    /// True if the die is currently locked
    /// </summary>
    /// <value></value>
    public bool bIsLocked { get; protected set; }

	/// <summary>
	/// Handles the tap interaction when the simulation mode is in Edit
	/// </summary>
	private void HandleTapForEditMode()
	{
		// Select
	}

	/// <summary>
	/// Handles the long press interaction when the simulation mode is in Edit
	/// </summary>
	private void HandleLongPressForEditMode()
	{
        bIsLocked = !bIsLocked;
        currentLockUI.ToggleLock(bIsLocked);
	}

	/// <summary>
	/// Handles the flick interaction when the simulation mode is Edit
	/// </summary>
	private void HandleFlickForEditMode()
	{
		// Do nothing
	}
}
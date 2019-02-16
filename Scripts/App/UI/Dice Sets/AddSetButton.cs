using UnityEngine;

/// <summary>
/// Created By: Jonathon Wigley - 10/18/2018
/// Last Edited By: Jonathon Wigley - 10/18/2018
/// </summary>

/// <summary>
/// Component attached to the add set button to add a set to the set database
/// </summary>
public class AddSetButton : MonoBehaviour
{
	/// <summary>
	/// Called when this button was pressed
	/// </summary>
	/// <param name="sender">Button that was pressed</param>
	public static event PressedDelegate OnPressed;
	public delegate void PressedDelegate(AddSetButton sender);

	/// <summary>
	/// Attached to a button in editor to add a dice set
	/// </summary>
	public void BTN_HandlePress()
	{
		OnPressed?.Invoke(this);
	}
}

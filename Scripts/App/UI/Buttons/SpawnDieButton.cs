using UnityEngine;

/// <summary>
/// Created By: Jonathon Wigley - 10/18/2018
/// Last Edited By: Jonathon Wigley - 10/18/2018
/// </summary>

/// <summary>
/// Component attached to buttons that should spawn dice
/// </summary>
public class SpawnDieButton : MonoBehaviour
{
	/// <summary>
	/// Die type this button spawns
	/// </summary>
	public DieType Type { get { return type; } }
	[SerializeField] private DieType type;

	/// <summary>
	/// Called when the button is pressed
	/// </summary>
	/// <param name="sender">Button that was pressed</param>
	/// <param name="type">Die type associated with the button</param>
	public static event PressedDelegate OnPressed;
	public delegate void PressedDelegate(SpawnDieButton sender, DieType type);

	/// <summary>
	/// Handles functionality for when this button is pressed
	/// </summary>
	public void BTN_HandlePressed()
	{
		OnPressed?.Invoke(this, Type);
	}
}

/// <summary>
/// Created By: Jonathon Wigley - 10/21/2018
/// Last Edited By: Jonathon Wigley - 10/21/2018
/// </summary>

/// <summary>
/// States that a toggled object can be in
/// </summary>
public enum ToggleState
{
	/// <summary>
	/// Toggled object is currently closed
	/// </summary>
	Closed,

	/// <summary>
	/// Toggled object is in the process of opening
	/// </summary>
	Opening,

	/// <summary>
	/// Toggled object is open
	/// </summary>
	Open,

	/// <summary>
	/// Toggled object is closing
	/// </summary>
	Closing
}

using UnityEngine;

/// <summary>
/// Created By: Jonathon Wigley - 7/1/2018
/// Last Edited By: Jonathon Wigley - 02/03/2019
/// </summary>

/// <summary>
/// Container for dice skin information
/// </summary>
[System.Serializable]
public class DiceSkinData : DatabaseData
{
	/// <summary>
	/// Name of the skin
	/// </summary>
	public string Name = "Dice Skin";

	/// <summary>
	/// Color to apply to the skin material
	/// </summary>
	public Color BaseColor = Color.white;

	/// <summary>
	/// Color that will be applied to the outline effect on the skin
	/// </summary>
	public Color OutlineColor = Color.white;

	/// <summary>
	/// Color that will be applied to the text when using the skin
	/// </summary>
	public Color TextColor = Color.white;
}

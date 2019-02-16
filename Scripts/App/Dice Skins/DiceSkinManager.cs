using UnityEngine;

/// <summary>
/// Created By: Jonathon Wigley - 11/04/2018
/// Last Edited By: Jonathon Wigley - 11/04/2018
/// </summary>

/// <summary>
/// Manager class for functionality dealing with dice skins
/// </summary>
public class DiceSkinManager : MonoBehaviour
{
	/// <summary>
	/// Reference to the dice skin database currently being used
	/// </summary>
	[SerializeField] DiceSkinDatabase database;

    /// <summary>
    /// Dispatched when the dice skin should change
    /// </summary>
    /// <param name="skin">Dice skin to change to</param>
    public static event DiceSkinChangedDelegate OnDiceSkinChanged;
    public delegate void DiceSkinChangedDelegate(DiceSkinData skin);

	private void OnEnable()
	{
		DiceSkinCell.OnPressed += DiceSkinCell_Pressed;
	}

	private void OnDisable()
	{
		DiceSkinCell.OnPressed -= DiceSkinCell_Pressed;
	}

	/// <summary>
	/// Called whenever a dice skin button is pressed
	/// </summary>
	/// <param name="skin">Skin to change to</param>
	private void DiceSkinCell_Pressed(DiceSkinData skin)
	{
		OnDiceSkinChanged?.Invoke(skin);
	}
}
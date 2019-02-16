using UnityEngine;

/// <summary>
/// Created By: Jonathon Wigley - 11/04/2018
/// Last Edited By: Jonathon Wigley - 11/04/2018
/// </summary>

/// <summary>
/// Manager class for functionality dealing with backgrounds
/// </summary>
public class BackgroundManager : MonoBehaviour
{
	/// <summary>
	/// Database that is currently being used
	/// </summary>
	[SerializeField] BackgroundDatabase database;

	/// <summary>
	/// Dispatched when the background should change
	/// </summary>
	/// <param name="background">Background to change to</param>
	public delegate void BackgroundChangedDelegate(BackgroundData background);
	public static event BackgroundChangedDelegate OnBackgroundChanged;

	private void OnEnable()
	{

	}

	private void OnDisable()
	{

	}

	private void BackgroundCell_Pressed(BackgroundData background)
	{
		OnBackgroundChanged?.Invoke(background);
	}
}
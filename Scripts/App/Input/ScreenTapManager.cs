using System;
using UnityEngine;
using TouchScript.Gestures;

/// <summary>
/// Created By: Jonathon Wigley - 10/21/2018
/// Last Edited By: Jonathon Wigley - 10/21/2018
/// </summary>

/// <summary>
/// Handles recongizing and dispatching events for taps on the screen
/// </summary>
public class ScreenTapManager : MonoBehaviour
{
	/// <summary>
	/// Dispatched when the screen is tapped
	/// </summary>
	/// <param name="screenPosition">Screen position of the registered tap</param>
	public static event TappedDelegate OnTapped;
	public delegate void TappedDelegate(Vector2 screenPosition);

	[SerializeField] private TapGesture tapGesture;
	[SerializeField] private float maxMovementDistance = 1.5f;
	[SerializeField] private float timeLimitForTap = .75f;

	private void Awake()
	{
		tapGesture.DistanceLimit = maxMovementDistance;
		tapGesture.TimeLimit = timeLimitForTap;
	}

	private void OnEnable()
	{
		tapGesture.Tapped += TapGesture_Tapped;
	}

	private void OnDisable()
	{
		tapGesture.Tapped -= TapGesture_Tapped;
	}

	/// <summary>
	/// Called when the tap gesture registers a tap
	/// </summary>
	private void TapGesture_Tapped(object sender, EventArgs args)
	{
		OnTapped?.Invoke(tapGesture.ScreenPosition);
	}
}

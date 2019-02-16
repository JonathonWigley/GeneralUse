using System;
using UnityEngine;
using TouchScript;
using TouchScript.Gestures;

/// <summary>
/// Created By: Jonathon Wigley - 09/24/2018
/// Last Edited By: Jonathon Wigley - 12/30/2018
/// </summary>

/// <summary>
/// Purpose:
/// Partial class of Die
/// Functions relating to TouchScript Gestures
/// <summary>
public partial class Die : PooledObject
{
	/// <summary>
	/// Tap gesture used by this die
	/// </summary>
    TapGesture tapGesture;

	/// <summary>
	/// Long press gesture used by this die
	/// </summary>
    LongPressGesture longPressGesture;

	/// <summary>
	/// Flick gesture used by this die
	/// </summary>
    CustomFlickGesture flickGesture;

	/// <summary>
	/// Drag gesture used by this die
	/// </summary>
	DragGesture dragGesture;

	/// <summary>
	/// True when a tap gesture was registered on the previous frame
	/// </summary>
	bool bWasTapped = false;

	/// <summary>
	/// True when a long press gesture was registered on the previous frame
	/// </summary>
	bool bWasLongPressed = false;

	/// <summary>
	/// True when a flick gesture was registered on the previous frame
	/// </summary>
	bool bWasFlicked = false;

	/// <summary>
	/// Get references to the gesture components for the die
	/// </summary>
	private void GetGestureReferences()
	{
		tapGesture = GetComponent<TapGesture>();
		flickGesture = GetComponent<CustomFlickGesture>();
		longPressGesture = GetComponent<LongPressGesture>();
		dragGesture = GetComponent<DragGesture>();
	}

	/// <summary>
	/// Initialize settings for the gesture components for the die
	/// </summary>
	private void InitGestureSettings()
	{
		tapGesture.DistanceLimit = diceSettings.TapMaxMoveDistance;
		tapGesture.TimeLimit = diceSettings.TapTimeThreshold;

		longPressGesture.TimeToPress = diceSettings.TapTimeThreshold;

		flickGesture.MinDistance = gameSettings.MinDistanceToFlick;
		flickGesture.MaxDistance = 15f;
		flickGesture.MaxTime = .15f;

		dragGesture.RequireGestureToFail = tapGesture;
		dragGesture.RequireGestureToFail = flickGesture;
	}

	/// <summary>
	/// Register to any gesture events
	/// </summary>
	private void RegisterGestureEvents()
	{
		tapGesture.Tapped += TapGesture_Tapped;
		longPressGesture.LongPressed += LongPressGesture_LongPressed;
		flickGesture.Flicked += FlickGesture_Flicked;

		if(TouchManager.Instance != null)
		{
			TouchManager.Instance.FrameStarted += TouchManager_FrameStarted;
		}
	}

	/// <summary>
	/// Unregister from any gesture events
	/// </summary>
	private void UnregisterGestureEvents()
	{
		tapGesture.Tapped -= TapGesture_Tapped;
		longPressGesture.LongPressed -= LongPressGesture_LongPressed;
		flickGesture.Flicked -= FlickGesture_Flicked;

		if(TouchManager.Instance != null)
		{
			TouchManager.Instance.FrameStarted -= TouchManager_FrameStarted;
		}
	}

	/// <summary>
	/// Called when the tap gesture is registered
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="args"></param>
	private void TapGesture_Tapped(object sender, EventArgs args)
	{
		bWasTapped = true;
	}

	/// <summary>
	/// Called when the long press gesture is registered
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="args"></param>
	private void LongPressGesture_LongPressed(object sender, EventArgs args)
	{
		bWasLongPressed = true;
	}

	/// <summary>
	/// Called when the flick gesture is recognized
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="args"></param>
	private void FlickGesture_Flicked(object sender, EventArgs args)
	{
		bWasFlicked = true;
	}

	/// <summary>
	/// Called when the touchmanager has started checking for gestures during a frame
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="args"></param>
	private void TouchManager_FrameStarted(object sender, EventArgs args)
	{
		if(GameManager.Instance.CurrentGameMode == GameMode.Simulation)
		{
			// Play mode interaction
			if(GameManager.Instance.CurrentSimulationMode == SimulationMode.Play)
			{
				if(bWasFlicked) HandleFlickForPlayMode();
				else if(bWasLongPressed) HandleLongPressForPlayMode();
				else if(bWasTapped) HandleTapForPlayMode();
			}

			// Edit mode interaction
			else if(GameManager.Instance.CurrentSimulationMode == SimulationMode.Edit)
			{
				if(bWasFlicked) HandleFlickForEditMode();
				else if(bWasLongPressed) HandleLongPressForEditMode();
				else if(bWasTapped) HandleTapForEditMode();
			}
		}

		// Clear any gesture flags
		bWasTapped = false;
		bWasFlicked = false;
		bWasLongPressed = false;
	}
}
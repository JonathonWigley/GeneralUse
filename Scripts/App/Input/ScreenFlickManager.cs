using System;
using UnityEngine;
using TouchScript.Gestures;

/// <summary>
/// Created By: Jonathon Wigley - 10/21/2018
/// Last Edited By: Jonathon Wigley - 10/21/2018
/// </summary>

/// <summary>
/// Manager for detecting flick gestures and their directions
/// </summary>
public class ScreenFlickManager : MonoBehaviour
{
	/// <summary>
	/// Reference to the flick gesture used for registering flicks
	/// </summary>
	[SerializeField] private FlickGesture flickGesture;

	[SerializeField] private float MinFlickDistance = 1f;

	/// <summary>
	/// Dispatched when the user flicks right
	/// </summary>
	public static event FlickRightDelegate OnFlickRight;
	public delegate void FlickRightDelegate();

	/// <summary>
	/// Dispatched when the user flicks left
	/// </summary>
	public static event FlickLeftDelegate OnFlickLeft;
	public delegate void FlickLeftDelegate();

	/// <summary>
	/// Dispatched when the user flicks up
	/// </summary>
	public static event FlickUpDelegate OnFlickUp;
	public delegate void FlickUpDelegate();

	/// <summary>
	/// Dispatched when the user flicks down
	/// </summary>
	public static event FlickDownDelegate OnFlickDown;
	public delegate void FlickDownDelegate();

	private void Start()
	{
		flickGesture.MinDistance = MinFlickDistance;
	}

	private void OnEnable()
	{
		flickGesture.Flicked += FlickGesture_Flicked;
	}

	private void OnDisable()
	{
		flickGesture.Flicked -= FlickGesture_Flicked;
	}

	/// <summary>
	/// Called when the flick gesture registers a flick
	/// </summary>
	private void FlickGesture_Flicked(object sender, EventArgs args)
	{
		Vector2 flickGestureDirection = flickGesture.ScreenFlickVector.normalized;

		// Horizontal flick
		if(Mathf.Abs(flickGestureDirection.x) >= Mathf.Abs(flickGestureDirection.y))
		{
			if(flickGestureDirection.x > 0)
				OnFlickRight?.Invoke();

			else
				OnFlickLeft?.Invoke();
		}

		// Vertical flick
		else
		{
			if(flickGestureDirection.y > 0)
				OnFlickUp?.Invoke();

			else
				OnFlickDown?.Invoke();
		}
	}
}

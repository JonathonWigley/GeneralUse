using System;
using System.Collections.Generic;
using TouchScript.Utils;
using TouchScript.Pointers;
using UnityEngine;
using UnityEngine.Profiling;
using TouchScript.Gestures;

/// <summary>
/// Created By: Jonathon Wigley - 12/30/2018
/// Last Edited By: Jonathon Wigley - 12/30/2018
/// </summary>

/// <summary>
/// Touchscript flick gesture with some added functionality
/// </summary>
public class CustomFlickGesture : FlickGesture
{
	/// <summary>
	/// Maximum distance that the flick can travel before it fails
	/// </summary>
	public float MaxDistance
	{
		get { return maxDistance; }
		set { maxDistance = value; }
	}
	[Tooltip("Maximum distance that the flick can travel before it fails")]
	[SerializeField] protected float maxDistance = 15f;

	/// <summary>
	/// Maximum amount of time the gesture can be held before it fails
	/// </summary>
	/// <value></value>
	public float MaxTime
	{
		get { return maxTime; }
		set { maxTime = value; }
	}
	[Tooltip("Maximum amount of time the gesture can be held before it fails")]
	[SerializeField] protected float maxTime = .3f;

	/// <summary>
	/// Time
	/// </summary>
	private float startTime;

	protected void Update()
	{
		// Check to see if time has run out for the gesture
		if(isActive)
		{
			bool bTimePastMax = Time.unscaledTime - startTime > MaxTime;
			if(bTimePastMax)
			{
				setState(GestureState.Failed);
			}
		}
	}

	/// <summary>
	/// Called when a pointer is newly pressed
	/// </summary>
	/// <param name="pointers"></param>
	protected override void pointersPressed(IList<Pointer> pointers)
	{
		base.pointersPressed(pointers);

		startTime = Time.unscaledTime;
	}

	/// <summary>
	/// Called every frame that a pointer is moving
	/// </summary>
	/// <param name="pointers"></param>
	protected override void pointersUpdated(IList<Pointer> pointers)
	{
		gestureSampler.Begin();

		if (isActive || !moving)
		{
			movementBuffer += ScreenPosition - PreviousScreenPosition;
			float dpiMovementThreshold = MovementThreshold * touchManager.DotsPerCentimeter;
			moving = movementBuffer.sqrMagnitude >= dpiMovementThreshold * dpiMovementThreshold;
		}

		float lastTime;
		Vector2 totalMovement = GetRecentMovementVector(out lastTime);

		bool bMovedMoreThanMaxDistance = totalMovement.magnitude >= MaxDistance * touchManager.DotsPerCentimeter;
		if(bMovedMoreThanMaxDistance)
		{
			setState(GestureState.Failed);
		}

		gestureSampler.End();
	}

	/// <summary>
	/// Created to separate out the handling of release functionalit for override
	/// Added by Jonathon Wigley
	/// </summary>
	/// <param name="lastTime">Most recently recorded time</param>
	/// <param name="totalMovement">Movement vector</param>
	protected override void HandleRelease(float lastTime, Vector2 totalMovement)
	{
		bool bMovedLessThanMinDistance = totalMovement.magnitude < MinDistance * touchManager.DotsPerCentimeter;
		bool bMovedMoreThanMaxDistance = totalMovement.magnitude >= MaxDistance * touchManager.DotsPerCentimeter;

		if (bMovedLessThanMinDistance || bMovedMoreThanMaxDistance)
		{
			setState(GestureState.Failed);
		}
		else
		{
			ScreenFlickVector = totalMovement;
			ScreenFlickTime = Time.unscaledTime - lastTime;
			setState(GestureState.Recognized);
		}
	}
}
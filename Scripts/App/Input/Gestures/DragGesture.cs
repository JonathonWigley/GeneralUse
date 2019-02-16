using System;
using System.Collections.Generic;
using UnityEngine;
using TouchScript.Gestures;
using TouchScript.Pointers;
using TouchScript.Layers;

/// <summary>
/// Created By: Jonathon Wigley - 12/11/2018
/// Last Edited By: Jonathon Wigley - 12/11/2018
/// </summary>

/// <summary>
/// Gesture that will pick up an object when started, drag it while held, and drop it when released
/// </summary>
public class DragGesture : Gesture
{
	/// <summary>
	/// First pointer to be recognized by this gesture
	/// </summary>
	private Pointer primaryPointer;

	/// <summary>
	/// 3D plane that the gesture projects to
	/// </summary>
	private Plane plane;

	/// <summary>
	/// Projection parameters for the projection layer
	/// </summary>
	private ProjectionParams projection;

	/// <summary>
	/// World coordinates where the gesture began
	/// </summary>
	private Vector3 startPosition;

	/// <summary>
	/// Duration that the gesture has been occurring
	/// </summary>
	public float Duration { get; private set; }

	/// <summary>
	/// Start position of the gesture
	/// </summary>
	public Vector3 StartPosition
	{
		get
		{
			switch(State)
			{
				case GestureState.Began:
				case GestureState.Changed:
				case GestureState.Ended:
					return startPosition;
				default:
					return transform.position;
			}
		}
	}

	/// <summary>
	/// Current position of the gesture
	/// </summary>
	public Vector3 Position
	{
		get
		{
			switch(State)
			{
				case GestureState.Began:
				case GestureState.Changed:
				case GestureState.Ended:
					return projection.ProjectTo(primaryPointer.Position, plane);
				default:
					return transform.position;
			}
		}
	}

	/// <summary>
	/// Dispatched when the pull gesture is first recognized
	/// </summary>
	public Action<DragGesture> OnPressed;

	/// <summary>
	///	Dispatched when the pull gesture is being updated/pulled
	/// </summary>
	public Action<DragGesture> OnDragged;

	/// <summary>
	/// Dispatched when the pull gesture has been released
	/// </summary>
	public Action<DragGesture> OnReleased;

	/// <summary>
	/// Update method for all pointers that were pressed this frame
	/// </summary>
	/// <param name="pointers">Pointers that were pressed this frame</param>
	protected override void pointersPressed(IList<Pointer> pointers)
	{
		if(State == GestureState.Idle)
		{
			// Get the first pointer that we care about
			primaryPointer = pointers[0];

			// Set up the layer projection
			projection = primaryPointer.GetPressData().Layer.GetProjectionParams(primaryPointer);
			plane = new Plane(Vector3.up, transform.position);
			startPosition = projection.ProjectTo(primaryPointer.Position, plane);

			// Change the state
			setState(GestureState.Began);
		}
	}

	/// <summary>
	/// Update method for all pointers that were updated this frame
	/// </summary>
	/// <param name="pointers">Pointers that were updated</param>
	protected override void pointersUpdated(IList<Pointer> pointers)
	{
		for (int i = 0; i < pointers.Count; i++)
		{
			// Change the state if the pointer we care about has moved
			if(pointers[i].Id == primaryPointer.Id)
			{
				// If we are moving into this function without having called pressed, call pressed
				// Fixes issues with waiting on another gesture to fail
				if(State != GestureState.Began && State != GestureState.Changed)
					pointersPressed(pointers);

				// Normal behaviour is to set the state to changed again
				else
					setState(GestureState.Changed);

				return;
			}
		}
	}

	/// <summary>
	/// Update method for all pointers that were released this frame
	/// </summary>
	/// <param name="pointers">Pointers that were released this frame</param>
	protected override void pointersReleased(IList<Pointer> pointers)
	{
		for (int i = 0; i < pointers.Count; i++)
		{
			// End the gesture if the pointer we care about was released
			if(pointers[i].Id == primaryPointer.Id)
			{
				setState(GestureState.Ended);
				return;
			}
		}
	}

	/// <summary>
	/// Update method for all pointers that were cancelled this frame
	/// </summary>
	/// <param name="pointers">Pointers that were cancelled this frame</param>
	protected override void pointersCancelled(IList<Pointer> pointers)
	{
		for (int i = 0; i < pointers.Count; i++)
		{
			// Cancel the gesture if the pointer we care about was cancelled
			if(pointers[i].Id == primaryPointer.Id)
			{
				setState(GestureState.Cancelled);
				return;
			}
		}
	}

	/// <summary>
	/// Called when the gesture transitions to the began state
	/// </summary>
	protected override void onBegan()
	{
		OnPressed?.Invoke(this);
	}

	/// <summary>
	/// Called when the gesture transitions to the changed state
	/// </summary>
	protected override void onChanged()
	{
		Duration += Time.deltaTime;
		OnDragged?.Invoke(this);
	}

	/// <summary>
	/// Called when the gesture transitions to the ended or recognized states
	/// </summary>
	protected override void onRecognized()
	{
		OnReleased?.Invoke(this);
	}

	/// <summary>
	/// Called when the gesture is recognized or failed
	/// </summary>
	protected override void reset()
	{
		base.reset();
		primaryPointer = null;
		Duration = 0f;
	}
}
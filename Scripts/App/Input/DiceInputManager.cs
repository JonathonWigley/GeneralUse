using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TouchScript;
using TouchScript.Pointers;

/// <summary>
/// Created By: Jonathon Wigley - 09/21/2018
/// Last Edited By: Jonathon Wigley - 11/14/2018
/// </summary>

/// <summary>
/// Purpose:
/// Control interactions with dice through input using
/// TouchScript's API
/// <summary>

public class DiceInputManager : MonoSingleton<DiceInputManager>
{
	protected ITouchManager touchManager; // Instance of Touchscript's ITouchManager
	protected List<ITouchable> currentTouchables = new List<ITouchable>(); // List of Touchable objects that are currently touched
	protected List<int> updatedPointerIDs = new List<int>(); // List of the IDs of pointers that have been updated during a frame

	protected override void Awake()
	{
		base.Awake();

		AssignTouchManager();
	}

	protected void OnEnable()
	{
		touchManager.PointersPressed += HandleInputPressed;
		touchManager.PointersUpdated += HandleInputMoved;
		touchManager.PointersReleased += HandleInputReleased;
		touchManager.PointersCancelled += HandleInputCancelled;
		touchManager.FrameFinished += HandleInputStationary;
	}

	protected void OnDisable()
	{
		touchManager.PointersPressed -= HandleInputPressed;
		touchManager.PointersUpdated -= HandleInputMoved;
		touchManager.PointersReleased -= HandleInputReleased;
		touchManager.PointersCancelled -= HandleInputCancelled;
		touchManager.FrameFinished -= HandleInputStationary;
	}

	/// <summary>
	/// Sets the reference to the touch manager
	/// </summary>
	protected void AssignTouchManager()
	{
		touchManager = TouchManager.Instance;
		if(touchManager == null)
			Debug.LogError("TouchManager instance does not exist in the scene.", this);
	}

#region Input Handlers
	/// <summary>
	/// Called when input from the user is first registered
	/// </summary>
	/// <param name="sender">TouchManager instance</param>
	/// <param name="args">Data about the input</param>
	protected void HandleInputPressed(object sender, PointerEventArgs args)
	{
		IList<Pointer> pointers = args.Pointers;
		for (int i = 0; i < pointers.Count; i++)
		{
			if(UIUtility.IsPositionOverUIObject(pointers[i].Position))
				continue;

			updatedPointerIDs.Add(pointers[i].Id);

			// Check to see if we hit any interactable object
			RaycastHit hit = RaycastUtility.GetWorldHit (pointers[i].Position, LayerMask.GetMask ("Dice", "InputTrigger"));
			if (hit.transform != null)
			{
				// If the object is touchable, call its handler and keep track of it
				ITouchable touchable = hit.transform.GetComponent<ITouchable>();
				if (touchable != null)
				{
					currentTouchables.Add (touchable);
					touchable.HandleInputPressed (pointers[i]);
				}
			}
		}
	}

	/// <summary>
	/// Called when input from the user has changed
	/// </summary>
	/// <param name="sender">TouchManager instance</param>
	/// <param name="args">Data about the input</param>
	protected void HandleInputMoved(object sender, PointerEventArgs args)
	{
		IList<Pointer> pointers = args.Pointers;
		for (int i = 0; i < pointers.Count; i++)
		{
			updatedPointerIDs.Add(pointers[i].Id);

			for (int j = 0; j < currentTouchables.Count; j++)
			{
				currentTouchables[j].HandleInputMoved(pointers[i]);
			}
		}
	}

	/// <summary>
	/// Called when input has been released by the user
	/// </summary>
	/// <param name="sender">TouchManager instance</param>
	/// <param name="args">Data about the input</param>
	protected void HandleInputReleased(object sender, PointerEventArgs args)
	{
		IList<Pointer> pointers = args.Pointers;
		for (int i = 0; i < pointers.Count; i++)
		{
			updatedPointerIDs.Add(pointers[i].Id);

			for (int j = currentTouchables.Count - 1; j >= 0; j--)
			{
				if(currentTouchables[j].HandleInputReleased(pointers[i]))
					currentTouchables.RemoveAt(j);
			}
		}

		// Clear any highlight or selected effects
		EventSystem.current.SetSelectedGameObject(null);
	}

	/// <summary>
	/// Called when input has been cancelled by some outside source
	/// </summary>
	/// <param name="sender">TouchManager instance</param>
	/// <param name="args">Data about the input</param>
	protected void HandleInputCancelled(object sender, PointerEventArgs args)
	{
		IList<Pointer> pointers = args.Pointers;
		for (int i = 0; i < pointers.Count; i++)
		{
			updatedPointerIDs.Add(pointers[i].Id);

			for (int j = currentTouchables.Count - 1; j >= 0; j--)
			{
				currentTouchables[j].HandleInputCancelled(pointers[i]);
				currentTouchables.RemoveAt(j);
			}
		}
	}

	/// <summary>
	/// Called when after all other pointer events. Updates any pointers
	/// that weren't moved this frame but are still active
	/// </summary>
	/// <param name="sender"></param>
	protected void HandleInputStationary(object sender, EventArgs args)
	{
		IList<Pointer> pointers = touchManager.PressedPointers;
		for (int i = 0; i < pointers.Count; i++)
		{
			// Ignore pointers that were already updated
			if(updatedPointerIDs.Contains(pointers[i].Id))
				continue;

			for (int j = currentTouchables.Count - 1; j >= 0; j--)
			{
				currentTouchables[j].HandleInputStationary(pointers[i]);
			}
		}

		updatedPointerIDs.Clear();
	}
#endregion
}

/// <summary>
/// Argument data for inputs
/// </summary>
public class InputData
{
	/// <summary>
	/// Position of the input on the screen
	/// </summary>
	public Vector2 ScreenPosition;

	/// <summary>
	/// Direction the input moved
	/// </summary>
	public Vector2 Direction;
}
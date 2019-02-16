using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Collection of utility functions for UI
/// </summary>
public static class UIUtility
{
		// Returns true if the ppinter is over any UI objects
		/// <summary>
		/// Checks to see if the given position is over any UI objects
		/// </summary>
		/// <param name="pointerPosition">Position to check</param>
		/// <returns>True if the point is over UI</returns>
		public static bool IsPositionOverUIObject(Vector2 pointerPosition)
		{
			// Create a new pointer event based on the current event system and get its position
			PointerEventData currentEventData = new PointerEventData(EventSystem.current);
			currentEventData.position = new Vector2(pointerPosition.x, pointerPosition.y);

			// Raycast out to see if we hit any UI
			List<RaycastResult> results = new List<RaycastResult>();
			EventSystem.current.RaycastAll(currentEventData, results);

			// Return true if we hit anything
			return results.Count > 0;
		}

		/*-------------------------------------------------------------------------------------------------------------------------------------*/

		/// <summary>
		/// Finds every UI object at the given position
		/// </summary>
		/// <param name="pointerPos">Position to check</param>
		/// <returns>List of all UI objects found</returns>
		public static List<RaycastResult> GetUIObjectsAtPosition(Vector2 pointerPos)
		{
			// Create a new pointer event based on the current event system and get its position
			PointerEventData currentEventData = new PointerEventData(EventSystem.current);
			currentEventData.position = new Vector2(pointerPos.x, pointerPos.y);

			// Raycast out to see if we hit any UI
			List<RaycastResult> results = new List<RaycastResult>();
			EventSystem.current.RaycastAll(currentEventData, results);

			// Return the list of hit objects
			return results;
		}
}

using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Created By: Jonathon Wigley - 10/12/2018
/// Last Edited By: Jonathon Wigley - 10/12/2018
/// </summary>

/// <summary>
/// Partial class of Die
/// Handles functionality dealing with the die's sides
/// </summary>
public partial class Die : PooledObject
{
    /// <summary>
    /// List of all the die sides associated with this die
    /// Sides add themselves to the die
    /// </summary>
    private List<DieSide> sides = new List<DieSide>(); // List of the sides for this die, sides add themselves to the list

    /// <summary>
    /// Number of sides this die has
    /// </summary>
    public int SideCount { get; private set; }

	/// <summary>
	/// Finds all the Die Sides for this die
	/// </summary>
	private void PopulateListOfSides()
	{
		sides.Clear();
		sides = GetComponentsInChildren<DieSide>().ToList();
		SideCount = sides.Count;
	}

	/// <summary>
	/// Returns true if one of this Die's sides is on the ground
	/// </summary>
	/// <returns>True if a side is on the ground</returns>
    public bool IsSideOnGround()
    {
		for (int i = 0; i < sides.Count; i++)
		{
			if(sides[i].bOnGround == true)
				return true;
		}

		return false;
    }

	/// <summary>
	/// Finds a Die Side that is currently touching the ground
	/// </summary>
	/// <returns>Side that is touching the ground. null if no side is touching the ground</returns>
	public DieSide GetSideOnGround()
	{
		for (int i = 0; i < sides.Count; i++)
		{
			if(sides[i].bOnGround == true)
				return sides[i];
		}

		return null;
	}

	/// <summary>
	/// Returns the side closest to the ground. If no side is on the ground,
	/// this causes every side on the die to raycast to the ground to find the closest
	/// </summary>
	/// <param name="checkForSidesOnGround">If true, checks for a side touching the ground before raycasting for the closest side. Default value is true</param>
	/// <returns>Die Side that is closest to the ground. Returns null if no side is close to the ground</returns>
	public DieSide GetSideClosestToTheGround(bool checkForSidesOnGround = true)
	{
		if(checkForSidesOnGround == true)
		{
			DieSide groundedSide = GetSideOnGround();
			if(groundedSide != null)
				return groundedSide;
		}

		// If no side was on the ground, find the closest one to the ground
		float closestDistance = float.PositiveInfinity;
		DieSide closestSide = null;

		for (int i = 0; i < sides.Count; i++)
		{
			// Raycasting function called
			float distance = sides[i].GetDistanceFromGround();
			if(distance < closestDistance)
			{
				closestDistance = distance;
				closestSide = sides[i];
			}
		}

		// Return null if the die isn't close to the ground
		if(closestDistance == float.PositiveInfinity)
			return null;

		return closestSide;
	}
}

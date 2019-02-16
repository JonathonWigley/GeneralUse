using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// Created By: Jonathon Wigley - 7/7/2018
/// Last Edited By: Jonathon Wigley - 7/7/2018
///
/// Purpose:
/// Stores the data for an individual die's roll
/// <summary>

[System.Serializable]
public class RollData : IComparable
{
	/// <summary>
	/// Type of die rolled
	/// </summary>
	public DieType DieType;

	/// <summary>
	/// Value that the die rolled
	/// </summary>
	public int RolledValue;

	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="dieType">Type of die rolled</param>
	/// <param name="rolledValue">Value that the die rolled</param>
	public RollData(DieType dieType, int rolledValue)
	{
		DieType = dieType;
		RolledValue = rolledValue;
	}

    public int CompareTo(object obj)
    {
		RollData otherData = obj as RollData;

		if(otherData == null || otherData.DieType == DieType)
			return 0;

		if(otherData.DieType < DieType)
			return 1;

		else
			return -1;
    }
}

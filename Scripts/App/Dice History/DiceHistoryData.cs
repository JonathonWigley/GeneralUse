using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Created By: Jonathon Wigley - 7/7/2018
/// Last Edited By: Jonathon Wigley - 02/03/2019
/// </summary>

/// <summary>
/// Database that stores information about the dice history
/// </summary>
[System.Serializable]
public class DiceHistoryData : DatabaseData
{
	/// <summary>
	/// Name given to the roll
	/// </summary>
	public string name;

	/// <summary>
	/// Roll data for each die that was rolled
	/// </summary>
	public List<RollData> rolls = new List<RollData>();

	/// <summary>
	/// Total value rolled
	/// </summary>
	protected int rollTotal = -1;
	public int RollTotal
	{
		get
		{
			// Calculate the roll total if it hasn't already been found
			if(rollTotal == -1)
			{
				rollTotal = 0;
				for (int i = 0; i < rolls.Count; i++)
					rollTotal += rolls[i].RolledValue;
			}

			return rollTotal;
		}
	}

	/// <summary>
	/// Highest value that was rolled on this roll
	/// </summary>
	/// <value></value>
	public int HightestRolledValue
	{
		get
		{
			if(highestRolledValue == -1)
			{
				highestRolledValue = 0;
				for (int i = 0; i < rolls.Count; i++)
				{
					int rolledValue = rolls[i].RolledValue;
					if(rolledValue > highestRolledValue)
						highestRolledValue = rolledValue;
				}
			}

			return highestRolledValue;
		}
	}
	protected int highestRolledValue = -1;

	/// <summary>
	/// Highest value that was rolled on this roll
	/// </summary>
	/// <value></value>
	public int LowestRolledValue
	{
		get
		{
			if(lowestRolledValue == -1)
			{
				lowestRolledValue = 0;
				for (int i = 0; i < rolls.Count; i++)
				{
					int rolledValue = rolls[i].RolledValue;
					if(rolledValue < lowestRolledValue)
						lowestRolledValue = rolledValue;
				}
			}

			return lowestRolledValue;
		}
	}
	protected int lowestRolledValue = -1;
}
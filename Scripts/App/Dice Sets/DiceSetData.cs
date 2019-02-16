using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Created By: Jonathon Wigley - 10/18/2018
/// Last Edited By: Jonathon Wigley - 02/03/2019
/// </summary>

/// <summary>
/// Data about a dice set
/// </summary>
[System.Serializable]
public class DiceSetData : DatabaseData
{
	/// <summary>
	/// Name of the dice set
	/// </summary>
	public string name;

	/// <summary>
	/// Types of dice found in this dice set
	/// </summary>
	public List<DieType> dieTypes;
}
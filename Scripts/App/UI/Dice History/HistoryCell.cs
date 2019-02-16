using UnityEngine;
using System.Collections.Generic;
using TMPro;

/// <summary>
/// Created By: Jonathon Wigley - 10/21/2018
/// Last Edited By: Jonathon Wigley - 02/03/2019
/// </summary>

/// <summary>
/// Component attached to history cells to manage their functionality
/// </summary>
public class HistoryCell : MonoBehaviour
{
	/// <summary>
	/// Reference to the text field that displays the roll total
	/// </summary>
	[SerializeField] private TextMeshProUGUI RollTotalField;

	/// <summary>
	/// Reference to the prefab used for populating rolled dice
	/// </summary>
	[SerializeField] private HistoryDieRollCell dieRollCellPrefab;

	/// <summary>
	/// Reference to the container object for the rolled dice prefabs
	/// </summary>
	[SerializeField] private Transform dieRollCellContainer;

	/// <summary>
	/// History variable associated with this cell
	/// </summary>
	public DiceHistoryData historyData;

	public void SetHistory(DiceHistoryData history)
	{
		historyData = history;
		SetRollTotal(history.RollTotal);
		SetRolledDice(history.rolls);
	}

	/// <summary>
	/// Set the roll total for this cell
	/// </summary>
	/// <param name="total"></param>
	private void SetRollTotal(int total)
	{
		RollTotalField.text = total.ToString();
	}

	/// <summary>
	/// Create the die rolls based on the list
	/// </summary>
	/// <param name="rollData">Data about the dice that were rolled</param>
	private void SetRolledDice(List<RollData> rollData)
	{
		rollData.Sort();
		for (int i = 0; i < rollData.Count; i++)
		{
			HistoryDieRollCell newCell = Instantiate(dieRollCellPrefab, dieRollCellContainer);
			newCell.SetRollData(rollData[i]);
		}
	}

	/// <summary>
	/// Determine if another object is equivalent to this one
	/// </summary>
	/// <param name="other">Object to test</param>
	/// <returns>True if the object shares the same ID as this one</returns>
	public override bool Equals(object other)
	{
		// Doesn't equal if the other object isn't a HistoryCell
		if(other.GetType().Equals(typeof(HistoryCell)) == false)
			return false;

		// Check if the ID's are the same
		HistoryCell otherCell = other as HistoryCell;
		if(historyData.ID == otherCell.historyData.ID)
			return true;
		else
			return false;
	}
}

using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Created By: Jonathon Wigley - 10/21/2018
/// Last Edited By: Jonathon Wigley - 02/03/2019
/// </summary>

/// <summary>
/// Collapsable view that also handles the UI for the dice history
/// </summary>
public class HistoryCollapsableView : CollapsableView
{
	[Header("History References")]
	/// <summary>
	/// Reference to the history cell prefab
	/// </summary>
	[SerializeField] private HistoryCell historyCellPrefab;

	/// <summary>
	/// Reference to the container object for history cells
	/// </summary>
	[SerializeField] private Transform historyCellContainer;

	/// <summary>
	/// List of currently active history cells
	/// </summary>
	private List<HistoryCell> historyCells = new List<HistoryCell>();

	private void OnEnable()
	{
		RefreshHistoryCells();

		DiceHistoryManager.OnHistoryAdded += DiceHistoryManager_HistoryAdded;
		DiceHistoryManager.OnHistoryRemoved += DiceHistoryManager_HistoryRemoved;
	}

	private void OnDisable()
	{
		DiceHistoryManager.OnHistoryAdded -= DiceHistoryManager_HistoryAdded;
		DiceHistoryManager.OnHistoryRemoved -= DiceHistoryManager_HistoryRemoved;
	}

	/// <summary>
	/// Called when history is added
	/// </summary>
	/// <param name="history">History that was added</param>
	private void DiceHistoryManager_HistoryAdded(DiceHistoryData history)
	{
		AddCell(history);
	}

	/// <summary>
	/// Called when history is removed
	/// </summary>
	/// <param name="history">History that was removed</param>
	private void DiceHistoryManager_HistoryRemoved(DiceHistoryData history)
	{
		RemoveCell(history);
	}

	/// <summary>
	/// Checks the history database and makes sure that the UI matches
	/// </summary>
	private void RefreshHistoryCells()
	{
		List<DiceHistoryData> historyData = new List<DiceHistoryData>(DiceHistoryManager.Instance.HistoryDatabase.Data);

		// Make the history cell list
		for (int i = 0; i < historyData.Count; i++)
		{
			// If no history cell exists where there should be data, add a cell
			if(historyCells.Count <= i)
			{
				AddCell(historyData[i]);
			}

			// Do nothing if the lists match
			else if(historyCells[i].historyData.ID == historyData[i].ID)
			{
				continue;
			}

			// History cell and history database entries are different, so remove the history cell
			else if(historyCells[i].historyData.ID != historyData[i].ID)
			{
				// Replace the incorrect history with proper history
				historyCells[i].SetHistory(historyData[i]);
			}
		}
	}

	/// <summary>
	/// Add a cell to the UI
	/// </summary>
	/// <param name="history">Information to use for the cell</param>
	private void AddCell(DiceHistoryData history)
	{
		HistoryCell newCell = Instantiate(historyCellPrefab, historyCellContainer);
		newCell.transform.SetAsFirstSibling();

		newCell.SetHistory(history);
		historyCells.Add(newCell);
	}

	/// <summary>
	/// Remove the cell representing the given history
	/// </summary>
	/// <param name="history"></param>
	private bool RemoveCell(DiceHistoryData history)
	{
		int removeIndex = historyCells.FindIndex(cell => cell.historyData.ID == history.ID);
		return RemoveCellAt(removeIndex);
	}

	/// <summary>
	/// Remove and destroy the history cell
	/// </summary>
	/// <param name="cellToRemove">Cell to remove</param>
	private bool RemoveCell(HistoryCell cellToRemove)
	{
		int indexToRemove = historyCells.FindIndex(cell => cell.historyData.Equals(cellToRemove.historyData));
		return RemoveCellAt(indexToRemove);
	}

	/// <summary>
	/// Remove the cell at the given index
	/// </summary>
	/// <param name="removeIndex">Index of the cell array to remove</param>
	private bool RemoveCellAt(int removeIndex)
	{
		if(removeIndex < 0 || removeIndex >= historyCells.Count)
			return false;

		Destroy(historyCells[removeIndex].gameObject);
		historyCells.RemoveAt(removeIndex);
		return true;
	}
}

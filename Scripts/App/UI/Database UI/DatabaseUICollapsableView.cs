using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Created By: Jonathon Wigley - 11/03/2018
/// Last Edited By: Jonathon Wigley - 11/04/2018
/// </summary>

/// <summary>
/// Parent class for a collapsable used to represent a database
/// </summary>
public abstract class DatabaseUICollapsableView<T> : CollapsableView where T : DatabaseData
{
	[Header("Database Settings")]
	[SerializeField] protected Database<T> database;

	/// <summary>
	/// Prefab of the cell to spawn to represent the data
	/// </summary>
	[SerializeField] protected DatabaseUICell cellPrefab;

	/// <summary>
	/// Transform of the parent container for the spawned cells
	/// </summary>
	[SerializeField] protected Transform cellContainerTransform;

	/// <summary>
	/// If true, new cells will be set as the first sibling in the hierarchy
	/// </summary>
	[Tooltip("If true, new cells will be set as the first sibling in the hierarchy")]
	[SerializeField] protected bool bPlaceNewCellsAtTop = false;

	/// <summary>
	/// List of all current cells
	/// </summary>
	protected List<DatabaseUICell> cells = new List<DatabaseUICell>();

	/// <summary>
	/// Reference to the most recent data obtained from the database
	/// </summary>
	protected List<DatabaseData> currentData = new List<DatabaseData>();

	protected virtual void OnEnable()
	{
		RefreshCells();
	}

	/// <summary>
	/// Called whenever data is added to the datbase
	/// </summary>
	/// <param name="data">Data that was added</param>
	protected virtual void DatabaseManager_DataAdded(DatabaseData data)
	{
		AddCell(data);
	}

	/// <summary>
	/// Called whenever data is removed from the database
	/// </summary>
	/// <param name="data">Data that was removed</param>
	protected virtual void DatabaseManager_DataRemoved(DatabaseData data)
	{
		RemoveCell(data);
	}

	/// <summary>
	/// Get the most recent data from the database
	/// </summary>
	protected abstract void GetDataFromDatabase();

	/// <summary>
	/// Refresh the UI based on the data currently in the database
	/// </summary>
	protected virtual void RefreshCells()
	{
		// Get the most up to date data from the database
		GetDataFromDatabase();

		int loopIndex = 0;
		for (int i = 0; i < currentData.Count; i++)
		{
			// Have a manual break out as a failsafe, shouldn't be needed
			if(loopIndex > 200)
			{
				Debug.LogError("Loop failsafe activated", this);
				return;
			}
			loopIndex++;

			// If no cell exists where there should be data, add a cell
			if(cells.Count <= i)
			{
				AddCell(currentData[i]);
			}

			// If the database and cell data match up, continue to the next one
			else if(cells[i].Data.Equals(currentData[i]))
			{
				continue;
			}

			// cell and database entries are different, so remove the cell
			else if(cells[i].Data.Equals(currentData[i]) == false)
			{
				// Remove the cell and compare the same data data against the next cell
				RemoveCell(currentData[i]);
				i--;
			}
		}
	}

	/// <summary>
	/// Add a cell to the view
	/// </summary>
	protected virtual void AddCell(DatabaseData data)
	{
		DatabaseUICell newCell = cellPrefab.GetPooledInstance<DatabaseUICell>();
		newCell.transform.SetParent(cellContainerTransform);

		if(bPlaceNewCellsAtTop)
			newCell.transform.SetAsFirstSibling();

		newCell.SetData(data);
		cells.Add(newCell);
	}

	/// <summary>
	/// Remove a cell from the view
	/// </summary>
	/// <param name="data">Data of the cell to remove</param>
	protected virtual bool RemoveCell(DatabaseData data)
	{
		int removeIndex = cells.FindIndex(x => x.Data.Equals(data));
		return RemoveCellAt(removeIndex);
	}

	/// <summary>
	/// Remove a cell from the view
	/// </summary>
	/// <param name="cell">UI Cell to remove</param>
	protected virtual bool RemoveCell(DatabaseUICell cell)
	{
		int removeIndex = cells.FindIndex(x => x.Data.Equals(cell.Data));
		return RemoveCellAt(removeIndex);
	}

	/// <summary>
	/// Remove a cell at a certain index from the current view
	/// </summary>
	/// <param name="removeIndex">Index to remove a cell at</param>
	protected virtual bool RemoveCellAt(int removeIndex)
	{
		if(removeIndex < 0 || removeIndex >= cells.Count)
			return false;

		Destroy(cells[removeIndex].gameObject);
		cells.RemoveAt(removeIndex);
		return true;
	}
}

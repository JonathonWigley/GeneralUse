using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Created By: Jonathon Wigley - 10/16/2018
/// Last Edited By: Jonathon Wigley - 02/03/2019
/// </summary>

/// <summary>
/// Manages functionality and updating of the dice history
/// </summary>
public class DiceHistoryManager : MonoSingleton<DiceHistoryManager>
{
	/// <summary>
	/// Reference to the dice history database currently being used to track history
	/// </summary>
	public DiceHistoryDatabase HistoryDatabase { get { return database.Clone(); } }
	[SerializeField] private DiceHistoryDatabase database;

	/// <summary>
	/// Keeps track of the number of rolls
	/// </summary>
	private int rollIndex = 1;

	/// <summary>
	/// Dispatched when history data is added to history
	/// </summary>
	/// <param name="history">History data that was added</param>
	public static event HistoryAddedDelegate OnHistoryAdded;
	public delegate void HistoryAddedDelegate(DiceHistoryData history);

	/// <summary>
	/// Dispatched when history data is removed from history
	/// </summary>
	/// <param name="history">History data that was removed</param>
	public static event HistoryRemovedDelegate OnHistoryRemoved;
	public delegate void HistoryRemovedDelegate(DiceHistoryData history);

	private void OnEnable()
	{
		DiceManager.OnAllDiceAtRest += DiceManager_AllDiceAtRest;
		database.OnDataRemoved += DiceHistoryDatabase_DataRemoved;
	}

	private void OnDisable()
	{
		DiceManager.OnAllDiceAtRest -= DiceManager_AllDiceAtRest;
		database.OnDataRemoved -= DiceHistoryDatabase_DataRemoved;
	}

	/// <summary>
	/// Called when all dice come to rest
	/// </summary>
	/// <param name="data">Data about all dice that were rolled</param>
	private void DiceManager_AllDiceAtRest(DiceData data)
	{
		// Get information about each die roll
		List<RollData> rollData = new List<RollData>();
		for (int i = 0; i < data.Dice.Count; i++)
		{
			Die die = data.Dice[i];
			rollData.Add(new RollData(die.DieType, die.RolledValue));
		}

		DiceHistoryData newHistory = new DiceHistoryData();
		newHistory.name = string.Format("Roll {0}", rollIndex);
		newHistory.rolls = rollData;

		// Add the history to the database
		AddHistory(newHistory);

		rollIndex++;
	}

	/// <summary>
	/// Called when the dice history database has data added removed from it
	/// Register to this so that when the database
	/// </summary>
	/// <param name="data">Data that was removed from the history</param>
	private void DiceHistoryDatabase_DataRemoved(DiceHistoryData data)
	{
		database.RemoveData(data);
		OnHistoryRemoved?.Invoke(data);
	}

	/// <summary>
	/// Called to clear the history
	/// </summary>
	public void BTN_ClearHistory()
	{
		for (int i = database.Data.Count - 1; i >= 0; i--)
		{
			RemoveHistory(database.Data[i]);
		}
	}

	/// <summary>
	/// Called by a button in editor to delete a piece of dice history
	/// </summary>
	/// <param name="history">History to be deleted</param>
	public void BTN_DeleteHistory(DiceHistoryData history)
	{
		RemoveHistory(history);
	}

	/// <summary>
	/// Add a piece of history to the history object
	/// </summary>
	/// <param name="data">History to be added</param>
	private void AddHistory(DiceHistoryData data)
	{
		if(database.Data.Count >= database.MaxDatabaseSize)
			RemoveHistory(database.Data[0]);

		database.AddData(data);
		OnHistoryAdded?.Invoke(data);
	}

	/// <summary>
	/// Remove a piece of history data from the database
	/// </summary>
	/// <param name="history">History to be removed</param>
	private void RemoveHistory(DiceHistoryData history)
	{
		database.RemoveData(history);

		OnHistoryRemoved?.Invoke(history);
	}
}

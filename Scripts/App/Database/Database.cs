using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

/// <summary>
/// Created By: Jonathon Wigley - 10/17/2018
/// Last Edited By: Jonathon Wigley - 02/03/2019
/// </summary>

/// <summary>
/// Parent class for any database in the project
/// </summary>
public abstract class Database<T> : ScriptableObject where T : DatabaseData
{
	/// <summary>
	/// List containing the recorded history
	/// </summary>
	public ReadOnlyCollection<T> Data => data.AsReadOnly();
	[SerializeField] protected List<T> data = new List<T>();

	/// <summary>
	/// Max size of the database, after which data will be dropped off
	/// </summary>
	public int MaxDatabaseSize { get { return maxDatabaseSize; } }
	protected int maxDatabaseSize = 100;

	/// <summary>
	/// Dispatched when data has been added to the database
	/// </summary>
	public Action<T> OnDataAdded;

	/// <summary>
	/// Dispatched when data has been removed from the database
	/// </summary>
	public Action<T> OnDataRemoved;

	protected virtual void OnEnable()
	{
		InitDatabase();
	}

	/// <summary>
	/// Add the data to the database
	/// </summary>
	/// <param name="newData">Data to add</param>
	public virtual void AddData(T newData)
	{
		// Drop off data from the end as we add new data
		if(data.Count >= maxDatabaseSize)
			data.RemoveAt(0);

		newData.ID = GetFirstAvailableID();
		data.Add(newData);

		OnDataAdded?.Invoke(newData);
	}

	/// <summary>
	/// Remove the data from the database
	/// </summary>
	/// <param name="dataToRemove">Data to remove</param>
	public virtual void RemoveData(T dataToRemove)
	{
		data.Remove(dataToRemove);

		OnDataRemoved?.Invoke(dataToRemove);
	}

	/// <summary>
	/// Clears all data from the database
	/// </summary>
	public virtual void Clear()
	{
		data.Clear();
	}

	/// <summary>
	/// Initialize settings for the database
	/// </summary>
	protected virtual void InitDatabase()
	{
		InitMaxDatabaseSize();
		TrimDatabaseToSize();
	}

	/// <summary>
	/// Set the max database size
	/// </summary>
	/// <param name="max">Max size to be set</param>
	protected abstract void InitMaxDatabaseSize();

	/// <summary>
	/// Clear out any data over the max size of the database
	/// </summary>
	protected virtual void TrimDatabaseToSize()
	{
		// Clear any data over the max
		if(data.Count > maxDatabaseSize)
		{
			for (int i = data.Count - 1; i >= maxDatabaseSize; i--)
			{
				RemoveData(data[i]);
			}
		}
	}

	/// <summary>
	/// Find the first available ID for a new piece of data
	/// </summary>
	protected virtual int GetFirstAvailableID()
	{
		// Find all ID's that have been used
		List<int> usedIds = new List<int>();
		for (int i = 0; i < data.Count; i++)
			usedIds.Add(data[i].ID);

		// Find all available ID's
		List<int> possibleIds = Enumerable.Range(0, maxDatabaseSize).Except(usedIds).ToList();

		// Return the first available ID
		return possibleIds.Count > 0 ? possibleIds[0] : -1;
	}
}
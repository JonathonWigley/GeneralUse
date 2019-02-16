using System;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Created By: Jonathon Wigley - 10/27/2018
/// Last Edited By: Jonathon Wigley - 02/03/2019
/// </summary>

/// <summary>
/// Collapsable view that manages the dice skin selection
/// </summary>
public class DiceSkinCollapsableView : DatabaseUICollapsableView<DiceSkinData>
{
	protected override void OnEnable()
	{
		database.OnDataAdded += Database_DataAdded;
		database.OnDataAdded += Database_DataRemoved;
	}

	protected void OnDisable()
	{
		database.OnDataAdded -= Database_DataAdded;
		database.OnDataAdded -= Database_DataRemoved;
	}

    protected void Database_DataAdded(DiceSkinData obj)
    {
    }

    protected void Database_DataRemoved(DiceSkinData obj)
    {
    }

    /// <summary>
    /// Get the most recent data from the dice set database
    /// </summary>
    protected override void GetDataFromDatabase()
    {
        // Clear the current data and get the most up to date data
        currentData.Clear();

        // Add all the sets to the list of current data
        for (int i = 0; i < database.Data.Count; i++)
        {
            currentData.Add(database.Data[i]);
        }
}
}

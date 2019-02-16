using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// Created By: Jonathon Wigley - 11/03/2018
/// Last Edited By: Jonathon Wigley - 11/04/2018
/// </summary>

/// <summary>
/// Collapsable view that contains the dice set cells
/// </summary>
public class DiceSetsCollapsableView : DatabaseUICollapsableView<DiceSetData>
{
    protected override void OnEnable()
    {
        base.OnEnable();

        DiceSetManager.OnSetAdded += DatabaseManager_DataAdded;
        AddButtonUI.OnPressed += AddButtonUI_Pressed;
    }

    private void OnDisable()
    {
        DiceSetManager.OnSetRemoved += DatabaseManager_DataRemoved;
        AddButtonUI.OnPressed -= AddButtonUI_Pressed;
    }

    private void AddButtonUI_Pressed()
    {
        if(bIsOpen == true)
        {
            DiceSetManager.Instance.AddDiceSetFromCurrentDice();
        }
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
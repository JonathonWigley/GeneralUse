using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Created By: Jonathon Wigley - 10/16/2018
/// Last Edited By: Jonathon Wigley - 02/03/2019
/// </summary>

/// <summary>
/// Dice set object that can be used to load a specific set of dice
/// </summary>
[CreateAssetMenu(fileName = "Dice Set", menuName = "Database/Data/Set")]
public class DiceSetDataReference : DatabaseDataReference<DiceSetData>
{
}
using System;
using UnityEngine;
using TMPro;

/// <summary>
/// Created By: Jonathon Wigley - 10/21/2018
/// Last Edited By: Jonathon Wigley - 10/21/2018
/// </summary>

/// <summary>
/// Component attached to die roll history cells to control the display of information
/// </summary>
public class HistoryDieRollCell : MonoBehaviour
{
	/// <summary>
	/// Reference to the text field that displays the die type
	/// </summary>
	[SerializeField] private TextMeshProUGUI dieTypeField;

	/// <summary>
	/// Reference to the text field that displays the rolled value
	/// </summary>
	[SerializeField] private TextMeshProUGUI rolledValueField;

	/// <summary>
	/// Set the die roll cell's data
	/// </summary>
	/// <param name="data">Data about the die roll</param>
	public void SetRollData(RollData data)
	{
		SetDieType(data.DieType);
		SetRolledValue(data.RolledValue);
	}

	/// <summary>
	/// Set the type of die that was rolled
	/// </summary>
	/// <param name="type"></param>
	private void SetDieType(DieType type)
	{
		dieTypeField.text = Enum.GetName(typeof(DieType), type).ToUpper();
	}

	/// <summary>
	/// Set the value of the die rolled
	/// </summary>
	/// <param name="val"></param>
	private void SetRolledValue(int val)
	{
		rolledValueField.text = val.ToString();
	}
}

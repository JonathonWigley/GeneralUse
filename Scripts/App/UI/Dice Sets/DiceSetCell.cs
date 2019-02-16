using UnityEngine;
using TMPro;

/// <summary>
/// Created By: Jonathon Wigley - 10/18/2018
/// Last Edited By: Jonathon Wigley - 02/03/2019
/// </summary>

/// <summary>
/// Component attached to set cells
/// </summary>
public class DiceSetCell : DatabaseUICell
{
	/// <summary>
	/// Reference to the text field that shows the set's name
	/// </summary>
	[SerializeField] private TextMeshProUGUI nameField;

	/// <summary>
	/// Assign the dice set reference associated with this cell
	/// </summary>
	/// <param name="newData"></param>
	public override void SetData(DatabaseData newData)
	{
		base.SetData(newData);

		DiceSetData set = newData as DiceSetData;
		nameField.text = set.name;
	}
}

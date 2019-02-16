using UnityEngine;
using System.Collections.Generic;
using TMPro;

/// <summary>
/// Created By: Jonathon Wigley
/// Last Edited By: Jonathon Wigley - 10/18/2018
/// </summary>

/// <summary>
/// Component that is attache to the text object controlling the roll total
/// to control updating of the roll total
/// </summary>
public class RollTotalDisplay : MonoBehaviour
{
	/// <summary>
	/// Reference to the text field that will display the roll total
	/// </summary>
	[SerializeField] private TextMeshProUGUI rollTotalTextField1;
	[SerializeField] private TextMeshProUGUI rollTotalTextField2;
	[SerializeField] private TextMeshProUGUI rollTotalTextField3;

	public delegate void PressedDelegate();
	public static event PressedDelegate OnPressed;

	private void Start()
	{
		RefreshTotals();
	}

	private void OnEnable()
	{
		DiceHistoryManager.OnHistoryAdded += DiceHistoryManager_HistoryAdded;
		DiceHistoryManager.OnHistoryRemoved += DiceHistoryManager_HistoryRemoved;
	}

	private void OnDisable()
	{
		DiceHistoryManager.OnHistoryAdded -= DiceHistoryManager_HistoryAdded;
	}

	/// <summary>
	/// Called when history is add to the dice history
	/// </summary>
	/// <param name="history"></param>
	private void DiceHistoryManager_HistoryAdded(DiceHistoryData history)
	{
		SetRollTotal(history.RollTotal);
	}

	private void DiceHistoryManager_HistoryRemoved(DiceHistoryData history)
	{
		RefreshTotals();
	}

	/// <summary>
	/// Opens the history panel
	/// </summary>
	public void BTN_OpenHistory()
	{
		OnPressed?.Invoke();
	}

	/// <summary>
	/// Initialize what totals are displayed based on the current history database
	/// </summary>
	private void RefreshTotals()
	{
		List<DiceHistoryData> data = new List<DiceHistoryData>(DiceHistoryManager.Instance.HistoryDatabase.Data);

		if(data.Count >= 1)
		{
			rollTotalTextField1.text = data[data.Count - 1].RollTotal.ToString();
		}
		else
		{
			rollTotalTextField1.text = "";
		}

		if(data.Count >= 2)
		{
			rollTotalTextField2.text = data[data.Count - 2].RollTotal.ToString();
		}
		else
		{
			rollTotalTextField2.text = "";
		}

		if(data.Count >= 3)
		{
			rollTotalTextField3.text = data[data.Count - 3].RollTotal.ToString();
		}
		else
		{
			rollTotalTextField3.text = "";
		}
	}

	/// <summary>
	/// Set the text for the roll total display
	/// </summary>
	/// <param name="total">string to set the total to</param>
	public void SetRollTotal(string total)
	{
		rollTotalTextField3.text = rollTotalTextField2.text;
		rollTotalTextField2.text = rollTotalTextField1.text;
		rollTotalTextField1.text = total;
	}

	/// <summary>
	/// Set the text for the roll total display
	/// </summary>
	/// <param name="total">Number to set the total to</param>
	public void SetRollTotal(int total)
	{
		rollTotalTextField3.text = rollTotalTextField2.text;
		rollTotalTextField2.text = rollTotalTextField1.text;
		rollTotalTextField1.text = total.ToString();
	}
}

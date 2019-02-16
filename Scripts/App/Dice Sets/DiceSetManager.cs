using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Created By: Jonathon Wigley - 10/16/2018
/// Last Edited By: Jonathon Wigley - 02/03/2019
/// </summary>

/// <summary>
/// Manages functionality for saved dice sets
/// </summary>
public class DiceSetManager : MonoSingleton<DiceSetManager>
{
	/// <summary>
	/// Reference to the dice set database currently being used
	/// </summary>
	[SerializeField] private DiceSetDatabase database;
	public DiceSetDatabase Database { get { return database.Clone(); } }

	/// <summary>
	/// Index used to name sets
	/// </summary>
	private int setIndex;

	/// <summary>
	/// Dispatched when a set is added to saved sets
	/// </summary>
	/// <param name="set">Set that was added</param>
	public static event SetAddedDelegate OnSetAdded;
	public delegate void SetAddedDelegate(DiceSetData set);

	/// <summary>
	/// Dispatched when a set is removed from saved sets
	/// </summary>
	/// <param name="set">Set that was removed</param>
	public static event SetRemovedDelegate OnSetRemoved;
	public delegate void SetRemovedDelegate(DiceSetData set);

	/// <summary>
	/// Called when a dice set is loaded
	/// </summary>
	/// <param name="set">Set to be loaded</param>
	public static event SetLoadedDelegate OnSetLoaded;
	public delegate void SetLoadedDelegate(DiceSetData set);

	private void Start()
	{
		setIndex = database.Data.Count + 1;
	}

	private void OnEnable()
	{
		AddSetButton.OnPressed += AddSetButton_Pressed;
		DiceSetCell.OnPressed += DiceSetCell_Pressed;
	}

	private void OnDisable()
	{
		AddSetButton.OnPressed -= AddSetButton_Pressed;
		DiceSetCell.OnPressed -= DiceSetCell_Pressed;
	}

	/// <summary>
	/// Called when the add dice set button is pressed
	/// </summary>
	/// <param name="sender">Add set button that was pressed</param>
	private void AddSetButton_Pressed(AddSetButton sender)
	{
		AddDiceSetFromCurrentDice();
	}

	/// <summary>
	/// Called when a dice set cell is pressed
	/// </summary>
	/// <param name="data">Set data associated with the dice set cell</param>
	private void DiceSetCell_Pressed(DatabaseData data)
	{
		DiceSetData set = data as DiceSetData;
		LoadDiceSet(set);
	}

	/// <summary>
	/// Adds a new dice set to the database using the dice currently on the board
	/// </summary>
	public void AddDiceSetFromCurrentDice()
	{
		// Get die types for each of the current dice
		List<Die> currentDice = DiceManager.Instance.Dice;
		List<DieType> dieTypes = new List<DieType>();
		for (int i = 0; i < currentDice.Count; i++)
		{
			dieTypes.Add(currentDice[i].DieType);
		}

		if(dieTypes.Count == 0)
			return;

		DiceSetData newSet = new DiceSetData();
		newSet.name = string.Format("Set {0}", setIndex);
		newSet.dieTypes = dieTypes;
		AddDiceSet(newSet);
	}

	/// <summary>
	/// Adds the given dice set to the list of dice sets
	/// </summary>
	/// <param name="set">Dice set to add</param>
	public void AddDiceSet(DiceSetData set)
	{
		database.AddData(set);
		OnSetAdded?.Invoke(set);
	}

	/// <summary>
	/// Remove the given dice set from the list of dice sets
	/// </summary>
	/// <param name="set"></param>
	public void RemoveDiceSet(DiceSetData set)
	{
		database.RemoveData(set);
		OnSetRemoved?.Invoke(set);
	}

	/// <summary>
	/// Called to load a dice set to the board
	/// </summary>
	/// <param name="set">Dice set that is being loaded</param>
	public void LoadDiceSet(DiceSetData set)
	{
		OnSetLoaded?.Invoke(set);
	}
}
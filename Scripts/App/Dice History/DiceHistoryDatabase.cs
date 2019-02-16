using UnityEngine;

/// <summary>
/// Created By: Jonathon Wigley - 7/7/2018
/// Last Edited By: Jonathon Wigley - 02/03/2019
/// </summary>

/// <summary>
/// Database for all known dice history
/// </summary>
[CreateAssetMenu(fileName = "New Dice History Database", menuName = "Database/Dice History")]
public class DiceHistoryDatabase : Database<DiceHistoryData>
{
	/// <summary>
	/// Sets the max database size
	/// </summary>
	protected override void InitMaxDatabaseSize()
	{
		maxDatabaseSize = 20;
	}

	/// <summary>
	/// Clones the database using json conversion
	/// </summary>
	/// <returns>Deep copy of the database</returns>
	public DiceHistoryDatabase Clone()
	{
		string json = JsonUtility.ToJson(this);
		DiceHistoryDatabase newDatabase = new DiceHistoryDatabase();
		JsonUtility.FromJsonOverwrite(json, newDatabase);
		return newDatabase;
	}
}

using UnityEngine;

/// <summary>
/// Created By: Jonathon Wigley - 02/03/2019
/// Last Edited By: Jonathon Wigley - 02/03/2019
/// </summary>

/// <summary>
/// Database for all existing dice sets
/// </summary>
[CreateAssetMenu(fileName = "New Dice Set Database", menuName = "Database/Dice Sets")]
public class DiceSetDatabase : Database<DiceSetData>
{
	/// <summary>
	/// Set the max size of the database
	/// </summary>
	protected override void InitMaxDatabaseSize()
	{
		maxDatabaseSize = 20;
	}

	/// <summary>
	/// Returns a clone of the database
	/// </summary>
	/// <returns>Deep copy of the database</returns>
	public DiceSetDatabase Clone()
	{
		string json = JsonUtility.ToJson(this);
		DiceSetDatabase newDatabase = new DiceSetDatabase();
		JsonUtility.FromJsonOverwrite(json, newDatabase);
		return newDatabase;
	}
}
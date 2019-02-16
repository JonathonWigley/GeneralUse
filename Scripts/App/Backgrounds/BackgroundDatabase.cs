using UnityEngine;

/// <summary>
/// Created By: Jonathon Wigley - 11/04/2018
/// Last Edited By: Jonathon Wigley - 02/03/2019
/// </summary>

/// <summary>
/// Database for all existing backgrounds
/// </summary>
[CreateAssetMenu(fileName = "New Background Database", menuName = "Database/Backgrounds")]
public class BackgroundDatabase : Database<BackgroundData>
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
	public BackgroundDatabase Clone()
	{
		string json = JsonUtility.ToJson(this);
		BackgroundDatabase newDatabase = new BackgroundDatabase();
		JsonUtility.FromJsonOverwrite(json, newDatabase);
		return newDatabase;
	}
}

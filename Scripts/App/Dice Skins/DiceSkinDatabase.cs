using UnityEngine;

/// <summary>
/// Created By: Jonathon Wigley - 7/1/2018
/// Last Edited By: Jonathon Wigley - 02/03/2019
/// </summary>

/// <summary>
/// Database for all existing dice skins
/// </summary>
[CreateAssetMenu(fileName = "New Dice Skin Database", menuName = "Database/Dice Skins")]
public class DiceSkinDatabase : Database<DiceSkinData>
{
	/// <summary>
	/// Sets the max size of the database
	/// </summary>
	protected override void InitMaxDatabaseSize()
	{
		maxDatabaseSize = 20;
	}

	/// <summary>
	/// Returns a clone of this object by serializing and
	/// deserializing into a new object
	/// </summary>
	/// <returns>Deep copy of the database</returns>
	public DiceSkinDatabase Clone()
	{
		string json = JsonUtility.ToJson(this);
		DiceSkinDatabase newSkins = new DiceSkinDatabase();
		JsonUtility.FromJsonOverwrite(json, newSkins);
		return newSkins;
	}
}

/// <summary>
/// Created By: Jonathon Wigley - 11/04/2018
/// Last Edited By: Jonathon Wigley - 02/03/2019
/// </summary>

/// <summary>
/// Parent class for any data that can be stored in a database
/// </summary>
public abstract class DatabaseData
{
	public int ID;

	/// <summary>
	/// Determine if equality of this type of object
	/// </summary>
	/// <param name="other">Object to compare against</param>
	/// <returns>True if this and the other object are equivalent</returns>
	public override bool Equals(object other)
	{
		if (other == null || GetType().Equals(other.GetType()) == false)
			return false;

		DatabaseData otherData = other as DatabaseData;

		if(ID == otherData.ID)
			return true;

		return false;
	}

	/// <summary>
	/// Get a hash code for this object
	/// </summary>
	/// <returns>ID of the data</returns>
	public override int GetHashCode()
	{
		return ID;
	}
}

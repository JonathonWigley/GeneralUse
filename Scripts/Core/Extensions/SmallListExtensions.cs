using EnhancedUI;

public static class SmallListExtensions
{
	/// <summary>
	/// Randomizes the contents of a list
	/// </summary>
	/// <param name="list"></param>
	/// <typeparam name="T"></typeparam>
	public static void RandomizeList<T>(this SmallList<T> list)
	{
		System.Random random = new System.Random();
		for (int i = 0; i < list.Count; i++)
			list.Swap(i, random.Next(i, list.Count));
	}

	/*-------------------------------------------------------------------------------------------------------------------------------------*/

	/// <summary>
	/// Swaps two items within a list at the given indicies
	/// </summary>
	/// <param name="i">Index of the first item to swap</param>
	/// <param name="j">Index of the second item to swap</param>
	public static void Swap<T>(this SmallList<T> list, int i, int j)
	{
		var temp = list[i];
		list[i] = list[j];
		list[j] = temp;
	}

	/*-------------------------------------------------------------------------------------------------------------------------------------*/

	/// <summary>
	/// Adds an object to the list if the list does not already contain the item
	/// </summary>
	/// <param name="objectToAdd">Object to add to the list</param>
	/// <returns>True if the object was added, false if it was a duplicate</returns>
	public static bool AddDistinct<T>(this SmallList<T> list, T objectToAdd)
	{
		if(list.Contains(objectToAdd))
			return false;

		list.Add(objectToAdd);
		return true;
	}
}

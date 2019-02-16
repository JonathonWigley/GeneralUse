using System.Collections.Generic;

public static class ArrayExtensions
{
	/// <summary>
	/// Returns the array as a List
	/// </summary>
	/// <param name="array">This array</param>
	/// <typeparam name="T">Generic object type for the array</typeparam>
	/// <returns>List of this array</returns>
	public static List<T> ToList<T>(this T[] array)
	{
		return new List<T>(array);
	}
}
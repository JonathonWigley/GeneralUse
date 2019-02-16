using UnityEngine;

/// <summary>
/// Created By: Jonathon Wigley - 10/13/2018
/// Last Edited By: Jonathon Wigley - 10/13/2018
/// </summary>

/// <summary>
/// Collection of Extensions for Unity's Transform class
/// </summary>
public static class TransformExtensions
{
	/// <summary>
	/// Returns an array of a transform's direct children, without any grandchildren
	/// </summary>
	/// <param name="parent">This transform</param>
	/// <returns>Direct children</returns>
	public static Transform[] GetDirectChildren(this Transform parent)
	{
		int childCount = parent.childCount;
		Transform[] children = new Transform[childCount];
		for(int i = 0; i < childCount; i++)
		{
			children[i] = parent.GetChild(i);
		}

		return children;
	}
}

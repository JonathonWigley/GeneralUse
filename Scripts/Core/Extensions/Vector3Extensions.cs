using UnityEngine;

public static class Vector3Extensions
{
	/// <summary>
	/// Returns the direction from this point another point
	/// </summary>
	/// <param name="other">Point to find the direction to</param>
	/// <returns>Direction from this point to the other point</returns>
	public static Vector3 DirectionTo(this Vector3 position, Vector3 other)
	{
		return (other - position).normalized;
	}

	/// <summary>
	/// Returns the distance from this point another point
	/// </summary>
	/// <param name="other">Point to find the distance to</param>
	/// <returns>Distance from this point to the other point</returns>
	public static float DistanceTo(this Vector3 position, Vector3 other)
	{
		return (other - position).magnitude;
	}

	/// <summary>
	/// Add values to the vector
	/// </summary>
	/// <param name="x">x value to add</param>
	/// <param name="y">y value to add</param>
	/// <param name="z">z value to add</param>
	public static Vector3 AddValues(this Vector3 vector, float? x = null, float? y = null, float? z = null)
	{
		Vector3 addVector = new Vector3(x != null ? (float)x : 0, y != null ? (float)y : 0, z != null ? (float)z : 0);
		return vector + addVector;
	}
}
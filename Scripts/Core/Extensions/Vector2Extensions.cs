using UnityEngine;

public static class Vector2Extensions
{
	/// <summary>
	/// Returns the direction from this point another point
	/// </summary>
	/// <param name="other">Point to find the direction to</param>
	/// <returns>Direction from this point to the other point</returns>
	public static Vector3 DirectionTo(this Vector2 position, Vector2 other)
	{
		return (other - position).normalized;
	}

	/// <summary>
	/// Returns the distance from this point another point
	/// </summary>
	/// <param name="other">Point to find the distance to</param>
	/// <returns>Distance from this point to the other point</returns>
	public static float DistanceTo(this Vector2 position, Vector2 other)
	{
		return (other - position).magnitude;
	}
}
using UnityEngine;

/// <summary>
/// Created By: Jonathon Wigley - 09/04/2018
/// Last Edited By: Jonathon Wigley - 12/29/2018
/// </summary>

/// <summary>
/// Set of utility methods for casting from the screen to the game world
/// </summary>
public static class RaycastUtility
{
	/// <summary>
	/// Raycasts from the given screen position to the world and returns the hit
	/// </summary>
	/// <param name="screenPosition">Screen position to cast from</param>
	/// <param name="mask">Layermask used in the cast</param>
	/// <param name="distance">Distance to cast. Default 100.</param>
	/// <returns>RaycastHit created by the cast</returns>
	public static RaycastHit GetWorldHit(Vector2 screenPosition, LayerMask mask, float distance = 100f)
	{
        Ray cameraToScreenRay = Camera.main.ScreenPointToRay(screenPosition);
        RaycastHit hit;
        Physics.Raycast(cameraToScreenRay, out hit, distance, mask);
		return hit;
	}

	/// <summary>
	/// Spherecasts from the given screen position to the world and returns the hit
	/// </summary>
	/// <param name="screenPosition">Screen position to cast from</param>
	/// <param name="mask">Layermask used in the cast</param>
	/// <param name="distance">Distance to cast. Default 100.</param>
	/// <returns>RaycastHit created by the cast</returns>
	public static RaycastHit[] GetWorldHitsSphere(Vector2 screenPosition, LayerMask mask, float radius = 2f, float distance = 100f)
	{
        Ray cameraToScreenRay = Camera.main.ScreenPointToRay(screenPosition);
        RaycastHit[] hits = new RaycastHit[3];
        Physics.SphereCastNonAlloc(cameraToScreenRay.origin, radius, cameraToScreenRay.direction, hits, distance, mask);

		return hits;
	}
}

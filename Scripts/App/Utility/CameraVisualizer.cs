using UnityEngine;

/// <summary>
/// Created By: Jonathon Wigley - 09/22/2018
/// Last Edited By: Jonathon Wigley - 09/22/2018
/// </summary>

/// <summary>
/// Purpose:
/// Visualize the bounds of the camera using the object layer given
/// </summary>
[ExecuteInEditMode]
public class CameraVisualizer : MonoBehaviour
{
	public LayerMask VisualizeMask;
	public GameSettings settings;
	Rect safeArea;

	private void Update()
	{
		safeArea = Screen.safeArea;
		if(settings != null)
		{
			Rect tempRect = new Rect(safeArea);
			tempRect.yMax -= settings.TopPadding * safeArea.height;
			tempRect.yMin += settings.BottomPadding * safeArea.height;
			tempRect.xMin += settings.LeftPadding * safeArea.width;
			tempRect.xMax -= settings.RightPadding * safeArea.width;
			safeArea = tempRect;
		}

	}

	/// <summary>
	/// Draws an outline of the camera's view on the ground
	/// </summary>
	private void OnDrawGizmos()
	{
		if(settings == null)
			return;

		Vector3 topLeft, topRight, bottomLeft, bottomRight;
		topLeft = GetHitPosition(new Vector2(safeArea.xMin, safeArea.yMin));
		topRight = GetHitPosition(new Vector2(safeArea.xMax, safeArea.yMin));
		bottomLeft = GetHitPosition(new Vector2(safeArea.xMin, safeArea.yMax));
		bottomRight = GetHitPosition(new Vector2(safeArea.xMax, safeArea.yMax));

		Gizmos.DrawSphere(topLeft, .1f);
		Gizmos.DrawSphere(topRight, .1f);
		Gizmos.DrawSphere(bottomLeft, .1f);
		Gizmos.DrawSphere(bottomRight, .1f);

		Gizmos.DrawLine(topLeft, topRight);
		Gizmos.DrawLine(topRight, bottomRight);
		Gizmos.DrawLine(bottomRight, bottomLeft);
		Gizmos.DrawLine(bottomLeft, topLeft);
	}

	/// <summary>
	/// Returns the ground position of the given screen position
	/// </summary>
	/// <param name="screenPosition"></param>
	/// <returns></returns>
	Vector3 GetHitPosition(Vector2 screenPosition)
	{
		RaycastHit hit = RaycastUtility.GetWorldHit(screenPosition, LayerMask.GetMask("Ground"));
		if(hit.transform == null)
		{
			return Vector3.zero;
		}

		return hit.point;
	}
}

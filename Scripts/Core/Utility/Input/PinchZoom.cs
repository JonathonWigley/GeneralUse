using UnityEngine;
using TouchScript.Gestures;
using TouchScript.Gestures.TransformGestures.Clustered;

/// <summary>
/// Created By: Jonathon Wigley - 09/22/2018
/// Last Edited By: Jonathon Wigley - 09/22/2018
/// </summary>

/// <summary>
/// Purpose:
/// Uses TouchScript to zoom the camera in and out when
/// the user uses a pinch gesture
/// <summary>

public class PinchZoom : MonoBehaviour
{
	public float ZoomSpeed { get; set; }
	public float MaxHeight { get; set; }
	public float MinHeight { get; set; }

	[SerializeField] private Transform cameraPivot;
	[SerializeField] private ClusteredScreenTransformGesture pinchGesture;

	GameSettings gameSettings;

	/*-------------------------------------------------------------------------------------------------------------------------------------*/

	private void Awake()
	{
		if(cameraPivot == null)
			cameraPivot = Camera.main.transform;

		gameSettings = GameManager.Instance.GameSettings;
	}

	/*-------------------------------------------------------------------------------------------------------------------------------------*/

	private void Start()
	{
		MaxHeight = gameSettings.ZoomOutMax;
		MinHeight = gameSettings.ZoomInMax;
		ZoomSpeed = gameSettings.ZoomLerpSpeed;
	}

	/*-------------------------------------------------------------------------------------------------------------------------------------*/

	private void OnEnable()
	{
		pinchGesture.Transformed += PinchGesture_Transformed;
	}

	/*-------------------------------------------------------------------------------------------------------------------------------------*/

	private void OnDisable()
	{
		pinchGesture.Transformed -= PinchGesture_Transformed;
	}

	/*-------------------------------------------------------------------------------------------------------------------------------------*/

	private void PinchGesture_Transformed(object sender, System.EventArgs e)
	{
		if(pinchGesture.ActivePointers.Count == 1)
		{
			Vector3 deltaPosition = pinchGesture.DeltaPosition;
		}

		if(pinchGesture.ActivePointers.Count == 2)
		{
			Vector3 deltaZoom = Vector3.down * (pinchGesture.DeltaScale - 1f) * ZoomSpeed;
			Vector3 targetPosition = cameraPivot.localPosition + deltaZoom;
			if(targetPosition.y > MaxHeight)
				targetPosition = new Vector3(targetPosition.x, MaxHeight, targetPosition.z);
			if(targetPosition.y < MinHeight)
				targetPosition = new Vector3(targetPosition.x, MinHeight, targetPosition.z);

			cameraPivot.localPosition = targetPosition;
		}
	}

	/*-------------------------------------------------------------------------------------------------------------------------------------*/
}

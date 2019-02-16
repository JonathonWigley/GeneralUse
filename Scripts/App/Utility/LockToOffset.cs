using UnityEngine;

/// <summary>
/// Created By: Jonathon Wigley - 11/10/2018
/// Last Edited By: Jonathon Wigley - 11/10/2018
/// </summary>

/// <summary>
/// Locks the object to an offset from the target
/// </summary>
public class LockToOffset : MonoBehaviour
{
	[Tooltip("Object this should be locked to")]
	[SerializeField] private Transform targetTransform;

	[Tooltip("Offset from the locked onto target")]
	[SerializeField] private Vector3 offset;

	/// <summary>
	/// Last position of this object
	/// </summary>
	Vector3 cachedPosition;

	private void Update()
	{
		if(targetTransform == null)
			return;
		Vector3 targetPosition = targetTransform.position + offset;

		if(targetPosition != cachedPosition)
		{
			transform.position = targetPosition;
			cachedPosition = targetPosition;
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockToOffsetBetweenCamera : MonoBehaviour
{
	[SerializeField] protected Transform targetTransform;
	[SerializeField] protected float offsetTowardCamera = 1f;

	Vector3 cachedPosition;

	protected void Update()
	{
		if(targetTransform == null)
			return;

		Vector3 directionToCamera = (Camera.main.transform.position - targetTransform.position).normalized;
		Vector3 targetPosition = targetTransform.position + directionToCamera * offsetTowardCamera;

		if(targetPosition != cachedPosition)
		{
			transform.position = targetPosition;
			cachedPosition = targetPosition;
		}
	}
}

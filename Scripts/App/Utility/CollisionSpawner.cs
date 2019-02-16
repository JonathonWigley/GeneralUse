using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Created By: Jonathon Wigley - 08/28/2018
/// Last Edited By: Jonathon Wigley - 08/28/2018
///
/// Purpose:
/// Create a rectangle on the ground in editor that shows
/// where the camera view extends to
///
/// References:
/// <summary>

public class CollisionSpawner : MonoBehaviour
{
	Rect safeArea;
	Transform boundsCamera;
	Vector3 cachedCameraPosition;
	GameSettings settings;

	bool sizesUpdating;
	bool positionsUpdating;

	BoxCollider leftCollider, rightCollider, topCollider, bottomCollider, cielingCollider;
	Rigidbody leftBody, rightBody, topBody, bottomBody, cielingBody;
	Vector3 leftTargetSize, rightTargetSize, topTargetSize, bottomTargetSize, cielingTargetSize;
	Vector3 leftTargetPosition, rightTargetPosition, topTargetPosition, bottomTargetPosition, cielingTargetPosition;

	private void Start()
	{
		settings = GameManager.Instance.GameSettings;
		boundsCamera = Camera.main.transform;

		GenerateColliders();
		UpdateTargets();
	}

	private void Update()
	{
		// If the camera has moved, update the target positions and sizes of each of the colliders
		bool boundsCameraMoved = cachedCameraPosition != boundsCamera.transform.position;
		if(boundsCameraMoved)
		{
			UpdateTargets();
			cachedCameraPosition = boundsCamera.transform.position;
		}

		// Update the size and position of each collider
		if(sizesUpdating)
			sizesUpdating = !UpdateColliderSizes();
		if(positionsUpdating)
			positionsUpdating = !UpdateColliderPositions();
	}

	/// <summary>
	/// Creates the collider for each of the arena sides
	/// </summary>
	private void GenerateColliders()
	{
		if(leftCollider == null)
		{
			leftCollider = new GameObject("Left Arena Collider").AddComponent<BoxCollider>();
			leftCollider.transform.SetParent(this.transform);
			leftBody = leftCollider.gameObject.AddComponent<Rigidbody>();
			leftBody.useGravity = false;
			leftBody.isKinematic = true;
			leftBody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
		}

		if(rightCollider == null)
		{
			rightCollider = new GameObject("Right Arena Collider").AddComponent<BoxCollider>();
			rightCollider.transform.SetParent(this.transform);
			rightBody = rightCollider.gameObject.AddComponent<Rigidbody>();
			rightBody.useGravity = false;
			rightBody.isKinematic = true;
			rightBody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
		}

		if(topCollider == null)
		{
			topCollider = new GameObject("Top Arena Collider").AddComponent<BoxCollider>();
			topCollider.transform.SetParent(this.transform);
			topBody = topCollider.gameObject.AddComponent<Rigidbody>();
			topBody.useGravity = false;
			topBody.isKinematic = true;
			topBody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
		}

		if(bottomCollider == null)
		{
			bottomCollider = new GameObject("Bottom Arena Collider").AddComponent<BoxCollider>();
			bottomCollider.transform.SetParent(this.transform);
			bottomBody = bottomCollider.gameObject.AddComponent<Rigidbody>();
			bottomBody.useGravity = false;
			bottomBody.isKinematic = true;
			bottomBody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
		}

		if(cielingCollider == null)
		{
			cielingCollider = new GameObject("Cieling Arena Collider").AddComponent<BoxCollider>();
			cielingCollider.transform.SetParent(this.transform);
			cielingBody = cielingCollider.gameObject.AddComponent<Rigidbody>();
			cielingBody.useGravity = false;
			cielingBody.isKinematic = true;
			cielingBody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
		}
	}

	/// <summary>
	/// Set the target sizes and positions for each collider
	/// </summary>
	private void UpdateTargets()
	{
		safeArea = Screen.safeArea;
		Rect tempRect = new Rect(safeArea);
		tempRect.yMax -= settings.TopPadding * safeArea.height;
		tempRect.yMin += settings.BottomPadding * safeArea.height;
		tempRect.xMin += settings.LeftPadding * safeArea.width;
		tempRect.xMax -= settings.RightPadding * safeArea.width;
		safeArea = tempRect;

		Vector3 center, bottomLeft, bottomRight, topLeft, topRight;
		center = GetGroundPosition(safeArea.center);
		topLeft = GetGroundPosition(new Vector2(safeArea.xMin, safeArea.yMin));
		topRight = GetGroundPosition(new Vector2(safeArea.xMax, safeArea.yMin));
		bottomLeft = GetGroundPosition(new Vector2(safeArea.xMin, safeArea.yMax));
		bottomRight = GetGroundPosition(new Vector2(safeArea.xMax, safeArea.yMax));

		SetTargetSizes(center, bottomLeft, bottomRight, topLeft, topRight);
		SetTargetPositions(center, bottomLeft, bottomRight, topLeft, topRight);

		sizesUpdating = true;
		positionsUpdating = true;
	}

	/// <summary>
	/// Set the target size for each of the colliders
	/// </summary>
	/// <param name="center">Center of the screen in worldspace</param>
	/// <param name="bottomLeft">Bottom left corner of the screen in worldspace</param>
	/// <param name="bottomRight">Borrom right corner of the screen in worldspace</param>
	/// <param name="topLeft">Top left corner of the screen in worldspace</param>
	/// <param name="topRight">Top right corner of the screen in worldspace</param>
	private void SetTargetSizes(Vector3 center, Vector3 bottomLeft, Vector3 bottomRight, Vector3 topLeft, Vector3 topRight)
	{
		leftTargetSize = new Vector3(1f, boundsCamera.position.y - .5f, Mathf.Abs(topLeft.z - bottomLeft.z));
		rightTargetSize = new Vector3(1f, boundsCamera.position.y - .5f, Mathf.Abs(topRight.z - bottomRight.z));
		topTargetSize = new Vector3(Mathf.Abs(topRight.x - topLeft.x), boundsCamera.position.y - .5f, 1f);
		bottomTargetSize = new Vector3(Mathf.Abs(bottomRight.x - bottomLeft.x), boundsCamera.position.y - .5f, 1f);
		cielingTargetSize = new Vector3(topRight.x - topLeft.x, 1f, topLeft.z - bottomLeft.z);
	}

	/// <summary>
	/// Set the target position for each of the colliders
	/// </summary>
	/// <param name="center">Center of the screen in worldspace</param>
	/// <param name="bottomLeft">Bottom left corner of the screen in worldspace</param>
	/// <param name="bottomRight">Borrom right corner of the screen in worldspace</param>
	/// <param name="topLeft">Top left corner of the screen in worldspace</param>
	/// <param name="topRight">Top right corner of the screen in worldspace</param>
	private void SetTargetPositions(Vector3 center, Vector3 bottomLeft, Vector3 bottomRight, Vector3 topLeft, Vector3 topRight)
	{
		leftTargetPosition = new Vector3(topLeft.x, (boundsCamera.position.y - .5f) / 2f, center.z);
		rightTargetPosition = new Vector3(topRight.x, (boundsCamera.position.y - .5f) / 2f, center.z);
		topTargetPosition = new Vector3(center.x, (boundsCamera.position.y - .5f) / 2f, topLeft.z);
		bottomTargetPosition = new Vector3(center.x, (boundsCamera.position.y - .5f) / 2f, bottomLeft.z);
		cielingTargetPosition = new Vector3(center.x, boundsCamera.position.y - .5f, center.z);
	}

	/// <summary>
	/// Lerp each of the collider's sizes to the target size
	/// </summary>
	/// <returns>Returns true if the target sizes have been reached</returns>
	private bool UpdateColliderSizes()
	{
		leftCollider.size = Vector3.Lerp(leftCollider.size, leftTargetSize, Time.deltaTime * settings.ColliderMoveSpeed);
		rightCollider.size = Vector3.Lerp(rightCollider.size, rightTargetSize, Time.deltaTime * settings.ColliderMoveSpeed);
		topCollider.size = Vector3.Lerp(topCollider.size, topTargetSize, Time.deltaTime * settings.ColliderMoveSpeed);
		bottomCollider.size = Vector3.Lerp(bottomCollider.size, bottomTargetSize, Time.deltaTime * settings.ColliderMoveSpeed);
		cielingCollider.size = Vector3.Lerp(cielingCollider.size, cielingTargetSize, Time.deltaTime * settings.ColliderMoveSpeed * 2);

		if ( 	Vector3.Distance(leftCollider.size, leftTargetSize) < .01f &&
				Vector3.Distance(rightCollider.size, rightTargetSize) < .01f &&
				Vector3.Distance(topCollider.size, topTargetSize) < .01f &&
				Vector3.Distance(bottomCollider.size, bottomTargetSize) < .01f &&
				Vector3.Distance(cielingCollider.size, cielingTargetSize) < .01f)
			{
				SetColliderSizesToTargetImmediate();
				return true;
			}

		return false;
	}

	/// <summary>
	/// Lerp each of the colliders to their target position
	/// </summary>
	/// <returns>Returns true if all of the colliders have reached their target position</returns>
	private bool UpdateColliderPositions()
	{
		leftCollider.transform.position = Vector3.Lerp(leftCollider.transform.position, leftTargetPosition, Time.deltaTime * settings.ColliderMoveSpeed);
		rightCollider.transform.position = Vector3.Lerp(rightCollider.transform.position, rightTargetPosition, Time.deltaTime * settings.ColliderMoveSpeed);
		topCollider.transform.position = Vector3.Lerp(topCollider.transform.position, topTargetPosition, Time.deltaTime * settings.ColliderMoveSpeed);
		bottomCollider.transform.position = Vector3.Lerp(bottomCollider.transform.position, bottomTargetPosition, Time.deltaTime * settings.ColliderMoveSpeed);
		cielingCollider.transform.position = Vector3.Lerp(cielingCollider.transform.position, cielingTargetPosition, Time.deltaTime * settings.ColliderMoveSpeed);

		Vector3 pos;
		pos = leftCollider.transform.position;
		leftCollider.transform.position = new Vector3(pos.x, leftCollider.size.y / 2f, pos.z);

		pos = rightCollider.transform.position;
		rightCollider.transform.position = new Vector3(pos.x, rightCollider.size.y / 2f, pos.z);

		pos = topCollider.transform.position;
		topCollider.transform.position = new Vector3(pos.x, topCollider.size.y / 2f, pos.z);

		pos = bottomCollider.transform.position;
		bottomCollider.transform.position = new Vector3(pos.x, bottomCollider.size.y / 2f, pos.z);

		if ( 	Vector3.Distance(leftCollider.transform.position, leftTargetPosition) < .01f &&
				Vector3.Distance(rightCollider.transform.position, rightTargetPosition) < .01f &&
				Vector3.Distance(topCollider.transform.position, topTargetPosition) < .01f &&
				Vector3.Distance(bottomCollider.transform.position, bottomTargetPosition) < .01f &&
				Vector3.Distance(cielingCollider.transform.position, cielingTargetPosition) < .01f)
			{
				SetColliderPositionsToTargetImmediate();
				return true;
			}

		return false;
	}

	/// <summary>
	/// Immediately change the collider positions to their target positions
	/// </summary>
	private void SetColliderPositionsToTargetImmediate()
	{
		leftCollider.transform.position = leftTargetPosition;
		rightCollider.transform.position = rightTargetPosition;
		topCollider.transform.position = topTargetPosition;
		bottomCollider.transform.position = bottomTargetPosition;
		cielingCollider.transform.position = cielingTargetPosition;
	}

	/// <summary>
	/// Immediately set the collider sizes to their target sizes
	/// </summary>
	private void SetColliderSizesToTargetImmediate()
	{
		leftCollider.size = leftTargetSize;
		rightCollider.size = rightTargetSize;
		topCollider.size = topTargetSize;
		bottomCollider.size = bottomTargetSize;
		cielingCollider.size = cielingTargetSize;
	}

	/// <summary>
	/// Returns the ground position of the given screen position
	/// </summary>
	/// <param name="screenPosition"></param>
	/// <returns></returns>
	Vector3 GetGroundPosition(Vector2 screenPosition)
	{
		RaycastHit hit = RaycastUtility.GetWorldHit(screenPosition, LayerMask.GetMask("Ground"));
		if(hit.transform == null)
		{
			return Vector3.zero;
		}

		return hit.point;
	}
}

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Created By: Jonathon Wigley - 11/11/2018
/// Last Edited By: Jonathon Wigley - 11/11/2018
/// </summary>

/// <summary>
/// Manages the lock canvas that shows the lock symbol for dice in edit mode
/// </summary>
public class DieLockUI : PooledObject
{
	public bool bIsLocked { get; private set; }

	///<summary>
	/// Distance offset from the target die
	///</summary>
	[SerializeField] private float offsetTowardCamera = 1f;

	///<summary>
	/// Image that displays the lock sprite
	///</summary>
	[SerializeField] private Image lockImage;

	///<summary>
	/// Sprite shown when the die is locked
	///</summary>
	[SerializeField] private Sprite lockedSprite;

	///<summary>
	/// Sprite shown when the die is unlocked
	///</summary>
	[SerializeField] private Sprite unlockedSprite;

	///<summary>
	/// Color of the locked sprite
	///</summary>
	[SerializeField] private Color lockedColor = Color.red;

	///<summary>
	/// Color of the unlocked sprite
	///</summary>
	[SerializeField] private Color unlockedColor = Color.green;

	///<summary>
	/// Die that this canvas is following
	///</summary>
	private Die targetDie;

	/// <summary>
	/// Target transform to follow (target die's transform)
	/// </summary>
	private Transform targetTransform;

	/// <summary>
	/// Last recorded target position
	/// </summary>
	private Vector3 cachedPosition;

	private void Update()
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

	/// <summary>
	/// Set the die this canvas should be locked to
	/// </summary>
	/// <param name="die"></param>
	public void SetTargetDie(Die die)
	{
		targetDie = die;

		if(die != null)
			targetTransform = targetDie.transform;
	}

	/// <summary>
	/// Lock the die if unlocked, or unlock it if locked
	/// </summary>
	public void ToggleLock()
	{
		ToggleLock(!bIsLocked);
	}

	/// <summary>
	/// Lock or unlock the die
	/// </summary>
	/// <param name="val">Lock if true, unlock if false</param>
	public void ToggleLock(bool val)
	{
		lockImage.sprite = val ? lockedSprite : unlockedSprite;
		lockImage.color = val ? lockedColor : unlockedColor;

		bIsLocked = val;
	}
}
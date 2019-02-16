using UnityEngine;

[CreateAssetMenu (fileName = "New Dice Settings", menuName = "Settings/Dice")]
public class DiceSettings : ScriptableObject
{
	/// <summary>
	/// Speed at which a die lerps to the touch position when held
	/// </summary>
	[Tooltip("Speed at which a die lerps to the touch position when held")]
	public float DragSpeed = 10f;

	/// <summary>
	/// Amount of time a touch can be held and still be counted as a tap
	/// </summary>
	[Tooltip("Amount of time a touch can be held and still be counted as a tap")]
	public float TapTimeThreshold = .3f;

	/// <summary>
	/// Distance a touch can move before it fails as a tap
	/// </summary>
	[Tooltip("Distance a touch can move before it fails as a tap")]
	public float TapMaxMoveDistance = 1.5f;

	/// <summary>
	/// Velocity a die needs to be moving at when released from a hold to roll
	/// </summary>
	[Tooltip("Velocity a die needs to be moving at when released from a hold to roll")]
	public float RollOnReleaseSpeedThreshold = 5f;
}
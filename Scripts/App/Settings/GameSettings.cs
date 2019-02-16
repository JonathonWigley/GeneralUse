using UnityEngine;
using System;

/// <summary>
/// Created By: Jonathon Wigley - 8/5/2018
/// Last Edited By: Jonathon Wigley - 10/14/2018
///
/// Purpose:
/// Scriptable object used to store presets of game settings
/// <summary>

[CreateAssetMenu (fileName = "New Game Settings", menuName = "Settings/Game")]
public class GameSettings : ScriptableObject
{
	[Header("Time")]
	public float TimeScale = 1.55f;

	[Header("Rolling")]
	public float VerticalRollForce = 10f;
	public float HorizontalRollForce = 1f;
	public float AngularRollForce = 10f;

	[Header("Bouncing")]
	public float VerticalBounceForce = 10f;
	public float HorizontalBounceForce = 1f;
	public float AngularBounceForce = 10f;

	[Header("Nudging")]
	public float VerticalNudgeForce = 10f;
	public float HorizontalNudgeForce = 1f;
	public float AngularNudgeForce = 10f;

	[Header("Throwing")]
	public float MaxThrowVelocityMagnitude = 15f; // Max velocity a die can go when thrown
	public float AngularThrowForceMultiplier = 2f; // Multiplies the velocity force given when calculating angular force for a throw
	public float VerticalThrowForce = 2f; // Amount of lift a die has when being thrown

	[Header("Flicking")]
	public float MinDistanceToFlick = .75f; // Distance in cm the user needs to flick to register a flick
	public float HorizontalFlickForce = 5f; // Horizontal force applied to a die when it is quickly flicked
	public float VerticalFlickForce = 5f; // Vertical force applied to a die when it is quickly flicked

	[Header("Dragging")]
	public float DiceHoldHeight = 2f;

	[Header("Zooming")]
	public float ZoomMagnitude = .3f; // Amount the screen zooms when the touches move
	public float ZoomLerpSpeed = 1f; // Speed at which the camera lerps to the target zoom position
	public float ZoomInMax = 6f; // Minimum y value that the camera can be at for zooming in
	public float ZoomOutMax = 15f; // Maximum y value that the camera can be at for zooming out
	public float ColliderMoveSpeed = 5f; // Speed at which the arena colliders move when zooming in and out

	[Header("Kill Boxes")]
	public LayerMask KillBoxLayerMask = ~0; // Object layers that killboxes will destroy
	public float KillBoxInitializeDelay = 1f; // Delay before killboxes become active

	[Header("Screen Safe Area")]
	[Range(0f, 1f)] public float TopPadding = 0f;
	[Range(0f, 1f)] public float BottomPadding = .25f;
	[Range(0f, 1f)] public float LeftPadding = 0f;
	[Range(0f, 1f)] public float RightPadding = 0f;

	/// <summary>
	/// Returns a clone of this object by serializing and deserializing
	/// it into a new instance
	/// </summary>
	/// <returns></returns>
	public GameSettings Clone()
	{
		string jsonClone = JsonUtility.ToJson(this);
		GameSettings newsSettings = new GameSettings();
		JsonUtility.FromJsonOverwrite(jsonClone, newsSettings);
		return newsSettings;
	}
}

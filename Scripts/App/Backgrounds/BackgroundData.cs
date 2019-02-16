using UnityEngine;

/// <summary>
/// Created By: Jonathon Wigley - 11/04/2018
/// Last Edited By: Jonathon Wigley - 02/03/2019
/// </summary>

/// <summary>
/// Data container for backgrounds
/// </summary>
[System.Serializable]
public class BackgroundData : DatabaseData
{
	public string BackgroundName = "Background";

	/// <summary>
	/// Base texture for the background
	/// </summary>
	public Texture BaseTexture;

	/// <summary>
	/// Texture for the background's normal map
	/// </summary>
	public Texture NormalTexture;

	/// <summary>
	/// Texture for the background's spec map
	/// </summary>
	public Texture SpecTexture;

	/// <summary>
	/// Base color applied to the base texture
	/// </summary>
	public Color BaseColor = Color.white;

	/// <summary>
	/// Overlay color for the background
	/// </summary>
	public Color OverlayColor = Color.white;

	/// <summary>
	/// Shadow color for the background
	/// </summary>
	public Color ShadowColor = Color.white;

	/// <summary>
	/// Overall strength of the color effect
	/// </summary>
	[Range(0f, 1f)] public float MasterStrength;

	/// <summary>
	/// Strength of the shadow on the background
	/// </summary>
	[Range(0f, 1f)] public float ShadowStrength;

	/// <summary>
	/// Strength of the overlay color on the background
	/// </summary>
	[Range(0f, 1f)] public float OverlayStrength;

	/// <summary>
	/// Smoothness of the texture
	/// </summary>
	[Range(0f, 1f)] public float Smoothness;
}
